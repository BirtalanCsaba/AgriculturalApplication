#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ArduinoJson.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>

//Sensor definitions

//Temperature and Humidity
#define DHTTYPE DHT11
#define DHTPIN  13
#define SOILPIN 14

//Product definitions
//Wifi definitions
#define ID "CsabiPlus"
//#define ID "DESKTOP-R6S29BR 3529"
#define PASSWORD "ibacs100"
//#define PASSWORD "2oX26?12"
//API definitions
//#define URL "http://192.168.0.105/api/" //API server URL

String PRODUCTID = "f7c7b9b6-5351-441a-b7f3-3c6248b531e1";
String URL = "http://192.168.0.105/api/";

const char* ssid = ID;
const char* password = PASSWORD;

int Delay = 30000;

//sensors
int IsWet;
int LightIntensity;
// create DHT22 instance
DHT dht(DHTPIN, DHTTYPE, 11);
float humidity, temp_c;
unsigned long previousMillis = 0;        // will store last temp was read
const long interval = 2000;              // interval at which to read sensor
//----------------------------

void setup() {
  // put your setup code here, to run once:
  pinMode(A0, OUTPUT);
  //Read Sensors
    dht.begin();

  
  //WIFI Connection
  //----------------------------
  // start serial communication to send messages to the computer
  Serial.begin(115200); 
  while(! Serial);
             // initialize temperature sensor
  
  //start connection
  Serial.println('\n');
  WiFi.begin(ssid, password);
  Serial.print("Connecting to ");
  Serial.print(ssid); Serial.println(" ...");

  // Wait for the Wi-Fi to connect
  int i = 0;
  while (WiFi.status() != WL_CONNECTED) { 
    delay(1000);
    Serial.print(++i); Serial.print(' ');
  }

  Serial.println('\n');
  Serial.println("Connection established!");  
  Serial.print("IP address:\t");
  Serial.println(WiFi.localIP());
  Serial.println('\n');
  //----------------------------
  
}

void loop() {
  // put your main code here, to run repeatedly:

  if(WiFi.status() == WL_CONNECTED) //if we are connected to the internet
  {

    //Read Sensors
    GetTemperatureAndHumidityValue();
    if((int)temp_c != 0) 
    Serial.println("Temperature = "+String((int)temp_c)+" C");
    if((int)humidity!=0)
    Serial.println("Humidity = "+String((int)humidity)+" %");

    GetSoilMoistureValue();
    GetLuminosity();
    //Send Data To API

    HTTPClient http;
    if(temp_c>100) 
    {
      temp_c=0;
      humidity=0;
    }
    String GetData = "?ProdId=" + PRODUCTID + "&Temp=" + String((int)temp_c) + "&Hum=" + String((int)humidity)+"&SoilHum="+String(IsWet)+"&Lum="+String(LightIntensity);
    String Link = URL+"Product/SendSensorValues"+GetData;
    Serial.println(Link);
    http.begin(Link);
    int httpCode = http.GET();
    if (httpCode > 0) //Check the returning code
    { 
      String payload = http.getString();
      Serial.print("Delay Time = ");Serial.println(payload);
      int temp;
      if(payload != "")
      {
        temp = payload.toInt();
        if(temp > 0) Delay = temp;
      }
    
    }
    
    http.end();
  }
  delay(Delay);
}

//functions for sensors
void GetTemperatureAndHumidityValue()
{
  // Wait at least 2 seconds seconds between measurements.
  // if the difference between the current time and last time you read
  // the sensor is bigger than the interval you set, read the sensor
  // Works better than delay for things happening elsewhere also
  unsigned long currentMillis = millis();
 
  if(currentMillis - previousMillis >= interval) {
    // save the last time you read the sensor 
    previousMillis = currentMillis;   

    // Reading temperature for humidity takes about 250 milliseconds!
    // Sensor readings may also be up to 2 seconds 'old' (it's a very slow sensor)
    humidity = dht.readHumidity();          // Read humidity (percent)
    temp_c = (dht.readTemperature(true)-32)/(1.8);     // Read temperature as Fahrenheit(Celsius Conversion)
    // Check if any reads failed and exit early (to try again).
    if (isnan(humidity) || isnan(temp_c)) {
      Serial.println("Failed to read from DHT sensor!");
      return;
    }
  }
}

void GetSoilMoistureValue()
{
  if(digitalRead(SOILPIN) == HIGH)
  {
    IsWet=0;
  }
  else
  {
    IsWet=1;
  }

  Serial.print("The soil is : ");Serial.println(IsWet);
}
void GetLuminosity()
{
  LightIntensity = analogRead(A0);
  Serial.print("Luminosity from sensor : ");Serial.println((int)LightIntensity);
  LightIntensity = (int)(Light(LightIntensity));
  Serial.print("Luminosity from sensor(converted lux) : ");Serial.println((int)LightIntensity);
}

double Light (int LDR_value)
{
  float lux=0.00,ADC_value=0.0048828125;
  lux=(250.000000/(ADC_value*LDR_value))-50.000000;
  return lux;
  /*double Vout=RawADC0*0.0048828125;
  int lux=500/(10*((5-Vout)/Vout));//use this equation if the LDR is in the upper part of the divider
  //int lux=(2500/Vout-500)/10;
  return lux;*/
}
//----------------------------

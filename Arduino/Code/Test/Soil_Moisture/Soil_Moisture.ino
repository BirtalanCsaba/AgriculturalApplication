#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ArduinoJson.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>

int sensor_pin = 12;

void setup() {
  Serial.begin(115200); 
  while(! Serial);
  Serial.println("Reading From the Sensor ...");
  delay(2000);
}

void loop() {
  // put your main code here, to run repeatedly:
 float moisture_percentage;

  moisture_percentage = (analogRead(sensor_pin)*3.08)/1024;

  Serial.print("Soil Moisture(in Percentage) = ");
  Serial.print(moisture_percentage);
  Serial.println("%");

  delay(1000);
/*if(digitalRead(sensor_pin) == HIGH){
Serial.println("da");
} else {
Serial.println("nu");
delay(1000);
}*/
}

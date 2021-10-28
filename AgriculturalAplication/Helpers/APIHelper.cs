using AgriculturalAplication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static AgriculturalAplication.Models.MainFormModel;
using static AgriculturalAplication.Models.ProductChartModel;

namespace AgriculturalAplication.Helpers
{
    public class APIHelper : IAPIHelper
    {
        
        private HttpClient apiClient;

        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region User

        public async Task<Guid> GetUserId(string username, string password)
        {
            var result = new Guid();

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string,string>("password",password)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/User/GetUserId", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<Guid>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            return result;
        }

        public async Task<bool> Authenticate(string username, string password) //pot sa pun un model
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string,string>("password",password)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/user", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<bool>(); //pot sa pun un model
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Guid> CreateUser(string username, string email, string password)
        {
            var result = new Guid();

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("Username",username),
                new KeyValuePair<string,string>("Email",email),
                new KeyValuePair<string,string>("Password",password)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/User/CreateUser", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<Guid>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            return result;
        }

        public async Task<string> GetUserEmail(string UserId)
        {
            var result = "";

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserId",UserId)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/User/GetUserEmail", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<string>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            return result;
        }

        public async Task UpdateUsername(string UserId,string Username)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserId",UserId),
                new KeyValuePair<string,string>("Username",Username)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/User/UpdateUsername", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task UpdateEmail(string UserId, string Email)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserId",UserId),
                new KeyValuePair<string,string>("Email",Email)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/User/UpdateEmail", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task UpdatePassword(string UserId, string Password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserId",UserId),
                new KeyValuePair<string,string>("Password",Password)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/User/UpdatePassword", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        #endregion

        #region Project

        public async Task<List<Project>> GetProjects(string UserId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("userid",UserId)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/project/GetProjectsById", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<Project>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task DeleteProject(string UserId, string ProjName)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("userid", UserId),
                new KeyValuePair<string, string>("projname", ProjName)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Project/1", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task CreateProjectWithImage(string UserId, string ProdId, string Name,
            string Description, byte[] Image)
        {
            FormUrlEncodedContent data;

            //string s = Encoding.ASCII.GetString(Image,0,Image.Length);
            string s = Convert.ToBase64String(Image);

            data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("ProjName", Name),
                new KeyValuePair<string, string>("ProjDesc", Description),
                new KeyValuePair<string, string>("UserId", UserId),
                new KeyValuePair<string, string>("ProdId", ProdId),
                new KeyValuePair<string, string>("ProjImage", s)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Project/2", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


        public async Task CreateProjectWithoutImage(string UserId, string ProdId, string Name,
            string Description)
        {
            FormUrlEncodedContent data;

            data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("ProjName", Name),
                new KeyValuePair<string, string>("ProjDesc", Description),
                new KeyValuePair<string, string>("UserId", UserId),
                new KeyValuePair<string, string>("ProdId", ProdId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Project/2", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> CheckIfProjectExist(string ProdId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProdId", ProdId)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Project/ValidateProject",data))
            {
                if(response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task EditProject(string ProdId,string ProjName,string ProjDesc,string Image)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProdId", ProdId),
                new KeyValuePair<string,string>("ProjName", ProjName),
                new KeyValuePair<string,string>("ProjDesc", ProjDesc),
                new KeyValuePair<string,string>("ProjImage", Image),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Project/EditProject", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        #endregion

        #region Product

        public async Task<bool> ValidateKey(string key)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProdKey",key),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> CheckIfKeyIsAvailable(string key)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProdKey",key),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product/CheckIfKeyIsAvailable", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<string> GetProductIdByKey(string key)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProdKey",key),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product/1", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<string>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<string> GetProductDescriptionById(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product/GetDescription", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<string>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<int> GetTemperatureById(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product/GetTemperature", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<int>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<int> GetHumidityById(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product/GetHumidity", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<int>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<int> GetLuminosityById(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product/GetLuminosity", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<int>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<int> GetSoilHumById(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product/GetSoilHumidity", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<int>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        #endregion

        #region Chart

        public async Task<List<TemperatureModel>> GetTemperatureList(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/ProductChart/GetChartValue", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<TemperatureModel>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<HumidityModel>> GetHumidityList(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/ProductChart/GetChartValue", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<HumidityModel>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<LuminosityModel>> GetLuminosityList(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/ProductChart/GetChartValue", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var chart = await response.Content.ReadAsAsync<List<LuminosityModel>>();
                    return chart;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<SoilHumidityModel>> GetSoilHumidityList(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProductId",ProductId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/ProductChart/GetChartValue", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<SoilHumidityModel>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        #endregion

        #region Settings

        public async Task<UserSettingsModel> GetSettings(string UserId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserId",UserId),
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/UserSettings/GetSettings", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<UserSettingsModel>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task CreateSettings(string UserId,string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserId",UserId),
                new KeyValuePair<string,string>("ProductId",ProductId)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/UserSettings/CreateSettings", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task DeleteSettings(string UserId, string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserId",UserId),
                new KeyValuePair<string,string>("ProductId",ProductId)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/UserSettings/DeleteSettings", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task SaveSensorSettingsChanges(string UserId, string ProductId, int TempMinValue, 
            int TempMaxValue, int HumMinValue, int HumMaxValue,int LumMinValue, int LumMaxValue)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserId",UserId),
                new KeyValuePair<string,string>("ProductId",ProductId),
                new KeyValuePair<string,string>("TempMinValue",TempMinValue.ToString()),
                new KeyValuePair<string,string>("TempMaxValue",TempMaxValue.ToString()),
                new KeyValuePair<string,string>("HumMinValue",HumMinValue.ToString()),
                new KeyValuePair<string,string>("HumMaxValue",HumMaxValue.ToString()),
                new KeyValuePair<string,string>("LumMinValue",LumMinValue.ToString()),
                new KeyValuePair<string,string>("LumMaxValue",LumMaxValue.ToString())

            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/UserSettings/SaveSensorChanges", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task ChangeDelayTime(string ProductId, int DelayTime)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ProdId",ProductId),
                new KeyValuePair<string,string>("DelayTime",DelayTime.ToString())
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/Product/ChangeDelayTime", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        #endregion

        #region Plants

        public async Task<List<VegetablesDataModel>> GetVegetables()
        {

            using (HttpResponseMessage response = await apiClient.GetAsync("api/VegetablesPlantData/GetValues"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<VegetablesDataModel>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<FlowersDataModel>> GetFlowers()
        {

            using (HttpResponseMessage response = await apiClient.GetAsync("api/FlowersPlantData/GetValues"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<FlowersDataModel>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task SendPlant(string ProductId,int PlantType, int PlantId)
        {
            var data = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string,string>("ProductId",ProductId),
                new KeyValuePair<string,string>("PlantType",PlantType.ToString()),
                new KeyValuePair<string,string>("PlantId",PlantId.ToString())
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/UserSettings/AddPlant", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<FlowersDataModel> GetPlantValuesById(int PlantType, int PlantId)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("PlantType",PlantType.ToString()),
                new KeyValuePair<string,string>("PlantId",PlantId.ToString())
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/UserSettings/GetPlantValuesById", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<FlowersDataModel>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<KeyValuePair<int,int>> GetSelectedPlant(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string,string>("ProductId",ProductId)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/UserSettings/GetSelectedPlant", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<KeyValuePair<int,int>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<string> GetSelectedPlantName(string ProductId)
        {
            var data = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string,string>("ProductId",ProductId)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("api/UserSettings/GetSelectedPlantName", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<string>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        #endregion
    }
}

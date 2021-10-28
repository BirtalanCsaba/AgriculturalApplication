using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgriculturalAplication.Models;
using static AgriculturalAplication.Models.MainFormModel;
using static AgriculturalAplication.Models.ProductChartModel;

namespace AgriculturalAplication.Helpers
{
    public interface IAPIHelper
    {
        Task<bool> Authenticate(string username, string password);
        Task<List<Project>> GetProjects(string UserId);
        Task DeleteProject(string UserId, string ProjName);
        Task<Guid> GetUserId(string username, string password);
        Task CreateProjectWithImage(string UserId, string ProdId, string Name, string Description, byte[] Image);
        Task CreateProjectWithoutImage(string UserId, string ProdId, string Name, string Description);
        Task<bool> ValidateKey(string key);
        Task<string> GetProductIdByKey(string key);
        Task<bool> CheckIfProjectExist(string ProdId);
        Task<string> GetProductDescriptionById(string ProductId);
        Task<int> GetTemperatureById(string ProductId);
        Task<int> GetHumidityById(string ProductId);
        Task<int> GetLuminosityById(string ProductId);
        Task<int> GetSoilHumById(string ProductId);
        Task<List<TemperatureModel>> GetTemperatureList(string ProductId);
        Task<List<HumidityModel>> GetHumidityList(string ProductId);
        Task<List<LuminosityModel>> GetLuminosityList(string ProductId);
        Task<List<SoilHumidityModel>> GetSoilHumidityList(string ProductId);
        Task EditProject(string ProdId, string ProjName, string ProjDesc, string Image);
        Task<UserSettingsModel> GetSettings(string UserId);
        Task CreateSettings(string UserId, string ProductId);
        Task DeleteSettings(string UserId, string ProductId);
        Task SaveSensorSettingsChanges(string UserId, string ProductId, int TempMinValue,
            int TempMaxValue, int HumMinValue, int HumMaxValue, int LumMinValue, int LumMaxValue);
        Task ChangeDelayTime(string ProductId, int DelayTime);
        Task<List<VegetablesDataModel>> GetVegetables();
        Task<List<FlowersDataModel>> GetFlowers();
        Task SendPlant(string ProductId, int PlantType, int PlantId);
        Task<KeyValuePair<int, int>> GetSelectedPlant(string ProductId);
        Task<FlowersDataModel> GetPlantValuesById(int PlantType, int PlantId);
        Task<Guid> CreateUser(string username, string email, string password);
        Task<bool> CheckIfKeyIsAvailable(string key);
        Task<string> GetSelectedPlantName(string ProductId);
        Task<string> GetUserEmail(string UserId);
        Task UpdateUsername(string UserId, string Username);
        Task UpdateEmail(string UserId, string Email);
        Task UpdatePassword(string UserId, string Password);
    }
}
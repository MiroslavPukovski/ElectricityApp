using ElectricityApp.Classes;
using ElectricityApp.Classes.api;
using ElectricityApp.Services;
using ElectricityApp.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectricityApp.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IDataAgregationService _dataAgregationService;

        public ApiController(IDataAgregationService dataAgregationService)
        {
            _dataAgregationService = dataAgregationService;
        }


        [HttpGet("api/GetConsumedAndProduced")]
        public async Task<Responce<string>> GetConsumedAndProduced() 
        {
            var result = await _dataAgregationService.DifferenceConsumedAndProduced();

            if (result.success)
            {
                return Responce<string>.Success(result.Content);
            }

            return Responce<string>.Failure(result.Content, 504, result.error);

        }

        [HttpGet("api/AllAgregatedData")]
        public async Task<Responce<List<ElectricityDto>>> GetAllAgregatedData()
        {
            var result = await _dataAgregationService.GetAllAgregatedData();

            if (result.success)
            {
                return Responce<List<ElectricityDto>>.Success(result.Content);
            }

            return Responce<List<ElectricityDto>>.Failure(result.Content, 504, result.error);
        }


        [HttpGet("api/AgregatedData/{pageNr}/{pageSize}")]
        public async Task<Responce<List<ElectricityDto>>> GetAgregatedDataByPage(int pageNr = 1, int pageSize = 20)
        {
            var result = await _dataAgregationService.GetAgregatedDataByPage(pageNr, pageSize);

            if (result.success)
            {
                return Responce<List<ElectricityDto>>.Success(result.Content);
            }

            return Responce<List<ElectricityDto>>.Failure(result.Content, 504, result.error);
        }
    }
}

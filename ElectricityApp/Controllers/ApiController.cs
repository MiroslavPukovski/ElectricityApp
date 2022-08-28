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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ladders.Controllers
{
    [Route("api/booking")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IApiClient apiClient;

        public BookingController(IApiClient client)
        {
            apiClient = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var someData = await apiClient.GetAsync("https://aberfitness.biz/some_service/whatever_api_youre_calling");
            // do something with someData
            return Ok();
        }

        [HttpGet("getSportsByVenue/{id}")]
        public async Task<IActionResult> GetSport([FromRoute] int id)
        {
            var sportsData = await apiClient.GetAsync("https://docker2.aberfitness.biz/booking-facilities/api/sports/getSportsByVenue/"+ id);
            if (!sportsData.IsSuccessStatusCode)
            {
                return (NoContent());
            }

            var info = await sportsData.Content.ReadAsStringAsync();
            var sports = JsonConvert.DeserializeObject<ICollection<Sport>>(info);

            return (Ok(sports));
        }
}
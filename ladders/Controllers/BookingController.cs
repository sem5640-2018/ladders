using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
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

        [HttpGet("{date}/{venueId}/{sportId}")]
        public async Task<IActionResult> GetTimes([FromRoute] DateTime date, [FromRoute] int venueId, [FromRoute] int sportId)
        {
            var dateToUse = date.ToString("yyyy-MM-dd");
            var timeData = await apiClient.GetAsync($"https://docker2.aberfitness.biz/booking-facilities/api/booking/{dateToUse}/{venueId}/{sportId}");

            if (!timeData.IsSuccessStatusCode)
            {
                return (NoContent());
            }

            var info = await timeData.Content.ReadAsStringAsync();
            var sports = JsonConvert.DeserializeObject<ICollection<DateTime>>(info);

            return (Ok(sports));
        }

        [HttpGet("getSportsByVenue/{id}")]
        public async Task<IActionResult> GetSport([FromRoute] int id)
        {
            var sportsData =
                await apiClient.GetAsync(
                    $"https://docker2.aberfitness.biz/booking-facilities/api/sports/getSportsByVenue/{id}");
            if (!sportsData.IsSuccessStatusCode)
            {
                return (NoContent());
            }

            var info = await sportsData.Content.ReadAsStringAsync();
            var sports = JsonConvert.DeserializeObject<ICollection<Sport>>(info);

            return (Ok(sports));
        }
    }
}
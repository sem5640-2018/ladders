using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ladders.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ladders.Shared
{
    public static class Helpers
    {
        public static bool AmIAdmin(ClaimsPrincipal user)
        {
            return user.HasClaim("user_type", "administrator") || user.HasClaim("user_type", "coordinator");
        }

        public static bool DoIHaveAnAccount(ClaimsPrincipal user, LaddersContext context)
        {
            var userId = GetMyName(user);

            return context.ProfileModel.Any(e => e.UserId == userId);
        }

        public static string GetMyName(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }

        public static async Task<ProfileModel> GetMe(ClaimsPrincipal user, LaddersContext context)
        {
            var name = GetMyName(user);
            return await context.ProfileModel.Include(a => a.CurrentRanking).ThenInclude(lad => lad.LadderModel).FirstOrDefaultAsync(e => e.UserId == name);
        }

        public static async Task<bool> EmailUser(IApiClient client, string UserId, string Subject, string Content)
        {
            var result = await client.PostAsync("https://docker2.aberfitness.biz/comms/api/Email/ToUser",
                new {Subject, Content, UserId});

            return result.IsSuccessStatusCode;
        }

        public static async Task<IEnumerable<Venue>> GetVenues(IApiClient apiClient)
        {
            var venueData = await apiClient.GetAsync("https://docker2.aberfitness.biz/booking-facilities/api/sports");

            if (!venueData.IsSuccessStatusCode) return null;

            var info = await venueData.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Venue>>(info);
        }

        public static async Task<IEnumerable<Sport>> GetSports(IApiClient apiClient)
        {
            var sportData = await apiClient.GetAsync("https://docker2.aberfitness.biz/booking-facilities/api/sports");

            if (!sportData.IsSuccessStatusCode) return null;

            var info = await sportData.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Sport>>(info);
        }
    }
}

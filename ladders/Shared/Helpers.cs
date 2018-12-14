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

        public static async Task<bool> EmailUser(string commsBaseUrl, IApiClient client, string UserId, string Subject, string Content)
        {
            var result = await client.PostAsync($"{commsBaseUrl}api/Email/ToUser",
                new {Subject, Content, UserId});

            return result.IsSuccessStatusCode;
        }

        public static async Task<IEnumerable<Venue>> GetVenues(string bookingBaseUrl, IApiClient apiClient)
        {
            var venueData = await apiClient.GetAsync($"{bookingBaseUrl}api/sports");

            if (!venueData.IsSuccessStatusCode) return null;

            var info = await venueData.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Venue>>(info);
        }

        public static async Task<IEnumerable<Sport>> GetSports(string bookingBaseUrl, IApiClient apiClient)
        {
            var sportData = await apiClient.GetAsync($"{bookingBaseUrl}api/sports");

            if (!sportData.IsSuccessStatusCode) return null;

            var info = await sportData.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Sport>>(info);
        }

        public static async Task<bool> FreeUpVenue(string bookingBaseUrl, IApiClient apiClient, int roomId)
        {
            var result =
                await apiClient.DeleteAsync($"{bookingBaseUrl}api/booking/" + roomId);

            return result.IsSuccessStatusCode;
        }

        public static bool IsUserInActiveChallenge(IEnumerable<Challenge> model, ProfileModel user)
        {
            bool Check(Challenge challenge)
            {
                if (challenge.ChallengeeId != user.Id && challenge.ChallengerId != user.Id)
                    return false;

                return challenge.Resolved;
            }

            return model.Where(Check).Any();
        }

        public static bool Check(Challenge challenge, ProfileModel user)
        {
            if (challenge.ChallengeeId != user.Id && challenge.ChallengerId != user.Id)
                return false;

            return challenge.Resolved;
        }
    }
}

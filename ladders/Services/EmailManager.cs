using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositories.Interfaces;
using ladders.Shared;
using Microsoft.Extensions.Configuration;

namespace ladders.Services
{
    public class EmailManager
    {
        private readonly IApiClient _apiClient;
        private readonly IConfigurationSection _appConfig;
        private readonly ILaddersRepository _laddersRepository;

        public EmailManager(IApiClient client, IConfiguration config, 
            ILaddersRepository laddersRepository)
        {
            _apiClient = client;
            _appConfig = config.GetSection("ladders");
            _laddersRepository = laddersRepository;
        }
        
        public async Task SendScheduledEmails()
        {
            await SendToAllLadders();
        }

        public async Task SendToAllLadders()
        {
            var allLadders = await _laddersRepository.GetAllAsyncIncludes();

            foreach (var ladders in allLadders)
            {
                await SendAllUsersEmail(ladders);
            }
        }

        private async Task SendAllUsersEmail(LadderModel ladderModel)
        {
            var ladderEmail = GetEmailForLadder(ladderModel);

            foreach (var ranking in ladderModel.CurrentRankings)
            {
                await Helpers.EmailUser(_appConfig.GetValue<string>("CommsUrl"),
                    _apiClient, ranking.User.UserId, "Weekly Update", ladderEmail);
            }
        }

        private string GetEmailForLadder(LadderModel ladderModel)
        {
            var emailContent = new StringBuilder();

            emailContent.AppendLine("Hi User, <br/>");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("    This week there have been many different challenges. <br/>");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("<br/>");

            foreach (var ranking in ladderModel.CurrentRankings.OrderBy(a => a.Position))
            {
                emailContent.AppendLine($"User {ranking.User.Name} is in position {ranking.Position} <br/>");
            }
            
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("Aberfitness.biz");

            return emailContent.ToString();
        }

        public static string GetNewChallengeEmail(Challenge challenge, bool forChallenger)
        {
            var emailContent = new StringBuilder();

            emailContent.AppendLine($"Hi User, <br/>");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("<br/>");
            
            emailContent.AppendLine(forChallenger
                ? $"    You have challenged {challenge.Challengee.Name} <br/>"
                : $"    You have been challenged by {challenge.Challenger.Name} <br/>");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("<br/>");

            emailContent.AppendLine($"Venue: {challenge.Booking.facility.venue.venueName} <br/>");
            emailContent.AppendLine($"Sport: {challenge.Booking.facility.sport.sportName} <br/>");
            emailContent.AppendLine($"DateTime: {challenge.ChallengedTime.ToString(CultureInfo.CurrentCulture)} <br/>");
            
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("<br/>");
            emailContent.AppendLine("Aberfitness.biz");

            return emailContent.ToString();
        }
    }
}
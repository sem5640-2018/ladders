using System;
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

            emailContent.AppendLine("Hi User,");
            emailContent.AppendLine();
            emailContent.AppendLine();
            emailContent.AppendLine("This week there have been many different challenges.");
            emailContent.AppendLine();
            emailContent.AppendLine();

            foreach (var ranking in ladderModel.CurrentRankings.OrderBy(a => a.Position))
            {
                emailContent.AppendLine($"User {ranking.User.Name} is in position {ranking.Position}");
            }
            
            emailContent.AppendLine();
            emailContent.AppendLine();
            emailContent.AppendLine("Aberfitness.biz");

            return emailContent.ToString();
        }
    }
}
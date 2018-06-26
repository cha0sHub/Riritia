using Riritia.Core;
using Riritia.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riritia.Interfaces.Model;
using System;

namespace Riritia.Purposes.Keyword
{
    public class KeywordPurpose : IPurpose
    {
        public static string KeywordName = "Keyword";

        public string Name => KeywordName;

        public bool Active { get; set; }

        public IHumanInterface HumanInterface { get; }

        public IOvermindAccessor OvermindAccessor { get; }

        private IDictionary<string, DateTime> LastTimeKeywordUsed { get; }

        public KeywordPurpose(IHumanInterface humanInterface, IOvermindAccessor overmindAccessor)
        {
            HumanInterface = humanInterface;
            OvermindAccessor = overmindAccessor;
            LastTimeKeywordUsed = new Dictionary<string, DateTime>();
        }


        public async Task<IPurposeFullfillment> WorkAsync(IContext context, IKalos message)
        {
            var purposeFullfillment = new PurposeFullfillment();

            if (!context.AddressedToSelf)
            {
                purposeFullfillment.Weight = -1;
                return purposeFullfillment;
            }

            var matchedWords = new List<IKeyword>();
            await Task.Run(() =>
            {
                foreach (var keyword in OvermindAccessor.GetAllKeywords())
                {
                    if (message.Msg.Contains(keyword.Word))
                    {
                        var previousMatch = matchedWords.FirstOrDefault();
                        if (previousMatch == null)
                        {
                            matchedWords.Add(keyword);
                        }
                        else if (previousMatch.Word.Length < keyword.Word.Length)
                        {
                            matchedWords.Remove(previousMatch);
                            matchedWords.Add(keyword);
                        }
                    }
                }
            });
            if (matchedWords.Count == 0)
            {
                purposeFullfillment.Weight = -1;
                return purposeFullfillment;
            }

            if (!LastTimeKeywordUsed.ContainsKey(matchedWords[0].Word))
                LastTimeKeywordUsed.Add(matchedWords[0].Word, DateTime.Now);
            else
            {
                if (LastTimeKeywordUsed[matchedWords[0].Word] > DateTime.Now.AddMinutes(-1))
                {
                    purposeFullfillment.Weight = -1;
                    return purposeFullfillment;
                }
                LastTimeKeywordUsed[matchedWords[0].Word] = DateTime.Now;
            }
            purposeFullfillment.Weight = 100;
            var responses = matchedWords[0].Response.Split('\n');
            foreach (var response in responses)
            {
                var outMessage = MessageCreator.CreateOutgoingMessage(message.Origin, HumanInterface.Name, response);
                purposeFullfillment.AddMessage(outMessage);
            }
            return purposeFullfillment;
        }
    }
}

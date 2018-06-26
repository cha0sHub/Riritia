using Riritia.Core;
using Riritia.Interfaces;
using Riritia.Interfaces.Model;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riritia.Purposes.WhatIs
{
    public class WhatIsPurpose : IPurpose
    {
        public static string PurposeName = "WhatIs";


        public string Name => PurposeName;

        public bool Active { get; set; }

        private IHumanInterface HumanInterface { get; }
        private IOvermindAccessor OvermindAccessor { get; }

        public WhatIsPurpose(IHumanInterface humanInterface, IOvermindAccessor overmindAccessor)
        {
            HumanInterface = humanInterface;
            OvermindAccessor = overmindAccessor;
        }


        public async Task<IPurposeFullfillment> WorkAsync(IContext context, IKalos message)
        {
            var purposeFullfillment = new PurposeFullfillment();
            var messageLower = message.Msg.ToLowerInvariant();

            var obj = ExtractObject(messageLower);
            if (string.IsNullOrEmpty(obj))
            {
                var subject = ExtractIndependentSubject(messageLower);
                if (string.IsNullOrEmpty(subject) || subject.Length < 2)
                {
                    purposeFullfillment.Weight = -1;
                    return purposeFullfillment;
                }
                IList<IWhatIs> results = null;
                await Task.Run(()=>
                {
                    results = OvermindAccessor.GetMatchingWhatIsAnswers(subject, string.Empty, "definition");
                });
                if (results.Count == 0)
                {
                    purposeFullfillment.Weight = -1;
                    return purposeFullfillment;
                }
                var response = CreateResponse(message, results);
                purposeFullfillment.AddMessage(response);
                purposeFullfillment.Weight = 1000001;
                return purposeFullfillment;
            }
            else
            {
                var subject = ExtractDependentSubject(messageLower);
                if (string.IsNullOrEmpty(subject))
                {
                    purposeFullfillment.Weight = -1;
                    return purposeFullfillment;
                }
                IList<IWhatIs> results = null;
                await Task.Run(() =>
                {
                    results = OvermindAccessor.GetMatchingWhatIsAnswers(subject, obj, "attribute");
                });
                if (results.Count == 0)
                {
                    purposeFullfillment.Weight = -1;
                    return purposeFullfillment;
                }
                var response = CreateResponse(message, results);
                purposeFullfillment.AddMessage(response);
                purposeFullfillment.Weight = 1000001;
                return purposeFullfillment;
            }
        }

        private IKalos CreateResponse(IKalos originalMessage, IList<IWhatIs> results)
        {
            var responseText = $"{originalMessage.Sender}, ";
            var builder = new StringBuilder();
            builder.Append(responseText);
            for (int i = 0; i < results.Count; i++)
            {
                var currentResult = results[i];
                if (i != 0)
                    builder.Append(", ");
                if (i == results.Count - 1 && i != 0)
                    builder.Append("and ");
                if (!string.IsNullOrEmpty(currentResult.Object))
                {
                    builder.Append($"{currentResult.Subject}'s {currentResult.Object} is {currentResult.Answer}");
                }
                else
                {
                    builder.Append($"{currentResult.Subject} is {currentResult.Answer}");
                }
            }
            builder.Append(".");
            responseText = builder.ToString();

            var outMessage = MessageCreator.CreateOutgoingMessage(originalMessage.Origin, HumanInterface.Name, responseText);
            return outMessage;
        }

        internal static string ExtractIndependentSubject(string message)
        {
            var currentStringStart = "what is ";
            var currentStringEnd = "?";
            if (message.Contains(currentStringStart) && message.Contains(currentStringEnd))
            {
                var startIndex = message.IndexOf(currentStringStart);
                var endIndex = message.IndexOf(currentStringEnd);
                var subjectStart = startIndex + currentStringStart.Length;
                var subjectLength = endIndex - subjectStart;
                var subject = message.Substring(subjectStart, subjectLength);
                return subject;
            }
            return string.Empty;
        }

        internal static string ExtractDependentSubject(string message)
        {
            var currentStringStart = "what is ";
            var currentStringEnd = "'s";
            if (message.Contains(currentStringStart) && message.Contains(currentStringEnd))
            {
                var startIndex = message.IndexOf(currentStringStart);
                var endIndex = message.IndexOf(currentStringEnd);
                var subjectStart = startIndex + currentStringStart.Length;
                var subjectLength = endIndex - subjectStart;
                var subject = message.Substring(subjectStart, subjectLength);
                return subject;
            }
            currentStringStart = " of ";
            currentStringEnd = ".";
            if (message.Contains(currentStringStart) && message.Contains(currentStringEnd))
            {
                var startIndex = message.IndexOf(currentStringStart);
                var endIndex = message.IndexOf(currentStringEnd);
                var subjectStart = startIndex + currentStringStart.Length;
                var subjectLength = endIndex - subjectStart;
                var subject = message.Substring(subjectStart, subjectLength);
                return subject;
            }
            return string.Empty;
        }

        internal static string ExtractObject(string message)
        {
            var currentStringStart = "what is the ";
            var currentStringEnd = " of ";
            if (message.Contains(currentStringStart) && message.Contains(currentStringEnd))
            {
                var startIndex = message.IndexOf(currentStringStart);
                var endIndex = message.IndexOf(currentStringEnd);
                var objectStart = startIndex + currentStringStart.Length;
                var objectLength = endIndex - objectStart;
                var obj = message.Substring(objectStart, objectLength);
                return obj;
            }
            currentStringStart = "'s ";
            currentStringEnd = "?";
            if (message.Contains(currentStringStart) && message.Contains(currentStringEnd))
            {
                var startIndex = message.IndexOf(currentStringStart);
                var endIndex = message.IndexOf(currentStringEnd);
                var objectStart = startIndex + currentStringStart.Length;
                var objectLength = endIndex - objectStart;
                var obj = message.Substring(objectStart, objectLength);
                return obj;
            }
            return string.Empty;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Riritia.Interfaces;
using Microsoft.Extensions.Configuration;
using Riritia.Interfaces.Model;
using Riritia.IdseConnection.Model;
using System;

namespace Riritia.IdseConnection
{
    public class IdseDataContext : DbContext, IOvermindAccessor
    {

        private DbSet<Keyword> Keyword { get; set; }
        private DbSet<WhatIs> WhatIs { get; set; }

        private IConfiguration Configuration { get; }

        public IdseDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseMySql(Configuration[SettingNames.ConnectionString]);

        public IList<IKeyword> GetMatchingKeywords(string word)
        {
            return Keyword.Where(keyword => keyword.Word.Equals(word)).ToList<IKeyword>();
        }
        public IList<IWhatIs> GetMatchingWhatIsAnswers(string subject, string obj, string relation)
        {
            var allWhatIs = WhatIs.ToList<IWhatIs>();
            var singularResults = WhatIs.Where(
                whatIs => whatIs.Subject.ToLowerInvariant().Equals(subject.ToLowerInvariant())
                && whatIs.Object.ToLowerInvariant().Equals(obj.ToLowerInvariant())
                && whatIs.Relation.ToLowerInvariant().Equals(relation.ToLowerInvariant()))
                .ToList<IWhatIs>();
            if (singularResults.Count == 1)
                return singularResults;
            return WhatIs.Where(
                whatIs => whatIs.Subject.ToLowerInvariant().Contains(subject.ToLowerInvariant()) 
                && whatIs.Object.ToLowerInvariant().Equals(obj.ToLowerInvariant()) 
                && whatIs.Relation.ToLowerInvariant().Equals(relation.ToLowerInvariant()))
                .ToList<IWhatIs>();
        }
        public IList<IWhatIs> GetMatchingWhatIsSubjects(string obj, string relation, string answer)
        {
            return WhatIs.Where(
                whatIs => whatIs.Answer.Equals(answer,StringComparison.InvariantCultureIgnoreCase)
                && whatIs.Object.Equals(obj, StringComparison.InvariantCultureIgnoreCase)
                && whatIs.Relation.Equals(relation, StringComparison.InvariantCultureIgnoreCase))
                .ToList<IWhatIs>();
        }

        public IList<IKeyword> GetAllKeywords()
        {
            return Keyword.ToList<IKeyword>();
        }
    }
}

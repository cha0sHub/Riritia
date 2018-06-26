using Riritia.Interfaces.Model;
using System.ComponentModel.DataAnnotations;

namespace Riritia.IdseConnection.Model
{
    internal class WhatIs : IWhatIs
    {
        [Key]
        public int Id { get; set; }

        public string Context { get; set; }

        public string Subject { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Object { get; set; }

        public string Relation { get; set; }

        public string Answer { get; set; }

        public WhatIs()
        {
            Object = string.Empty;
        }
    }
}

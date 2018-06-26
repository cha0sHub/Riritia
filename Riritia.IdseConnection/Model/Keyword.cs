using Riritia.Interfaces.Model;
using System.ComponentModel.DataAnnotations;

namespace Riritia.IdseConnection.Model
{
    internal class Keyword : IKeyword
    {
        [Key]
        public int Id { get; set; }
        public string Response { get; set; }

        public string Word { get; set; }
    }
}

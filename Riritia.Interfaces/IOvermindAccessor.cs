using Riritia.Interfaces.Model;
using System.Collections.Generic;

namespace Riritia.Interfaces
{
    public interface IOvermindAccessor
    {
        IList<IKeyword> GetAllKeywords();
        IList<IKeyword> GetMatchingKeywords(string word);
        IList<IWhatIs> GetMatchingWhatIsAnswers(string subject, string obj, string relation);
        IList<IWhatIs> GetMatchingWhatIsSubjects(string obj, string relation, string answer);
    }
}

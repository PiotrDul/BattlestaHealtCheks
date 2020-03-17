using BattlestaHealthChecks.Models;
using BattlestaHealthChecks.Services;
using BattlestaHealthChecks.ViewModels;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Interfeces.Services
{
    public interface ISampleService
    {
        // Task<IEnumerable<SampleViewModel>> GetSamples(int page);
        Task<PagingList<Sample>> GetSamples(int page);

        Task<SampleViewModel> GetSample(int id);
    }
}

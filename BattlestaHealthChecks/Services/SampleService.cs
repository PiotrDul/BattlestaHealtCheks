using AutoMapper;
using BattlestaHealthChecks.Context;
using BattlestaHealthChecks.Interfeces.Services;
using BattlestaHealthChecks.Models;
using BattlestaHealthChecks.ViewModels;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Services
{
    public class SampleService : ISampleService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public SampleService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagingList<Sample>> GetSamples(int page)
        {
            //var samples = await _context.Sample
            //    .AsNoTracking()
            //    .OrderByDescending(s => s.Date)
            //    .Skip((page - 1) * 12)
            //    .Take(12)
            //    .ToListAsync();
            var item = _context.Sample.AsNoTracking().OrderByDescending(s => s.Date);
            //var result = _mapper.Map<List<SampleViewModel>>(item); 
            var model = await PagingList<Sample>.CreateAsync(item, 12,page);
            //var result = _mapper.Map<List<SampleViewModel>>(samples);
            return model;
        }

        public async Task<SampleViewModel> GetSample(int id)
        {
            var sample = await _context.Sample
                .AsNoTracking()
                .AsQueryable()
                .Where(t => t.Id == id)
                .Include(s => s.Elements)
                .FirstOrDefaultAsync();

            var result = _mapper.Map<SampleViewModel>(sample);
            foreach (var element in result.Elements)
            {
                element.LoadTime = element.LoadTime / 1000;
            }
            return result;
        }
    }
}

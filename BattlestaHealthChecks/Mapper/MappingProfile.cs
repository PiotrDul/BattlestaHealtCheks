using AutoMapper;
using BattlestaHealthChecks.Models;
using BattlestaHealthChecks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Sample, SampleViewModel>().ReverseMap();
            CreateMap<CheckedElement, CheckedElementViewModel>().ReverseMap();

        }
    }
}

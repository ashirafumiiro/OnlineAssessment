using AutoMapper;
using DataAccess.Enities;
using WebPortal.Areas.Admin.Models;

namespace WebPortal.Extensions
{
    public static class Extensions
    {
        public static Institution ToEntity(this InstitutionViewModel value)
        {
            var target = new Institution();
            var config = new MapperConfiguration(cfg => 
            { 
                cfg.CreateMap<InstitutionViewModel, Institution>()
                .ForMember(p => p.Address, y => y.Ignore());
            });
            var mapper = config.CreateMapper();
            target = mapper.Map<Institution>(value);
            target.Address = new DataAccess.Enities.Address()
            {
                Address1 = value.Address.Address1,
                Address2 = value.Address.Address2,
                Address3 = value.Address.Address3,
                Country = value.Address.Country
            };
            if(value.Address.Id != null) target.Address.Id = value.Address.Id.Value;
            return target;
        }

        public static InstitutionViewModel ToViewModel(this Institution value)
        {
            var target = new InstitutionViewModel();
            var config = new MapperConfiguration(cfg => 
            { cfg.CreateMap<Institution, InstitutionViewModel>()
                .ForMember(p => p.Address, y => y.Ignore()); 
            });
            var mapper = config.CreateMapper();
            target = mapper.Map<InstitutionViewModel>(value);
            target.Address = new Areas.Admin.Models.Address()
            {
                Id = value.AddressId,// value.Address.Id,
                Address1 = value.Address?.Address1 ?? string.Empty,
                Address2 = value.Address?.Address2 ?? string.Empty,
                Address3 = value.Address?.Address3 ?? string.Empty,
                Country = value.Address?.Country ?? string.Empty
            };
            return target;
        }
    }
}

using AutoMapper;
using miniapp.EntityFrameworkCore.Entities;
using miniapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miniapp.EntityFrameworkCore.Context
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<AppUser, SignUpViewModel>().ReverseMap();
            CreateMap<AppUser, AppUserViewModel>().ReverseMap();
            CreateMap<BaseEntity, EntityBaseViewModel>().ReverseMap();
            CreateMap<ToDo, ToDoViewModel>().ReverseMap();
            CreateMap<Menu, MenuViewModel>().ReverseMap();
            //.ForMember(o=> o.PrimaryId, ex => ex.MapFrom(o => o.Id));  // If need to Map for property name change

            

        }
    }
}

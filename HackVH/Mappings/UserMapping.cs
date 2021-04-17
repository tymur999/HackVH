using AutoMapper;
using HackVH.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HackVH.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<IdentityUser, UserDto>();
        }
    }
}
using AutoMapper;
using ModelLayer.Dto;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.BLL.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User,UserResponseDto>().ReverseMap();
            CreateMap<UserRequestDto, User>();
           
        }
    }
}

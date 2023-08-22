using APIWebAspNet_config.Models;
using APIWebAspNet_config.Models.DTOs;
using AutoMapper;


namespace APIWebAspNet_config
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponDTO>().ReverseMap();
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponUpdateDTO>().ReverseMap();
        }
    }
}


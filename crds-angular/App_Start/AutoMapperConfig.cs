using crds_angular.Models;
using crds_angular.Models.Crossroads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            //AutoMapper.Mapper.CreateMap<Dictionary<string,object>, Person>();
            AutoMapper.Mapper.CreateMap<Dictionary<string, object>, AccountInfo>()
                .ForMember(dest => dest.EmailNotifications,
                          opts => opts.MapFrom(src => src["Bulk_Email_Opt_Out"]))
                .ForMember(dest => dest.ContactId,
                           opts => opts.MapFrom(src => src["Contact_ID"]));
        }
    }

}

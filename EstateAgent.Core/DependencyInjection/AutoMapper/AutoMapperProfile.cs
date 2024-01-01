using AutoMapper;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Core.DependencyInjection.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {

            #region AboutMapping

            CreateMap<AboutListDto, AboutEntity>()
                .ReverseMap();
            CreateMap<AboutDto, AboutEntity>()
                .ReverseMap();

            #endregion


            #region AboutMediaMapping

            CreateMap<AboutMediaListDto, AboutMediaEntity>()
                .ReverseMap();
            CreateMap<AboutMediaDto, AboutMediaEntity>()
                .ReverseMap();

            #endregion


            #region AgentMapping

            CreateMap<AgentListDto, AgentEntity>()
                .ReverseMap();
            CreateMap<AgentListDto, AgentListEntity>()
                            .ReverseMap();

            CreateMap<AgentDto, AgentEntity>()
                .ReverseMap();

            #endregion


            #region BlogMapping

            CreateMap<BlogListDto, BlogEntity>()
                .ReverseMap();
            CreateMap<BlogListDto, BlogListEntity>()
                            .ReverseMap();

            CreateMap<BlogDto, BlogEntity>()
                .ReverseMap();

            #endregion


            #region ContactInfoMapping

            CreateMap<ContactInfoListDto, ContactInfoEntity>()
                .ReverseMap();
            CreateMap<ContactInfoDto, ContactInfoEntity>()
                .ReverseMap();

            #endregion

            #region HomeMapping

            CreateMap<HomeListDto, HomeListEntity>()
                .ReverseMap();
            

            #endregion



            #region IdentityMapping

            CreateMap<IdentityListDto, IdentityEntity>()
                .ReverseMap();
            CreateMap<IdentityDto, IdentityEntity>()
                .ReverseMap();

            #endregion


            #region MediaMapping

            CreateMap<MediaListDto, MediaEntity>()
                .ReverseMap();
            CreateMap<MediaDto, MediaEntity>()
                .ReverseMap();

            #endregion


            #region PropertyMapping

            CreateMap<PropertyListDto, PropertyEntity>()
                .ReverseMap();
            CreateMap<PropertyListDto, PropertyListEntity>()
                            .ReverseMap();

            CreateMap<PropertyDto, PropertyEntity>()
                .ReverseMap();

            #endregion


            #region PropertyMediaMapping

            CreateMap<PropertyMediaListDto, PropertyMediaEntity>()
                .ReverseMap();
            CreateMap<PropertyMediaDto, PropertyMediaEntity>()
                .ReverseMap();

            #endregion

            #region RoleMethodMapping

            CreateMap<RoleMethodListDto, RoleMethodEntity>()
                .ReverseMap();
            CreateMap<RoleMethodDto, RoleMethodEntity>()
                .ReverseMap();

            #endregion




            #region
            CreateMap<SessionListDto, SessionListEntity>()
                .ReverseMap();
            CreateMap<SessionDto, SessionEntity>()
                .ReverseMap();
            #endregion

            #region SubscribeMapping

            CreateMap<SubscribeListDto, SubscribeEntity>()
                .ReverseMap();
            CreateMap<SubscribeDto, SubscribeEntity>()
                .ReverseMap();

            #endregion


            #region SystemSettingsMapping

            CreateMap<SystemSettingsListDto, SystemSettingsEntity>()
                .ReverseMap();
            CreateMap<SystemSettingsDto, SystemSettingsEntity>()
                .ReverseMap();

            #endregion


            #region UserMapping

            CreateMap<UserListDto, UserEntity>()
                .ReverseMap();
            CreateMap<UserDto, UserEntity>()
                .ReverseMap();

            #endregion


            #region UserRoleMapping

            CreateMap<UserRoleListDto, UserRoleEntity>()
                .ReverseMap();
            CreateMap<UserRoleListDto, UserRoleListEntity>()
                            .ReverseMap();

            CreateMap<UserRoleDto, UserRoleEntity>()
                .ReverseMap();

            #endregion


        }
    }
}

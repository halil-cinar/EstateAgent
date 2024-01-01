using EstateAgent.Core.DependencyInjection.AutoMapper;
using AutoMapper;
using Microsoft.AspNetCore.Http.Features;

using System.Data.Entity;
using System.Reflection;
using EstateAgent.DataAccess;
using EstateAgent.Entities;
using EstateAgent.Core.DataAccess.EntityFramework;
using EstateAgent.DataAccess.EntityFramework;
using EstateAgent.Entities.Validators;
using EstateAgent.Business.Abstract;
using EstateAgent.Business;
using NToastNotify;

namespace EstateAgent.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.AddHttpContextAccessor();

            //Repositories
            builder.Services.AddScoped<IEntityRepository<AboutEntity>, EfGenericEntityRepositoryDal<AboutEntity>>();
            builder.Services.AddScoped<IEntityRepository<AboutMediaEntity>, EfGenericEntityRepositoryDal<AboutMediaEntity>>();
            builder.Services.AddScoped<IEntityRepository<AgentEntity>, EfGenericEntityRepositoryDal<AgentEntity>>();
            builder.Services.AddScoped<IEntityRepository<AgentListEntity>, EfGenericEntityRepositoryDal<AgentListEntity>>();
            builder.Services.AddScoped<IEntityRepository<BlogEntity>, EfGenericEntityRepositoryDal<BlogEntity>>();
            builder.Services.AddScoped<IEntityRepository<BlogListEntity>, EfGenericEntityRepositoryDal<BlogListEntity>>();
            builder.Services.AddScoped<IEntityRepository<ContactInfoEntity>, EfGenericEntityRepositoryDal<ContactInfoEntity>>();
            builder.Services.AddScoped<IEntityRepository<HomeListEntity>, EfGenericEntityRepositoryDal<HomeListEntity>>();
            builder.Services.AddScoped<IEntityRepository<IdentityEntity>, EfGenericEntityRepositoryDal<IdentityEntity>>();
            builder.Services.AddScoped<IEntityRepository<MediaEntity>, EfGenericEntityRepositoryDal<MediaEntity>>();
            builder.Services.AddScoped<IEntityRepository<PropertyEntity>, EfGenericEntityRepositoryDal<PropertyEntity>>();
            builder.Services.AddScoped<IEntityRepository<PropertyListEntity>, EfGenericEntityRepositoryDal<PropertyListEntity>>();
            builder.Services.AddScoped<IEntityRepository<PropertyMediaEntity>, EfGenericEntityRepositoryDal<PropertyMediaEntity>>();
            builder.Services.AddScoped<IEntityRepository<RoleMethodEntity>, EfGenericEntityRepositoryDal<RoleMethodEntity>>();
            builder.Services.AddScoped<IEntityRepository<SessionEntity>, EfGenericEntityRepositoryDal<SessionEntity>>();
            builder.Services.AddScoped<IEntityRepository<SessionListEntity>, EfGenericEntityRepositoryDal<SessionListEntity>>();
            builder.Services.AddScoped<IEntityRepository<SubscribeEntity>, EfGenericEntityRepositoryDal<SubscribeEntity>>();
            builder.Services.AddScoped<IEntityRepository<SystemSettingsEntity>, EfGenericEntityRepositoryDal<SystemSettingsEntity>>();
            builder.Services.AddScoped<IEntityRepository<UserEntity>, EfGenericEntityRepositoryDal<UserEntity>>();
            builder.Services.AddScoped<IEntityRepository<UserRoleEntity>, EfGenericEntityRepositoryDal<UserRoleEntity>>();
            builder.Services.AddScoped<IEntityRepository<UserRoleListEntity>, EfGenericEntityRepositoryDal<UserRoleListEntity>>();



            //Validators

            builder.Services.AddScoped<BaseEntityValidator<AboutEntity>, AboutValidator>();
            builder.Services.AddScoped<BaseEntityValidator<AboutMediaEntity>, AboutMediaValidator>();
            builder.Services.AddScoped<BaseEntityValidator<AgentEntity>, AgentValidator>();
            builder.Services.AddScoped<BaseEntityValidator<BlogEntity>, BlogValidator>();
            builder.Services.AddScoped<BaseEntityValidator<ContactInfoEntity>, ContactInfoValidator>();
            builder.Services.AddScoped<BaseEntityValidator<HomeListEntity>, HomeValidator>();
            builder.Services.AddScoped<BaseEntityValidator<IdentityEntity>, IdentityValidator>();
            builder.Services.AddScoped<BaseEntityValidator<MediaEntity>, MediaValidator>();
            builder.Services.AddScoped<BaseEntityValidator<PropertyEntity>, PropertyValidator>();
            builder.Services.AddScoped<BaseEntityValidator<PropertyMediaEntity>, PropertyMediaValidator>();
            builder.Services.AddScoped<BaseEntityValidator<RoleMethodEntity>, RoleMethodValidator>();
            builder.Services.AddScoped<BaseEntityValidator<SessionEntity>, SessionValidator>();
            builder.Services.AddScoped<BaseEntityValidator<SubscribeEntity>, SubscribeValidator>();
            builder.Services.AddScoped<BaseEntityValidator<SystemSettingsEntity>, SystemSettingsValidator>();
            builder.Services.AddScoped<BaseEntityValidator<UserEntity>, UserValidator>();
            builder.Services.AddScoped<BaseEntityValidator<UserRoleEntity>, UserRoleValidator>();

            //Managers

            builder.Services.AddScoped<IAboutService, AboutManager>();
            builder.Services.AddScoped<IAboutMediaService, AboutMediaManager>();
            builder.Services.AddScoped<IAccountService, AccountManager>();
            builder.Services.AddScoped<IAgentService, AgentManager>();
            builder.Services.AddScoped<IBlogService, BlogManager>();
            builder.Services.AddScoped<IContactInfoService, ContactInfoManager>();
            builder.Services.AddScoped<IHomeService, HomeManager>();
            builder.Services.AddScoped<IIdentityService, IdentityManager>();
            builder.Services.AddScoped<IMediaService, MediaManager>();
            builder.Services.AddScoped<IMessageService, MessageManager>();
            builder.Services.AddScoped<IPropertyService, PropertyManager>();
            builder.Services.AddScoped<IPropertyMediaService, PropertyMediaManager>();
            builder.Services.AddScoped<IRoleMethodService, RoleMethodManager>();
            builder.Services.AddScoped<ISessionService, SessionManager>();
            builder.Services.AddScoped<ISubscribeService, SubscribeManager>();
            builder.Services.AddScoped<ISystemSettingService, SystemSettingsManager>();
            builder.Services.AddScoped<IUserService, UserManager>();
            builder.Services.AddScoped<IUserRoleService, UserRoleManager>();


            //Toast
            builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
            {
                ProgressBar = true,
                Timeout = 5000
            });

            //Form file size changed
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;

            });
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = long.MaxValue;

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseNToastNotify();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
        
    }
}
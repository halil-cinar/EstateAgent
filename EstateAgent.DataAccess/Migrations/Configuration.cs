namespace EstateAgent.DataAccess.Migrations
{
    using EstateAgent.Core.ExtensionsMethods;
    using EstateAgent.Entities;
    using EstateAgent.Entities.Enums;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EstateAgent.DataAccess.EntityFramework.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EstateAgent.DataAccess.EntityFramework.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.




            if (context.Users.Any())
            {
                return;
            }

            var user = new UserEntity
            {
                BirthDate = DateTime.Now,
                Email = "",
                IsDeletable = false,
                IsDeleted = false,
                Name = "Admin",
                Surname = "",
                PhoneNumber = ""
            };
            context.Users.Add(user);

            var userRole = new UserRoleEntity
            {
                IsDeletable = false,
                IsDeleted = false,
                Role = Entities.Enums.RoleTypes.Admin,
                UserId = user.Id,
            };
            context.UserRoles.Add(userRole);
            var salt = ExtensionsMethods.GenerateRandomString(64);

            var identity = new IdentityEntity
            {
                UserId = user.Id,
                IsDeleted = false,
                IsDeletable = true,
                IsValid = true,
                UserName = "Admin",
                PasswordSalt = salt,
                PasswordHash = ExtensionsMethods.CalculateMD5Hash(salt + "As12345+" + salt)
            };

            context.Identities.Add(identity);



            var views = new string[]
            {
                "USE [RealEstateDB]\nGO\ncreate view AgentListView as\nselect \n\t  A.Id,\n      A.UserId,\n      A.Name,\n      A.Description,\n      A.Email,\n      A.PhoneNumber,\n      A.MediaId,\n      A.IsDeleted,\n      A.IsDeletable,\n\t  U.Name as \"UserName\",\n      U.Surname as \"UserSurname\",\n      U.BirthDate as \"UserBirthDate\",\n      U.PhoneNumber as \"UserPhoneNumber\",\n      U.Email as \"UserEmail\",\n      U.ProfilePhotoId as \"UserProfilePhotoId\",\n      U.IsDeleted as \"UserIsDeleted\",\n      U.IsDeletable as \"UserIsDeletable\"\n\n\nfrom Agent A\nleft join [User] U on A.UserId=U.Id\n\n",
                "USE [RealEstateDB]\nGO\ncreate view BlogListView as\nselect \n\t  B.Id,\n      B.Title,\n      B.Description,\n      B.Content,\n      B.PostedDate,\n      B.MediaId,\n      B.UserId,\n      B.IsDeleted,\n      B.IsDeletable,\n\t  U.Name as \"UserName\",\n      U.Surname as \"UserSurname\",\n      U.BirthDate as \"UserBirthDate\",\n      U.PhoneNumber as \"UserPhoneNumber\",\n      U.Email as \"UserEmail\",\n      U.ProfilePhotoId as \"UserProfilePhotoId\",\n      U.IsDeleted as \"UserIsDeleted\",\n      U.IsDeletable as \"UserIsDeletable\"\n\nfrom Blog B\nleft join [User] U on B.UserId=U.Id",
                "USE [RealEstateDB]\nGO\ncreate view PropertyListView as\nselect \n\t  P.Id,\n      P.Title,\n      P.Price,\n      P.Address,\n      P.AgentId,\n      P.BedRoomCount,\n      P.LivingRoomCount,\n      P.ParkingCount,\n      P.KitchenCount,\n      P.Details,\n      P.LocationLatitude,\n      P.LocationLongitude,\n      P.PropertySaleStatus,\n      P.IsDeleted,\n      P.IsDeletable,\n\n      A.UserId as \"AgentUserId\",\n      A.Name as \"AgentName\",\n      A.Description as \"AgentDescription\",\n      A.Email as \"AgentEmail\",\n      A.PhoneNumber as \"AgentPhoneNumber\",\n      A.MediaId as \"AgentMediaId\",\n      A.IsDeleted as \"AgentIsDeleted\",\n      A.IsDeletable as \"AgentIsDeletable\",\n\t  U.Name as \"AgentUserName\",\n      U.Surname as \"AgentUserSurname\",\n      U.BirthDate as \"AgentUserBirthDate\",\n      U.PhoneNumber as \"AgentUserPhoneNumber\",\n      U.Email as \"AgentUserEmail\",\n      U.ProfilePhotoId as \"AgentUserProfilePhotoId\",\n      U.IsDeleted as \"AgentUserIsDeleted\",\n      U.IsDeletable as \"AgentUserIsDeletable\"\n\nfrom Property P \nleft join Agent A on P.AgentId=A.Id\nleft join [User] U on A.UserId=U.Id",
                "USE [RealEstateDB]\nGO\ncreate view UserRoleListView as\nselect \n\nUR.Id,\n      UR.UserId,\n      UR.Role,\n      UR.IsDeleted,\n      UR.IsDeletable,\n\n      U.Name as \"UserName\",\n      U.Surname as \"UserSurname\",\n      U.BirthDate as \"UserBirthDate\",\n      U.PhoneNumber as \"UserPhoneNumber\",\n      U.Email as \"UserEmail\",\n      U.ProfilePhotoId as \"UserProfilePhotoId\",\n      U.IsDeleted as \"UserIsDeleted\",\n      U.IsDeletable as \"UserIdDeletable\"\n\nfrom UserRole UR\nleft join [User] U on UR.UserId=U.Id\n",
                "USE [RealEstateDB]\nGO\ncreate view SessionListView as\nselect \n\t  S.Id, \n      S.UserId, \n      S.IdentityId, \n      S.IpAddress, \n      S.ExpiryDate, \n      S.IsActive, \n      S.CreateTime, \n      S.IsDeleted, \n      S.IsDeletable, \n\t  U.Name as \"UserName\" , \n      U.Surname as \"UserSurname\" , \n      U.BirthDate as \"UserBirthDate\" , \n      U.PhoneNumber as \"UserPhoneNumber\" , \n      U.Email as \"UserEmail\" , \n      U.ProfilePhotoId as \"UserProfilePhotoId\" , \n\t  I.UserName as \"IdentityUserName\"\nfrom Session S\nleft join IdentityEnt I on S.IdentityId=I.Id\nleft join [User] U on S.UserId=U.Id\n",
                "USE [RealEstateDB]\nGO\nCREATE view HomeListView as\nselect \ncast(1 as bigint) as \"Id\", \n(select COUNT(*) from Property where PropertySaleStatus=2 and IsDeleted=0) as \"SalePropertyCount\",\n(select COUNT(*) from Property where PropertySaleStatus=1 and IsDeleted=0) as \"RentPropertyCount\",\n(select COUNT(*) from Agent where IsDeleted=0) as \"AgentCount\",\n(select COUNT(*) from Blog where IsDeleted=0 ) as \"BlogCount\",\ncast(1 as bit) as \"IsDeleted\",\ncast(1 as bit) as \"IsDeletable\"\n\nGO",
            };

            foreach (var view in views)
            {
                context.Database.ExecuteSqlCommand(view);
            }

            var methods = Enum.GetValues(typeof(MethodTypes));
            var rolemethods = new List<RoleMethodEntity>();
            foreach (var method in methods)
            {
                rolemethods.Add(new RoleMethodEntity
                {
                    Method = (MethodTypes)method,
                    Role = RoleTypes.Admin
                });

            }
            foreach (var rolemethod in rolemethods)
            {
                context.RoleMethods.Add(rolemethod);
            }
            context.SaveChanges();


            var agentmethod = new List<MethodTypes>();
            foreach (var method in methods)
            {
                agentmethod.Add((MethodTypes)method);
            }

            //agentmethod.Remove(MethodTypes.UserGet);
            agentmethod.Remove(MethodTypes.UserGetAll);
            agentmethod.Remove(MethodTypes.UserAdd);
            //agentmethod.Remove(MethodTypes.UserGetOptionList);
            agentmethod.Remove(MethodTypes.UserLoadMoreFilter);
            agentmethod.Remove(MethodTypes.UserRemove);


            agentmethod.Remove(MethodTypes.AgentGetAll);
            agentmethod.Remove(MethodTypes.AgentAdd);
            //agentmethod.Remove(MethodTypes.AgentGetOptionList);
            agentmethod.Remove(MethodTypes.AgentLoadMoreFilter);
            agentmethod.Remove(MethodTypes.AgentRemove);

            agentmethod.Remove(MethodTypes.AboutGet);
            agentmethod.Remove(MethodTypes.AboutGetAll);
            agentmethod.Remove(MethodTypes.AboutAdd);
            //agentmethod.Remove(MethodTypes.AboutGetOptionList);
            agentmethod.Remove(MethodTypes.AboutLoadMoreFilter);
            agentmethod.Remove(MethodTypes.AboutRemove);
            agentmethod.Remove(MethodTypes.AboutUpdate);



            agentmethod.Remove(MethodTypes.AboutMediaGet);
            agentmethod.Remove(MethodTypes.AboutMediaGetAll);
            agentmethod.Remove(MethodTypes.AboutMediaAdd);
            //agentmethod.Remove(MethodTypes.AboutMediaGetOptionList);
            agentmethod.Remove(MethodTypes.AboutMediaLoadMoreFilter);
            agentmethod.Remove(MethodTypes.AboutMediaRemove);

            agentmethod.Remove(MethodTypes.ContactInfoGet);
            agentmethod.Remove(MethodTypes.ContactInfoGetAll);
            agentmethod.Remove(MethodTypes.ContactInfoAdd);
            //agentmethod.Remove(MethodTypes.ContactInfoGetOptionList);
            agentmethod.Remove(MethodTypes.ContactInfoLoadMoreFilter);
            agentmethod.Remove(MethodTypes.ContactInfoRemove);

            agentmethod.Remove(MethodTypes.SystemSettingsGet);
            agentmethod.Remove(MethodTypes.SystemSettingsGetAll);
            agentmethod.Remove(MethodTypes.SystemSettingsAdd);
            //agentmethod.Remove(MethodTypes.SystemSettingsGetOptionList);
            agentmethod.Remove(MethodTypes.SystemSettingsLoadMoreFilter);
            agentmethod.Remove(MethodTypes.SystemSettingsRemove);


            rolemethods.Clear();

            foreach (var method in agentmethod)
            {
                rolemethods.Add(new RoleMethodEntity
                {
                    Method = (MethodTypes)method,
                    Role = RoleTypes.Agent
                });

            }
            foreach (var rolemethod in rolemethods)
            {
                context.RoleMethods.Add(rolemethod);
            }
            context.SaveChanges();


            var userMethods=new List<MethodTypes>();


            userMethods.Add(MethodTypes.UserGet);
            userMethods.Add(MethodTypes.UserUpdate);


            rolemethods.Clear();

            foreach (var method in userMethods)
            {
                rolemethods.Add(new RoleMethodEntity
                {
                    Method = (MethodTypes)method,
                    Role = RoleTypes.User
                });

            }
            foreach (var rolemethod in rolemethods)
            {
                context.RoleMethods.Add(rolemethod);
            }
            context.SaveChanges();
















            var systemSettings = new List<SystemSettingsEntity>();

            systemSettings.Add(new SystemSettingsEntity
            {
                IsDeletable = false,
                IsDeleted = false,
                Key = "logo",
                Name = "Logo Id",
                Value = ""
            });
            systemSettings.Add(new SystemSettingsEntity
            {
                IsDeletable = false,
                IsDeleted = false,
                Key = "smtpServer",
                Name = "Smtp Server Address",
                Value = ""
            });
            systemSettings.Add(new SystemSettingsEntity
            {
                IsDeletable = false,
                IsDeleted = false,
                Key = "smtpPort",
                Name = "Smtp Port No",
                Value = ""
            });
            systemSettings.Add(new SystemSettingsEntity
            {
                IsDeletable = false,
                IsDeleted = false,
                Key = "smtpPassword",
                Name = "Smtp Password",
                Value = ""
            });
            systemSettings.Add(new SystemSettingsEntity
            {
                IsDeletable = false,
                IsDeleted = false,
                Key = "smtpDisplayName",
                Name = "Smtp Display Name",
                Value = ""
            });
            systemSettings.Add(new SystemSettingsEntity
            {
                IsDeletable = false,
                IsDeleted = false,
                Key = "smtpEnableSsl",
                Name = "Smtp Enable Ssl",
                Value = ""
            });
            systemSettings.Add(new SystemSettingsEntity
            {
                IsDeletable = false,
                IsDeleted = false,
                Key = "smtpDisplayAddress",
                Name = "Smtp Display Address",
                Value = ""
            });

            foreach (var systemSetting in systemSettings)
            {
                context.SystemSettings.Add(systemSetting);
            }
            context.SaveChanges();

            var contactInfo = new ContactInfoEntity
            {
                Address = "",
                Name = "",
                Phone = "",
                XUrl = "",
                LocationLongitude = "0",
                LocationLatitude = "0",
                LinkedinUrl = "",
                Email = "",
                FacebookUrl = "",
                InstagramUrl = "",
                IsDeletable = false,
                IsDeleted = false

            };
            context.ContactInfos.Add(contactInfo);
            context.SaveChanges();
        }
    }
}

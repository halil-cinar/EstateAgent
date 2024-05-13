using EstateAgent.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.DataAccess.EntityFramework
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext():base("server=.;Initial Catalog=RealEstateDB;Integrated Security=True")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AboutEntity> Abouts { get; set; }

        public DbSet<AboutMediaEntity> AboutMedias { get; set; }

        public DbSet<AgentEntity> Agents { get; set; }

        public DbSet<BlogEntity> Blogs { get; set; }

        public DbSet<ContactInfoEntity> ContactInfos { get; set; }

        public DbSet<IdentityEntity> Identities { get; set; }

        public DbSet<MediaEntity> Medias { get; set; }

        //public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<PropertyEntity> Properties { get; set; }

        public DbSet<PropertyMediaEntity> PropertyMedias { get; set; }

        public DbSet<RoleMethodEntity> RoleMethods { get; set; }

        public DbSet<SubscribeEntity> Subscribes { get; set; }

        public DbSet<SystemSettingsEntity> SystemSettings { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<UserRoleEntity> UserRoles { get; set; }

        public DbSet<SessionEntity> Sessions { get; set; }


        // Views

        public DbSet<PropertyListEntity> PropertyLists { get; set; }

        public DbSet<BlogListEntity> BlogLists { get; set; }

        public DbSet<AgentListEntity> AgentLists { get; set; }

        public DbSet<UserRoleListEntity> UserRoleLists { get; set; }

        public DbSet<SessionListEntity> SessionLists { get; set; }

        public DbSet<HomeListEntity> HomeLists { get; set; }

    }
}

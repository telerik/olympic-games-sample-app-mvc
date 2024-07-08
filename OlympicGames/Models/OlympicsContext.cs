namespace OlympicGames.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.SQLite;

    public partial class OlympicsEntities : DbContext
    {
        public OlympicsEntities()
            : base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder()
                {
                    DataSource = $"{AppDomain.CurrentDomain.GetData("DataDirectory").ToString()}\\demos.db",
                    ForeignKeys = true
                }.ConnectionString
            },
            true)
        {
            Database.SetInitializer<OlympicsEntities>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<athlete> athletes { get; set; }
        public virtual DbSet<country> countries { get; set; }
        public virtual DbSet<game> games { get; set; }
        public virtual DbSet<medal> medals { get; set; }
        public virtual DbSet<result> results { get; set; }
        public virtual DbSet<results1> results1 { get; set; }
        public virtual DbSet<sport> sports { get; set; }
    }
}
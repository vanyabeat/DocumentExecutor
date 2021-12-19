using Microsoft.EntityFrameworkCore;

namespace DocumentExecutor.Model.Data
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<ExecutorRecord> ExecutorRecords { get; set; }
        public DbSet<ExecutorRecordData> ExecutorRecordDatas { get; set; }
        public static string DatabaseName { get; set; }
        public static string Host { get; set; }
        public static string Password { get; set; }
        public static string Port { get; set; }
        public static string User { get; set; }

        public ApplicationContext()
        {
            if (Database.CanConnect())
            {
                Database.EnsureCreated();
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(@$"Host={Host};Database={DatabaseName};Username={User};Password={Password};Port={Port};");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExecutorRecord>()
                .HasAlternateKey(c => new { c.Guid });

            //modelBuilder.Entity<ExecutorRecord>().HasOne(s => s.RecordData)
            //    .WithMany()
            //    .HasForeignKey(e => e.RecordDataId);

            modelBuilder.Entity<ExecutorRecordData>().HasOne(s => s.ExecutorRecord)
                .WithMany()
                .HasForeignKey(e => e.RecordGuid);
        }

    }
}

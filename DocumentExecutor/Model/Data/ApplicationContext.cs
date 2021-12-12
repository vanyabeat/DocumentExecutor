using Microsoft.EntityFrameworkCore;

namespace DocumentExecutor.Model.Data
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<ExecutorRecord> ExecutorRecords { get; set; }
        public string DatabaseName { get; }
        public string Host { get; }
        public string Password { get; }
        public string Port { get; }
        public string User { get; }

        public ApplicationContext(string host, string database, string user, string password, string port)
        {
            Host = host;
            DatabaseName = database;
            User = user;
            Password = password;
            Port = port;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(@$"Host={Host};Database={DatabaseName};Username={User};Password={Password};Port={Port};");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExecutorRecord>()
                .HasAlternateKey(c => new { c.Guid });
        }

    }
}

using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Repository
{
    public class TodoDb : DbContext
    {
        private readonly string connectionString;
        public TodoDb(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("Postgres") ?? throw new ArgumentNullException("Connection string is empty");

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}

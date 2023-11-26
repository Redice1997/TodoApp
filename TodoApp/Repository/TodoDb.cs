using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Repository
{
    public class TodoDb : DbContext
    {
        private readonly string connectionString;
        public TodoDb(string connectionString) {
            this.connectionString = connectionString;
            Database.EnsureCreated(); 
        }
         
        public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}

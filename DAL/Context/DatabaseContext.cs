using Microsoft.EntityFrameworkCore;
using ProducerConsumer.Model;

namespace ProducerConsumer.DAL
{
    public class DataBaseContext: DbContext
    {
        public DbSet<ConsumerTask> ConsumerTasks { get; set; }
        public DbSet<Consumer> Consumers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=ProducerConsumer;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConsumerTask>()
                .HasOne(pt => pt.Consumer)
                .WithMany(p => p.ConsumerTasks)
                .HasForeignKey(pt => pt.ConsumerId);
        }
    }
}

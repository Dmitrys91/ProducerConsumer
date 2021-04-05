using ProducerConsumer.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IConsumersRepository
    {
        Task<Consumer> CreateAsync(Consumer consumer);
    }

    public class ConsumersRepository : BaseRepository, IConsumersRepository
    {
        public ConsumersRepository() : base() { }

        public async Task<Consumer> CreateAsync(Consumer consumer)
        {
            if (consumer is null)
                return null;

            await _context.Consumers.AddAsync(consumer);

            var result = await _context.SaveChangesAsync();

            // invoke creation tasks

            var consumerTasks = new List<ConsumerTask>()
            {
                new ConsumerTask
                {
                    ConsumerId = consumer.ConsumerId,
                    CreationTime = DateTime.UtcNow,
                    ModificationTime = DateTime.UtcNow,
                    Status = Status.Pending,
                    ConsumerTaskText = "Custom text 1"
                },
                new ConsumerTask
                {
                    ConsumerId = consumer.ConsumerId,
                    CreationTime = DateTime.UtcNow,
                    ModificationTime = DateTime.UtcNow,
                    Status = Status.Pending,
                    ConsumerTaskText = "Custom text 1"
                }
            };

            await _context.ConsumerTasks.AddRangeAsync(consumerTasks);

            await _context.SaveChangesAsync();

            return result != 0 ? consumer : null;
        }
    }
}

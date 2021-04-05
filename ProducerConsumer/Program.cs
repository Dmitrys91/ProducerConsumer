using DAL.Repository;
using ProducerConsumer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ProducerConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Plese set consumers creation count:");

            var count = Console.Read();

            var queue = new BufferBlock<Consumer>(new DataflowBlockOptions { BoundedCapacity = count });

            var producer = Produce(queue, count);
            var consumer = Consume(queue);

            await Task.WhenAll(producer, consumer, queue.Completion);     

            var consumers = await consumer;
            
            Console.WriteLine("Please press any key to see actual tasks for each consumer");
            Console.ReadKey();

            ConsumerTasksRepository repo = new ConsumerTasksRepository();

            var items = await repo.GetTasksTextByConsumerIds(consumers.Select(x => x.ConsumerId).ToArray());

            foreach(var item in items)
            {
                Console.WriteLine($"for consumer with id = {item.Key} we have following tasks: {string.Join(", ", item.Value)}");
            }

            var statuses = await repo.GetStatsByStatus();

            foreach(var status in statuses)
            {
                Console.WriteLine($"in status {status.Key} we have { status.Value} tasks");
            }

            var avgProcessTime = await repo.GetAverageTasksProcessTime();

            Console.WriteLine($"Average processing time is: {avgProcessTime.Minutes} minutes");

            var errorPercentage = await repo.GetErrorsPercentage();

            Console.WriteLine($"{errorPercentage}% of errors");
        }

        private static async Task Produce(BufferBlock<Consumer> queue, int count)
        {
            for (var i = 0; i < count; i++)
            {
                await queue.SendAsync(new Consumer());
            }
        }

        private static async Task<IEnumerable<Consumer>> Consume(BufferBlock<Consumer> queue)
        {
            var tasksRepo = new ConsumersRepository();

            var ret = new List<Consumer>();

            while (await queue.OutputAvailableAsync())
            {
                var item = await queue.ReceiveAsync();

                await tasksRepo.CreateAsync(item);

                ret.Add(item);

                if(queue.Count == 0)
                  queue.Complete();
            }

            return ret;
        }
    }
}

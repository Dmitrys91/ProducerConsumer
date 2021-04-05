using Microsoft.EntityFrameworkCore;
using ProducerConsumer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IConsumerTasksRepository
    {
        Task<IEnumerable<ConsumerTask>> GetLastestPendingTasks(int consumerId);

        Task<IDictionary<int, IEnumerable<string>>> GetTasksTextByConsumerIds(int[] consumerIds);

        Task<IDictionary<Status, int>> GetStatsByStatus();

        Task<TimeSpan> GetAverageTasksProcessTime();

        Task<int> GetErrorsPercentage();
    }

    public class ConsumerTasksRepository : BaseRepository, IConsumerTasksRepository
    {
        public ConsumerTasksRepository() : base() { }

        /// <summary>
        /// Could be set in the config 
        /// </summary>
        private readonly int _bulkSize = 5;

        public async Task<IEnumerable<ConsumerTask>> GetLastestPendingTasks(int consumerId)
        {
            return await _context.ConsumerTasks
                .Where(x => x.ConsumerId == consumerId && x.Status == Status.Pending)
                .AsNoTracking()
                .OrderByDescending(x => x.CreationTime)
                .Take(_bulkSize)
                .ToListAsync();
        }

        public async Task<IDictionary<int, IEnumerable<string>>> GetTasksTextByConsumerIds(int[] consumerIds)
        {
            var tasks = _context.ConsumerTasks
                 .AsNoTracking()
                 .Where(x => consumerIds.Contains(x.ConsumerId))
                 .OrderByDescending(x => x.CreationTime)
                 .AsEnumerable()
                 .GroupBy(x=> x.ConsumerId)
                 .ToDictionary(x => x.Key, x => x.Select(y=> y.ConsumerTaskText));

            
            return await Task.FromResult(tasks);
        }

        public async Task<IDictionary<Status, int>> GetStatsByStatus()
        {
            var result =  _context.ConsumerTasks
                .AsNoTracking()
                .AsEnumerable()
                .GroupBy(x => x.Status)
                .ToDictionary(group => group.Key, group => group.Count());

            return await Task.FromResult(result);
        }

        public async Task<TimeSpan> GetAverageTasksProcessTime()
        {
            var result = await _context.ConsumerTasks
                .AsNoTracking()
                .Where(x => x.Status == Status.Done)
                .ToListAsync();

            if (!result.Any())
                return TimeSpan.Zero;

            var times = result
                .Select(x => x.ModificationTime - x.CreationTime);

            int i = 0;
            int TotalSeconds = 0;

            var ArrayDuration = times.ToArray();

            for (i = 0; i < ArrayDuration.Length; i++)
            {
                TotalSeconds = (int)(ArrayDuration[i].TotalSeconds) + TotalSeconds;
            }

            if (TotalSeconds == 0)
                return TimeSpan.Zero;

            var totalTime = TimeSpan.FromSeconds(TotalSeconds);

            var avg = totalTime / result.Count;

            return avg;
        }

        public TimeSpan TotalTime(IEnumerable<TimeSpan> TheCollection)
        {
            int i = 0;
            int TotalSeconds = 0;

            var ArrayDuration = TheCollection.ToArray();

            for (i = 0; i < ArrayDuration.Length; i++)
            {
                TotalSeconds = (int)(ArrayDuration[i].TotalSeconds) + TotalSeconds;
            }

            return TimeSpan.FromSeconds(TotalSeconds);
        }

        public async Task<int> GetErrorsPercentage()
        {
            var errorsCount = await _context.ConsumerTasks
               .Where(x => x.Status == Status.Error)
               .GroupBy(x => x.Status)
               .Select(x => x.Count())
               .FirstOrDefaultAsync();

            var generalCount = await _context.ConsumerTasks.CountAsync();

            var percent = generalCount / 100;

            var result = errorsCount / percent;

            return result;
        }
    }
}

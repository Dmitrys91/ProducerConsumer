using System;
using System.Collections.Generic;
using System.Text;

namespace ProducerConsumer.Model
{
    public class Consumer
    {
        public int ConsumerId { get; set; }

        public ICollection<ConsumerTask> ConsumerTasks
        {
            get; set;
        }
    }
}

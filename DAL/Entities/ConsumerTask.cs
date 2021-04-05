using System;
using System.Collections.Generic;
using System.Text;

namespace ProducerConsumer.Model
{
    public class ConsumerTask
    {
        public int ConsumerTaskId { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string ConsumerTaskText { get; set; }

        public int ConsumerId { get; set; }

        public Status Status { get; set; }

        public Consumer Consumer { get; set; }
    }

    public enum Status
    {
        Pending = 0,

        InProgress = 1,

        Error = 2, 

        Done = 3
    }
}

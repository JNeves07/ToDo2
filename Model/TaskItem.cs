using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo2.Model
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Category { get; set; }

        public DateTime DueDate { get; set; }

    }
}

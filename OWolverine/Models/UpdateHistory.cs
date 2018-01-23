using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models
{
    public class UpdateHistory
    {
        public long Id { get; set; }
        public string Context { get; set; }
        public string FieldName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

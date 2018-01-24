using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OWolverine.Models.Ogame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models
{
    public class ScoreHistory
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public int OldValue { get; set; }
        public int NewValue { get; set; }
        public Score Score { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

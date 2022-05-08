using System;
using System.Collections.Generic;

#nullable disable

namespace Escalonamento.Models
{
    public partial class Job
    {
        public Job()
        {
            JobOps = new HashSet<JobOp>();
            SimJobs = new HashSet<SimJob>();
        }

        public int IdJob { get; set; }

        public virtual ICollection<JobOp> JobOps { get; set; }
        public virtual ICollection<SimJob> SimJobs { get; set; }
    }
}

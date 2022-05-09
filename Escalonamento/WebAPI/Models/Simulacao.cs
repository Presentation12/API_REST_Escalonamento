using System;
using System.Collections.Generic;

#nullable disable

namespace Escalonamento.Models
{
    public partial class Simulacao
    {
        public Simulacao()
        {
            SimJobs = new HashSet<SimJob>();
        }

        public int IdSim { get; set; }
        public string Estado { get; set; }
        public int? IdUser { get; set; }

        public virtual Utilizador IdUserNavigation { get; set; }
        public virtual ICollection<SimJob> SimJobs { get; set; }
    }
}

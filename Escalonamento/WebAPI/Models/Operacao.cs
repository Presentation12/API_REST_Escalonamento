using System;
using System.Collections.Generic;

#nullable disable

namespace Escalonamento.Models
{
    public partial class Operacao
    {
        public Operacao()
        {
            JobOps = new HashSet<JobOp>();
        }

        public int IdOp { get; set; }
        public int? IdMaq { get; set; }

        public virtual Maquina IdMaqNavigation { get; set; }
        public virtual ICollection<JobOp> JobOps { get; set; }
    }
}

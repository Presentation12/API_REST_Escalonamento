using System;
using System.Collections.Generic;

#nullable disable

namespace Escalonamento.Models
{
    public partial class JobOp
    {
        public int IdJob { get; set; }
        public int IdOp { get; set; }
        public int? Duracao { get; set; }

        public virtual Job IdJobNavigation { get; set; }
        public virtual Operacao IdOpNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace Escalonamento.Models
{
    public partial class SimJob
    {
        public int IdJob { get; set; }
        public int IdSim { get; set; }

        public virtual Job IdJobNavigation { get; set; }
        public virtual Simulacao IdSimNavigation { get; set; }
    }
}

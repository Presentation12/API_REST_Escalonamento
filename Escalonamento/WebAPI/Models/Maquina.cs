using System;
using System.Collections.Generic;

#nullable disable

namespace Escalonamento.Models
{
    public partial class Maquina
    {
        public Maquina()
        {
            Operacaos = new HashSet<Operacao>();
        }

        public int IdMaq { get; set; }
        public bool? Estado { get; set; }

        public virtual ICollection<Operacao> Operacaos { get; set; }
    }
}

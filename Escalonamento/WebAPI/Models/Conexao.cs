using System;
using System.Collections.Generic;

#nullable disable

namespace Escalonamento.Models
{
    public partial class Conexao
    {
        public int IdUser { get; set; }
        public int IdSim { get; set; }
        public int IdJob { get; set; }
        public int IdOp { get; set; }
        public int? IdMaq { get; set; }
        public int? Duracao { get; set; }
        public bool? Estado { get; set; }
        public int? TempoInicial { get; set; }
    }
}

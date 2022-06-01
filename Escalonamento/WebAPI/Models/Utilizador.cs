using System;
using System.Collections.Generic;

#nullable disable

namespace Escalonamento.Models
{
    public partial class Utilizador
    {
        public int IdUser { get; set; }
        public string Mail { get; set; }
        public bool? Aut { get; set; }
        public string PassSalt { get; set; }
        public string PassHash { get; set; }
        public string Estado { get; set; }
    }
}

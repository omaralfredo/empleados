using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class empleados
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } 
        public string Apellido { get; set; }
        public string Ciudad { get; set; }
        public int Salario { get; set; }
        
    }
}
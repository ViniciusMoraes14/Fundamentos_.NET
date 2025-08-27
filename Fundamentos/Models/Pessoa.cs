using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundamentos.Models
{
    public class Pessoa
    {
        public string Name { get; set; }
        public int idade { get; set; }

        public void Apresentar()
        {
            Console.WriteLine($"Olá, meu nome é {Name}, e tenho {idade} anos");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class EnumMetodos
    {
        enum metodos
        {
            Dinheiro = 1,
            Débito = 2,
            Crédito = 3,
            CartãoConsumo = 4,
            Implantação = 5,
            Negativa = 0
        }
    }
}

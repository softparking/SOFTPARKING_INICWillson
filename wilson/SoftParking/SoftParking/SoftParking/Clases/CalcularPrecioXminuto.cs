using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftParking.Clases
{
    public class CalcularPrecioXminuto
    {
        public double ValorMinuto(TimeSpan tiempo, double precioHora)
        {   double resultado = 0;
            double valorMin = precioHora/60;
            int totaTiempo = (int)tiempo.TotalMinutes;

           resultado = totaTiempo * valorMin;

            return resultado;
        }
        
    }
}
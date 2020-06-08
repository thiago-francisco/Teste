using TesteAPI.Models;
using System;
using System.Linq;

namespace TesteAPI.Data
{
    public static class InicializadorBD
    {
        public static void Inicializar(Contexto context)
        {
            context.Database.EnsureCreated();
        }
    }
}

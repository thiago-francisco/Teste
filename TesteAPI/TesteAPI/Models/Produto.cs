using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TesteAPI.Models
{
    public class Produto
    {
        public int ProdutoID { get; set; }
        public DateTime DataEntrega { get; set; }
        [MaxLength(50)]
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public DateTime DataImportacao { get; set; }

        
    }
}

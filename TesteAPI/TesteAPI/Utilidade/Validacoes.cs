using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using TesteAPI.Models;

namespace TesteAPI.Utilidade
{
    public static class Validacoes
    {
        public static Tuple<string, Produto> ValidarLinha(int Linha, string DataEntrada, string NomeProduto, string Quantidade, string ValorUnitario)
        {
            string RetornoString = "";
            Produto Produto = new Produto();

            //Valida a data de entrada
            string[] Validacao = DataEntrada.Split('/');
            if (Validacao.Length < 3)
                RetornoString = "O valor da data deve estar preenchido, e no formato 'DD/MM/AAAA'. ";

            DateTime DataFinal = new DateTime();
            try
            {
                DataFinal = new DateTime(Convert.ToInt32(Validacao[2]), Convert.ToInt32(Validacao[1]), Convert.ToInt32(Validacao[0]));
            }
            catch
            {
                RetornoString += "Existe um valor Inválido na data preenchida. ";
            }
            if (DataFinal <= DateTime.Now)
                RetornoString += "A Data não pode ser menor ou igual a data atual. ";
            Produto.DataEntrega = DataFinal;

            //Valida o nome do produto
            if (NomeProduto.Length > 50)
                RetornoString += "O nome do produto não pode ter mais de 50 caracteres. ";
            else
                Produto.NomeProduto = NomeProduto;

            //Valida a Quantidade
            int QuantidadeFinal = 0;
            try
            {
                QuantidadeFinal = Convert.ToInt32(Quantidade);
            }
            catch
            {
                RetornoString += "Verifique a quantidade, o valor deve ser um número inteiro. ";
            }

            if (QuantidadeFinal == 0)
                RetornoString += "Verifique a quantidade, o valor deve ser maior que zero. ";
            Produto.Quantidade = QuantidadeFinal;

            //Valida o valor unitário
            double ValorFinal = 0;
            try
            {
                ValorFinal = Convert.ToDouble("");
            }
            catch
            {
                RetornoString += "Verifique o Valor, o valor deve ser um número decimal. ";
            }
            ValorFinal = Math.Round(ValorFinal, 2);
            Produto.ValorUnitario = ValorFinal;

            if (RetornoString == "")
                RetornoString = "OK";
            else
                RetornoString = $"Erros na linha {Linha}: " + RetornoString;
            Produto.DataImportacao = DateTime.Now;

            return new Tuple<string, Produto>(RetornoString, Produto);
        }
    }
}

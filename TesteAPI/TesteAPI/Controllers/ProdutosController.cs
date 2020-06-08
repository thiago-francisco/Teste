using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using TesteAPI.Models;

namespace TesteAPI.Controllers
{
    [ApiController]
    [Route("Produtos")]
    public class ProdutosController : Controller
    {
        private readonly Contexto Contexto;

        public ProdutosController(Contexto _Contexto)
        {
            Contexto = _Contexto;
        }

        [HttpGet]
        [Route("GetImportacoes")]
        public List<Produto> GetImportacoes()
        {
            return Contexto.Produtos.ToList<Produto>();
        }

        [HttpGet]
        [Route("GetImportacao/{ProdutoID}")]
        public async Task<IActionResult> GetImportacao(int ProdutoID)
        {
            if (Contexto.Produtos.Where(x => x.ProdutoID == ProdutoID).Count() == 0)
                return BadRequest($"Não existe produto com o ID {ProdutoID}");
            else
                return Ok(Contexto.Produtos.Single(x => x.ProdutoID == ProdutoID));
        }

        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Insert(IFormFile Arquivo)
        {
            bool Valido = true;
            string RetornoString = "";
            List<Produto> Produtos = new List<Produto>();
            if (Path.GetExtension(Arquivo.FileName).ToLower() != ".xls" || Path.GetExtension(Arquivo.FileName).ToLower() != ".xlsx")
            {
                return BadRequest("Arquivo não está com a extensão correta, as extensões aceitas são '.xls' e '.xlsx'");
            }

            if (Arquivo == null || Arquivo.Length == 0)
            {
                return BadRequest("O arquivo está vazio, envie um arquivo com informações.");
            }

            using (var memoryStream = new MemoryStream())
            {
                Arquivo.CopyToAsync(memoryStream).ConfigureAwait(false);

                using (var package = new ExcelPackage(memoryStream))
                {
                    for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
                    {
                        var totalRows = package.Workbook.Worksheets[i].Dimension?.Rows;
                        var totalCollumns = package.Workbook.Worksheets[i].Dimension?.Columns;
                        for (int j = 1; j <= totalRows.Value; j++)
                        {
                            if (package.Workbook.Worksheets[i].Cells[j, 0].Value.ToString() != "DataEntrega")
                            {
                                var Validacao = Utilidade.Validacoes.ValidarLinha(j + 1,
                                package.Workbook.Worksheets[i].Cells[j, 0].Value.ToString(),
                                package.Workbook.Worksheets[i].Cells[j, 1].Value.ToString(),
                                package.Workbook.Worksheets[i].Cells[j, 2].Value.ToString(),
                                package.Workbook.Worksheets[i].Cells[j, 3].Value.ToString());
                                if (Validacao.Item1 != "OK")
                                {
                                    RetornoString += Validacao.Item1 + "\n";
                                    Valido = false;
                                }
                                else
                                    Produtos.Add(Validacao.Item2);
                            }
                        }
                    }
                    if (RetornoString == "")
                    {
                        foreach (Produto Produto in Produtos)
                        {
                            Contexto.Produtos.Add(Produto);
                            Contexto.SaveChanges();
                        }
                        return Ok("OK.");
                    }
                    else
                        return BadRequest(RetornoString);
                }

                
            }

        }

    }
}
using Escalonamento.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Escalonamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperacaoController : ControllerBase
    {
        #region GET


        /// <summary>
        /// Método que devolve a lista inteira das operações
        /// </summary>
        /// <returns> Operações na base de dados </returns>
        [HttpGet]
        public IEnumerable<Operacao> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Operacao.ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        /// <summary>
        /// Método que devolve uma operação passada por id
        /// </summary>
        /// <param name="id_operacao"> ID da operação </param>
        /// <returns> Operação </returns>
        [HttpGet("{id_operacao}")]
        public Operacao Get(int id_operacao)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Operacao.Where(o => o.IdOp == id_operacao).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        #endregion

        #region POST

        /// <summary>
        /// Método que adiciona uma nova operação na base de dados
        /// </summary>
        /// <param name="op"> Informação da operação </param>
        /// <returns> Resultado do método </returns>
        [HttpPost]
        public JsonResult Post(Operacao op)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Operacao operacao = new Operacao();

                    operacao.IdOp = op.IdOp;
                    operacao.IdMaq = op.IdMaq;

                    context.Operacao.Add(operacao);
                    context.SaveChanges();

                    return new JsonResult("Operação adicionada com sucesso!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new JsonResult("Erro");
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Método que remove uma operação da base de dados
        /// </summary>
        /// <param name="id_operacao"> ID da operação </param>
        /// <returns> Resultado do método </returns>
        [HttpDelete("{id_operacao}")]
        public JsonResult Delete(int id_operacao)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Operacao operacao = context.Operacao.Where(o => o.IdOp == id_operacao).FirstOrDefault();

                    context.Operacao.Remove(operacao);

                    context.SaveChanges();
                    return new JsonResult("Operacao removida com sucesso!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new JsonResult("Erro");
            }
        }

        #endregion
    }
}
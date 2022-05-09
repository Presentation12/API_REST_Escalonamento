using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Escalonamento.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Escalonamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulacaoController : ControllerBase
    {

        #region GET

        /// <summary>
        /// Método que retorna lista de simulações
        /// </summary>
        /// <returns> Lista de simulações </returns>
        [HttpGet]
        public IEnumerable<Simulacao> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Simulacao.ToList();
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Método que retorna uma simulação em específico
        /// </summary>
        /// <param name="id"> ID da simulação </param>
        /// <returns> Simulação </returns>
        [HttpGet("{id}")]
        public Simulacao Get(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault();
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
        /// Método que adiciona uma simulação à base de dados
        /// </summary>
        /// <param name="sim"> Informação da simulação </param>
        /// <returns> Resultado do método </returns>
        [HttpPost]
        public JsonResult Post([FromBody] Simulacao sim)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao simulacao = new Simulacao();

                    simulacao.IdSim = sim.IdSim;
                    simulacao.Estado = sim.Estado;
                    simulacao.IdUserNavigation = context.Utilizador.Where(u => u.IdUser == sim.IdUser).FirstOrDefault();

                    context.Simulacao.Add(simulacao);

                    context.SaveChanges();
                    return new JsonResult("Simulacao adicionada com sucesso!");
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return new JsonResult("Erro");
            }
        }

        #endregion

        #region PATCH

        /// <summary>
        /// Método que atualiza a informação de uma certa simulação
        /// </summary>
        /// <param name="id"> ID da simulação </param>
        /// <param name="sim"> Informação da simulação </param>
        /// <returns> Estado do método </returns>
        [HttpPatch("{id}")]
        public JsonResult Patch(int id, [FromBody] Simulacao sim)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao simulacao = context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault();

                    simulacao.IdSim = sim.IdSim == 0 ? simulacao.IdSim : sim.IdSim;
                    simulacao.Estado = sim.Estado is null ? simulacao.Estado : sim.Estado;
                    if(sim.IdUser != 0) simulacao.IdUserNavigation = context.Utilizador.Where(u => u.IdUser == sim.IdUser).FirstOrDefault();

                    context.SaveChanges();
                    return new JsonResult("Simulacao alterada com sucesso!");
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
        /// Método que elimina uma certa simulação da base de dados
        /// </summary>
        /// <param name="id"> ID da simulação </param>
        /// <returns> Estado do método </returns>
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao sim = context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault();

                    context.Simulacao.Remove(sim);

                    context.SaveChanges();
                    return new JsonResult("Simulação removida com sucesso!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        #endregion
    }
}

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
        public IActionResult Post([FromBody] Simulacao sim)
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
                    return Ok();
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
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
        public IActionResult Patch(int id, [FromBody] Simulacao sim)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao simulacao = context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault();

                    simulacao.Estado = sim.Estado is null ? simulacao.Estado : sim.Estado;
                    if(sim.IdUser != 0) simulacao.IdUserNavigation = context.Utilizador.Where(u => u.IdUser == sim.IdUser).FirstOrDefault();

                    context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método que arquiva uma simulação
        /// </summary>
        /// <param name="id"> ID da simulação </param>
        /// <returns> Estado do método </returns>
        [HttpPatch("delete/{id}")]
        public IActionResult ArquivarSimulacao(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao simulacao = context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault();

                    simulacao.Estado = "Inativo";

                    context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
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
        public IActionResult Delete(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao sim = context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault();

                    context.Simulacao.Remove(sim);

                    context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        #endregion

        #region ALGORITHM

        //Arranjar forma de passar os tempos para cada operacao
        public void algoritmo(int numMachines, int numJobs, int numOperations)
        {
            //lista com rows todas ligacao job com maquina e duracao, posicao na lista é a operação 

        }

        #endregion
    }
}

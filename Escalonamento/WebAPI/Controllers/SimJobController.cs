using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Escalonamento.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Escalonamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimJobController : ControllerBase
    {
        #region GET

        /// <summary>
        /// Método que retorna todas as ligações entre um job e uma simulação
        /// </summary>
        /// <returns> Lista de SimJobs </returns>
        [HttpGet, Authorize(Roles = "Admin, Utilizador")]
        public IEnumerable<SimJob> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext()) 
                { 
                    return context.SimJob.ToList(); 
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Método que retorna uma certa ligação entre um job e uma simulação
        /// </summary>
        /// <param name="idsim"> ID da simulação </param>
        /// <param name="idjob"> ID do job </param>
        /// <returns></returns>
        [HttpGet("{idsim}_{idjob}"), Authorize(Roles = "Admin, Utilizador")]
        public SimJob Get(int idsim, int idjob)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.SimJob.Where(sj => sj.IdSim == idsim && sj.IdJob == idjob).FirstOrDefault();
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
        /// Método que adiciona uma nova ligação entre job e simulação
        /// </summary>
        /// <param name="sj"> Informação do SimJob </param>
        /// <returns> Estado do Método </returns>
        [HttpPost, Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Post([FromBody] SimJob sj)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    SimJob simJob = new SimJob();

                    simJob.IdJobNavigation = context.Job.Where(j => j.IdJob == sj.IdJob).FirstOrDefault();
                    simJob.IdSimNavigation = context.Simulacao.Where(s => s.IdSim == sj.IdSim).FirstOrDefault();

                    context.SimJob.Add(simJob);

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
        /// Método que remove uma ligação entre job e simulação da base de dados
        /// </summary>
        /// <param name="idsim"> ID da simulação </param>
        /// <param name="idjob"> ID do job </param>
        /// <returns> Estado do método </returns>
        [HttpDelete("{idsim}_{idjob}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(int idsim, int idjob)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    SimJob simJob = context.SimJob.Where(sj => sj.IdSim == idsim && sj.IdJob == idjob).FirstOrDefault();

                    context.SimJob.Remove(simJob);

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
    }
}

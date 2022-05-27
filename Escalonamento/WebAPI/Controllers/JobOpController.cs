using Escalonamento.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class JobOpController : ControllerBase
    {
        #region GET

        /// <summary>
        /// Método que retorna todas as ligações entre um job e uma operação
        /// </summary>
        /// <returns> Lista de JobsOps </returns>
        [HttpGet, Authorize(Roles = "Admin, Utilizador")]
        public IEnumerable<JobOp> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext()) 
                { 
                    return context.JobOp.ToList(); 
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Método que retorna uma certa ligação entre um job e uma operação
        /// </summary>
        /// <param name="idop"> ID da operação </param>
        /// <param name="idjob"> ID do job </param>
        /// <returns></returns>
        [HttpGet("{idop}_{idjob}"), Authorize(Roles = "Admin, Utilizador")]
        public JobOp Get(int idop, int idjob)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.JobOp.Where(jo => jo.IdOp == idop && jo.IdJob == idjob).FirstOrDefault();
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
        /// Método que adiciona uma nova ligação entre job e operação
        /// </summary>
        /// <param name="jo"> Informação do JobOp </param>
        /// <returns> Estado do Método </returns>
        [HttpPost, Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Post([FromBody] JobOp jo)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    JobOp jobOp = new JobOp();
                    
                    jobOp.Duracao = jo.Duracao;
                    jobOp.IdJobNavigation = context.Job.Where(j => j.IdJob == jo.IdJob).FirstOrDefault();
                    jobOp.IdOpNavigation = context.Operacao.Where(o => o.IdOp == jo.IdOp).FirstOrDefault();

                    context.JobOp.Add(jobOp);

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
        /// Método que remove uma ligação entre job e operação da base de dados
        /// </summary>
        /// <param name="idop"> ID da operação </param>
        /// <param name="idjob"> ID do job </param>
        /// <returns> Estado do método </returns>
        [HttpDelete("{idop}_{idjob}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(int idop, int idjob)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    JobOp jobOp = context.JobOp.Where(jo => jo.IdJob == idjob && jo.IdJob == idjob).FirstOrDefault();

                    context.JobOp.Remove(jobOp);

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

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
    public class JobController : ControllerBase
    {
        #region GET

        /// <summary>
        /// Método que devolve a lista de jobs
        /// </summary>
        /// <returns> Jobs na base de dados </returns>
        [HttpGet]
        public IEnumerable<Job> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Job.ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        /// <summary>
        /// Método que devolve um job passado por id
        /// </summary>
        /// <param name="id_job"> ID do job </param>
        /// <returns> Job </returns>
        [HttpGet("{id_job}")]
        public Job Get(int id_job)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Job.Where(o => o.IdJob == id_job).FirstOrDefault();
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
        /// Método que adiciona um novo job na base de dados
        /// </summary>
        /// <param name="job"> Informação do job </param>
        /// <returns> Resultado do método </returns>
        [HttpPost]
        public JsonResult Post(Job job)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Job trabalho = new Job();

                    trabalho.IdJob = job.IdJob;

                    context.Job.Add(trabalho);
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
        /// Método que remove uma jobs da base de dados
        /// </summary>
        /// <param name="id_job"> ID do trabalho </param>
        /// <returns> Resultado do método </returns>
        [HttpDelete("{id_trabalho}")]
        public JsonResult Delete(int id_trabalho)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Job trabalho = context.Job.Where(o => o.IdJob == id_trabalho).FirstOrDefault();

                    context.Job.Remove(trabalho);

                    context.SaveChanges();
                    return new JsonResult("Job removido com sucesso!");
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

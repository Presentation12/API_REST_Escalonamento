using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Escalonamento.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Escalonamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public UtilizadorController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        #region GET


        /// <summary>
        /// Método que devolve a lista inteira de utilizadores
        /// </summary>
        /// <returns> Utilizadores na base de dados </returns>
        [HttpGet]
        public IEnumerable<Utilizador> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Utilizador.ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        /// <summary>
        /// Método que devolve a um utilizador específico
        /// </summary>
        /// <param name="id"> ID do utilizador </param>
        /// <returns> Utilizador em específico </returns>
        [HttpGet("{id}")]
        public Utilizador Get(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Utilizador.Where(u => u.IdUser == id).FirstOrDefault();
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
        /// Método que adiciona um novo utilizador na base de dados
        /// </summary>
        /// <param name="uti"> Informação do utilizador </param>
        /// <returns> Resultado do método </returns>
        [HttpPost]
        public JsonResult Post(Utilizador uti)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Utilizador utilizador = new Utilizador();

                    utilizador.IdUser = uti.IdUser;
                    utilizador.Mail = uti.Mail;
                    utilizador.PassHash = uti.PassHash;
                    utilizador.PassSalt = uti.PassSalt;
                    utilizador.Aut = uti.Aut;

                    context.Utilizador.Add(utilizador);
                    context.SaveChanges();

                    return new JsonResult("Utilizador adicionado com sucesso!");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new JsonResult("Erro");
            }
        }

        #endregion

        #region PATCH

        /// <summary>
        /// Método que atualiza a informação de um utilizador
        /// </summary>
        /// <param name="id"> ID do utilizador alvo </param>
        /// <param name="value"> Informação nova do utilizador </param>
        [HttpPatch("{id}")]
        public JsonResult Patch(int id, [FromBody] Utilizador uti)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Utilizador utilizador = context.Utilizador.Where(u => u.IdUser == id).FirstOrDefault();

                    utilizador.Mail = uti.Mail is null ? utilizador.Mail : uti.Mail;
                    utilizador.PassHash = uti.PassHash is null ? utilizador.PassHash : uti.PassHash;
                    utilizador.PassSalt = uti.PassSalt is null ? utilizador.PassSalt : uti.PassHash;
                    utilizador.Aut = uti.Aut is null ? utilizador.Aut : uti.Aut;

                    context.SaveChanges();

                    return new JsonResult("Utilizador alterado com sucesso!");
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
        /// Método que remove um utilizador da base de dados
        /// </summary>
        /// <param name="id"> ID do utilizador </param>
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Utilizador utilizador = context.Utilizador.Where(u => u.IdUser == id).FirstOrDefault();

                    context.Utilizador.Remove(utilizador);

                    context.SaveChanges();
                    return new JsonResult("Utilizador removido com sucesso!");
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

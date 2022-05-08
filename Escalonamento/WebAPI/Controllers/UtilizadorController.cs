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

        [HttpGet]
        public IEnumerable<Utilizador> Get()
        {
            using (var context = new EscalonamentoContext())
            {
                return context.Utilizador.ToList();
            }
        }

        // GET api/<UtilizadorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UtilizadorController>
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

        // PUT api/<UtilizadorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UtilizadorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

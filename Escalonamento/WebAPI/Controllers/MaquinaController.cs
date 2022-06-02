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
    public class MaquinaController : ControllerBase
    {
        #region GET

        /// <summary>
        /// Método que devolve a lista inteira das maquinas
        /// </summary>
        /// <returns> Maquinas na base de dados </returns>
        [HttpGet, Authorize(Roles = "Admin, Utilizador")]
        public IEnumerable<Maquina> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Maquina.ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Método que devolve uma maquina passada por id
        /// </summary>
        /// <param name="id_maquina"> ID da maquina </param>
        /// <returns> Máquina </returns>
        [HttpGet("{id_maquina}"), Authorize(Roles = "Admin, Utilizador")]
        public Maquina Get(int id_maquina)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Maquina.Where(m => m.IdMaq == id_maquina).FirstOrDefault();
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
        /// Método que adiciona uma nova maquina na base de dados
        /// </summary>
        /// <param name="maq"> Informação da maquina </param>
        /// <returns> Resultado do método </returns>
        [HttpPost, Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Post(Maquina maq)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Maquina maquina = new Maquina();

                    maquina.IdMaq = maq.IdMaq;
                    maquina.Estado = "Ativo";

                    context.Maquina.Add(maquina);
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

        #region PATCH

        /// <summary>
        /// Método que atualiza a informação de uma certa máquina
        /// </summary>
        /// <param name="id_maquina"> ID da máquina </param>
        /// <param name="maq"> Informação da máquina </param>
        /// <returns> Estado do método </returns>
        [HttpPatch("{id_maquina}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Patch(int id_maquina, [FromBody] Maquina maq)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Maquina maquina = context.Maquina.Where(m => m.IdMaq == id_maquina).FirstOrDefault();

                    maquina.Estado = maq.Estado is null ? maquina.Estado : maq.Estado;

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
        /// Método que remove uma maquina da base de dados
        /// </summary>
        /// <param name="id_maquina"> ID da maquina </param>
        /// <returns> Resultado do método </returns>
        [HttpDelete("{id_maquina}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(int id_maquina)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Maquina maquina = context.Maquina.Where(m => m.IdMaq == id_maquina).FirstOrDefault();
                    List<Conexao> conexoes = context.Conexao.Where(c => c.IdMaq == id_maquina).ToList();

                    foreach (Conexao con in conexoes)
                    {
                        con.IdMaq = null;
                        con.Duracao = null;
                    }

                    maquina.Estado = "Inativo";

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

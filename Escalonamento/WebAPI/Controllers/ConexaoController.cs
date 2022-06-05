using Escalonamento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Escalonamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConexaoController : ControllerBase
    {
        #region GET

        /// <summary>
        /// Método que devolve a lista inteira das conexões ativos
        /// </summary>
        /// <returns> conexões na base de dados </returns>
        [HttpGet("active"), Authorize(Roles = "Admin")]
        public IActionResult GetActives()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return new JsonResult(context.Conexao.Where(c => c.Estado == true).ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método que devolve a lista inteira das conexões
        /// </summary>
        /// <returns> conexões na base de dados </returns>
        [HttpGet("inactive"), Authorize(Roles = "Admin")]
        public IActionResult GetInactives()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return new JsonResult(context.Conexao.ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método que devolve as conexões passada de um utilizador pelo seu id
        /// </summary>
        /// <param name="id_utilizador"> ID da utilizador </param>
        /// <returns> Conexao </returns>
        [HttpGet("{id_utilizador}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult GetByIdUser(int id_utilizador)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);
                        
                        Utilizador user = context.Utilizador.Where(u => u.IdUser == id_utilizador && u.Estado != "Inativo").FirstOrDefault();

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }

                    return new JsonResult(context.Conexao.Where(c => c.IdUser == id_utilizador && c.Estado == true).ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Obter todas os dados de uma simulacao
        /// </summary>
        /// <param name="id_utilizador"></param>
        /// <param name="id_simulacao"></param>
        /// <returns></returns>
        [HttpGet("{id_utilizador}/{id_simulacao}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult GetSimulacaoByUser(int id_utilizador, int id_simulacao)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        Utilizador user = context.Utilizador.Where(u => u.IdUser == id_utilizador && u.Estado != "Inativo").FirstOrDefault();

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }

                    return new JsonResult(context.Conexao.Where(c => c.IdUser == id_utilizador && c.Estado == true && c.IdSim == id_simulacao).ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpGet("planear/{IdUser}/{IdSim}"), Authorize]
        public IActionResult Planner(int IdUser, int IdSim)
        {
             return AssignedTask.AlgoritmoEscalonamento(IdUser, IdSim);
        }

        public struct status
        {
            public int status_value;
            public string status_text;
        }

        [HttpPost("planearmanual"), Authorize]
        public IActionResult PlannerManual(List<Conexao> sim)
        {
            try
            {
                int TempoConclusao = 0;
                status status = new status();

                for (int i = 0; i < sim.Count; i++)
                {
                    if ((sim[i].TempoInicial + sim[i].Duracao) > TempoConclusao)
                        TempoConclusao = (int)sim[i].TempoInicial + (int)sim[i].Duracao;

                    if (i != 0)
                    {
                        if ((sim[i].IdOp == (sim[i - 1].IdOp + 1)) && (sim[i].IdJob == sim[i - 1].IdJob))
                        {
                            if (sim[i].TempoInicial < sim[i - 1].TempoInicial + sim[i - 1].Duracao)
                            {
                                status.status_value = 0;
                                status.status_text = "Job " + sim[i].IdJob + " Op " + sim[i].IdOp + " esta a comecar antes da operacao anterior acabar.";

                                return new JsonResult(status);
                            }
                        }
                    }
                }

                foreach(Conexao conexao in sim)
                {
                    foreach(Conexao con in sim)
                    {
                        if((conexao.IdMaq == con.IdMaq) && !(conexao.IdSim == con.IdSim && conexao.IdJob == con.IdJob && conexao.IdOp == con.IdOp))
                        {
                            if(conexao.TempoInicial >= con.TempoInicial && conexao.TempoInicial < con.TempoInicial + con.Duracao)
                            {
                                status.status_value = 0;
                                status.status_text = "Job " + conexao.IdJob + " Op " + conexao.IdOp + " tem uma maquina atribuida que esta ocupada.";

                                return new JsonResult(status);
                            }
                        }
                    }
                }

                status.status_value = 1;
                status.status_text = TempoConclusao.ToString();

                return new JsonResult(status);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        #endregion

        #region POST

        /// <summary>
        /// Método que adiciona uma nova conexao na base de dados
        /// </summary>
        /// <param name="con"> Informação da conexao </param>
        /// <returns> Resultado do método </returns>
        [HttpPost, Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Post(Conexao con)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Conexao conexao = new Conexao();

                    conexao.IdUser = con.IdUser;
                    conexao.IdSim = con.IdSim;
                    conexao.IdJob = con.IdJob;
                    conexao.IdOp = con.IdOp;
                    conexao.IdMaq = con.IdMaq;
                    conexao.Duracao = con.Duracao;
                    conexao.Estado = true;
                    conexao.TempoInicial = con.TempoInicial;

                    context.Conexao.Add(conexao);
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

        #region UPDATE

        /// <summary>
        /// Método que atualiza a informação de uma certa máquina e duracao pertencente a uma conexão
        /// </summary>
        /// <param name="con"> conexao </param>
        /// <returns> Resultado do método </returns>
        [HttpPatch, Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Patch([FromBody]Conexao con)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        Utilizador user = context.Utilizador.Where(u => u.IdUser == con.IdUser && u.Estado != "Inativo").FirstOrDefault();

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }

                    Conexao conexao = context.Conexao.Where(c => c.IdUser == con.IdUser && c.IdSim == con.IdSim && c.IdJob == con.IdJob && c.IdOp == con.IdOp).FirstOrDefault();

                    conexao.IdMaq = con.IdMaq is null ? conexao.IdMaq : con.IdMaq;
                    conexao.Duracao = con.Duracao is null ? conexao.Duracao : con.Duracao;
                    conexao.TempoInicial = con.TempoInicial is null ? conexao.TempoInicial : con.TempoInicial;

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

        /// Método que remove uma maquina da base de dados
        /// </summary>
        /// <param name="id_maquina"> ID da maquina </param>
        /// <returns> Resultado do método </returns>
        [HttpDelete("{id_utilizador}/{id_simulacao}"), Authorize(Roles = "Admin")]
        public IActionResult DeleteByAdmin(int id_utilizador, int id_simulacao)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    List<Conexao> conexoes = context.Conexao.Where(c => c.IdSim == id_simulacao && c.IdUser == id_utilizador).ToList();

                    foreach (Conexao con in conexoes)
                    {
                        con.Estado = false;
                    }

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
        /// Método que altera o estado de uma conexão para inativo ("remove a conexão")
        /// </summary>
        /// <param name="id_utilizador"> id do utilizador </param>
        /// <param name="id_simulacao"> id do simulacao </param>
        /// <returns> Resultado do metodo </returns>
        [HttpDelete("deletesim/{id_utilizador}/{id_simulacao}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Delete(int id_utilizador, int  id_simulacao)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        Utilizador user = context.Utilizador.Where(u => u.IdUser == id_utilizador && u.Estado != "Inativo").FirstOrDefault();

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }

                    List<Conexao> conexoes = context.Conexao.Where(c => c.IdSim == id_simulacao && c.IdUser == id_utilizador).ToList();

                    foreach (Conexao con in conexoes)
                    {
                        con.Estado = false;
                    }

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

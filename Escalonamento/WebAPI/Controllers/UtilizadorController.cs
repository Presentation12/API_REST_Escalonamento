using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Escalonamento.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Escalonamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public UtilizadorController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        #region AUXILIARY METHODS

        /// <summary>
        /// Método que verifica se a conta inserida existe
        /// </summary>
        /// <param name="utilizador"> Utilizador </param>
        /// <returns> Resultado da verificação </returns>
        public static bool VerifyAccount(Utilizador utilizador)
        {
            using (var context = new EscalonamentoContext())
            {
                Utilizador user = context.Utilizador.FirstOrDefault(aux => aux.Mail == utilizador.Mail);

                if (user == null)
                {
                    throw new ArgumentException("Cliente não existe!", "account");
                }
                else
                {
                    if (!HashSaltPW.VerifyPasswordHash(utilizador.PassHash, Convert.FromBase64String(user.PassHash), Convert.FromBase64String(user.PassSalt)))
                    {
                        throw new ArgumentException("Password Errada.", "account");
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Método para gerar token de sessão para o utilizador em causa
        /// </summary>
        /// <param name="utilizador"> Utilizador </param>
        /// <returns> Token JWT Bearer </returns>
        private string CreateToken(Utilizador utilizador)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, utilizador.Mail),
                new Claim(ClaimTypes.Role, "Utilizador")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        /// <summary>
        /// Método que esconde a palavra pass do utilizador
        /// </summary>
        /// <param name="utilizador"> Utilizador </param>
        public void HidePassWord(Utilizador utilizador)
        {
            try
            {
                utilizador.PassHash = "Hidden";
                utilizador.PassSalt = "Hidden";
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Método que esconde as palavras pass dos utilizadores todos
        /// </summary>
        /// <param name="utilizadores"> Lista de Utilizadores </param>
        public void HidePassWord(List<Utilizador> utilizadores)
        {
            try
            {
                foreach (Utilizador utilizador in utilizadores)
                {
                    utilizador.PassHash = "Hidden";
                    utilizador.PassSalt = "Hidden";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

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
                    List<Utilizador> utilizadores = context.Utilizador.ToList();
                   // HidePassWord(utilizadores);

                    return utilizadores;
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
                    Utilizador user = context.Utilizador.Where(u => u.IdUser == id).FirstOrDefault();
                    //HidePassWord(user);

                    return user;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Login do utilizador
        /// </summary>
        /// <param name="account"> Utilizador </param>
        /// <returns> Estado do método </returns>
        [Route("Login")]
        [HttpGet]
        public IActionResult Login(Utilizador utilizador)
        {
            using (var context = new EscalonamentoContext())
            {
                try
                {
                    Utilizador user = context.Utilizador.FirstOrDefault(aux => aux.Mail == utilizador.Mail);

                    VerifyAccount(utilizador);

                    string token = CreateToken(utilizador);
                    return new JsonResult(token);
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae);
                    return new JsonResult(ae.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new JsonResult(e);
                }
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

                    HashSaltPW.CreatePasswordHash(uti.PassHash, out byte[] passwordHash, out byte[] passwordSalt);
                    utilizador.PassHash = Convert.ToBase64String(passwordHash);
                    utilizador.PassSalt = Convert.ToBase64String(passwordSalt);

                    utilizador.Aut = uti.Aut;
                    utilizador.Estado = uti.Estado;

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
                    utilizador.Aut = uti.Aut is null ? utilizador.Aut : uti.Aut;
                    utilizador.Estado = uti.Estado is null ? utilizador.Estado : uti.Estado;

                    //Mudar mail no token do user

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

        /// <summary>
        /// Método de recuperação/update da palavra pass do utilizador
        /// </summary>
        /// <param name="utilizador"> Utilizador </param>
        /// <returns> Estado do método </returns>
        [Route("recoverpassword")]
        [HttpPatch, Authorize(Roles = "Utilizador")]
        public IActionResult RecoverPassword(Utilizador utilizador)
        {
            using (var context = new EscalonamentoContext())
            {
                try
                {
                    string mailUtilizador = User.FindFirstValue(ClaimTypes.Email);

                    Utilizador user = context.Utilizador.Where(c => c.Mail == utilizador.Mail && c.Estado == "Ativo").FirstOrDefault();

                    if (user == null) return new JsonResult("Email nao encontrado");

                    if (mailUtilizador != user.Mail)
                    {
                        return Forbid();
                    }

                    HashSaltPW.CreatePasswordHash(utilizador.PassHash, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PassHash = Convert.ToBase64String(passwordHash);
                    user.PassSalt = Convert.ToBase64String(passwordSalt);

                    context.SaveChanges();
                    return new JsonResult("Password atualizada com sucesso");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new JsonResult("Error");
                }
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

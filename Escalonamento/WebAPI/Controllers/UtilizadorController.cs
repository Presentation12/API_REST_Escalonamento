using System.IO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data;
using Escalonamento.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


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
                Utilizador user = context.Utilizador.FirstOrDefault(aux => aux.Mail == utilizador.Mail && aux.Estado != "Inativo");

                if (user == null)
                {
                    throw new ArgumentException("Utilizador não existe!", "account");
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
        /// Método para gerar tolem para admin
        /// </summary>
        /// <param name="utilizador"></param>
        /// <returns></returns>
        private string CreateTokenAdmin(Utilizador utilizador)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, utilizador.Mail),
                new Claim(ClaimTypes.Role, "Admin")
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
        /// Método para gerar token de sessão para o utilizador em causa
        /// </summary>
        /// <param name="utilizador"> Utilizador </param>
        /// <returns> Token JWT Bearer </returns>
        private string CreateTokenUser(Utilizador utilizador)
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
            catch (Exception e)
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
        [HttpGet, Authorize(Roles = "Admin")]
        public IEnumerable<Utilizador> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    List<Utilizador> utilizadores = context.Utilizador.ToList();
                    HidePassWord(utilizadores);

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
        /// Obter User pela sua token
        /// </summary>
        /// <returns></returns>
        [HttpGet("getuserbytoken"), Authorize(Roles = "Utilizador, Admin")]
        public IActionResult GetUserByToken()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    string MailUser = User.FindFirstValue(ClaimTypes.Email);
                    Utilizador user = context.Utilizador.Where(u => u.Mail == MailUser).FirstOrDefault();
                    if (user == null) return BadRequest();
                    return new JsonResult(user);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }

        }

        /// <summary>
        /// Método que devolve a um utilizador específico
        /// </summary>
        /// <param name="id"> ID do utilizador </param>
        /// <returns> Utilizador em específico </returns>
        [HttpGet("{id}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Get(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        Console.WriteLine(UserMail);

                        Utilizador utilizador = context.Utilizador.Where(u => u.IdUser == id && u.Estado != "Inativo").FirstOrDefault();

                        if (utilizador == null) return BadRequest();

                        if (UserMail != utilizador.Mail)
                        {
                            return Forbid();
                        }
                    }

                    Utilizador user = context.Utilizador.Where(u => u.IdUser == id).FirstOrDefault();
                    HidePassWord(user);

                    return new JsonResult(user);
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

        struct login
        {
            public string token { get; set; }
            public string role { get; set; }
        }

        /// <summary>
        /// Login do utilizador
        /// </summary>
        /// <param name="account"> Utilizador </param>
        /// <returns> Estado do método </returns>
        [Route("Login")]
        [HttpPost]
        public IActionResult Login(Utilizador utilizador)
        {
            using (var context = new EscalonamentoContext())
            {
                try
                {
                    login login = new login();
                    Utilizador user = context.Utilizador.FirstOrDefault(aux => aux.Mail == utilizador.Mail);

                    VerifyAccount(utilizador);

                    string token;

                    if (user.Aut == false) token = CreateTokenUser(utilizador);
                    else token = CreateTokenAdmin(utilizador);

                    login.token = token;
                    if (user.Aut == false) login.role = "Utilizador";
                    else login.role = "Admin";

                    return new JsonResult(login);
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae);
                    return new JsonResult(ae.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return BadRequest();
                }
            }
        }

        /// <summary>
        /// Método que adiciona um novo utilizador na base de dados
        /// </summary>
        /// <param name="uti"> Informação do utilizador </param>
        /// <returns> Resultado do método </returns>
        [HttpPost]
        public IActionResult Post(Utilizador uti)
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
                    utilizador.Estado = "Ati";

                    context.Utilizador.Add(utilizador);
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
        /// Método que atualiza a informação de um utilizador
        /// </summary>
        /// <param name="id"> ID do utilizador alvo </param>
        /// <param name="value"> Informação nova do utilizador </param>
        [HttpPatch("{id}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Patch(int id, [FromBody] Utilizador uti)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        Utilizador user = context.Utilizador.Where(u => u.IdUser == id && u.Estado != "Inativo").FirstOrDefault();

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }

                    Utilizador utilizador = context.Utilizador.Where(u => u.IdUser == id).FirstOrDefault();

                    utilizador.Mail = uti.Mail is null ? utilizador.Mail : uti.Mail;
                    utilizador.Aut = uti.Aut is null ? utilizador.Aut : uti.Aut;
                    utilizador.Estado = uti.Estado is null ? utilizador.Estado : uti.Estado;

                    if (utilizador.Aut == false) CreateTokenUser(utilizador);
                    else CreateTokenAdmin(utilizador);

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
        /// Método de recuperação/update da palavra pass do utilizador
        /// </summary>
        /// <param name="utilizador"> Utilizador </param>
        /// <returns> Estado do método </returns>
        [Route("recoverpassword/{id}")]
        [HttpPatch, Authorize(Roles = "Admin ,Utilizador")]
        public IActionResult RecoverPassword(int id, Utilizador utilizador)
        {
            using (var context = new EscalonamentoContext())
            {
                try
                {
                    Utilizador user = context.Utilizador.Where(c => c.IdUser == id && c.Estado != "Inativo").FirstOrDefault();

                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string mailUtilizador = User.FindFirstValue(ClaimTypes.Email);

                        if (user == null) return BadRequest();

                        if (mailUtilizador != user.Mail)
                        {
                            return Forbid();
                        }
                    }
                    else if (User.HasClaim(ClaimTypes.Role, "Admin"))
                    {
                        if (user == null) return BadRequest();
                    }


                    HashSaltPW.CreatePasswordHash(utilizador.PassHash, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PassHash = Convert.ToBase64String(passwordHash);
                    user.PassSalt = Convert.ToBase64String(passwordSalt);

                    context.SaveChanges();
                    return Ok();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return BadRequest();
                }
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Método que permite arquivar um utilizador
        /// </summary>
        /// <param name="id_utilizador"> ID do Utilizador </param>
        /// <returns> Estado do método </returns>
        [HttpDelete("delete/{id}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult ArquivarUtilizador(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Utilizador user = context.Utilizador.Where(u => u.IdUser == id && u.Estado != "Inativo").FirstOrDefault();

                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        if (user == null) return BadRequest();
                        Console.WriteLine(user.Mail);
                        Console.WriteLine(UserMail);
                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }
                    else if (User.HasClaim(ClaimTypes.Role, "Admin"))
                    {
                        if (user == null) return BadRequest();
                    }

                    user.Estado = "Inativo";

                    List<Conexao> conns = context.Conexao.Where(c => c.IdUser == user.IdUser).ToList();

                    foreach (Conexao con in conns)
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
        /// Método que remove um utilizador da base de dados
        /// </summary>
        /// <param name="id"> ID do utilizador </param>
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Utilizador utilizador = context.Utilizador.Where(u => u.IdUser == id).FirstOrDefault();

                    context.Utilizador.Remove(utilizador);

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

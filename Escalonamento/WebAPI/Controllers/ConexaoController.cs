﻿using Escalonamento.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet, Authorize(Roles = "Admin")]
        public IEnumerable<Conexao> GetActives()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Conexao.Where(c => c.Estado == true).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Método que devolve a lista inteira das conexões
        /// </summary>
        /// <returns> conexões na base de dados </returns>
        [HttpGet, Authorize(Roles = "Admin")]
        public IEnumerable<Conexao> GetInactives()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Conexao.ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Método que devolve as conexões passada de um utilizador pelo seu id
        /// </summary>
        /// <param name="id_utilizador"> ID da utilizador </param>
        /// <returns> Conexao </returns>
        [HttpGet("{id_utilizador}"), Authorize(Roles = "Admin, Utilizador")]
        public IEnumerable<Conexao> GetByIdUser(int id_utilizador)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Conexao.Where(c => c.IdUser == id_utilizador && c.Estado == true).ToList();
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
        [HttpDelete("{id_utilizador}/{id_simulacao}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Delete(int id_utilizador, int  id_simulacao)
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

        #endregion
    }
}
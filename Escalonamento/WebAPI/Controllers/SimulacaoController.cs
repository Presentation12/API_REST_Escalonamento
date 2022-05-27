using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Escalonamento.Models;
using Google.OrTools.Sat;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Escalonamento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulacaoController : ControllerBase
    {
        #region GET

        /// <summary>
        /// Método que retorna lista de simulações
        /// </summary>
        /// <returns> Lista de simulações </returns>
        [HttpGet, Authorize(Roles = "Admin")]
        public IEnumerable<Simulacao> Get()
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    return context.Simulacao.ToList();
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Método que retorna uma simulação em específico
        /// </summary>
        /// <param name="id"> ID da simulação </param>
        /// <returns> Simulação </returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao sim = context.Simulacao.Where(s => s.IdSim == id && s.Estado != "Inativo").FirstOrDefault();
                    Utilizador user = context.Utilizador.Where(u => u.IdUser == sim.IdUser && u.Estado != "Inativo").FirstOrDefault();

                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }

                        return new JsonResult(context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault());
                    }
                    else if (User.HasClaim(ClaimTypes.Role, "Admin"))
                    {
                        return new JsonResult(context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault());
                    }
                    else return BadRequest();

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
        /// Método que adiciona uma simulação à base de dados
        /// </summary>
        /// <param name="sim"> Informação da simulação </param>
        /// <returns> Resultado do método </returns>
        [HttpPost("{idUser}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Post(int idUser, [FromBody] Simulacao sim)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        Utilizador user = context.Utilizador.Where(u => u.IdUser == idUser && u.Estado != "Inativo").FirstOrDefault();

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }

                    Simulacao simulacao = new Simulacao();

                    simulacao.IdSim = sim.IdSim;
                    simulacao.Estado = sim.Estado;
                    simulacao.IdUserNavigation = context.Utilizador.Where(u => u.IdUser == idUser).FirstOrDefault();

                    context.Simulacao.Add(simulacao);

                    context.SaveChanges();
                    return Ok();
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        #endregion

        #region PATCH

        /// <summary>
        /// Método que atualiza a informação de uma certa simulação
        /// </summary>
        /// <param name="id"> ID da simulação </param>
        /// <param name="sim"> Informação da simulação </param>
        /// <returns> Estado do método </returns>
        [HttpPatch("{idSim}"), Authorize(Roles = "Admin, Utilizador")]
        public IActionResult Patch(int idSim, [FromBody] Simulacao sim)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        Utilizador user = context.Utilizador.Where(u => u.IdUser == sim.IdUser && u.Estado != "Inativo").FirstOrDefault();

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }

                    Simulacao simulacao = context.Simulacao.Where(s => s.IdSim == idSim).FirstOrDefault();

                    simulacao.Estado = sim.Estado is null ? simulacao.Estado : sim.Estado;
                    if(sim.IdUser != 0) simulacao.IdUserNavigation = context.Utilizador.Where(u => u.IdUser == sim.IdUser).FirstOrDefault();

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
        /// Método que arquiva uma simulação
        /// </summary>
        /// <param name="id"> ID da simulação </param>
        /// <returns> Estado do método </returns>
        [HttpPatch("delete/{id}")]
        public IActionResult ArquivarSimulacao(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao simulacao = context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault();

                    if (User.HasClaim(ClaimTypes.Role, "Utilizador"))
                    {
                        string UserMail = User.FindFirstValue(ClaimTypes.Email);

                        Utilizador user = context.Utilizador.Where(u => u.IdUser == simulacao.IdUser && u.Estado != "Inativo").FirstOrDefault();

                        if (user == null) return BadRequest();

                        if (UserMail != user.Mail)
                        {
                            return Forbid();
                        }
                    }

                    simulacao.Estado = "Inativo";

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
        /// Método que elimina uma certa simulação da base de dados
        /// </summary>
        /// <param name="id"> ID da simulação </param>
        /// <returns> Estado do método </returns>
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                using (var context = new EscalonamentoContext())
                {
                    Simulacao sim = context.Simulacao.Where(s => s.IdSim == id).FirstOrDefault();

                    context.Simulacao.Remove(sim);

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
        
        #region ALGORITHM

        //Esquecer este algoritmo
        //Arranjar forma de passar os tempos para cada operacao
        public void algoritmo(int numMaquinas, int numJobs, int numOperations)
        {
            //ALTERAR, ENVIAR DADOS POR PARAMETROS
            var allJobs =
                new[] {
                    new[] {
                        // job0
                        new { machine = 0, duration = 3 }, // task0
                        new { machine = 1, duration = 2 }, // task1
                        new { machine = 2, duration = 2 }, // task2
                    }
                        .ToList(),
                    new[] {
                        // job1
                        new { machine = 0, duration = 2 }, // task0
                        new { machine = 2, duration = 1 }, // task1
                        new { machine = 1, duration = 4 }, // task2
                    }
                        .ToList(),
                    new[] {
                        // job2
                        new { machine = 1, duration = 4 }, // task0
                        new { machine = 2, duration = 3 }, // task1
                    }.ToList(),
                }.ToList();

            int numMachines = 0;
            foreach (var job in allJobs)
            {
                foreach (var task in job)
                {
                    numMachines = Math.Max(numMachines, 1 + task.machine);
                }
            }
            int[] allMachines = Enumerable.Range(0, numMachines).ToArray();

            // Computes horizon dynamically as the sum of all durations.
            int horizon = 0;
            foreach (var job in allJobs)
            {
                foreach (var task in job)
                {
                    horizon += task.duration;
                }
            }

            CpModel model = new CpModel();

            Dictionary<Tuple<int, int>, Tuple<IntVar, IntVar, IntervalVar>> allTasks =
            new Dictionary<Tuple<int, int>, Tuple<IntVar, IntVar, IntervalVar>>(); // (start, end, duration)
            Dictionary<int, List<IntervalVar>> machineToIntervals = new Dictionary<int, List<IntervalVar>>();
            for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
            {
                var job = allJobs[jobID];
                for (int taskID = 0; taskID < job.Count(); ++taskID)
                {
                    var task = job[taskID];
                    String suffix = $"_{jobID}_{taskID}";
                    IntVar start = model.NewIntVar(0, horizon, "start" + suffix);
                    IntVar end = model.NewIntVar(0, horizon, "end" + suffix);
                    IntervalVar interval = model.NewIntervalVar(start, task.duration, end, "interval" + suffix);
                    var key = Tuple.Create(jobID, taskID);
                    allTasks[key] = Tuple.Create(start, end, interval);
                    if (!machineToIntervals.ContainsKey(task.machine))
                    {
                        machineToIntervals.Add(task.machine, new List<IntervalVar>());
                    }
                    machineToIntervals[task.machine].Add(interval);
                }
            }

            // Create and add disjunctive constraints.
            foreach (int machine in allMachines)
            {
                model.AddNoOverlap(machineToIntervals[machine]);
            }

            // Precedences inside a job.
            for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
            {
                var job = allJobs[jobID];
                for (int taskID = 0; taskID < job.Count() - 1; ++taskID)
                {
                    var key = Tuple.Create(jobID, taskID);
                    var nextKey = Tuple.Create(jobID, taskID + 1);
                    model.Add(allTasks[nextKey].Item1 >= allTasks[key].Item2);
             
                    //Está em Python !!!
                   // model.Add(allTasks[job, taskID + 1].start >= allTasks[job, taskID].end);

                }
            }

            // Makespan objective.
            IntVar objVar = model.NewIntVar(0, horizon, "makespan");

            List<IntVar> ends = new List<IntVar>();
            for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
            {
                var job = allJobs[jobID];
                var key = Tuple.Create(jobID, job.Count() - 1);
                ends.Add(allTasks[key].Item2);
            }
            model.AddMaxEquality(objVar, ends);
            model.Minimize(objVar);

            //Está em Python !!! 
           // model.AddMaxEquality(obj_var,[all_tasks[(job, len(jobs_data[job]) - 1)].end for job in all_jobs]) ;

            CpSolver solver = new CpSolver();
            CpSolverStatus status = solver.Solve(model);
            Console.WriteLine($"Solve status: {status}");

            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                Console.WriteLine("Solution:");

                //Dictionary<int, List<AssignedTask>> assignedJobs = new Dictionary<int, List<AssignedTask>>();
                for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
                {
                    var job = allJobs[jobID];
                    for (int taskID = 0; taskID < job.Count(); ++taskID)
                    {
                        var task = job[taskID];
                        var key = Tuple.Create(jobID, taskID);
                        int start = (int)solver.Value(allTasks[key].Item1);
                      /*  if (!assignedJobs.ContainsKey(task.machine))
                        {
                            assignedJobs.Add(task.machine, new List<AssignedTask>());
                        }
                        assignedJobs[task.machine].Add(new AssignedTask(jobID, taskID, start, task.duration));*/
                    }
                }

                // Create per machine output lines.
                String output = "";
                foreach (int machine in allMachines)
                {
                    // Sort by starting time.
                    //assignedJobs[machine].Sort();
                    String solLineTasks = $"Machine {machine}: ";
                    String solLine = "           ";

                    /*foreach (var assignedTask in assignedJobs[machine])
                    {
                        String name = $"job_{assignedTask.jobID}_task_{assignedTask.taskID}";
                        // Add spaces to output to align columns.
                        solLineTasks += $"{name,-15}";

                        String solTmp = $"[{assignedTask.start},{assignedTask.start + assignedTask.duration}]";
                        // Add spaces to output to align columns.
                        solLine += $"{solTmp,-15}";
                    }*/
                    output += solLineTasks + "\n";
                    output += solLine + "\n";
                }
                // Finally print the solution found.
                Console.WriteLine($"Optimal Schedule Length: {solver.ObjectiveValue}");
                Console.WriteLine($"\n{output}");
            }
            else
            {
                Console.WriteLine("No solution found.");
            }
        }

        #endregion
    }
}

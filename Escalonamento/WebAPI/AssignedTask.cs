﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Escalonamento.Models;
using Google.OrTools.Sat;
using Microsoft.AspNetCore.Mvc;

namespace Escalonamento
{
    public class AssignedTask : IComparable
    {
        public int jobID;
        public int taskID;
        public int start;
        public int duration;

        public AssignedTask(int jobID, int taskID, int start, int duration)
        {
            this.jobID = jobID;
            this.taskID = taskID;
            this.start = start;
            this.duration = duration;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            AssignedTask otherTask = obj as AssignedTask;
            if (otherTask != null)
            {
                if (this.start != otherTask.start)
                    return this.start.CompareTo(otherTask.start);
                else
                    return this.duration.CompareTo(otherTask.duration);
            }
            else
                throw new ArgumentException("Object is not a Temperature");
        }

        public struct Row
        {
            public int machine;
            public int duration;
        }

        public struct OutputFrontend
        {
            public double DuracaoTotal;
            public string output;
            public double conflicts;
            public double branches;
            public double wallTime;
        }

        public static IActionResult AlgoritmoEscalonamento(int IdUser, int IdSim)
        {
            using (var context = new EscalonamentoContext())
            {
                var allJobs = new List<List<Row>>();
                Row op = new Row();
                List<Row> job_row;
                List<Conexao> simulacao = context.Conexao.Where(s => s.IdSim == IdSim && s.IdUser == IdUser).ToList();
                Conexao lastIDJob = context.Conexao.Where(c => c.IdSim == IdSim && c.IdUser == IdUser)
                .OrderBy(c => c.IdUser).ThenBy(c=> c.IdSim).ThenBy(c=> c.IdJob).ThenBy(c => c.IdOp).LastOrDefault();
                OutputFrontend outputFrontend = new OutputFrontend();

                for(int i = 1; i <= lastIDJob.IdJob; i++)
                {
                    job_row = new List<Row>();
                    foreach (Conexao con in simulacao)
                    {
                        if (con.IdJob == i)
                        {
                            op.machine = (int)con.IdMaq;
                            op.duration = (int)con.Duracao;

                            job_row.Add(op);
                        }
                    }

                    allJobs.Add(job_row);
                }

                int numMachines = 0;
                foreach (var job in allJobs)
                {
                    foreach (var task in job)
                    {
                        numMachines = Math.Max(numMachines, task.machine);
                    }
                }
                int[] allMachines = Enumerable.Range(1, numMachines).ToArray();

                // Computes horizon dynamically as the sum of all durations.
                int horizon = 0;
                foreach (var job in allJobs)
                {
                    foreach (var task in job)
                    {
                        horizon += task.duration;
                    }
                }

                // Creates the model.
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

                // Solve
                CpSolver solver = new CpSolver();
                CpSolverStatus status = solver.Solve(model);
                Console.WriteLine($"Solve status: {status}");

                if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
                {
                    Console.WriteLine("Solution:");

                    Dictionary<int, List<AssignedTask>> assignedJobs = new Dictionary<int, List<AssignedTask>>();
                    for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
                    {
                        var job = allJobs[jobID];
                        for (int taskID = 0; taskID < job.Count(); ++taskID)
                        {
                            var task = job[taskID];
                            var key = Tuple.Create(jobID, taskID);
                            int start = (int)solver.Value(allTasks[key].Item1);
                            if (!assignedJobs.ContainsKey(task.machine))
                            {
                                assignedJobs.Add(task.machine, new List<AssignedTask>());
                            }
                            assignedJobs[task.machine].Add(new AssignedTask(jobID, taskID, start, task.duration));
                        }
                    }

                    // Create per machine output lines.
                    String output = "";
                    foreach (int machine in allMachines)
                    {
                        // Sort by starting time.
                        assignedJobs[machine].Sort();
                        String solLineTasks = $"Machine {machine}: ";
                        String solLine = "           ";

                        foreach (var assignedTask in assignedJobs[machine])
                        {
                            String name = $"job_{assignedTask.jobID+1}_task_{assignedTask.taskID+1}";
                            // Add spaces to output to align columns.
                            solLineTasks += $"{name,-15}";

                            String solTmp = $"[{assignedTask.start},{assignedTask.start + assignedTask.duration}]";
                            // Add spaces to output to align columns.
                            solLine += $"{solTmp,-15}";
                        }
                        output += solLineTasks + "\n";
                        output += solLine + "\n";
                    }
                    // Finally print the solution found.
                    Console.WriteLine($"Optimal Schedule Length: {solver.ObjectiveValue}");
                    Console.WriteLine($"\n{output}");

                    outputFrontend.DuracaoTotal = solver.ObjectiveValue;
                    outputFrontend.output = output;
                }
                else
                {
                    Console.WriteLine("No solution found.");
                }

                Console.WriteLine("Statistics");
                Console.WriteLine($"  conflicts: {solver.NumConflicts()}");
                Console.WriteLine($"  branches : {solver.NumBranches()}");
                Console.WriteLine($"  wall time: {solver.WallTime()}s");

                outputFrontend.conflicts = solver.NumConflicts();
                outputFrontend.branches = solver.NumBranches();
                outputFrontend.wallTime = solver.WallTime();

                return new JsonResult(outputFrontend);
            }
        }
    }
}

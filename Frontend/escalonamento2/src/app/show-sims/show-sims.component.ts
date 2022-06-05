import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-show-sims',
  templateUrl: './show-sims.component.html',
  styleUrls: ['./show-sims.component.css']
})
export class ShowSimsComponent implements OnInit {

  constructor(private service: SharedService) { }

  ConexoesSimulacao: any = [];
  User: any = {};
  SimulacaoSelectedId: any;
  state: number = 0;
  SelectConexao: any = {};
  SelectedMachine: any = {
    IdMaq: "",
    Duracao: ""
  }


  //valores a mandar no update de uma cell + SimulacaoSelectedId + User.IdUser
  IdJob: any;
  IdOp: any;
  IdMaq: any;
  Duracao: any;
  NewCell: any = [];
  IdJobAux: any;
  IdOpAux: any;
  EstadoPlano:any={};

  Maquinas: any;

  getmaquinas() {
    this.service.GetMaquina().subscribe(data => {
      this.Maquinas = data;
    })
  }

  pesquisaMaquina() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data
    })

    this.SelectConexao = {
      IdUser: `${this.User.IdUser}`,
      IdSim: `${this.SimulacaoSelectedId}`,
      IdJob: `${this.IdJobAux}`,
      IdOp: `${this.IdOpAux}`
    }

    this.service.GetMaquinaByJobOp(this.SelectConexao).subscribe(
      data =>
        this.SelectedMachine = data,
      error => alert("Erro: Máquina não encontrada")
    )
  }

  Output: any = {};

  executaPlano() {
    let canExecute = 1
    for(let i = 0; i < this.ConexoesSimulacao.length; i++){
      if(this.ConexoesSimulacao[i].IdMaq == null){
        alert(`Maquina indisponivel: Job ${this.ConexoesSimulacao[i].IdJob} Operation ${this.ConexoesSimulacao[i].IdOp}`)
        canExecute = 0;
      }
    }

    if(canExecute == 1){
      this.service.GetUserByToken().subscribe(data => {
        this.User = data

        this.service.PlanearSim(this.User.IdUser, this.SimulacaoSelectedId).subscribe(data => {
          this.Output = data;
          var content = `Duração total: ${this.Output.DuracaoTotal}\n\n${this.Output.output}\n\nConflicts: ${this.Output.conflicts}\nBranches: ${this.Output.branches}\nWall Time: ${this.Output.wallTime}`;

          content = "data:application/txt, " + encodeURIComponent(content);
          var x = document.createElement("A");
          x.setAttribute("href", content);
          x.setAttribute("download", "plano.txt");
          document.body.appendChild(x);
          x.click();
        });
      })
    }
  }

  ExecutaPlanoManual(){
    let canExecute = 1;
    let tempoPlanoTotal ="";

    for(let i = 0; i < this.ConexoesSimulacao.length; i++){
      if(this.ConexoesSimulacao[i].TempoInicial == null){
        alert(`Campo por preencher: linha ${i+1}`)
        canExecute = 0;
      }

      if(this.ConexoesSimulacao[i].TempoInicial < 0){
        alert(`Campo Invalido: linha ${i+1}`)
        canExecute = 0;
      }

      if(this.ConexoesSimulacao[i].IdMaq == null){
        alert(`Maquina indisponivel: linha ${i+1}`)
        canExecute = 0;
      }
    }

    if(canExecute == 1) {
      this.service.PlanearSimManual(this.ConexoesSimulacao).subscribe(data => {
        this.EstadoPlano = data;

        if(this.EstadoPlano.status_value == 0){
          alert(this.EstadoPlano.status_text)
        }
        else if(this.EstadoPlano.status_value == 1){
          tempoPlanoTotal = this.EstadoPlano.status_text;

          var content = `Duracao total: ${this.EstadoPlano.status_text}\n\n`;
          let check;
          var solLine = "           ";
          var solLineJob = "               ";

          for(let i = 0; i < this.Maquinas.length; i++){
            check = 0;

            for(let j = 0; j < this.ConexoesSimulacao.length; j++){

              if(check == 0 && this.ConexoesSimulacao[j].IdMaq == this.Maquinas[i].IdMaq){
                content += `Machine ${this.ConexoesSimulacao[j].IdJob}: `
                check = 1;
              }

              if(this.ConexoesSimulacao[j].IdMaq == this.Maquinas[i].IdMaq)
                content += `job_${this.ConexoesSimulacao[j].IdJob}_task_${this.ConexoesSimulacao[i].IdOp}`+solLine
            }
            content += "\n"+solLineJob

            for(let j = 0; j < this.ConexoesSimulacao.length; j++){
              if(this.ConexoesSimulacao[j].IdMaq == this.Maquinas[i].IdMaq)
                content += `[${this.ConexoesSimulacao[j].TempoInicial}, ${this.ConexoesSimulacao[i].TempoInicial+this.ConexoesSimulacao[i].Duracao}]`+solLine
            }
            content += "\n"
          }

          content = "data:application/txt, " + encodeURIComponent(content);
          var x = document.createElement("A");
          x.setAttribute("href", content);
          x.setAttribute("download", "planomanual.txt");
          document.body.appendChild(x);
          x.click();
        }
      });
    }
  }

  refreshCliente() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;
    })
  }

  download() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;

      if (this.SimulacaoSelectedId == "---" || this.SimulacaoSelectedId == undefined) {
        alert("Selecione uma simulação para fazer download!")
      }
      else {
        this.service.GetSimulacaoByUser(this.User.IdUser, this.SimulacaoSelectedId).subscribe(data => {
          this.ConexoesSimulacao = data;
        });

        var content = "ID Job;ID Operacao;ID Maquina;Duracao\r";

        for (let i = 0; i < this.ConexoesSimulacao.length; i++) {
          content += `${this.ConexoesSimulacao[i].IdJob};${this.ConexoesSimulacao[i].IdOp};${this.ConexoesSimulacao[i].IdMaq};${this.ConexoesSimulacao[i].Duracao}\r`;
        }

        content = "data:application/csv, " + encodeURIComponent(content);
        var x = document.createElement("A");
        x.setAttribute("href", content);
        x.setAttribute("download", "simulacao.csv");
        document.body.appendChild(x);
        x.click();
      }
    })
  }

  refreshPage(): void {
    window.location.reload();
  }

  //remover uma simulação de um user
  removeSimulation() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;

      if (this.SimulacaoSelectedId == "---" || this.SimulacaoSelectedId == undefined) {
        alert("Simulação não selecionada");
      }
      else {
        var c = confirm("Você tem a certeza?");
        if (c == true) {
          this.service.DeleteSimulationByUser(this.User.IdUser, this.SimulacaoSelectedId).subscribe();
          alert(`Simulação ${this.SimulacaoSelectedId} removida com sucesso`);
          this.refreshPage();
        }
      }
    });
  }

  // alterar dados de uma celula da tabela (id da maquina e duracao)
  updateCell() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;

      this.NewCell = {
        IdUser: `${this.User.IdUser}`,
        IdSim: `${this.SimulacaoSelectedId}`,
        IdJob: `${this.IdJob}`,
        IdOp: `${this.IdOp}`,
        IdMaq: `${this.IdMaq}`,
        Duracao: `${this.Duracao}`
      }
      if (this.NewCell.IdSim != "undefined" && this.NewCell.IdJob != "undefined" && this.NewCell.IdOp != "undefined" && this.NewCell.IdMaq != "undefined" && this.NewCell.Duracao != "undefined") {

        this.service.UpdateCellByUser(this.NewCell).subscribe();
        alert("Sucesso");
        this.refreshPage();
      }
      else {
        alert("Campos vazios!");
      }

    });
  }

  //buscar todas as conexoes de uma simulacao
  refreshTable() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;

      if (this.SimulacaoSelectedId != "---") {
        this.state = 1;
        this.service.GetSimulacaoByUser(this.User.IdUser, this.SimulacaoSelectedId).subscribe(data => {
          this.ConexoesSimulacao = data;
        });
      }
      else this.state = 0;
    })
  }

  ConexoesSimulacoes: any = [];
  ConexoesSimulacoesIDs: any = [];
  exists: number = 1;

  //buscar todas as conexoes
  refreshSimulacoesIds() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;
      this.service.GetConexoesByUser(this.User.IdUser).subscribe(data => {
        this.ConexoesSimulacoes = data;

        for (let i = 0; i < this.ConexoesSimulacoes.length; i++) {
          if (this.ConexoesSimulacoesIDs.length == 0) {
            this.ConexoesSimulacoesIDs.push(this.ConexoesSimulacoes[i].IdSim)
          }
          else {
            this.exists = 0;

            for (let j = 0; j < this.ConexoesSimulacoesIDs.length; j++) {
              if (this.ConexoesSimulacoesIDs[j] == this.ConexoesSimulacoes[i].IdSim) {
                this.exists = 1
                break;
              }
            }

            if (this.exists == 0) {
              this.ConexoesSimulacoesIDs.push(this.ConexoesSimulacoes[i].IdSim)
            }
          }
        }
      });
    })
  }

  ngOnInit(): void {
    this.refreshCliente();
    this.refreshSimulacoesIds();
    this.getmaquinas();
  }

}

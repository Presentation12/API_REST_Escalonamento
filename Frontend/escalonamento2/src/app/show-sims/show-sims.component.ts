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

  //valores a mandar no update de uma cell + SimulacaoSelectedId + User.IdUser
  IdJob: any;
  IdOp: any;
  IdMaq: any;
  Duracao: any;
  NewCell: any = [];


  Maquinas: any;

  getmaquinas() {
    this.service.GetMaquina().subscribe(data => {
      this.Maquinas = data;
    })
  }

  refreshCliente() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;
    })
  }

  refresh(): void {
    window.location.reload();
  }

  //remover uma simulação de um user
  removeSimulation() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;
      if (this.SimulacaoSelectedId != "---") {
        this.service.DeleteSimulationByUser(this.User.IdUser, this.SimulacaoSelectedId).subscribe();
        this.refresh();
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
        this.refresh();
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

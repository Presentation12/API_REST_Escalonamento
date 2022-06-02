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

  refreshCliente(){
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;
    })
  }

  //buscar todas as conexoes de uma simulacao
  refreshTable() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;

      if(this.SimulacaoSelectedId != "---"){
        this.state = 1;
        this.service.GetSimulacaoByUser(this.User.IdUser, this.SimulacaoSelectedId).subscribe(data=>{
          this.ConexoesSimulacao = data;
        });
      }
      else this.state = 0;
    })
  }

  ConexoesSimulacoes: any = [];
  ConexoesSimulacoesIDs : any = [];
  exists:number = 1;

  //buscar todas as conexoes
  refreshSimulacoes()
  {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;
      this.service.GetConexoesByUser(this.User.IdUser).subscribe(data=>{
        this.ConexoesSimulacoes = data;

        for(let i = 0; i < this.ConexoesSimulacoes.length; i++){
          if(this.ConexoesSimulacoesIDs.length == 0){
            this.ConexoesSimulacoesIDs.push(this.ConexoesSimulacoes[i].IdSim)
          }
          else{
            this.exists = 0;

            for(let j = 0; j < this.ConexoesSimulacoesIDs.length; j++){
              if(this.ConexoesSimulacoesIDs[j] == this.ConexoesSimulacoes[i].IdSim) {
                this.exists = 1
                break;
              }
            }

            if(this.exists == 0){
              this.ConexoesSimulacoesIDs.push(this.ConexoesSimulacoes[i].IdSim)
            }
          }
        }
      });
    })
  }

  ngOnInit(): void {
    this.refreshCliente();
    this.refreshSimulacoes();
  }

}

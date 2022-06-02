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
  //buscar todas as conexoes de uma simulacao
  refreshTable() {
    this.service.GetSimulacaoByUser().subscribe(data=>{
      this.ConexoesSimulacao = data;
    });
  }

  ConexoesSimulacoes: any = [];
  //buscar todas as conexoes
  refreshSimulacoes()
  {
    this.service.GetConexoesByUser().subscribe(data=>{
      this.ConexoesSimulacoes = data;
    });
  }

  ngOnInit(): void {
    this.refreshTable();
    this.refreshSimulacoes();
  }

}

import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-simulacao',
  templateUrl: './simulacao.component.html',
  styleUrls: ['./simulacao.component.css']
})
export class SimulacaoComponent implements OnInit {

  state:number=0;
  selectedSimString:any;
  selectedSim:any={};
  Conta:any={
    Mail:""
  };
  UserSims:any=[];
  JobsNum:any;
  OperationsNum:any;
  MachinesNum:any;
  newSim:any={};
  lastSim:any={IdSim:""};
  newMach:any={};
  tableMachines:any=[];
  newJob:any={};
  tableJobs:any=[];

  constructor(private service: SharedService) { }

  public loadScript(url: any) {
    let node = document.createElement("script");
    node.src = url;
    node.type = 'text/javascript';
    document.body.append(node);
  }

  ngOnInit(): void {
    this.loadScript("../assets/simulacao.js")
    this.refreshUser();
    this.GetSimsUser();
  }

  createSim(){
    this.service.GetUserByToken().subscribe(data => {
      this.Conta = data;
      this.service.AddSimulacao(this.Conta.IdUser, this.newSim).subscribe()
    })
    
    for (let i = 0; i < this.JobsNum; i++) {
      this.service.AddJob(this.newJob).subscribe();
    }

    for(let i = 0; i < this.MachinesNum; i++){
      this.service.AddMaquina(this.newMach).subscribe();
    }
  }

  activateNewTable(){
    this.state = 3;
  }

  selectSim(){
    let stringId = this.selectedSimString.split(" ");
    this.service.GetSimulacaoByID(stringId[1]).subscribe(data => {
      this.selectedSim = data;
    })
  }

  ShowBtn(){
    this.state = 4;
  }

  getLastSim(){
    this.service.GetUserByToken().subscribe(data => {
      this.Conta = data
      this.service.GetLastSimulacaoByUser(this.Conta.IdUser).subscribe(data => {
        this.lastSim = data;
      })
    })
  }

  refreshUser(){
    this.service.GetUserByToken().subscribe(data => {
      this.Conta=data;
    })
  }

  GetSimsUser(){
    this.service.GetUserByToken().subscribe(data => {
      this.Conta=data;
      this.service.GetSimulacoesByUser(this.Conta.IdUser).subscribe(data => {
        this.UserSims = data;
      })
    })
  }

  activateNew(){
    this.state = 1;
  }

  deactivateNew(){
    this.state = 0;
  }

  activateSimGen(){
    this.state = 2;
  }

}

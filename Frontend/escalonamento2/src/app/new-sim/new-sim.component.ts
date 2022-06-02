import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-new-sim',
  templateUrl: './new-sim.component.html',
  styleUrls: ['./new-sim.component.css']
})

export class NewSimComponent implements OnInit {

  constructor(private service: SharedService) { }

  public loadScript(url: any) {
    let node = document.createElement("script");
    node.src = url;
    node.type = 'text/javascript';
    document.body.append(node);
  }

  User:any={};
  NovaConexao : any = {};
  IdSim:any;
  IdJob:any;
  IdOp:any;
  IdMaq:any;
  IdUser:any;
  Duracao:any;
  Maquinas:any=[];

  refreshUser(){
    this.service.GetUserByToken().subscribe(data => {
      this.User = data
    })
  }

  clear(){
    this.IdSim="";
    this.IdJob="";
    this.IdOp="";
    this.IdMaq="";
    this.IdUser="";
    this.Duracao="";
  }

  getmaquinas(){
    this.service.GetMaquina().subscribe(data => {
      this.Maquinas = data;
    })
  }

  SubmitConexao()
  {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data;
      this.NovaConexao = {
        IdUser: `${this.User.IdUser}`,
        IdSim: `${this.IdSim}`,
        IdJob: `${this.IdJob}`,
        IdOp: `${this.IdOp}`,
        IdMaq: `${this.IdMaq}`,
        Duracao: `${this.Duracao}`
      }
      console.log(this.NovaConexao)
      //this.service.PostConexao(this.NovaConexao).subscribe();
      alert(`Adicionou uma conexão à simulacao ${this.NovaConexao.IdSim}`)
      this.clear()
    })
  }

  ngOnInit(): void {
    this.loadScript("../assets/simgenerator.js")
    this.getmaquinas();
  }
}

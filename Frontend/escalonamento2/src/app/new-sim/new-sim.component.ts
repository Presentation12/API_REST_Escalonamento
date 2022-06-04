import { JsonpClientBackend } from '@angular/common/http';
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

  User: any = {};
  NovaConexao: any = {};
  IdSim: any;
  IdJob: any;
  IdOp: any;
  IdMaq: any;
  IdUser: any;
  Duracao: any;
  Maquinas: any = [];

  // para buscar respostas de requests (400, 404, etc)
  ErrorMessage: any;

  refreshPage(): void {
    window.location.reload();
  }

  refreshUser() {
    this.service.GetUserByToken().subscribe(data => {
      this.User = data
    })
  }

  clear() {
    this.IdSim = "";
    this.IdJob = "";
    this.IdOp = "";
    this.IdMaq = "";
    this.IdUser = "";
    this.Duracao = "";
  }

  getmaquinas() {
    this.service.GetMaquina().subscribe(data => {
      this.Maquinas = data;
    })
  }

  SubmitConexao() {
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
      if (this.NovaConexao.IdSim > 0 && this.NovaConexao.IdJob > 0 && this.NovaConexao.IdOp > 0 && this.NovaConexao.Duracao > 0) {
        this.service.PostConexao(this.NovaConexao).subscribe();

        //verificar se retorna nao badrequest (ou seja, não conseguiu inserir -> que a conexao ja existe)
        //if () {
          alert(`Adicionou uma conexão à simulacao ${this.NovaConexao.IdSim}`);
          this.refreshPage();
        //}
        //else {
          //alert("Erro: Você inseriu um conexão que já existe");
        //}
      }
      else {
        alert("Você adicionou algum elemento igual ou abaixo de zero");
      }
    })

  }

  ngOnInit(): void {
    this.loadScript("../assets/simgenerator.js")
    this.getmaquinas();
  }
}

import { JsonpClientBackend } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-sim',
  templateUrl: './new-sim.component.html',
  styleUrls: ['./new-sim.component.css']
})

export class NewSimComponent implements OnInit {

  constructor(private service: SharedService, private router: Router) { }

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

  retroceder(){
    this.service.GetUserByToken().subscribe(data => {
        this.User = data
        if(this.User.Aut == true){
          this.router.navigateByUrl('/admin');
        }
        else{
          this.router.navigateByUrl('/cliente');
        }
      }
    )
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
      if(this.IdMaq == "---" || this.IdMaq == undefined) this.IdMaq = "";


      this.User = data;
      this.NovaConexao = {
        IdUser: `${this.User.IdUser}`,
        IdSim: `${this.IdSim}`,
        IdJob: `${this.IdJob}`,
        IdOp: `${this.IdOp}`,
        IdMaq: `${this.IdMaq}`,
        Duracao: `${this.Duracao}`
      }

      if(this.Duracao == "" || this.Duracao == undefined) this.NovaConexao.Duracao = null;

      if (this.NovaConexao.IdSim > 0 && this.NovaConexao.IdJob > 0 && this.NovaConexao.IdOp > 0 && (this.NovaConexao.Duracao > 0 || this.NovaConexao.Duracao == null)) {
        this.service.PostConexao(this.NovaConexao).subscribe(
          data =>  alert(`Adicionou uma conexão à simulacao ${this.NovaConexao.IdSim}`),
          error => alert("Erro: Você inseriu uma conexão que já existe")
          );
      }
      else {
        alert("Erro ao adicionar a máquina. Verifique os campos");
      }
    })
  }

  ngOnInit(): void {
    this.loadScript("../assets/simgenerator.js")
    this.getmaquinas();
  }
}

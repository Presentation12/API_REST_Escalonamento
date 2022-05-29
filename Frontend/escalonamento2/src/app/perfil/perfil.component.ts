import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit {

  Pass:any;
  ConfirmPass:any;
  UserDados:any={
    Mail:"",
    PassHash:""
  }
  Conta:any={
    Mail:""
  };
  state:number=0;

  constructor(private service : SharedService) { }

  public loadScript(url: any) {
    let node = document.createElement("script");
    node.src = url;
    node.type = 'text/javascript';
    document.body.append(node);
  }

  ngOnInit(): void {
    this.loadScript("../../assets/perfil.js")
    this.refreshUser();
  }

  options(){
    this.state = 1;
  }

  optionsBack(){
    this.state = 0;
  }

  refreshUser(){
    this.service.GetUserByToken().subscribe(data => {
      this.Conta=data;
    })
  }

  Cancelar(){
    this.Pass = "";
    this.ConfirmPass ="" ;
  }

  Suspender(){
    if(confirm("Tem a certeza que pretende eliminar esta conta?")){
      this.service.GetUserByToken().subscribe(data => {
        this.Conta = data;
        this.service.DeleteUser(this.Conta.IdUser).subscribe();
      })
    }
  }

  UpdateClientePassword(){
    this.service.GetUserByToken().subscribe(data => {
      this.UserDados = data;
      if (this.Pass==this.ConfirmPass)
      {
        this.UserDados.PassHash = this.Pass

        this.service.RecoverPassword(this.UserDados.IdUser,this.UserDados).subscribe();

        alert("Palavra Pass alterada com sucesso!")
        this.Pass = "";
        this.ConfirmPass ="" ;
      }
      else{alert("Passwords não correspondentes")}
    })
 }

  logout(){
    localStorage.setItem('token', '');
  }

}

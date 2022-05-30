import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router'

declare function mostraPass():any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  Account:any={
    Mail:"",
    PassHash:""
  }
  NewAccount:any={
    Mail:"",
    PassHash:"",
    Aut:""
  }
  loginData:any;

  constructor(private service: SharedService, private router: Router) { }
  public loadScript(url : any)
  {
    let node = document.createElement("script");
    node.src=url;
    node.type ='text/javascript';
    document.body.append(node);
  }

  ngOnInit(): void {
    this.loadScript("../assets/login.js")
  }

  login(): void {
    this.service.Login(this.Account).subscribe(data=>{
     this.loginData = data;
     if (data == "Utilizador não existe! (Parameter 'account')") alert("Utilizador não existe!")
     else if(data == "Password Errada. (Parameter 'account')") alert("Password Errada.")
     else{
       if(this.loginData.role == "Utilizador"){
        localStorage.setItem('token', this.loginData.token.toString());
        this.router.navigateByUrl('/perfil').then(() =>{
          this.router.navigate([decodeURI('/perfil')]);
        });
       }
       else if(this.loginData.role == "Admin"){
        localStorage.setItem('token', this.loginData.token.toString());
        this.router.navigateByUrl('/admin').then(() =>{
          this.router.navigate([decodeURI('/admin')]);
        });
       }
     }
    });
   }

   registaConta() {
    if(this.NewAccount.Aut == "") this.NewAccount.Aut = false;
    this.service.RegistUser(this.NewAccount).subscribe()
  }

  MostraPass()
  {
    mostraPass();
  }
}

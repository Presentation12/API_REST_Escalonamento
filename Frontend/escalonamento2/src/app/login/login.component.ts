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
     if (data == "Utilizador não existe! (Parameter 'account')") alert("Utilizador não existe!")
     else if(data == "Password Errada. (Parameter 'account')") alert("Password Errada.")
     else{
       localStorage.setItem('token', data.toString());
       this.router.navigateByUrl('/perfil').then(() =>{
         this.router.navigate([decodeURI('/perfil')]);
       });
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

import { Component, OnInit } from '@angular/core';

declare function mostraPass():any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor() { 
    this.loadScript("assets/login.js")
  }

  public loadScript(url : any)
  {
    let node = document.createElement("script");
    node.src=url;
    node.type ='text/javascript';
    document.body.append(node);
  }

  ngOnInit(): void {
  }

  MostraPass()
  {
    mostraPass();
  }

}

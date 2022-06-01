import { Component, OnInit } from '@angular/core';

declare function createTable(r:any,c:any):any;

@Component({
  selector: 'app-new-sim',
  templateUrl: './new-sim.component.html',
  styleUrls: ['./new-sim.component.css']
})

export class NewSimComponent implements OnInit {

  constructor() { }

  public loadScript(url: any) {
    let node = document.createElement("script");
    node.src = url;
    node.type = 'text/javascript';
    document.body.append(node);
  }

  ngOnInit(): void {
    this.loadScript("../assets/simgenerator.js")
  }

  CriarTabela(){
    let r = window.prompt("Insira o número de Jobs");
    let c = window.prompt("Insira o número de Operations");
    createTable(r,c);
  }
}

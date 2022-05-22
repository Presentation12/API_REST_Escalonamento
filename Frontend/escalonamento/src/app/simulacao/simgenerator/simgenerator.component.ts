import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-simgenerator',
  templateUrl: './simgenerator.component.html',
  styleUrls: ['./simgenerator.component.css']
})
export class SimgeneratorComponent implements OnInit {

  constructor() { }

  public loadScript(url: any) {
    let node = document.createElement("script");
    node.src = url;
    node.type = 'text/javascript';
    document.body.append(node);
  }

  ngOnInit(): void {
    this.loadScript("../../assets/simgenerator.js")
  }

}

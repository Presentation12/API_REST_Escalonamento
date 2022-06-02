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

  NovaConexao : any = {};
  IdSim:any;
  IdJob:any;
  IdOp:any;
  IdMaq:any;
  Duracao:any;

  SubmitConexao()
  {
    this.NovaConexao = {
      IdUser: `${}`,
      IdSim: `${this.IdSim}`,
      IdJob: `${this.IdJob}`,
      IdOp: `${this.IdOp}`,
      IdMaq: `${this.IdMaq}`,
      Duracao: `${this.Duracao}`
    }
    this.service.PostConexao(this.NovaConexao).subscribe();
  }

  ngOnInit(): void {
    this.loadScript("../assets/simgenerator.js")
  }
}

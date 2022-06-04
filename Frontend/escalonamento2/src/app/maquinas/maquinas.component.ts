import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-maquinas',
  templateUrl: './maquinas.component.html',
  styleUrls: ['./maquinas.component.css']
})
export class MaquinasComponent implements OnInit {

  constructor(private service: SharedService) { }

  Maquinas: any = [];

  refreshPage(): void {
    window.location.reload();
  }

  getmaquinas() {
    this.service.GetMaquina().subscribe(data => {
      this.Maquinas = data;
    })
  }

  DesativarMaquina(idMaq: any) {
    var c = confirm("Você tem a certeza?")
    if (c == true) {
      this.service.DeleteMaquina(idMaq).subscribe();
      alert(`Máquina ${idMaq} removida com sucesso`);
      this.refreshPage();
    }
  }

  NovaMaquina(){
    let any = {};
    this.service.AddMaquina(any).subscribe();
    alert(`Nova máquina criada com sucesso`);
    this.refreshPage();
  }

  ngOnInit(): void {
    this.getmaquinas();
  }

}

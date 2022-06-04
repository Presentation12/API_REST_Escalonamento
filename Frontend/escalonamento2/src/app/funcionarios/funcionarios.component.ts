import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-funcionarios',
  templateUrl: './funcionarios.component.html',
  styleUrls: ['./funcionarios.component.css']
})
export class FuncionariosComponent implements OnInit {

  constructor(private service: SharedService) { }

  Funcionarios : any = [];

  refreshPage(): void {
    window.location.reload();
  }

  DesativarFuncionario(idUser: any)
  {
    var c = confirm("Você tem a certeza?")
    if(c == true)
    {
      this.service.DeleteUser(idUser).subscribe();
      alert(`Funcionário ${idUser} removido com sucesso`);
      this.refreshPage();
    }
  }

  refreshFuncionarios()
  {
    this.service.GetUtilizador().subscribe(data =>{
      this.Funcionarios = data;
    })
  }

  ngOnInit(): void {
    this.refreshFuncionarios();
  }

}

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { SimulacaoComponent } from './simulacao/simulacao.component';
import { PerfilComponent } from './simulacao/perfil/perfil.component';

const routes: Routes = [
  {path: 'login',component:LoginComponent},
  {path: 'simulacao',component:SimulacaoComponent},
  {path: 'perfil',component:PerfilComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

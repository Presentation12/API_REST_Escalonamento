import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { PerfilComponent } from './perfil/perfil.component';
import { NewSimComponent } from './new-sim/new-sim.component';
import { ShowSimsComponent } from './show-sims/show-sims.component';
import { AdminComponent } from './admin/admin.component';
import { FuncionariosComponent } from './funcionarios/funcionarios.component';

const routes: Routes = [
  {path: 'login',component:LoginComponent},
  {path: 'perfil',component:PerfilComponent},
  {path: 'newsim',component:NewSimComponent},
  {path: 'showsims',component:ShowSimsComponent},
  {path: 'admin',component:AdminComponent},
  {path: 'funcionarios', component:FuncionariosComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

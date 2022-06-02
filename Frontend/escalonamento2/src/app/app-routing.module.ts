import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { PerfilComponent } from './perfil/perfil.component';
import { NewSimComponent } from './new-sim/new-sim.component';
import { ShowSimsComponent } from './show-sims/show-sims.component';
import { AdminComponent } from './admin/admin.component';

const routes: Routes = [
  {path: 'login',component:LoginComponent},
  {path: 'perfil',component:PerfilComponent},
  {path: 'perfil/newsim',component:NewSimComponent},
  {path: 'perfil/showsims',component:ShowSimsComponent},
  {path: 'admin',component:AdminComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

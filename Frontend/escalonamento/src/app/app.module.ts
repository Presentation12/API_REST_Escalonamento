import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { SharedService } from './shared.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SimulacaoComponent } from './simulacao/simulacao.component';
import { LoginComponent } from './login/login.component';
import { PerfilComponent } from './simulacao/perfil/perfil.component';
import { SimgeneratorComponent } from './simulacao/simgenerator/simgenerator.component';

@NgModule({
  declarations: [
    AppComponent,
    SimulacaoComponent,
    LoginComponent,
    PerfilComponent,
    SimgeneratorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: LoginComponent }
    ])
  ],
  providers: [SharedService],
  bootstrap: [AppComponent]
})
export class AppModule { }

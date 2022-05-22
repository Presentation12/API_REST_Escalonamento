import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  readonly APIUrl = "http://localhost:5000/api/"

  private tokenHardcode:any=""
  headersAuth = new HttpHeaders().append("Authorization", `Bearer ${this.tokenHardcode}`);

  constructor(private http:HttpClient) { }

  //#region Utilizador

  GetUtilizador():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/utilizador', {headers: this.headersAuth});
  }

  GetUtilizadorByID(id:any):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/utilizador/'+id, {headers: this.headersAuth});
  }

  Login():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/utilizador/login', {headers: this.headersAuth});
  }

  RegistUser(object:any){
    return this.http.post(this.APIUrl+'/utilizador', object, {headers: this.headersAuth});
  }

  EditUser(object:any, id:any){
    return this.http.patch(this.APIUrl+'/utilizador/' + id, object, {headers: this.headersAuth});
  }

  RecoverPassword(object:any){
    return this.http.patch(this.APIUrl+'/utilizador/recoverpassword', object, {headers: this.headersAuth});
  }

  DeleteUser(){
    return this.http.patch(this.APIUrl+'/utilizador/delete', {headers: this.headersAuth});
  }

  //FALTA METODO EM QUE UM ADMIN ELIMINA OUTRO USER

  //#endregion

  //#region Job

  GetJob():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/job', {headers: this.headersAuth});
  }

  GetJobByID(val:any):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/job/'+val, {headers: this.headersAuth});
  }

  AddJob(object:any){
    return this.http.post(this.APIUrl+'/job', object, {headers: this.headersAuth});
  }

  DeleteJob(id:any){
    return this.http.delete(this.APIUrl+'/job/'+id, {headers: this.headersAuth});
  }

  //#endregion

  //#region Maquina

  GetMaquina():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/maquina', {headers: this.headersAuth});
  }

  GetMaquinaByID(id:any):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/maquina/'+id, {headers: this.headersAuth});
  }

  AddMaquina(object:any){
    return this.http.post(this.APIUrl+'/maquina', object, {headers: this.headersAuth});
  }

  EditMaquina(object:any, id:any){
    return this.http.patch(this.APIUrl+'/maquina/' + id, object, {headers: this.headersAuth});
  }

  DeleteMaquina(id:any){
    return this.http.delete(this.APIUrl+'/maquina/'+id, {headers: this.headersAuth});
  }

  //#endregion

  //#region Operacao

  GetOperacao():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/operacao', {headers: this.headersAuth});
  }

  GetOperacaoByID(id:any):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/operacao/'+id, {headers: this.headersAuth});
  }

  AddOperacao(object:any){
    return this.http.post(this.APIUrl+'/operacao', object, {headers: this.headersAuth});
  }

  DeleteOperacao(id:any){
    return this.http.delete(this.APIUrl+'/operacao/'+id, {headers: this.headersAuth});
  }

  //#endregion

  //#region Simulacao

  GetSimulacao():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/simulacao', {headers: this.headersAuth});
  }

  GetSimulacaoByID(id:any):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/simulacao/'+ id, {headers: this.headersAuth});
  }

  AddSimulacao(object:any){
    return this.http.post(this.APIUrl+'/simulacao', object, {headers: this.headersAuth});
  }

  EditSimulacao(object:any, id:any){
    return this.http.patch(this.APIUrl+'/simulacao/'+ id, object, {headers: this.headersAuth})
  }

  DeleteSimulacao(id:any){
    return this.http.patch(this.APIUrl+'/simulacao/delete/'+ id, {headers: this.headersAuth});
  }
  //#endregion
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { UserForLogin, UserLogin } from './models/auth';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { PayLoad } from './models/payload';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _httpClient: HttpClient, private router: Router) {

    this.userPayLoad = this.decodedToken();

  }
  url: string = "https://localhost:7192/api/auth";
  userPayLoad: PayLoad;
  //login
  authUser(user: UserForLogin): Observable<UserLogin> {
    return this._httpClient.post<UserLogin>(`${this.url}/login`, user);
  }
  logOut() {
    localStorage.clear();
  
    this.router.navigateByUrl('/login')
  }

  storeId(idvalue: number) {
    localStorage.setItem('id', idvalue.toString());
  }

  getUserId() {
    return localStorage.getItem('id');
  }


  storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
  }
  getToken() {
    return localStorage.getItem('token');
  }
  isLogged(): boolean {
    const token = localStorage.getItem('token');
    const userId = localStorage.getItem('id');
    return !!userId && !!token;
  }
  storeFullName(fullName: string) {
    localStorage.setItem('fullName', fullName);
  }

  getFullName() {
    return localStorage.getItem('fullName');
  }


  storeEmail(email: string) {
    localStorage.setItem('email', email);
  }

  getEmail() {
    return localStorage.getItem('email');
  }





  decodedToken() {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken();
    if (token)
      return jwtHelper.decodeToken(token);
  }
}

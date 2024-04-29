import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Status } from '../models/status';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _httpClient: HttpClient) { }
  url: string = "https://localhost:7192/api/User";

  addUpdateUser(user: User): Observable<Status> {
    return this._httpClient.post<Status>(`${this.url}/AddUpdateUser`, user)
  }


  getAllUser(): Observable<User[]> {
    return this._httpClient.get<User[]>(`${this.url}/getAllUser`);

  }


  deleteUser(id: number): Observable<Status> {

    return this._httpClient.delete<Status>(`${this.url}/deleteUser/${id}`);
  }


  getUserById(id: number): Observable<User> {

    return this._httpClient.get<User>(`${this.url}/getUserById/${id}`);
  }
}

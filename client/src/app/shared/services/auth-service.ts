import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) { }

  baseUrl: string = 'https://localhost:7175/api';

  registerUser(user: any) {
    return this.http.post(`${this.baseUrl}/signup`, user);
  }

  userSignIn(credentials: any) {
    return this.http.post(`${this.baseUrl}/signin`, credentials);
  }
}

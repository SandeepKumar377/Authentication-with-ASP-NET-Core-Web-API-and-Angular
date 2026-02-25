import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKET_KEY } from '../constants';

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
  isLoggedIn(): boolean {
    return localStorage.getItem(TOKET_KEY) !== null ? true : false;
  }
  deleteToken() {
    localStorage.removeItem(TOKET_KEY);
  }
  saveToken(token: string) {
    localStorage.setItem(TOKET_KEY, token);
  }
}

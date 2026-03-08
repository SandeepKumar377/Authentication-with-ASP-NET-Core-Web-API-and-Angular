import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKET_KEY } from '../constants';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) { }

  baseUrl: string = environment.apiBaseUrl;

  registerUser(user: any) {
    return this.http.post(`${this.baseUrl}/signup`, user);
  }

  userSignIn(credentials: any) {
    return this.http.post(`${this.baseUrl}/signin`, credentials);
  }
  isLoggedIn(): boolean {
    return this.getToken() !== null ? true : false;
  }
  deleteToken() {
    localStorage.removeItem(TOKET_KEY);
  }
  saveToken(token: string) {
    localStorage.setItem(TOKET_KEY, token);
  }
  getToken() {
    return localStorage.getItem(TOKET_KEY);
  }
  getUserClaims() {
    const token = this.getToken();
    if (!token) return null;
    window.atob(token.split('.')[1]);
    return JSON.parse(window.atob(token.split('.')[1]));
  }
  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;
    const payload = JSON.parse(atob(token.split('.')[1]));
    const expiry = payload.exp;
    const now = Math.floor(Date.now() / 1000);
    return now >= expiry;
  }
}

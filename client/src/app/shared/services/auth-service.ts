import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) { }

  baseUrl: string = 'https://localhost:7175/api';

  registerUser(user: any) {
    console.log('Registering user with data:', user);
    return this.http.post(`${this.baseUrl}/signup`, user);
  }
}

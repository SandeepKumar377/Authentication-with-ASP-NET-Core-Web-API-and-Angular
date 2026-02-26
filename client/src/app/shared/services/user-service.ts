import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { AuthService } from './auth-service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl: string = environment.apiBaseUrl;

  constructor(private http: HttpClient,
    private authService: AuthService
  ) { }

  getUserProfile() {
    return this.http.get(this.baseUrl + '/UserProfile');
  }
}

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth-service';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  constructor(private router: Router,
    private authService: AuthService
  ) {

  }

  logout() {
    this.authService.deleteToken();
    this.router.navigateByUrl('/signin');
  }
}

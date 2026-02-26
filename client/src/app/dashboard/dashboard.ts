import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth-service';
import { UserService } from '../shared/services/user-service';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {

  fullName: string = '';

  constructor(private router: Router,
    private authService: AuthService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe({
      next: (response: any) => {
        this.fullName = response.fullName;
        console.log('User Profile:', response);
      },
      error: err => {
        console.error('Error fetching user profile:', err);
        if (err.status === 401) {
          this.router.navigateByUrl('/signin');
        } else {
          console.error('An error occurred while fetching user profile. Please try again later.');
        }
      }
    });
  }

  logout() {
    this.authService.deleteToken();
    this.router.navigateByUrl('/signin');
  }
}

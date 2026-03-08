import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../shared/services/auth-service';
import { HideIfClaimsNotMet } from '../../directives/hide-if-claims-not-met';
import { claimRequired } from '../../shared/utils/claimRequire-utils';
import { UserService } from '../../shared/services/user-service';

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, RouterLink, HideIfClaimsNotMet],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css',
})
export class MainLayout {

  claimRequired = claimRequired;
  userEmail: string = '';

  constructor(private authService: AuthService,
    private userService: UserService,
    private router: Router) { }

  ngOnInit(): void {
    if (this.authService.isLoggedIn() && this.authService.isTokenExpired()) {
      this.authService.deleteToken();
      this.router.navigateByUrl('/signin');
      return;
    }
    const userClaims = this.authService.getUserClaims();
    console.log(userClaims);
    if (userClaims) {
      this.userEmail = userClaims['email'] || '';
    }
  }

  logout() {
    this.authService.deleteToken();
    this.router.navigateByUrl('/signin');
  }
}

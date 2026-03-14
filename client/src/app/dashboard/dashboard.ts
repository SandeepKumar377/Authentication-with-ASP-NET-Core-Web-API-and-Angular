import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth-service';
import { UserService } from '../shared/services/user-service';
import { HideIfClaimsNotMet } from '../directives/hide-if-claims-not-met';
import { claimRequired } from '../shared/utils/claimRequire-utils';

@Component({
  selector: 'app-dashboard',
  imports: [HideIfClaimsNotMet],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {

  claimRequired = claimRequired;

}

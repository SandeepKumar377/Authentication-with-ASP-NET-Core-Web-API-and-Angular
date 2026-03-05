import { Directive, ElementRef, Input } from '@angular/core';
import { AuthService } from '../shared/services/auth-service';

@Directive({
  selector: '[appHideIfClaimsNotMet]',
})
export class HideIfClaimsNotMet {

  @Input("appHideIfClaimsNotMet") claimRequired!: Function;


  constructor(private authService: AuthService,
    private elementRef: ElementRef
  ) { }

  ngOnInit(): void {
    const claims = this.authService.getUserClaims();
    if (!this.claimRequired(claims)) {
      this.elementRef.nativeElement.style.display = 'none';
    }
  }
}

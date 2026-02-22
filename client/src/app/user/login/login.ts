import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../shared/services/auth-service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  form: any;
  isSubmitted: boolean = false;

  constructor(public formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]]
    })
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.form.valid) {
      this.authService.userSignIn(this.form.value).subscribe({
        next: (response: any) => {
          if (response) {
            this.form.reset();
            this.isSubmitted = false;
            localStorage.setItem('token', response.token);
            this.router.navigateByUrl('/dashboard');
            this.toastr.success('User logged in successfully');
            console.log('Login successful:', response);
          } else {
            this.toastr.error('Login failed');
            console.error('Login failed:', response);
          }
        },
        error: err => {
          if (err.status === 401) {
            this.toastr.error('Invalid email or password');
          } else {
            this.toastr.error('An error occurred during login. Please try again later.');
          }
          console.error('Login error:', err);
        }
      });
    }
    else {
      console.log('Form is invalid');
    }
  }

  hasDisplayableError(controlName: string): boolean {
    const control = this.form.get(controlName);
    return control && control.invalid && (control.dirty || control.touched || this.isSubmitted);
  }


}

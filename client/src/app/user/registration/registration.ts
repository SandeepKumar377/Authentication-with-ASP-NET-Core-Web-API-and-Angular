import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FirstKeyPipe } from '../../shared/pipes/first-key-pipe';
import { AuthService } from '../../shared/services/auth-service';
import { ToastrService } from 'ngx-toastr';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule, FirstKeyPipe, RouterLink],
  templateUrl: './registration.html',
  styleUrl: './registration.css',
})
export class Registration {

  form: any;

  constructor(public formBuilder: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService
  ) { }

  isSubmitted: boolean = false;

  ngOnInit(): void {

    this.form = this.formBuilder.nonNullable.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required,
      Validators.minLength(5),
        // Validators.pattern(/(?=.*[^a-zA-Z0-9 ])/)
      ]], confirmPassword: ['']
    }, { validators: this.passwordMatchValidator });
  }
  passwordMatchValidator(form: any) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    if (password && confirmPassword && password !== confirmPassword) {
      form.get('confirmPassword')?.setErrors({ mismatch: true });
    } else {
      form.get('confirmPassword')?.setErrors(null);
    }
    return null;
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.form.valid) {
      console.log('Form Values:', this.form.value);
      this.authService.registerUser(this.form.value).subscribe({
        next: (response: any) => {
          if (response && response.succeeded) {
            this.form.reset();
            this.isSubmitted = false;
            this.toastr.success('User registered successfully');
            console.log('User registered successfully', response);
          } else {
            this.toastr.error('Registration failed');
            console.error('Registration failed:', response);
          }
        },
        error: (error) => {
          if (error.error || error.error.errors) {
            error.error.errors.forEach((error: any) => {
              switch (error.code) {
                case 'DuplicateUserName':
                  this.toastr.error('Email is already in use');
                  break;
                case 'PasswordTooShort':
                  this.toastr.error('Password must be at least 6 characters long');
                  break;
                case 'PasswordRequiresNonAlphanumeric':
                  this.toastr.error('Password must contain at least one special character');
                  break;
                default:
                  this.toastr.error(error.description);
              }
            });
          }
          else {
            this.toastr.error('An unexpected error occurred');
            console.error('Error registering user:', error);
          }
        }
      });
    }
  }

  hasDisplayableError(controlName: string): boolean {
    const control = this.form.get(controlName);
    return control && control.invalid && (control.dirty || control.touched || this.isSubmitted);
  }

}

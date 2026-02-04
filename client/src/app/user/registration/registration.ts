import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule],
  templateUrl: './registration.html',
  styleUrl: './registration.css',
})
export class Registration {

  form: any;

  constructor(public formBuilder: FormBuilder) { }

  isSubmitted: boolean = false;

  ngOnInit(): void {

    this.form = this.formBuilder.nonNullable.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required,
      Validators.minLength(6),
      Validators.pattern(/(?=.*[^a-zA-Z0-9 ])/)]],
      confirmPassword: ['']
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
    if (this.form.invalid) {
      return;
    }
    console.log(this.form.value);
  }

  hasDisplayableError(controlName: string): boolean {
    const control = this.form.get(controlName);
    return control && control.invalid && (control.dirty || control.touched || this.isSubmitted);
  }

}

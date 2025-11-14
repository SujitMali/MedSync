import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToasterService } from '../../Services/toaster.service';
import { AlertService } from '../../Services/alert.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginModel = { email: '', password: '' };
  errorMessage: string | null = null;
  isLoading = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    private toaster: ToasterService,
    private alert: AlertService
  ) { }

  onSubmit(form: NgForm) {
    const errors = this.validateForm(form);

    if (errors.length > 0) {
      const formattedErrors = errors.map(e => `• ${e}`).join('<br>');
      this.alert.warning(formattedErrors, 'Validation Errors');
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    this.authService.login(this.loginModel).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.toaster.success(response.message);

        const role = this.authService.getUserRole()?.toLowerCase() || '';
        const routes: Record<string, string> = {
          admin: '/admin/dashboard',
          doctor: '/doctor/view-appointment-request'
        };
        this.router.navigate([routes[role] || '/']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Backend Offline';
        this.toaster.error(this.errorMessage ?? 'Backend Offline');
      }
    });
  }

  private validateForm(form: NgForm): string[] {
    const { email, password } = this.loginModel;
    const errors: string[] = [];

    if (form.invalid) {
      form.control.markAllAsTouched();
      errors.push('Please fill in all required fields correctly.');
    }
    if (!this.isValidEmail(email)) errors.push('Please enter a valid email address.');
    if (!password) errors.push('Password cannot be empty.');
    else if (!this.isValidPassword(password))
      errors.push('Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.');
    return errors;
  }

  resetForm(form: NgForm) {
    form.resetForm();
    this.loginModel = { email: '', password: '' };
    this.errorMessage = null;
  }

  isValidEmail(email: string): boolean {
    return /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/.test(email?.trim() || '');
  }

  isValidPassword(password: string): boolean {
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(password || '');
  }
}

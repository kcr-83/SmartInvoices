import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { UserService } from '../../../services/user.service';
import { NotificationService } from '../../../services/notification.service';
import { LoginCredentials } from '../../../models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    MatSnackBarModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly userService = inject(UserService);
  private readonly notificationService = inject(NotificationService);

  readonly loginForm: FormGroup;
  readonly isLoading = signal(false);
  readonly hidePassword = signal(true);

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false]
    });

    // Check if already logged in
    if (this.userService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading.set(true);

      const credentials: LoginCredentials = {
        email: this.loginForm.value.email!,
        password: this.loginForm.value.password!
      };      this.userService.login(credentials).subscribe({
        next: (response) => {
          this.notificationService.showSuccess('Zalogowano pomyślnie!');

          // Sprawdź czy jest zapisany URL przekierowania
          const redirectUrl = sessionStorage.getItem('redirectUrl');
          if (redirectUrl) {
            sessionStorage.removeItem('redirectUrl');
            this.router.navigate([redirectUrl]);
          } else {
            // Domyślne przekierowanie
            this.router.navigate(['/dashboard']);
          }
        },
        error: (error) => {
          console.error('Login error:', error);
          this.notificationService.showError('Błąd logowania. Sprawdź dane i spróbuj ponownie.');
        },
        complete: () => {
          this.isLoading.set(false);
        }
      });    } else {
      this.notificationService.showWarning('Proszę wypełnić wszystkie wymagane pola.');
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched() {
    Object.keys(this.loginForm.controls).forEach(key => {
      const control = this.loginForm.get(key);
      control?.markAsTouched();
    });
  }

  getEmailErrorMessage(): string {
    const emailControl = this.loginForm.get('email');
    if (emailControl?.hasError('required')) {
      return 'Adres e-mail jest wymagany';
    }
    if (emailControl?.hasError('email')) {
      return 'Wprowadź prawidłowy adres e-mail';
    }
    return '';
  }

  getPasswordErrorMessage(): string {
    const passwordControl = this.loginForm.get('password');
    if (passwordControl?.hasError('required')) {
      return 'Hasło jest wymagane';
    }
    if (passwordControl?.hasError('minlength')) {
      return 'Hasło musi mieć co najmniej 6 znaków';
    }
    return '';
  }

  togglePasswordVisibility() {
    this.hidePassword.set(!this.hidePassword());
  }

  fillDemoCredentials(userType: 'admin' | 'user') {
    if (userType === 'admin') {
      this.loginForm.patchValue({
        email: 'admin@smartinvoices.com',
        password: 'password123'
      });
    } else {
      this.loginForm.patchValue({
        email: 'user@smartinvoices.com',
        password: 'password123'
      });
    }
  }
}

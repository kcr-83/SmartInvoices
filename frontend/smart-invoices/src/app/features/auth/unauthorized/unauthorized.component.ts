import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    <div class="unauthorized-container">
      <mat-card class="unauthorized-card">
        <mat-card-header>
          <div mat-card-avatar class="error-avatar">
            <mat-icon>block</mat-icon>
          </div>
          <mat-card-title>Brak uprawnień</mat-card-title>
          <mat-card-subtitle>Nie masz uprawnień do tej strony</mat-card-subtitle>
        </mat-card-header>

        <mat-card-content>
          <p>Przepraszamy, ale nie masz odpowiednich uprawnień do wyświetlenia tej strony.</p>
          <p>Jeśli uważasz, że to błąd, skontaktuj się z administratorem systemu.</p>
        </mat-card-content>

        <mat-card-actions>
          <button mat-raised-button color="primary" (click)="goHome()">
            <mat-icon>home</mat-icon>
            Wróć do strony głównej
          </button>
          <button mat-button (click)="goBack()">
            <mat-icon>arrow_back</mat-icon>
            Wstecz
          </button>
        </mat-card-actions>
      </mat-card>
    </div>
  `,
  styles: [`
    .unauthorized-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: calc(100vh - 64px);
      padding: 24px;
      background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
    }

    .unauthorized-card {
      max-width: 500px;
      width: 100%;
      text-align: center;
    }

    .error-avatar {
      background-color: #f44336;
      color: white;
    }

    mat-card-content {
      margin: 24px 0;
    }

    mat-card-content p {
      margin-bottom: 16px;
      color: rgba(0, 0, 0, 0.6);
    }

    mat-card-actions {
      justify-content: center;
      gap: 16px;
    }

    button {
      min-width: 140px;
    }

    mat-icon {
      margin-right: 8px;
    }
  `]
})
export class UnauthorizedComponent {
  private readonly router = inject(Router);

  goHome(): void {
    this.router.navigate(['/invoices']);
  }

  goBack(): void {
    window.history.back();
  }
}

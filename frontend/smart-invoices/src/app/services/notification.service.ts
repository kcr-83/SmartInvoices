import { Injectable, inject } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

export interface NotificationConfig extends MatSnackBarConfig {
  type?: 'success' | 'error' | 'warning' | 'info';
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly snackBar = inject(MatSnackBar);

  private readonly defaultConfigs = {
    success: {
      duration: 3000,
      panelClass: ['success-snackbar'],
      horizontalPosition: 'center' as const,
      verticalPosition: 'bottom' as const
    },
    error: {
      duration: 5000,
      panelClass: ['error-snackbar'],
      horizontalPosition: 'center' as const,
      verticalPosition: 'bottom' as const
    },
    warning: {
      duration: 4000,
      panelClass: ['warning-snackbar'],
      horizontalPosition: 'center' as const,
      verticalPosition: 'bottom' as const
    },
    info: {
      duration: 3000,
      panelClass: ['info-snackbar'],
      horizontalPosition: 'center' as const,
      verticalPosition: 'bottom' as const
    }
  };

  /**
   * Wyświetla powiadomienie o sukcesie
   */
  showSuccess(message: string, action = 'Zamknij', config?: NotificationConfig) {
    const finalConfig = { ...this.defaultConfigs.success, ...config };
    return this.snackBar.open(message, action, finalConfig);
  }

  /**
   * Wyświetla powiadomienie o błędzie
   */
  showError(message: string, action = 'Zamknij', config?: NotificationConfig) {
    const finalConfig = { ...this.defaultConfigs.error, ...config };
    return this.snackBar.open(message, action, finalConfig);
  }

  /**
   * Wyświetla ostrzeżenie
   */
  showWarning(message: string, action = 'Zamknij', config?: NotificationConfig) {
    const finalConfig = { ...this.defaultConfigs.warning, ...config };
    return this.snackBar.open(message, action, finalConfig);
  }

  /**
   * Wyświetla powiadomienie informacyjne
   */
  showInfo(message: string, action = 'Zamknij', config?: NotificationConfig) {
    const finalConfig = { ...this.defaultConfigs.info, ...config };
    return this.snackBar.open(message, action, finalConfig);
  }

  /**
   * Wyświetla powiadomienie z automatycznym określeniem typu na podstawie zawartości
   */
  show(message: string, action = 'Zamknij', config?: NotificationConfig) {
    const type = config?.type || this.determineType(message);
    const finalConfig = { ...this.defaultConfigs[type], ...config };
    return this.snackBar.open(message, action, finalConfig);
  }

  /**
   * Automatycznie określa typ powiadomienia na podstawie zawartości wiadomości
   */
  private determineType(message: string): 'success' | 'error' | 'warning' | 'info' {
    const lowerMessage = message.toLowerCase();

    if (lowerMessage.includes('błąd') || lowerMessage.includes('error') ||
        lowerMessage.includes('niepowodzenie') || lowerMessage.includes('failed')) {
      return 'error';
    }

    if (lowerMessage.includes('ostrzeżenie') || lowerMessage.includes('warning') ||
        lowerMessage.includes('uwaga') || lowerMessage.includes('attention')) {
      return 'warning';
    }

    if (lowerMessage.includes('sukces') || lowerMessage.includes('pomyślnie') ||
        lowerMessage.includes('zapisano') || lowerMessage.includes('success') ||
        lowerMessage.includes('utworzono') || lowerMessage.includes('zaktualizowano')) {
      return 'success';
    }

    return 'info';
  }

  /**
   * Zamyka wszystkie aktywne powiadomienia
   */
  dismissAll() {
    this.snackBar.dismiss();
  }
}

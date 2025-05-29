import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDividerModule } from '@angular/material/divider';

import { UserService } from '../../../services/user.service';
import { User, UserRole } from '../../../models/user.model';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    MatSnackBarModule,
    MatChipsModule,
    MatSlideToggleModule,
    MatMenuModule,
    MatTooltipModule,
    MatProgressSpinnerModule,    MatToolbarModule,
    MatSortModule,
    MatPaginatorModule,
    MatDividerModule
  ],
  template: `
    <div class="user-management-container">
      <!-- Header -->
      <mat-toolbar color="primary">
        <button mat-icon-button (click)="goBack()">
          <mat-icon>arrow_back</mat-icon>
        </button>
        <span>Zarządzanie użytkownikami</span>
        <span class="spacer"></span>
        <button mat-raised-button color="accent" (click)="openAddUserDialog()">
          <mat-icon>person_add</mat-icon>
          Dodaj użytkownika
        </button>
      </mat-toolbar>

      <!-- Filters Card -->
      <mat-card class="filters-card">
        <mat-card-header>
          <mat-card-title>Filtry</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <div class="filters-form">
            <div class="filter-row">
              <mat-form-field appearance="outline">
                <mat-label>Wyszukaj</mat-label>
                <input matInput [(ngModel)]="searchTerm" placeholder="Imię, nazwisko, email...">
                <mat-icon matSuffix>search</mat-icon>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Rola</mat-label>
                <mat-select [(ngModel)]="roleFilter">
                  <mat-option value="">Wszystkie</mat-option>
                  <mat-option value="Admin">Administrator</mat-option>
                  <mat-option value="User">Użytkownik</mat-option>
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Status</mat-label>
                <mat-select [(ngModel)]="statusFilter">
                  <mat-option value="">Wszystkie</mat-option>
                  <mat-option value="true">Aktywny</mat-option>
                  <mat-option value="false">Nieaktywny</mat-option>
                </mat-select>
              </mat-form-field>
            </div>

            <div class="filter-actions">
              <button mat-button (click)="clearFilters()">
                <mat-icon>clear</mat-icon>
                Wyczyść filtry
              </button>
            </div>
          </div>
        </mat-card-content>
      </mat-card>

      <!-- Users Statistics -->
      <div class="stats-cards">
        <mat-card class="stat-card">
          <mat-card-content>
            <div class="stat-number">{{ stats().total }}</div>
            <div class="stat-label">Wszyscy użytkownicy</div>
          </mat-card-content>
        </mat-card>

        <mat-card class="stat-card">
          <mat-card-content>
            <div class="stat-number">{{ stats().admins }}</div>
            <div class="stat-label">Administratorzy</div>
          </mat-card-content>
        </mat-card>

        <mat-card class="stat-card">
          <mat-card-content>
            <div class="stat-number">{{ stats().users }}</div>
            <div class="stat-label">Zwykli użytkownicy</div>
          </mat-card-content>
        </mat-card>

        <mat-card class="stat-card">
          <mat-card-content>
            <div class="stat-number">{{ stats().active }}</div>
            <div class="stat-label">Aktywni</div>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Users Table -->
      <mat-card class="table-card">
        <mat-card-header>
          <mat-card-title>
            Lista użytkowników ({{ filteredUsers().length }})
          </mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <div class="table-container" *ngIf="!loading(); else loadingTemplate">
            <table mat-table [dataSource]="filteredUsers()" class="users-table" matSort>
              <!-- Avatar Column -->
              <ng-container matColumnDef="avatar">
                <th mat-header-cell *matHeaderCellDef>Avatar</th>
                <td mat-cell *matCellDef="let user">
                  <div class="avatar" [style.background-color]="getAvatarColor(user.firstName)">
                    {{ getInitials(user.firstName, user.lastName) }}
                  </div>
                </td>
              </ng-container>

              <!-- Name Column -->
              <ng-container matColumnDef="name">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Imię i nazwisko</th>
                <td mat-cell *matCellDef="let user">
                  <div class="user-info">
                    <div class="user-name">{{ user.firstName }} {{ user.lastName }}</div>
                    <div class="user-email">{{ user.email }}</div>
                  </div>
                </td>
              </ng-container>

              <!-- Role Column -->
              <ng-container matColumnDef="role">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Rola</th>
                <td mat-cell *matCellDef="let user">
                  <mat-chip [color]="user.role === 'Admin' ? 'primary' : 'default'" selected>
                    {{ getRoleText(user.role) }}
                  </mat-chip>
                </td>
              </ng-container>

              <!-- Status Column -->
              <ng-container matColumnDef="status">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
                <td mat-cell *matCellDef="let user">
                  <mat-chip [color]="user.isActive ? 'accent' : 'warn'" selected>
                    {{ user.isActive ? 'Aktywny' : 'Nieaktywny' }}
                  </mat-chip>
                </td>
              </ng-container>

              <!-- Last Login Column -->
              <ng-container matColumnDef="lastLogin">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Ostatnie logowanie</th>
                <td mat-cell *matCellDef="let user">
                  {{ user.lastLoginAt ? formatDate(user.lastLoginAt) : 'Nigdy' }}
                </td>
              </ng-container>

              <!-- Created Date Column -->
              <ng-container matColumnDef="createdAt">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Data utworzenia</th>
                <td mat-cell *matCellDef="let user">
                  {{ formatDate(user.createdAt) }}
                </td>
              </ng-container>

              <!-- Actions Column -->
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef>Akcje</th>
                <td mat-cell *matCellDef="let user">
                  <button mat-icon-button [matMenuTriggerFor]="actionsMenu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu #actionsMenu="matMenu">
                    <button mat-menu-item (click)="editUser(user)">
                      <mat-icon>edit</mat-icon>
                      Edytuj
                    </button>
                    <button mat-menu-item (click)="toggleUserStatus(user)">
                      <mat-icon>{{ user.isActive ? 'block' : 'check_circle' }}</mat-icon>
                      {{ user.isActive ? 'Dezaktywuj' : 'Aktywuj' }}
                    </button>
                    <button mat-menu-item (click)="resetPassword(user)">
                      <mat-icon>vpn_key</mat-icon>
                      Resetuj hasło
                    </button>
                    <mat-divider></mat-divider>
                    <button mat-menu-item (click)="deleteUser(user)" [disabled]="user.id === currentUser()?.id">
                      <mat-icon color="warn">delete</mat-icon>
                      Usuń
                    </button>
                  </mat-menu>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
            </table>
          </div>

          <!-- Empty State -->
          <div *ngIf="filteredUsers().length === 0 && !loading()" class="empty-state">
            <mat-icon>person_off</mat-icon>
            <h3>Brak użytkowników</h3>
            <p>Nie znaleziono użytkowników pasujących do kryteriów wyszukiwania.</p>
          </div>
        </mat-card-content>
      </mat-card>
    </div>

    <!-- Loading Template -->
    <ng-template #loadingTemplate>
      <div class="loading-container">
        <mat-spinner></mat-spinner>
        <p>Ładowanie użytkowników...</p>
      </div>
    </ng-template>
  `,
  styles: [`
    .user-management-container {
      padding: 16px;
      max-width: 1400px;
      margin: 0 auto;
    }

    .spacer {
      flex: 1 1 auto;
    }

    .filters-card {
      margin: 16px 0;
    }

    .filters-form {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .filter-row {
      display: flex;
      gap: 16px;
      flex-wrap: wrap;
    }

    .filter-row mat-form-field {
      flex: 1;
      min-width: 200px;
    }

    .filter-actions {
      display: flex;
      justify-content: flex-end;
    }

    .stats-cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 16px;
      margin: 16px 0;
    }

    .stat-card {
      text-align: center;
    }

    .stat-number {
      font-size: 2rem;
      font-weight: bold;
      color: #1976d2;
      margin-bottom: 8px;
    }

    .stat-label {
      color: #666;
      font-size: 0.9rem;
    }

    .table-card {
      margin: 16px 0;
    }

    .table-container {
      overflow: auto;
      max-height: 600px;
    }

    .users-table {
      width: 100%;
    }

    .avatar {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      color: white;
      font-weight: bold;
      font-size: 14px;
    }

    .user-info {
      display: flex;
      flex-direction: column;
    }

    .user-name {
      font-weight: 500;
      margin-bottom: 2px;
    }

    .user-email {
      font-size: 0.8rem;
      color: #666;
    }

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 64px;
      color: #666;
    }

    .loading-container mat-spinner {
      margin-bottom: 16px;
    }

    .empty-state {
      text-align: center;
      padding: 64px 32px;
      color: #666;
    }

    .empty-state mat-icon {
      font-size: 64px;
      height: 64px;
      width: 64px;
      margin-bottom: 16px;
      opacity: 0.6;
    }

    .empty-state h3 {
      margin: 16px 0 8px 0;
      color: #333;
    }

    @media (max-width: 768px) {
      .user-management-container {
        padding: 8px;
      }

      .filter-row {
        flex-direction: column;
      }

      .filter-row mat-form-field {
        min-width: unset;
      }

      .stats-cards {
        grid-template-columns: repeat(2, 1fr);
      }
    }
  `]
})
export class UserManagementComponent {
  private readonly router = inject(Router);
  private readonly userService = inject(UserService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  // Filters
  readonly searchTerm = signal('');
  readonly roleFilter = signal<UserRole | ''>('');
  readonly statusFilter = signal<string>('');

  // Data
  readonly users = this.userService.users;
  readonly loading = this.userService.loading;
  readonly currentUser = this.userService.currentUser;

  // Table configuration
  displayedColumns: string[] = [
    'avatar',
    'name',
    'role',
    'status',
    'lastLogin',
    'createdAt',
    'actions'
  ];

  // Computed properties
  readonly filteredUsers = computed(() => {
    const users = this.users();
    const searchTerm = this.searchTerm().toLowerCase();
    const roleFilter = this.roleFilter();
    const statusFilter = this.statusFilter();

    return users.filter(user => {
      const matchesSearch = !searchTerm ||
        user.firstName.toLowerCase().includes(searchTerm) ||
        user.lastName.toLowerCase().includes(searchTerm) ||
        user.email.toLowerCase().includes(searchTerm);

      const matchesRole = !roleFilter || user.role === roleFilter;

      const matchesStatus = !statusFilter ||
        (statusFilter === 'true' && user.isActive) ||
        (statusFilter === 'false' && !user.isActive);

      return matchesSearch && matchesRole && matchesStatus;
    });
  });

  readonly stats = computed(() => {
    const users = this.users();
    return {
      total: users.length,
      admins: users.filter(u => u.role === UserRole.Admin).length,
      users: users.filter(u => u.role === UserRole.User).length,
      active: users.filter(u => u.isActive).length
    };
  });

  ngOnInit() {
    // Load users data
    this.userService.loadUsers();
  }

  goBack() {
    this.router.navigate(['/admin']);
  }

  clearFilters() {
    this.searchTerm.set('');
    this.roleFilter.set('');
    this.statusFilter.set('');
  }

  getAvatarColor(name: string): string {
    const colors = [
      '#1976d2', '#388e3c', '#f57c00', '#7b1fa2',
      '#303f9f', '#00796b', '#5d4037', '#455a64'
    ];
    const index = name.charCodeAt(0) % colors.length;
    return colors[index];
  }

  getInitials(firstName: string, lastName: string): string {
    return (firstName.charAt(0) + lastName.charAt(0)).toUpperCase();
  }

  getRoleText(role: UserRole): string {
    return role === UserRole.Admin ? 'Administrator' : 'Użytkownik';
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('pl-PL', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  openAddUserDialog() {
    // TODO: Implement add user dialog
    this.snackBar.open('Funkcja dodawania użytkownika zostanie wkrótce dodana', 'Zamknij', {
      duration: 3000
    });
  }

  editUser(user: User) {
    // TODO: Implement edit user dialog
    this.snackBar.open(`Edycja użytkownika ${user.firstName} ${user.lastName} zostanie wkrótce dodana`, 'Zamknij', {
      duration: 3000
    });
  }

  toggleUserStatus(user: User) {
    const newStatus = !user.isActive;
    const action = newStatus ? 'aktywowany' : 'dezaktywowany';

    // TODO: Implement actual user status toggle via service
    this.snackBar.open(`Użytkownik ${user.firstName} ${user.lastName} został ${action}`, 'Zamknij', {
      duration: 3000
    });
  }

  resetPassword(user: User) {
    // TODO: Implement password reset
    this.snackBar.open(`Link do resetowania hasła został wysłany na adres ${user.email}`, 'Zamknij', {
      duration: 3000
    });
  }

  deleteUser(user: User) {
    if (user.id === this.currentUser()?.id) {
      this.snackBar.open('Nie możesz usunąć swojego własnego konta', 'Zamknij', {
        duration: 3000
      });
      return;
    }

    // TODO: Implement user deletion with confirmation dialog
    this.snackBar.open(`Usunięcie użytkownika ${user.firstName} ${user.lastName} zostanie wkrótce dodane`, 'Zamknij', {
      duration: 3000
    });
  }
}

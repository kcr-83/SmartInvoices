import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';
import { SelectionModel } from '@angular/cdk/collections';

import { ChangeRequestService } from '../../../services/change-request.service';
import { RefundRequestService } from '../../../services/refund-request.service';
import { UserService } from '../../../services/user.service';
import { ChangeRequest, RequestStatus } from '../../../models/change-request.model';
import { RefundRequest } from '../../../models/refund-request.model';

interface RequestItem {
  id: string;
  type: 'change' | 'refund';
  invoiceNumber?: string;
  requestNumber: string;
  status: RequestStatus;
  createdAt: Date;
  requestedBy?: string;
  amount?: number;
  description: string;
  originalData?: any;
}

@Component({
  selector: 'app-request-management',
  standalone: true,  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatTabsModule,
    MatChipsModule,
    MatMenuModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    MatToolbarModule,
    MatSortModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
    MatDividerModule
  ],
  template: `
    <div class="request-management-container">
      <!-- Header -->
      <mat-toolbar color="primary">
        <button mat-icon-button (click)="goBack()">
          <mat-icon>arrow_back</mat-icon>
        </button>
        <span>Zarządzanie żądaniami</span>
        <span class="spacer"></span>
        <button mat-icon-button (click)="refreshData()" matTooltip="Odśwież dane">
          <mat-icon>refresh</mat-icon>
        </button>
      </mat-toolbar>

      <!-- Statistics Cards -->
      <div class="stats-cards">
        <mat-card class="stat-card">
          <mat-card-content>
            <div class="stat-number">{{ stats().totalRequests }}</div>
            <div class="stat-label">Wszystkie żądania</div>
          </mat-card-content>
        </mat-card>

        <mat-card class="stat-card pending">
          <mat-card-content>
            <div class="stat-number">{{ stats().pendingRequests }}</div>
            <div class="stat-label">Oczekujące</div>
          </mat-card-content>
        </mat-card>

        <mat-card class="stat-card approved">
          <mat-card-content>
            <div class="stat-number">{{ stats().approvedRequests }}</div>
            <div class="stat-label">Zatwierdzone</div>
          </mat-card-content>
        </mat-card>

        <mat-card class="stat-card rejected">
          <mat-card-content>
            <div class="stat-number">{{ stats().rejectedRequests }}</div>
            <div class="stat-label">Odrzucone</div>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Filters Card -->
      <mat-card class="filters-card">
        <mat-card-header>
          <mat-card-title>Filtry i wyszukiwanie</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <div class="filters-form">
            <div class="filter-row">
              <mat-form-field appearance="outline">
                <mat-label>Wyszukaj</mat-label>
                <input matInput [(ngModel)]="searchTerm" placeholder="Numer faktury, opis...">
                <mat-icon matSuffix>search</mat-icon>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Typ żądania</mat-label>
                <mat-select [(ngModel)]="typeFilter">
                  <mat-option value="">Wszystkie</mat-option>
                  <mat-option value="change">Zmiany pozycji</mat-option>
                  <mat-option value="refund">Zwroty</mat-option>
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Status</mat-label>
                <mat-select [(ngModel)]="statusFilter">
                  <mat-option value="">Wszystkie</mat-option>
                  <mat-option value="Pending">Oczekujące</mat-option>
                  <mat-option value="Approved">Zatwierdzone</mat-option>
                  <mat-option value="Rejected">Odrzucone</mat-option>
                  <mat-option value="Completed">Zakończone</mat-option>
                </mat-select>
              </mat-form-field>
            </div>

            <div class="filter-row">
              <mat-form-field appearance="outline">
                <mat-label>Data od</mat-label>
                <input matInput [matDatepicker]="dateFromPicker" [(ngModel)]="dateFromFilter">
                <mat-datepicker-toggle matIconSuffix [for]="dateFromPicker"></mat-datepicker-toggle>
                <mat-datepicker #dateFromPicker></mat-datepicker>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Data do</mat-label>
                <input matInput [matDatepicker]="dateToPicker" [(ngModel)]="dateToFilter">
                <mat-datepicker-toggle matIconSuffix [for]="dateToPicker"></mat-datepicker-toggle>
                <mat-datepicker #dateToPicker></mat-datepicker>
              </mat-form-field>

              <div class="filter-actions">
                <button mat-button (click)="clearFilters()">
                  <mat-icon>clear</mat-icon>
                  Wyczyść filtry
                </button>
              </div>
            </div>
          </div>
        </mat-card-content>
      </mat-card>

      <!-- Bulk Actions -->
      <mat-card class="bulk-actions-card" *ngIf="selection.hasValue()">
        <mat-card-content>
          <div class="bulk-actions">
            <span class="selection-info">
              Wybrano {{ selection.selected.length }} żądań
            </span>
            <div class="actions">
              <button mat-raised-button color="primary"
                      (click)="bulkApprove()"
                      [disabled]="!canBulkApprove()">
                <mat-icon>check</mat-icon>
                Zatwierdź wybrane
              </button>
              <button mat-raised-button color="warn"
                      (click)="bulkReject()"
                      [disabled]="!canBulkReject()">
                <mat-icon>close</mat-icon>
                Odrzuć wybrane
              </button>
            </div>
          </div>
        </mat-card-content>
      </mat-card>

      <!-- Requests Table -->
      <mat-card class="table-card">
        <mat-card-header>
          <mat-card-title>
            Lista żądań ({{ filteredRequests().length }})
          </mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <div class="table-container" *ngIf="!loading(); else loadingTemplate">
            <table mat-table [dataSource]="filteredRequests()" class="requests-table" matSort>
              <!-- Checkbox Column -->
              <ng-container matColumnDef="select">
                <th mat-header-cell *matHeaderCellDef>
                  <mat-checkbox (change)="$event ? toggleAllRows() : null"
                                [checked]="selection.hasValue() && isAllSelected()"
                                [indeterminate]="selection.hasValue() && !isAllSelected()">
                  </mat-checkbox>
                </th>
                <td mat-cell *matCellDef="let row">
                  <mat-checkbox (click)="$event.stopPropagation()"
                                (change)="$event ? selection.toggle(row) : null"
                                [checked]="selection.isSelected(row)">
                  </mat-checkbox>
                </td>
              </ng-container>

              <!-- Type Column -->
              <ng-container matColumnDef="type">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Typ</th>
                <td mat-cell *matCellDef="let request">
                  <mat-chip [color]="request.type === 'change' ? 'primary' : 'accent'" selected>
                    <mat-icon>{{ request.type === 'change' ? 'edit' : 'assignment_return' }}</mat-icon>
                    {{ getTypeText(request.type) }}
                  </mat-chip>
                </td>
              </ng-container>

              <!-- Request Number Column -->
              <ng-container matColumnDef="requestNumber">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Numer żądania</th>
                <td mat-cell *matCellDef="let request">
                  <a [routerLink]="getRequestRoute(request)" class="request-link">
                    {{ request.requestNumber }}
                  </a>
                </td>
              </ng-container>

              <!-- Invoice Number Column -->
              <ng-container matColumnDef="invoiceNumber">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Numer faktury</th>
                <td mat-cell *matCellDef="let request">
                  {{ request.invoiceNumber }}
                </td>
              </ng-container>

              <!-- Description Column -->
              <ng-container matColumnDef="description">
                <th mat-header-cell *matHeaderCellDef>Opis</th>
                <td mat-cell *matCellDef="let request">
                  <div class="description" [matTooltip]="request.description">
                    {{ request.description | slice:0:50 }}{{ request.description.length > 50 ? '...' : '' }}
                  </div>
                </td>
              </ng-container>

              <!-- Status Column -->
              <ng-container matColumnDef="status">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
                <td mat-cell *matCellDef="let request">
                  <mat-chip [class]="'status-' + request.status.toLowerCase()">
                    {{ getStatusText(request.status) }}
                  </mat-chip>
                </td>
              </ng-container>

              <!-- Requested By Column -->
              <ng-container matColumnDef="requestedBy">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Zgłoszone przez</th>
                <td mat-cell *matCellDef="let request">
                  {{ request.requestedBy }}
                </td>
              </ng-container>

              <!-- Amount Column -->
              <ng-container matColumnDef="amount">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Kwota</th>
                <td mat-cell *matCellDef="let request">
                  <span *ngIf="request.amount" class="amount">
                    {{ formatCurrency(request.amount) }}
                  </span>
                  <span *ngIf="!request.amount">-</span>
                </td>
              </ng-container>

              <!-- Created Date Column -->
              <ng-container matColumnDef="createdAt">
                <th mat-header-cell *matHeaderCellDef mat-sort-header>Data utworzenia</th>
                <td mat-cell *matCellDef="let request">
                  {{ formatDate(request.createdAt) }}
                </td>
              </ng-container>

              <!-- Actions Column -->
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef>Akcje</th>
                <td mat-cell *matCellDef="let request">
                  <button mat-icon-button [matMenuTriggerFor]="actionsMenu"
                          (click)="$event.stopPropagation()">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu #actionsMenu="matMenu">
                    <button mat-menu-item [routerLink]="getRequestRoute(request)">
                      <mat-icon>visibility</mat-icon>
                      Zobacz szczegóły
                    </button>
                    <mat-divider></mat-divider>
                    <button mat-menu-item
                            (click)="approveRequest(request)"
                            [disabled]="request.status !== 'Pending'">
                      <mat-icon color="primary">check</mat-icon>
                      Zatwierdź
                    </button>
                    <button mat-menu-item
                            (click)="rejectRequest(request)"
                            [disabled]="request.status !== 'Pending'">
                      <mat-icon color="warn">close</mat-icon>
                      Odrzuć
                    </button>
                  </mat-menu>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns;"
                  (click)="navigateToRequest(row)"
                  class="clickable-row"></tr>
            </table>
          </div>

          <!-- Empty State -->
          <div *ngIf="filteredRequests().length === 0 && !loading()" class="empty-state">
            <mat-icon>assignment_late</mat-icon>
            <h3>Brak żądań</h3>
            <p>Nie znaleziono żądań pasujących do kryteriów wyszukiwania.</p>
          </div>
        </mat-card-content>
      </mat-card>
    </div>

    <!-- Loading Template -->
    <ng-template #loadingTemplate>
      <div class="loading-container">
        <mat-spinner></mat-spinner>
        <p>Ładowanie żądań...</p>
      </div>
    </ng-template>
  `,
  styles: [`
    .request-management-container {
      padding: 16px;
      max-width: 1600px;
      margin: 0 auto;
    }

    .spacer {
      flex: 1 1 auto;
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

    .stat-card.pending {
      border-left: 4px solid #ff9800;
    }

    .stat-card.approved {
      border-left: 4px solid #4caf50;
    }

    .stat-card.rejected {
      border-left: 4px solid #f44336;
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

    .filters-card,
    .bulk-actions-card {
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
      align-items: center;
    }

    .filter-row mat-form-field {
      flex: 1;
      min-width: 200px;
    }

    .filter-actions {
      margin-left: auto;
    }

    .bulk-actions {
      display: flex;
      align-items: center;
      justify-content: space-between;
    }

    .bulk-actions .actions {
      display: flex;
      gap: 12px;
    }

    .selection-info {
      font-weight: 500;
      color: #1976d2;
    }

    .table-card {
      margin: 16px 0;
    }

    .table-container {
      overflow: auto;
      max-height: 700px;
    }

    .requests-table {
      width: 100%;
    }

    .clickable-row {
      cursor: pointer;
    }

    .clickable-row:hover {
      background-color: #f5f5f5;
    }

    .request-link {
      color: #1976d2;
      text-decoration: none;
      font-weight: 500;
    }

    .request-link:hover {
      text-decoration: underline;
    }

    .description {
      max-width: 200px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }

    .amount {
      font-weight: 500;
      color: #2e7d32;
    }

    /* Status styles */
    .status-pending {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .status-approved {
      background-color: #e8f5e8;
      color: #388e3c;
    }

    .status-rejected {
      background-color: #ffebee;
      color: #d32f2f;
    }

    .status-completed {
      background-color: #e3f2fd;
      color: #1976d2;
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
      .request-management-container {
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

      .bulk-actions {
        flex-direction: column;
        gap: 16px;
        align-items: stretch;
      }

      .bulk-actions .actions {
        justify-content: center;
      }
    }
  `]
})
export class RequestManagementComponent {
  private readonly router = inject(Router);
  private readonly changeRequestService = inject(ChangeRequestService);
  private readonly refundRequestService = inject(RefundRequestService);
  private readonly userService = inject(UserService);
  private readonly snackBar = inject(MatSnackBar);

  // Selection model for bulk operations
  selection = new SelectionModel<RequestItem>(true, []);

  // Filters
  readonly searchTerm = signal('');
  readonly typeFilter = signal<'change' | 'refund' | ''>('');
  readonly statusFilter = signal<string>('');
  readonly dateFromFilter = signal<Date | null>(null);
  readonly dateToFilter = signal<Date | null>(null);

  // Data
  readonly changeRequests = this.changeRequestService.changeRequests;
  readonly refundRequests = this.refundRequestService.refundRequests;
  readonly users = this.userService.users;
  readonly loading = computed(() =>
    this.changeRequestService.loading() ||
    this.refundRequestService.loading()
  );

  // Table configuration
  displayedColumns: string[] = [
    'select',
    'type',
    'requestNumber',
    'invoiceNumber',
    'description',
    'status',
    'requestedBy',
    'amount',
    'createdAt',
    'actions'
  ];

  // Computed properties
  readonly allRequests = computed((): RequestItem[] => {
    const changeReqs = this.changeRequests().map(req => ({
      id: req.id,
      type: 'change' as const,
      invoiceNumber: req.invoiceNumber,
      requestNumber: `CHG-${req.id}`,
      status: req.status,
      createdAt: req.createdAt,
      requestedBy: req.requestedBy,
      description: req.justification,
      originalData: req
    }));

    const refundReqs = this.refundRequests().map(req => ({
      id: req.id,
      type: 'refund' as const,
      invoiceNumber: req.invoiceNumber,
      requestNumber: `REF-${req.id}`,
      status: req.status,
      createdAt: req.createdAt,
      requestedBy: req.requestedBy,
      amount: req.requestedAmount,
      description: req.reason,
      originalData: req
    }));

    return [...changeReqs, ...refundReqs].sort((a, b) =>
      new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
    );
  });

  readonly filteredRequests = computed(() => {
    const requests = this.allRequests();
    const searchTerm = this.searchTerm().toLowerCase();
    const typeFilter = this.typeFilter();
    const statusFilter = this.statusFilter();
    const dateFrom = this.dateFromFilter();
    const dateTo = this.dateToFilter();

    return requests.filter(request => {      const matchesSearch = !searchTerm ||
        request.invoiceNumber?.toLowerCase().includes(searchTerm) ||
        request.requestNumber.toLowerCase().includes(searchTerm) ||
        request.description.toLowerCase().includes(searchTerm) ||
        request.requestedBy?.toLowerCase().includes(searchTerm);

      const matchesType = !typeFilter || request.type === typeFilter;

      const matchesStatus = !statusFilter || request.status === statusFilter;

      const requestDate = new Date(request.createdAt);
      const matchesDateFrom = !dateFrom || requestDate >= dateFrom;
      const matchesDateTo = !dateTo || requestDate <= dateTo;

      return matchesSearch && matchesType && matchesStatus && matchesDateFrom && matchesDateTo;
    });
  });

  readonly stats = computed(() => {
    const requests = this.allRequests();    return {
      totalRequests: requests.length,
      pendingRequests: requests.filter(r => r.status === RequestStatus.Pending).length,
      approvedRequests: requests.filter(r => r.status === RequestStatus.Approved).length,
      rejectedRequests: requests.filter(r => r.status === RequestStatus.Rejected).length
    };
  });

  ngOnInit() {
    // Load data
    this.changeRequestService.loadChangeRequests();
    this.refundRequestService.loadRefundRequests();
    this.userService.loadUsers();
  }

  goBack() {
    this.router.navigate(['/admin']);
  }

  refreshData() {
    this.changeRequestService.loadChangeRequests();
    this.refundRequestService.loadRefundRequests();
    this.userService.loadUsers();
  }

  clearFilters() {
    this.searchTerm.set('');
    this.typeFilter.set('');
    this.statusFilter.set('');
    this.dateFromFilter.set(null);
    this.dateToFilter.set(null);
  }

  getTypeText(type: 'change' | 'refund'): string {
    return type === 'change' ? 'Zmiana' : 'Zwrot';
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'Pending': return 'Oczekujące';
      case 'Approved': return 'Zatwierdzone';
      case 'Rejected': return 'Odrzucone';
      case 'Completed': return 'Zakończone';
      default: return status;
    }
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('pl-PL', {
      style: 'currency',
      currency: 'PLN'
    }).format(amount);
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

  getRequestRoute(request: RequestItem): string[] {
    if (request.type === 'change') {
      return ['/change-requests', request.id];
    } else {
      return ['/refund-requests', request.id];
    }
  }

  navigateToRequest(request: RequestItem) {
    this.router.navigate(this.getRequestRoute(request));
  }

  // Selection methods
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.filteredRequests().length;
    return numSelected === numRows;
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.filteredRequests());
  }
  canBulkApprove(): boolean {
    return this.selection.selected.some(request => request.status === RequestStatus.Pending);
  }

  canBulkReject(): boolean {
    return this.selection.selected.some(request => request.status === RequestStatus.Pending);
  }

  bulkApprove() {
    const pendingRequests = this.selection.selected.filter(request => request.status === RequestStatus.Pending);

    pendingRequests.forEach(request => {
      this.approveRequest(request);
    });

    this.selection.clear();
    this.snackBar.open(`Zatwierdzono ${pendingRequests.length} żądań`, 'Zamknij', {
      duration: 3000
    });
  }
  bulkReject() {
    const pendingRequests = this.selection.selected.filter(request => request.status === RequestStatus.Pending);

    pendingRequests.forEach(request => {
      this.rejectRequest(request);
    });

    this.selection.clear();
    this.snackBar.open(`Odrzucono ${pendingRequests.length} żądań`, 'Zamknij', {
      duration: 3000
    });
  }
  approveRequest(request: RequestItem) {
    if (request.type === 'change') {
      this.changeRequestService.updateChangeRequestStatus(request.id, RequestStatus.Approved);
    } else {
      this.refundRequestService.updateRefundRequestStatus(request.id, RequestStatus.Approved);
    }

    this.snackBar.open(`Żądanie ${request.requestNumber} zostało zatwierdzone`, 'Zamknij', {
      duration: 3000
    });
  }

  rejectRequest(request: RequestItem) {
    if (request.type === 'change') {
      this.changeRequestService.updateChangeRequestStatus(request.id, RequestStatus.Rejected);
    } else {
      this.refundRequestService.updateRefundRequestStatus(request.id, RequestStatus.Rejected);
    }

    this.snackBar.open(`Żądanie ${request.requestNumber} zostało odrzucone`, 'Zamknij', {
      duration: 3000
    });
  }
}

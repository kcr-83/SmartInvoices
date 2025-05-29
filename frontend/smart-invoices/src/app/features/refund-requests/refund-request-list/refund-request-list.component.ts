import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';

import { RefundRequestService } from '../../../services/refund-request.service';
import { UserService } from '../../../services/user.service';
import { RefundRequest, RefundReason, RequestStatus } from '../../../models';
import { UserRole } from '../../../models/user.model';

@Component({
  selector: 'app-refund-request-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSortModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatMenuModule,
    MatTooltipModule
  ],
  templateUrl: './refund-request-list.component.html',
  styleUrl: './refund-request-list.component.scss'
})
export class RefundRequestListComponent {
  private readonly refundRequestService = inject(RefundRequestService);
  private readonly userService = inject(UserService);
  private readonly router = inject(Router);
  // Filters
  readonly invoiceNumberFilter = signal('');
  readonly statusFilter = signal<RequestStatus | ''>('');
  readonly typeFilter = signal<RefundReason | ''>('');
  readonly dateFromFilter = signal<Date | null>(null);
  readonly dateToFilter = signal<Date | null>(null);

  // Sorting
  readonly sortField = signal<keyof RefundRequest>('requestDate');
  readonly sortDirection = signal<'asc' | 'desc'>('desc');

  // Table configuration
  readonly displayedColumns = [
    'invoiceNumber',
    'type',
    'amount',
    'status',
    'requestDate',
    'requestedBy',
    'actions'
  ];
  // Computed properties
  readonly currentUser = this.userService.currentUser;
  readonly isAdmin = computed(() => this.currentUser()?.role === UserRole.Admin);

  readonly refundRequests = computed(() => {
    const requests = this.refundRequestService.refundRequests();
    const invoiceFilter = this.invoiceNumberFilter().toLowerCase();
    const statusFilter = this.statusFilter();
    const typeFilter = this.typeFilter();
    const dateFrom = this.dateFromFilter();
    const dateTo = this.dateToFilter();

    let filtered = requests.filter(request => {
      const matchesInvoice = !invoiceFilter ||
        (request.invoiceNumber && request.invoiceNumber.toLowerCase().includes(invoiceFilter));
      const matchesStatus = !statusFilter || request.status === statusFilter;
      const matchesType = !typeFilter || request.type === typeFilter;      let matchesDateRange = true;
      if (dateFrom || dateTo) {
        if (request.requestDate) {
          const requestDate = new Date(request.requestDate);
          if (dateFrom && requestDate < dateFrom) matchesDateRange = false;
          if (dateTo && requestDate > dateTo) matchesDateRange = false;
        } else {
          matchesDateRange = false;
        }
      }

      return matchesInvoice && matchesStatus && matchesType && matchesDateRange;
    });

    // Apply sorting
    const field = this.sortField();
    const direction = this.sortDirection();

    filtered.sort((a, b) => {
      let valueA = a[field];
      let valueB = b[field];

      // Handle different data types
      if (field === 'requestDate') {
        valueA = new Date(valueA as string).getTime();
        valueB = new Date(valueB as string).getTime();
      } else if (field === 'amount') {
        valueA = Number(valueA);
        valueB = Number(valueB);
      } else {
        valueA = String(valueA).toLowerCase();
        valueB = String(valueB).toLowerCase();
      }

      if (valueA < valueB) return direction === 'asc' ? -1 : 1;
      if (valueA > valueB) return direction === 'asc' ? 1 : -1;
      return 0;
    });

    return filtered;
  });

  readonly loading = this.refundRequestService.loading;
  readonly error = this.refundRequestService.error;
  // Status and type options
  readonly statusOptions: { value: RequestStatus; label: string }[] = [
    { value: RequestStatus.Pending, label: 'Oczekujące' },
    { value: RequestStatus.UnderReview, label: 'W trakcie weryfikacji' },
    { value: RequestStatus.Approved, label: 'Zatwierdzone' },
    { value: RequestStatus.Rejected, label: 'Odrzucone' },
    { value: RequestStatus.Cancelled, label: 'Anulowane' },
    { value: RequestStatus.Processing, label: 'W trakcie' },
    { value: RequestStatus.Completed, label: 'Zakończone' }
  ];

  readonly reasonOptions: { value: RefundReason; label: string }[] = [
    { value: RefundReason.ProductDefective, label: 'Wadliwy produkt' },
    { value: RefundReason.ServiceNotProvided, label: 'Usługa nie została dostarczona' },
    { value: RefundReason.Overcharged, label: 'Zawyżona kwota' },
    { value: RefundReason.Duplicate, label: 'Duplikat płatności' },
    { value: RefundReason.Cancelled, label: 'Anulowane' },
    { value: RefundReason.Other, label: 'Inny powód' }
  ];

  ngOnInit() {
    this.refundRequestService.loadRefundRequests();
  }

  reloadRefundRequests() {
    this.refundRequestService.loadRefundRequests();
  }

  onSort(field: keyof RefundRequest) {
    if (this.sortField() === field) {
      this.sortDirection.set(this.sortDirection() === 'asc' ? 'desc' : 'asc');
    } else {
      this.sortField.set(field);
      this.sortDirection.set('asc');
    }
  }

  getSortIcon(field: keyof RefundRequest): string {
    if (this.sortField() !== field) return 'unfold_more';
    return this.sortDirection() === 'asc' ? 'keyboard_arrow_up' : 'keyboard_arrow_down';
  }
  getStatusColor(status: RequestStatus): string {
    switch (status) {
      case RequestStatus.Pending: return 'warn';
      case RequestStatus.UnderReview: return 'accent';
      case RequestStatus.Approved: return 'primary';
      case RequestStatus.Rejected: return 'accent';
      case RequestStatus.Cancelled: return '';
      case RequestStatus.Processing: return 'primary';
      case RequestStatus.Completed: return '';
      default: return '';
    }
  }

  getStatusLabel(status: RequestStatus): string {
    const option = this.statusOptions.find(opt => opt.value === status);
    return option?.label || status;
  }

  getTypeLabel(type: RefundReason | undefined): string {
    if (!type) return '';
    const option = this.reasonOptions.find(opt => opt.value === type);
    return option?.label || type;
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('pl-PL', {
      style: 'currency',
      currency: 'PLN'
    }).format(amount);
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('pl-PL');
  }

  clearFilters() {
    this.invoiceNumberFilter.set('');
    this.statusFilter.set('');
    this.typeFilter.set('');
    this.dateFromFilter.set(null);
    this.dateToFilter.set(null);
  }

  viewDetails(request: RefundRequest) {
    this.router.navigate(['/refund-requests', request.id]);
  }
  approveRequest(request: RefundRequest, event?: Event) {
    event?.stopPropagation();
    if (this.isAdmin()) {
      this.refundRequestService.updateRefundRequestStatus(request.id, RequestStatus.Approved);
    }
  }

  rejectRequest(request: RefundRequest, event?: Event) {
    event?.stopPropagation();
    if (this.isAdmin()) {
      this.refundRequestService.updateRefundRequestStatus(request.id, RequestStatus.Rejected);
    }
  }

  canApprove(request: RefundRequest): boolean {
    return this.isAdmin() && request.status === RequestStatus.Pending;
  }

  canReject(request: RefundRequest): boolean {
    return this.isAdmin() && request.status === RequestStatus.Pending;
  }

  exportToCSV() {
    const requests = this.refundRequests();
    if (requests.length === 0) return;

    const headers = [
      'Numer faktury',
      'Typ',
      'Kwota',
      'Status',
      'Data żądania',
      'Zgłaszający',
      'Powód'
    ];

    const csvContent = [
      headers.join(','),      ...requests.map(request => [
        request.invoiceNumber || '',
        this.getTypeLabel(request.type),
        (request.amount || request.requestedAmount || 0).toString(),
        this.getStatusLabel(request.status),
        request.requestDate ? this.formatDate(request.requestDate.toString()) : '',
        request.requestedBy || '',
        `"${(request.reason || '').replace(/"/g, '""')}"`
      ].join(','))
    ].join('\n');

    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', `refund-requests-${new Date().toISOString().split('T')[0]}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}

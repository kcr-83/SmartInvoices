import { Component, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import {
  ChangeRequest,
  RequestStatus,
  ChangeRequestFilter
} from '../../../models';
import { ChangeRequestService } from '../../../services/change-request.service';

@Component({
  selector: 'app-change-request-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatChipsModule,
    MatCardModule,
    MatPaginatorModule,
    MatSortModule,    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    RouterModule,
    FormsModule
  ],
  templateUrl: './change-request-list.component.html',
  styleUrls: ['./change-request-list.component.scss']
})
export class ChangeRequestListComponent implements OnInit {
  displayedColumns: string[] = [
    'invoiceNumber',
    'originalDescription',
    'requestedDescription',
    'originalQuantity',
    'requestedQuantity',
    'originalPrice',
    'requestedPrice',
    'status',
    'createdAt',
    'actions'
  ];
  // Filter signals
  filter = signal<ChangeRequestFilter>({});
  // Status options
  statusOptions = Object.values(RequestStatus);
  // Computed filtered and sorted change requests
  filteredChangeRequests = computed(() => {
    const requests = this.changeRequestService.changeRequests();
    const filter = this.filter();

    return requests.filter(request => {
      const matchesInvoice = !filter.invoiceNumber ||
        request.invoiceNumber?.toLowerCase().includes(filter.invoiceNumber.toLowerCase());
      const matchesStatus = !filter.status || request.status === filter.status;
      const matchesDateFrom = !filter.dateFrom ||
        new Date(request.createdAt) >= filter.dateFrom;
      const matchesDateTo = !filter.dateTo ||
        new Date(request.createdAt) <= filter.dateTo;

      return matchesInvoice && matchesStatus && matchesDateFrom && matchesDateTo;
    });
  });
  // Loading state
  loading = computed(() => this.changeRequestService.loading());

  constructor(private changeRequestService: ChangeRequestService) {}

  ngOnInit(): void {
    this.changeRequestService.loadChangeRequests();
  }
  // Filter methods
  updateFilter(updates: Partial<ChangeRequestFilter>): void {
    this.filter.update(current => ({ ...current, ...updates }));
  }

  clearFilters(): void {
    this.filter.set({});
  }

  // Status color helper
  getStatusColor(status: RequestStatus): string {
    switch (status) {
      case RequestStatus.Pending:
        return 'accent';
      case RequestStatus.Approved:
        return 'primary';
      case RequestStatus.Rejected:
        return 'warn';
      default:
        return '';
    }
  }

  // Status display helper
  getStatusDisplay(status: RequestStatus): string {
    switch (status) {
      case RequestStatus.Pending:
        return 'Oczekuje';
      case RequestStatus.Approved:
        return 'Zaakceptowana';
      case RequestStatus.Rejected:
        return 'Odrzucona';
      default:
        return status;
    }
  }

  // Actions
  approveRequest(requestId: string): void {
    this.changeRequestService.approveChangeRequest(requestId);
  }

  rejectRequest(requestId: string): void {
    this.changeRequestService.rejectChangeRequest(requestId);
  }

  // Event handlers
  onInvoiceNumberChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.updateFilter({invoiceNumber: target.value});
  }
}

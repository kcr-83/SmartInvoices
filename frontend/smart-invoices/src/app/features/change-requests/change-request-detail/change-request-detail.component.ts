import { Component, signal, computed, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { MatTabsModule } from '@angular/material/tabs';
import { MatListModule } from '@angular/material/list';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import {
  ChangeRequest,
  RequestStatus,
  InvoiceItem
} from '../../../models';
import { ChangeRequestService } from '../../../services/change-request.service';
import { InvoiceService } from '../../../services/invoice.service';

@Component({
  selector: 'app-change-request-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatChipsModule,
    MatTabsModule,
    MatListModule,
    MatDialogModule,
    MatSnackBarModule
  ],
  templateUrl: './change-request-detail.component.html',
  styleUrls: ['./change-request-detail.component.scss']
})
export class ChangeRequestDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  changeRequestId = signal<string>('');

  // Get change request from service
  changeRequest = computed(() => {
    const id = this.changeRequestId();
    return this.changeRequestService.changeRequests()
      .find(request => request.id === id);
  });
  // Get related invoice
  invoice = computed(() => {
    const request = this.changeRequest();
    if (!request) return null;

    return this.invoiceService.invoices()
      .find(invoice => invoice.id === request.invoiceId);
  });

  loading = computed(() => this.changeRequestService.loading());

  constructor(
    private changeRequestService: ChangeRequestService,
    private invoiceService: InvoiceService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.changeRequestId.set(id);
        this.loadData();
      }
    });
  }

  private loadData(): void {
    this.changeRequestService.loadChangeRequests();
    this.invoiceService.loadInvoices();
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

  // Calculate financial impact
  getFinancialImpact(): number {
    const request = this.changeRequest();
    if (!request) return 0;

    const originalTotal = request.originalItem.quantity * request.originalItem.unitPrice;
    const requestedTotal = request.requestedItem.quantity * request.requestedItem.unitPrice;

    return requestedTotal - originalTotal;
  }

  // Actions
  approveRequest(): void {
    const request = this.changeRequest();
    if (!request) return;

    this.changeRequestService.approveChangeRequest(request.id);
    this.snackBar.open('Żądanie zostało zaakceptowane', 'Zamknij', {
      duration: 3000,
      panelClass: ['success-snackbar']
    });
  }

  rejectRequest(): void {
    const request = this.changeRequest();
    if (!request) return;

    this.changeRequestService.rejectChangeRequest(request.id);
    this.snackBar.open('Żądanie zostało odrzucone', 'Zamknij', {
      duration: 3000,
      panelClass: ['error-snackbar']
    });
  }

  navigateToInvoice(): void {
    const request = this.changeRequest();
    if (!request) return;

    this.router.navigate(['/invoices', request.invoiceId]);
  }

  goBack(): void {
    this.router.navigate(['/change-requests']);
  }
}

import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { RefundRequestService } from '../../../services/refund-request.service';
import { InvoiceService } from '../../../services/invoice.service';
import { UserService } from '../../../services/user.service';
import { RefundRequest, RefundReason, RequestStatus, RefundStatus } from '../../../models';
import { InvoiceStatus, Invoice } from '../../../models/invoice.model';
import { UserRole } from '../../../models/user.model';

@Component({
  selector: 'app-refund-request-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatListModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule
  ],  templateUrl: './refund-request-detail.component.html',
  styleUrl: './refund-request-detail.component.scss'
})
export class RefundRequestDetailComponent {
  // Expose enums to template
  protected readonly RefundReason = RefundReason;
  protected readonly RequestStatus = RequestStatus;
  protected readonly InvoiceStatus = InvoiceStatus;

  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly refundRequestService = inject(RefundRequestService);
  private readonly invoiceService = inject(InvoiceService);
  private readonly userService = inject(UserService);

  readonly requestId = signal<string>('');
  readonly selectedTabIndex = signal(0);

  // Computed properties
  readonly currentUser = this.userService.currentUser;
  readonly isAdmin = computed(() => this.currentUser()?.role === UserRole.Admin);

  readonly refundRequest = computed(() => {
    const id = this.requestId();
    return this.refundRequestService.refundRequests().find(req => req.id === id);
  });

  readonly relatedInvoice = computed(() => {
    const request = this.refundRequest();
    if (!request) return null;
    return this.invoiceService.invoices().find(inv => inv.invoiceNumber === request.invoiceNumber);
  });

  readonly loading = computed(() =>
    this.refundRequestService.loading() || this.invoiceService.loading()
  );

  readonly error = computed(() =>
    this.refundRequestService.error() || this.invoiceService.error()
  );
  readonly canApprove = computed(() => {
    const request = this.refundRequest();
    return this.isAdmin() && request?.status === RequestStatus.Pending;
  });

  readonly canReject = computed(() => {
    const request = this.refundRequest();
    return this.isAdmin() && request?.status === RequestStatus.Pending;
  });

  readonly canProcess = computed(() => {
    const request = this.refundRequest();
    return this.isAdmin() && request?.status === RequestStatus.Approved;
  });

  readonly refundHistory = computed(() => {
    const request = this.refundRequest();
    if (!request) return [];

    // Mock history - in real app, this would come from the service
    const history = [
      {
        id: '1',
        action: 'Utworzenie żądania',
        description: `Użytkownik ${request.requestedBy} utworzył żądanie zwrotu`,
        user: request.requestedBy,
        date: request.requestDate,
        icon: 'add_circle'
      }
    ];    if (request.processedBy && request.requestDate) {
      const processedDate = new Date(request.requestDate);
      processedDate.setDate(processedDate.getDate() + 1);

      history.push({
        id: '2',
        action: request.status === RequestStatus.Approved ? 'Zatwierdzenie' :
               request.status === RequestStatus.Rejected ? 'Odrzucenie' : 'Przetwarzanie',
        description: `${request.processedBy} ${
          request.status === RequestStatus.Approved ? 'zatwierdził' :
          request.status === RequestStatus.Rejected ? 'odrzucił' : 'rozpoczął przetwarzanie'
        } żądania zwrotu`,
        user: request.processedBy,
        date: processedDate,
        icon: request.status === RequestStatus.Approved ? 'check_circle' :
              request.status === RequestStatus.Rejected ? 'cancel' : 'play_circle'
      });
    }

    return history.sort((a, b) => {
      const dateA = a.date ? new Date(a.date).getTime() : 0;
      const dateB = b.date ? new Date(b.date).getTime() : 0;
      return dateB - dateA;
    });
  });

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.requestId.set(params['id']);
    });

    // Load data
    this.refundRequestService.loadRefundRequests();
    this.invoiceService.loadInvoices();
  }

  goBack() {
    this.router.navigate(['/refund-requests']);
  }
  getStatusColor(status: RefundStatus): string {
    switch (status) {
      case RequestStatus.Pending: return 'warn';
      case RequestStatus.Approved: return 'primary';
      case RequestStatus.Rejected: return 'accent';
      case RequestStatus.Processing: return 'primary';
      case RequestStatus.Completed: return '';
      default: return '';
    }
  }
  getStatusLabel(status: RequestStatus | undefined): string {
    if (!status) return '';

    const statusMap: Record<string, string> = {
      [RequestStatus.Pending]: 'Oczekujące',
      [RequestStatus.UnderReview]: 'W trakcie weryfikacji',
      [RequestStatus.Approved]: 'Zatwierdzone',
      [RequestStatus.Rejected]: 'Odrzucone',
      [RequestStatus.Cancelled]: 'Anulowane',
      [RequestStatus.Processing]: 'W trakcie',
      [RequestStatus.Completed]: 'Zakończone'
    };
    return statusMap[status] || status;
  }
  getTypeLabel(type: RefundReason | undefined): string {
    if (!type) return '';

    const typeMap: Record<string, string> = {
      [RefundReason.ProductDefective]: 'Wadliwy produkt',
      [RefundReason.ServiceNotProvided]: 'Usługa nie została dostarczona',
      [RefundReason.Overcharged]: 'Zawyżona kwota',
      [RefundReason.Duplicate]: 'Duplikat płatności',
      [RefundReason.Cancelled]: 'Anulowane',
      [RefundReason.Other]: 'Inny powód'
    };

    return typeMap[type] || type;
  }
  formatCurrency(amount: number | undefined): string {
    if (amount === undefined || amount === null) return '0,00 PLN';
    return new Intl.NumberFormat('pl-PL', {
      style: 'currency',
      currency: 'PLN'
    }).format(amount);
  }

  formatDate(date: string | Date | undefined): string {
    if (!date) return '';
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return dateObj.toLocaleDateString('pl-PL', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  formatShortDate(date: string | Date | undefined): string {
    if (!date) return '';
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return dateObj.toLocaleDateString('pl-PL');
  }
  approveRequest() {
    const request = this.refundRequest();
    if (request && this.canApprove()) {
      this.refundRequestService.updateRefundRequestStatus(request.id, RequestStatus.Approved);
    }
  }

  rejectRequest() {
    const request = this.refundRequest();
    if (request && this.canReject()) {
      this.refundRequestService.updateRefundRequestStatus(request.id, RequestStatus.Rejected);
    }
  }

  processRequest() {
    const request = this.refundRequest();
    if (request && this.canProcess()) {
      this.refundRequestService.updateRefundRequestStatus(request.id, RequestStatus.Processing);
    }
  }

  completeRequest() {
    const request = this.refundRequest();
    if (request && this.isAdmin()) {
      this.refundRequestService.updateRefundRequestStatus(request.id, RequestStatus.Completed);
    }
  }

  viewInvoice() {
    const invoice = this.relatedInvoice();
    if (invoice) {
      this.router.navigate(['/invoices', invoice.id]);
    }
  }

  downloadDocument(documentUrl: string) {
    // In a real app, this would download the document
    window.open(documentUrl, '_blank');
  }

  exportToPDF() {
    const request = this.refundRequest();
    if (!request) return;

    // Mock PDF export - in real app, this would generate and download a PDF    console.log('Exporting refund request to PDF:', request);

    // Create a simple text representation for demo
    const content = `
ŻĄDANIE ZWROTU FAKTURY

Numer faktury: ${request.invoiceNumber || 'Brak'}
Typ zwrotu: ${request.type ? this.getTypeLabel(request.type) : 'Brak'}
Kwota: ${this.formatCurrency(request.amount)}
Status: ${this.getStatusLabel(request.status)}
Data żądania: ${this.formatDate(request.requestDate)}
Zgłaszający: ${request.requestedBy || 'Brak'}

Powód:
${request.reason || 'Brak'}
    `.trim();

    const blob = new Blob([content], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `refund-request-${request.invoiceNumber}-${request.id}.txt`;
    link.click();
    URL.revokeObjectURL(url);
  }
}

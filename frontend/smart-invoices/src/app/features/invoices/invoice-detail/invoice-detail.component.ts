import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDividerModule } from '@angular/material/divider';

import { InvoiceService, ChangeRequestService, RefundRequestService } from '../../../services';
import { Invoice, InvoiceStatus, ChangeRequest, RefundRequest } from '../../../models';

@Component({
  selector: 'app-invoice-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatChipsModule,
    MatToolbarModule,
    MatTabsModule,
    MatDividerModule
  ],
  template: `
    <div class="invoice-detail-container" *ngIf="invoice(); else loadingTemplate">
      <!-- Header -->
      <mat-toolbar color="primary">
        <button mat-icon-button (click)="goBack()">
          <mat-icon>arrow_back</mat-icon>
        </button>
        <span>Faktura {{ invoice()?.invoiceNumber }}</span>
        <span class="spacer"></span>
        <button mat-raised-button color="accent" (click)="exportToPdf()">
          <mat-icon>file_download</mat-icon>
          Eksportuj PDF
        </button>
      </mat-toolbar>

      <div class="content">
        <!-- Invoice Information -->
        <mat-card class="invoice-info-card">
          <mat-card-header>
            <mat-card-title>Informacje o fakturze</mat-card-title>
            <mat-card-subtitle>
              <mat-chip [class]="'status-' + invoice()?.status">
                {{ getStatusText(invoice()?.status!) }}
              </mat-chip>
            </mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            <div class="invoice-header-info">
              <div class="info-section">
                <h4>Podstawowe informacje</h4>
                <div class="info-grid">
                  <div class="info-item">
                    <label>Numer faktury:</label>
                    <span>{{ invoice()?.invoiceNumber }}</span>
                  </div>
                  <div class="info-item">
                    <label>Data wystawienia:</label>
                    <span>{{ invoice()?.issueDate | date:'dd.MM.yyyy' }}</span>
                  </div>
                  <div class="info-item">
                    <label>Termin płatności:</label>
                    <span>{{ invoice()?.dueDate | date:'dd.MM.yyyy' }}</span>
                  </div>
                  <div class="info-item">
                    <label>Kwota całkowita:</label>
                    <span class="amount">{{ invoice()?.totalAmount | currency:'PLN':'symbol':'1.2-2' }}</span>
                  </div>
                  <div class="info-item">
                    <label>Waluta:</label>
                    <span>{{ invoice()?.currency }}</span>
                  </div>
                </div>
              </div>

              <mat-divider></mat-divider>

              <div class="info-section">
                <div class="parties-info">
                  <div class="party-info">
                    <h4>Dostawca</h4>
                    <p><strong>{{ invoice()?.vendorName }}</strong></p>
                    <p *ngIf="invoice()?.vendorAddress">{{ invoice()?.vendorAddress }}</p>
                  </div>
                  <div class="party-info">
                    <h4>Klient</h4>
                    <p><strong>{{ invoice()?.customerName }}</strong></p>
                    <p *ngIf="invoice()?.customerAddress">{{ invoice()?.customerAddress }}</p>
                  </div>
                </div>
              </div>
            </div>
          </mat-card-content>
        </mat-card>

        <!-- Tabs for Items and Requests -->
        <mat-card class="tabs-card">
          <mat-tab-group>
            <!-- Invoice Items Tab -->
            <mat-tab label="Pozycje faktury">
              <div class="tab-content">
                <div class="items-header">
                  <h3>Pozycje na fakturze</h3>
                  <button mat-raised-button color="primary" (click)="requestChange()">
                    <mat-icon>edit</mat-icon>
                    Zgłoś zmianę
                  </button>
                </div>

                <div class="table-container">
                  <table mat-table [dataSource]="invoice()?.items || []" class="items-table">
                    <!-- Description Column -->
                    <ng-container matColumnDef="description">
                      <th mat-header-cell *matHeaderCellDef>Opis</th>
                      <td mat-cell *matCellDef="let item">{{ item.description }}</td>
                    </ng-container>

                    <!-- Quantity Column -->
                    <ng-container matColumnDef="quantity">
                      <th mat-header-cell *matHeaderCellDef>Ilość</th>
                      <td mat-cell *matCellDef="let item">{{ item.quantity }}</td>
                    </ng-container>

                    <!-- Unit Price Column -->
                    <ng-container matColumnDef="unitPrice">
                      <th mat-header-cell *matHeaderCellDef>Cena jednostkowa</th>
                      <td mat-cell *matCellDef="let item">
                        {{ item.unitPrice | currency:'PLN':'symbol':'1.2-2' }}
                      </td>
                    </ng-container>

                    <!-- Total Price Column -->
                    <ng-container matColumnDef="totalPrice">
                      <th mat-header-cell *matHeaderCellDef>Cena całkowita</th>
                      <td mat-cell *matCellDef="let item">
                        {{ item.totalPrice | currency:'PLN':'symbol':'1.2-2' }}
                      </td>
                    </ng-container>

                    <!-- Category Column -->
                    <ng-container matColumnDef="category">
                      <th mat-header-cell *matHeaderCellDef>Kategoria</th>
                      <td mat-cell *matCellDef="let item">
                        <mat-chip *ngIf="item.category">{{ item.category }}</mat-chip>
                      </td>
                    </ng-container>

                    <tr mat-header-row *matHeaderRowDef="itemColumns"></tr>
                    <tr mat-row *matRowDef="let row; columns: itemColumns;"></tr>
                  </table>
                </div>

                <div class="items-summary">
                  <p><strong>Suma: {{ invoice()?.totalAmount | currency:'PLN':'symbol':'1.2-2' }}</strong></p>
                </div>
              </div>
            </mat-tab>            <!-- Change Requests Tab -->
            <mat-tab [label]="'Wnioski o zmiany (' + changeRequestsCount() + ')'">
              <div class="tab-content">
                <div class="requests-header">
                  <h3>Wnioski o zmiany</h3>
                  <button mat-raised-button color="primary" (click)="requestChange()">
                    <mat-icon>add</mat-icon>
                    Nowy wniosek
                  </button>
                </div>

                <div *ngIf="changeRequests().length === 0" class="empty-state">
                  <mat-icon>assignment</mat-icon>
                  <p>Brak wniosków o zmiany dla tej faktury</p>
                </div>

                <div *ngFor="let request of changeRequests()" class="request-card">
                  <mat-card>
                    <mat-card-header>
                      <mat-card-title>Wniosek #{{ request.id }}</mat-card-title>
                      <mat-card-subtitle>
                        <mat-chip [class]="'status-' + request.status">
                          {{ getRequestStatusText(request.status) }}
                        </mat-chip>
                      </mat-card-subtitle>
                    </mat-card-header>
                    <mat-card-content>
                      <p><strong>Uzasadnienie:</strong> {{ request.justification }}</p>
                      <p><strong>Data utworzenia:</strong> {{ request.createdAt | date:'dd.MM.yyyy HH:mm' }}</p>
                      <div *ngIf="request.adminComments">
                        <p><strong>Komentarz administratora:</strong> {{ request.adminComments }}</p>
                      </div>
                    </mat-card-content>
                    <mat-card-actions>
                      <button mat-button [routerLink]="['/change-requests', request.id]">
                        Zobacz szczegóły
                      </button>
                    </mat-card-actions>
                  </mat-card>
                </div>
              </div>
            </mat-tab>            <!-- Refund Requests Tab -->
            <mat-tab [label]="'Wnioski o zwrot (' + refundRequestsCount() + ')'">
              <div class="tab-content">
                <div class="requests-header">
                  <h3>Wnioski o zwrot</h3>
                  <button mat-raised-button color="warn" (click)="requestRefund()">
                    <mat-icon>money_off</mat-icon>
                    Zgłoś zwrot
                  </button>
                </div>

                <div *ngIf="refundRequests().length === 0" class="empty-state">
                  <mat-icon>money_off</mat-icon>
                  <p>Brak wniosków o zwrot dla tej faktury</p>
                </div>

                <div *ngFor="let request of refundRequests()" class="request-card">
                  <mat-card>
                    <mat-card-header>
                      <mat-card-title>Wniosek o zwrot #{{ request.id }}</mat-card-title>
                      <mat-card-subtitle>
                        <mat-chip [class]="'status-' + request.status">
                          {{ getRequestStatusText(request.status) }}
                        </mat-chip>
                      </mat-card-subtitle>
                    </mat-card-header>
                    <mat-card-content>
                      <p><strong>Powód:</strong> {{ getRefundReasonText(request.reason) }}</p>
                      <p><strong>Kwota:</strong> {{ request.requestedAmount | currency:'PLN':'symbol':'1.2-2' }}</p>
                      <p><strong>Uzasadnienie:</strong> {{ request.justification }}</p>
                      <p><strong>Data utworzenia:</strong> {{ request.createdAt | date:'dd.MM.yyyy HH:mm' }}</p>
                    </mat-card-content>
                    <mat-card-actions>
                      <button mat-button [routerLink]="['/refund-requests', request.id]">
                        Zobacz szczegóły
                      </button>
                    </mat-card-actions>
                  </mat-card>
                </div>
              </div>
            </mat-tab>
          </mat-tab-group>
        </mat-card>
      </div>
    </div>

    <!-- Loading Template -->
    <ng-template #loadingTemplate>
      <div class="loading-container">
        <p>Ładowanie szczegółów faktury...</p>
      </div>
    </ng-template>
  `,
  styles: [`
    .invoice-detail-container {
      min-height: 100vh;
    }

    .spacer {
      flex: 1 1 auto;
    }

    .content {
      padding: 16px;
      max-width: 1200px;
      margin: 0 auto;
    }

    .invoice-info-card,
    .tabs-card {
      margin: 16px 0;
    }

    .invoice-header-info {
      display: flex;
      flex-direction: column;
      gap: 24px;
    }

    .info-section h4 {
      margin: 0 0 16px 0;
      color: #333;
    }

    .info-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 16px;
    }

    .info-item {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }

    .info-item label {
      font-weight: 500;
      color: #666;
      font-size: 0.9em;
    }

    .info-item span {
      font-size: 1.1em;
    }

    .amount {
      font-weight: 600;
      color: #1976d2;
    }

    .parties-info {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 32px;
    }

    .party-info h4 {
      margin: 0 0 8px 0;
      color: #333;
    }

    .party-info p {
      margin: 4px 0;
    }

    .tab-content {
      padding: 24px 0;
    }

    .items-header,
    .requests-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 24px;
    }

    .items-header h3,
    .requests-header h3 {
      margin: 0;
    }

    .table-container {
      overflow: auto;
      margin: 16px 0;
    }

    .items-table {
      width: 100%;
    }

    .items-summary {
      text-align: right;
      margin-top: 16px;
      padding: 16px;
      background-color: #f5f5f5;
      border-radius: 4px;
    }

    .request-card {
      margin: 16px 0;
    }

    .empty-state {
      text-align: center;
      padding: 32px;
      color: #666;
    }

    .empty-state mat-icon {
      font-size: 48px;
      height: 48px;
      width: 48px;
      margin-bottom: 16px;
      opacity: 0.6;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      align-items: center;
      height: 200px;
    }

    /* Status styles */
    .status-draft { background-color: #e3f2fd; color: #1976d2; }
    .status-sent { background-color: #fff3e0; color: #f57c00; }
    .status-paid { background-color: #e8f5e8; color: #388e3c; }
    .status-overdue { background-color: #ffebee; color: #d32f2f; }
    .status-cancelled { background-color: #f5f5f5; color: #757575; }
    .status-pending { background-color: #fff3e0; color: #f57c00; }
    .status-under_review { background-color: #e3f2fd; color: #1976d2; }
    .status-approved { background-color: #e8f5e8; color: #388e3c; }
    .status-rejected { background-color: #ffebee; color: #d32f2f; }

    @media (max-width: 768px) {
      .content {
        padding: 8px;
      }

      .parties-info {
        grid-template-columns: 1fr;
        gap: 16px;
      }

      .info-grid {
        grid-template-columns: 1fr;
      }

      .items-header,
      .requests-header {
        flex-direction: column;
        align-items: stretch;
        gap: 16px;
      }
    }
  `]
})
export class InvoiceDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly invoiceService = inject(InvoiceService);
  private readonly changeRequestService = inject(ChangeRequestService);
  private readonly refundRequestService = inject(RefundRequestService);

  // Signals
  readonly invoice = signal<Invoice | null>(null);
  readonly changeRequests = signal<ChangeRequest[]>([]);
  readonly refundRequests = signal<RefundRequest[]>([]);

  // Computed properties
  readonly changeRequestsCount = computed(() => this.changeRequests().length);
  readonly refundRequestsCount = computed(() => this.refundRequests().length);

  // Table columns
  itemColumns: string[] = ['description', 'quantity', 'unitPrice', 'totalPrice', 'category'];

  ngOnInit(): void {
    const invoiceId = this.route.snapshot.paramMap.get('id');
    if (invoiceId) {
      this.loadInvoiceDetails(invoiceId);
    }
  }

  private loadInvoiceDetails(invoiceId: string): void {
    // Load invoice
    this.invoiceService.getInvoice(invoiceId).subscribe(invoice => {
      if (invoice) {
        this.invoice.set(invoice);
      } else {
        this.router.navigate(['/invoices']);
      }
    });

    // Load change requests
    this.changeRequestService.getChangeRequestsForInvoice(invoiceId).subscribe(requests => {
      this.changeRequests.set(requests);
    });

    // Load refund requests
    this.refundRequestService.getRefundRequestsForInvoice(invoiceId).subscribe(requests => {
      this.refundRequests.set(requests);
    });
  }

  goBack(): void {
    this.router.navigate(['/invoices']);
  }

  exportToPdf(): void {
    const invoiceId = this.invoice()?.id;
    if (invoiceId) {
      this.invoiceService.exportToPdf(invoiceId).subscribe(blob => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `faktura-${this.invoice()?.invoiceNumber}.pdf`;
        link.click();
        window.URL.revokeObjectURL(url);
      });
    }
  }

  requestChange(): void {
    // Navigate to change request form with invoice ID
    this.router.navigate(['/change-requests'], {
      queryParams: { invoiceId: this.invoice()?.id }
    });
  }

  requestRefund(): void {
    // Navigate to refund request form with invoice ID
    this.router.navigate(['/refund-requests'], {
      queryParams: { invoiceId: this.invoice()?.id }
    });
  }

  getStatusText(status: InvoiceStatus): string {
    const statusMap = {
      [InvoiceStatus.Draft]: 'Szkic',
      [InvoiceStatus.Sent]: 'Wysłana',
      [InvoiceStatus.Paid]: 'Opłacona',
      [InvoiceStatus.Overdue]: 'Przeterminowana',
      [InvoiceStatus.Cancelled]: 'Anulowana'
    };
    return statusMap[status] || status;
  }

  getRequestStatusText(status: string): string {
    const statusMap: { [key: string]: string } = {
      'pending': 'Oczekujący',
      'under_review': 'W trakcie przeglądu',
      'approved': 'Zatwierdzony',
      'rejected': 'Odrzucony',
      'cancelled': 'Anulowany'
    };
    return statusMap[status] || status;
  }

  getRefundReasonText(reason: string): string {
    const reasonMap: { [key: string]: string } = {
      'product_defective': 'Produkt wadliwy',
      'service_not_provided': 'Usługa nie została wykonana',
      'overcharged': 'Naliczona za wysoka kwota',
      'duplicate': 'Duplikat faktury',
      'cancelled': 'Anulowanie',
      'other': 'Inny powód'
    };
    return reasonMap[reason] || reason;
  }
}

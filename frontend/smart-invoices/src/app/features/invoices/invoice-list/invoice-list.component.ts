import { Component, OnInit, signal, computed, inject, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { InvoiceService } from '../../../services';
import { Invoice, InvoiceStatus, InvoiceFilter, InvoiceSortOptions } from '../../../models';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatChipsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatToolbarModule,
    MatTooltipModule
  ],
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InvoiceListComponent implements OnInit {
  private readonly invoiceService = inject(InvoiceService);
  private readonly fb = inject(FormBuilder);

  // Table configuration
  displayedColumns: string[] = [
    'invoiceNumber',
    'issueDate',
    'dueDate',
    'vendorName',
    'customerName',
    'totalAmount',
    'status',
    'actions'
  ];

  // Filter form
  filterForm: FormGroup = this.fb.group({
    searchTerm: [''],
    status: [''],
    dateFrom: [null],
    dateTo: [null],
    minAmount: [null],
    maxAmount: [null]
  });

  // Computed properties
  readonly loading = this.invoiceService.loading;
  readonly filteredInvoices = this.invoiceService.filteredInvoices;
  readonly filteredInvoicesCount = computed(() => this.filteredInvoices().length);

  ngOnInit(): void {
    this.invoiceService.loadInvoices().subscribe();
  }

  applyFilters(): void {
    const formValue = this.filterForm.value;
    const filter: InvoiceFilter = {
      searchTerm: formValue.searchTerm || undefined,
      status: formValue.status || undefined,
      dateFrom: formValue.dateFrom || undefined,
      dateTo: formValue.dateTo || undefined,
      minAmount: formValue.minAmount || undefined,
      maxAmount: formValue.maxAmount || undefined
    };

    this.invoiceService.updateFilter(filter);
  }

  clearFilters(): void {
    this.filterForm.reset();
    this.invoiceService.clearFilters();
  }

  refreshInvoices(): void {
    this.invoiceService.loadInvoices().subscribe();
  }

  exportToPdf(invoiceId: string): void {
    this.invoiceService.exportToPdf(invoiceId).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `faktura-${invoiceId}.pdf`;
      link.click();
      window.URL.revokeObjectURL(url);
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

  getInvoiceCountText(): string {
    const count = this.filteredInvoicesCount();
    if (count === 1) return 'fakturę';
    if (count >= 2 && count <= 4) return 'faktury';
    return 'faktur';
  }
}

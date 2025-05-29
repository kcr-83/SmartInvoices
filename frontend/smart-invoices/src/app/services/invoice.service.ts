import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, map, delay, of } from 'rxjs';
import {
  Invoice,
  InvoiceFilter,
  InvoiceSortOptions,
  InvoiceStatus,
  ApiResponse,
  PaginatedResponse,
  PaginationOptions
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private readonly apiUrl = '/api/invoices';
  // Signals for reactive state management
  private readonly _invoices = signal<Invoice[]>([]);
  private readonly _loading = signal(false);
  private readonly _error = signal<string | null>(null);
  private readonly _filter = signal<InvoiceFilter>({});
  private readonly _sortOptions = signal<InvoiceSortOptions>({ field: 'issueDate', direction: 'desc' });

  // Public computed signals
  readonly invoices = this._invoices.asReadonly();
  readonly loading = this._loading.asReadonly();
  readonly error = this._error.asReadonly();
  readonly filter = this._filter.asReadonly();
  readonly sortOptions = this._sortOptions.asReadonly();

  // Filtered and sorted invoices
  readonly filteredInvoices = computed(() => {
    let result = this._invoices();
    const filterValue = this._filter();
    const sortValue = this._sortOptions();

    // Apply filters
    if (filterValue.searchTerm) {
      const term = filterValue.searchTerm.toLowerCase();
      result = result.filter(invoice =>
        invoice.invoiceNumber.toLowerCase().includes(term) ||
        invoice.vendorName.toLowerCase().includes(term) ||
        invoice.customerName.toLowerCase().includes(term)
      );
    }

    if (filterValue.status) {
      result = result.filter(invoice => invoice.status === filterValue.status);
    }

    if (filterValue.dateFrom) {
      result = result.filter(invoice => new Date(invoice.issueDate) >= filterValue.dateFrom!);
    }

    if (filterValue.dateTo) {
      result = result.filter(invoice => new Date(invoice.issueDate) <= filterValue.dateTo!);
    }

    if (filterValue.minAmount !== undefined) {
      result = result.filter(invoice => invoice.totalAmount >= filterValue.minAmount!);
    }

    if (filterValue.maxAmount !== undefined) {
      result = result.filter(invoice => invoice.totalAmount <= filterValue.maxAmount!);
    }

    // Apply sorting
    result.sort((a, b) => {
      let aValue: any, bValue: any;

      switch (sortValue.field) {
        case 'invoiceNumber':
          aValue = a.invoiceNumber;
          bValue = b.invoiceNumber;
          break;
        case 'issueDate':
          aValue = new Date(a.issueDate);
          bValue = new Date(b.issueDate);
          break;
        case 'dueDate':
          aValue = new Date(a.dueDate);
          bValue = new Date(b.dueDate);
          break;
        case 'totalAmount':
          aValue = a.totalAmount;
          bValue = b.totalAmount;
          break;
        case 'vendorName':
          aValue = a.vendorName;
          bValue = b.vendorName;
          break;
        default:
          return 0;
      }

      if (aValue < bValue) return sortValue.direction === 'asc' ? -1 : 1;
      if (aValue > bValue) return sortValue.direction === 'asc' ? 1 : -1;
      return 0;
    });

    return result;
  });

  constructor(private http: HttpClient) {
    this.loadMockData();
  }

  // Load invoices from API (currently using mock data)
  loadInvoices(): Observable<Invoice[]> {
    this._loading.set(true);

    // Mock API call - replace with actual HTTP call
    return this.getMockInvoices().pipe(
      delay(500), // Simulate network delay
      map(invoices => {
        this._invoices.set(invoices);
        this._loading.set(false);
        return invoices;
      })
    );
  }

  // Get single invoice by ID
  getInvoice(id: string): Observable<Invoice | undefined> {
    return of(this._invoices().find(invoice => invoice.id === id)).pipe(
      delay(300)
    );
  }

  // Update filter
  updateFilter(filter: Partial<InvoiceFilter>): void {
    this._filter.update(current => ({ ...current, ...filter }));
  }

  // Update sort options
  updateSortOptions(options: InvoiceSortOptions): void {
    this._sortOptions.set(options);
  }

  // Clear filters
  clearFilters(): void {
    this._filter.set({});
  }

  // Export invoice to PDF (mock implementation)
  exportToPdf(invoiceId: string): Observable<Blob> {
    // Mock implementation - would call actual API
    return of(new Blob(['Mock PDF content'], { type: 'application/pdf' })).pipe(
      delay(1000)
    );
  }

  // Private method to load mock data
  private loadMockData(): void {
    this.loadInvoices().subscribe();
  }

  // Mock data generator
  private getMockInvoices(): Observable<Invoice[]> {
    const mockInvoices: Invoice[] = [
      {
        id: '1',
        invoiceNumber: 'INV-2025-001',
        issueDate: new Date('2025-01-15'),
        dueDate: new Date('2025-02-15'),
        totalAmount: 1250.00,
        currency: 'PLN',
        status: InvoiceStatus.Paid,
        vendorName: 'Tech Solutions Sp. z o.o.',
        vendorAddress: 'ul. Technologiczna 15, 00-001 Warszawa',
        customerName: 'Business Corp Sp. z o.o.',
        customerAddress: 'ul. Biznesowa 25, 00-002 Kraków',
        items: [
          {
            id: '1-1',
            invoiceId: '1',
            description: 'Licencja oprogramowania - roczna',
            quantity: 1,
            unitPrice: 1000.00,
            totalPrice: 1000.00,
            category: 'Software'
          },
          {
            id: '1-2',
            invoiceId: '1',
            description: 'Wsparcie techniczne',
            quantity: 5,
            unitPrice: 50.00,
            totalPrice: 250.00,
            category: 'Services'
          }
        ],
        createdAt: new Date('2025-01-15'),
        updatedAt: new Date('2025-01-16')
      },
      {
        id: '2',
        invoiceNumber: 'INV-2025-002',
        issueDate: new Date('2025-02-01'),
        dueDate: new Date('2025-03-01'),
        totalAmount: 750.50,
        currency: 'PLN',
        status: InvoiceStatus.Sent,
        vendorName: 'Marketing Pro Sp. z o.o.',
        vendorAddress: 'ul. Reklamowa 30, 00-003 Gdańsk',
        customerName: 'Small Business Ltd.',
        customerAddress: 'ul. Mała 5, 00-004 Wrocław',
        items: [
          {
            id: '2-1',
            invoiceId: '2',
            description: 'Kampania reklamowa online',
            quantity: 1,
            unitPrice: 750.50,
            totalPrice: 750.50,
            category: 'Marketing'
          }
        ],
        createdAt: new Date('2025-02-01'),
        updatedAt: new Date('2025-02-01')
      },
      {
        id: '3',
        invoiceNumber: 'INV-2025-003',
        issueDate: new Date('2025-03-10'),
        dueDate: new Date('2025-03-25'),
        totalAmount: 2100.00,
        currency: 'PLN',
        status: InvoiceStatus.Overdue,
        vendorName: 'Hardware Store Ltd.',
        vendorAddress: 'ul. Sprzętowa 10, 00-005 Poznań',
        customerName: 'Office Solutions Sp. z o.o.',
        customerAddress: 'ul. Biurowa 20, 00-006 Łódź',
        items: [
          {
            id: '3-1',
            invoiceId: '3',
            description: 'Laptop Dell Latitude',
            quantity: 2,
            unitPrice: 800.00,
            totalPrice: 1600.00,
            category: 'Hardware'
          },
          {
            id: '3-2',
            invoiceId: '3',
            description: 'Monitor 24"',
            quantity: 2,
            unitPrice: 250.00,
            totalPrice: 500.00,
            category: 'Hardware'
          }
        ],
        createdAt: new Date('2025-03-10'),
        updatedAt: new Date('2025-03-10')
      }
    ];

    return of(mockInvoices);
  }
}

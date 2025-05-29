export interface Invoice {
  id: string;
  invoiceNumber: string;
  number?: string; // Added for template compatibility
  issueDate: Date;
  dueDate: Date;
  totalAmount: number;
  currency: string;
  status: InvoiceStatus;
  vendorName: string;
  vendorAddress?: string;
  customerName: string;
  customer?: { name: string }; // Added for template compatibility
  customerAddress?: string;
  items: InvoiceItem[];
  createdAt: Date;
  updatedAt: Date;
}

export interface InvoiceItem {
  id: string;
  invoiceId: string;
  description: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  category?: string;
  notes?: string;
}

export enum InvoiceStatus {
  Draft = 'draft',
  Sent = 'sent',
  Paid = 'paid',
  Overdue = 'overdue',
  Cancelled = 'cancelled'
}

export interface InvoiceFilter {
  searchTerm?: string;
  status?: InvoiceStatus;
  dateFrom?: Date;
  dateTo?: Date;
  minAmount?: number;
  maxAmount?: number;
}

export interface InvoiceSortOptions {
  field: 'invoiceNumber' | 'issueDate' | 'dueDate' | 'totalAmount' | 'vendorName';
  direction: 'asc' | 'desc';
}

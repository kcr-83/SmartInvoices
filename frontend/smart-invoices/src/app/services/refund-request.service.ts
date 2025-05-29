import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay, map } from 'rxjs';
import {
  RefundRequest,
  CreateRefundRequestDto,
  RequestStatus,
  RefundReason,
  RefundRequestFilter,
  SupportingDocument,
  ApiResponse
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class RefundRequestService {
  private readonly apiUrl = '/api/refund-requests';
  // Signals for state management
  private readonly _refundRequests = signal<RefundRequest[]>([]);
  private readonly _loading = signal(false);
  private readonly _error = signal<string | null>(null);

  // Public readonly signals
  readonly refundRequests = this._refundRequests.asReadonly();
  readonly loading = this._loading.asReadonly();
  readonly error = this._error.asReadonly();

  constructor(private http: HttpClient) {
    this.loadMockData();
  }

  // Load all refund requests
  loadRefundRequests(): Observable<RefundRequest[]> {
    this._loading.set(true);

    return this.getMockRefundRequests().pipe(
      delay(500),
      map(requests => {
        this._refundRequests.set(requests);
        this._loading.set(false);
        return requests;
      })
    );
  }

  // Get refund requests for specific invoice
  getRefundRequestsForInvoice(invoiceId: string): Observable<RefundRequest[]> {
    return of(this._refundRequests().filter(req => req.invoiceId === invoiceId)).pipe(
      delay(300)
    );
  }

  // Get single refund request
  getRefundRequest(id: string): Observable<RefundRequest | undefined> {
    return of(this._refundRequests().find(req => req.id === id)).pipe(
      delay(300)
    );
  }

  // Create new refund request
  createRefundRequest(dto: CreateRefundRequestDto): Observable<RefundRequest> {
    const newRequest: RefundRequest = {
      id: this.generateId(),
      invoiceId: dto.invoiceId,
      userId: 'current-user-id', // Would come from auth service
      reason: dto.reason,
      customReason: dto.customReason,
      justification: dto.justification,
      supportingDocuments: dto.supportingDocuments ?
        this.mockUploadDocuments(dto.supportingDocuments) : [],
      requestedAmount: 0, // Would be calculated based on invoice
      status: RequestStatus.Pending,
      createdAt: new Date(),
      updatedAt: new Date()
    };

    // Mock API call
    return of(newRequest).pipe(
      delay(1000),
      map(request => {
        this._refundRequests.update(requests => [...requests, request]);
        return request;
      })
    );
  }

  // Update refund request status (admin function)
  updateRefundRequestStatus(id: string, status: RequestStatus, adminComments?: string): Observable<RefundRequest> {
    return of(this._refundRequests().find(req => req.id === id)).pipe(
      delay(500),
      map(request => {
        if (!request) {
          throw new Error('Refund request not found');
        }

        const updatedRequest: RefundRequest = {
          ...request,
          status,
          adminComments,
          processedAt: new Date(),
          processedBy: 'admin-user-id',
          updatedAt: new Date()
        };

        this._refundRequests.update(requests =>
          requests.map(req => req.id === id ? updatedRequest : req)
        );

        return updatedRequest;
      })
    );
  }

  // Cancel refund request (user function)
  cancelRefundRequest(id: string): Observable<RefundRequest> {
    return this.updateRefundRequestStatus(id, RequestStatus.Cancelled);
  }

  // Get user's refund requests
  getUserRefundRequests(userId: string): Observable<RefundRequest[]> {
    return of(this._refundRequests().filter(req => req.userId === userId)).pipe(
      delay(300)
    );
  }

  // Filter refund requests
  filterRefundRequests(filter: RefundRequestFilter): Observable<RefundRequest[]> {
    let result = this._refundRequests();

    if (filter.status) {
      result = result.filter(req => req.status === filter.status);
    }

    if (filter.reason) {
      result = result.filter(req => req.reason === filter.reason);
    }

    if (filter.dateFrom) {
      result = result.filter(req => new Date(req.createdAt) >= filter.dateFrom!);
    }

    if (filter.dateTo) {
      result = result.filter(req => new Date(req.createdAt) <= filter.dateTo!);
    }

    if (filter.minAmount !== undefined) {
      result = result.filter(req => req.requestedAmount >= filter.minAmount!);
    }

    if (filter.maxAmount !== undefined) {
      result = result.filter(req => req.requestedAmount <= filter.maxAmount!);
    }

    return of(result).pipe(delay(300));
  }

  // Upload supporting documents (mock implementation)
  uploadSupportingDocuments(files: File[]): Observable<SupportingDocument[]> {
    return of(this.mockUploadDocuments(files)).pipe(delay(2000));
  }

  // Private helper methods
  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  private mockUploadDocuments(files: File[]): SupportingDocument[] {
    return files.map(file => ({
      id: this.generateId(),
      fileName: file.name,
      fileType: file.type,
      fileSize: file.size,
      uploadedAt: new Date(),
      url: `mock://documents/${this.generateId()}`
    }));
  }

  private loadMockData(): void {
    this.loadRefundRequests().subscribe();
  }

  private getMockRefundRequests(): Observable<RefundRequest[]> {
    const mockRequests: RefundRequest[] = [
      {
        id: 'rr-1',
        invoiceId: '2',
        userId: 'user-2',
        reason: RefundReason.ServiceNotProvided,
        justification: 'Kampania reklamowa nie została zrealizowana zgodnie z umową',
        supportingDocuments: [
          {
            id: 'doc-1',
            fileName: 'email-correspondence.pdf',
            fileType: 'application/pdf',
            fileSize: 245760,
            uploadedAt: new Date('2025-02-10'),
            url: 'mock://documents/doc-1'
          }
        ],
        requestedAmount: 750.50,
        status: RequestStatus.UnderReview,
        createdAt: new Date('2025-02-10'),
        updatedAt: new Date('2025-02-12')
      },
      {
        id: 'rr-2',
        invoiceId: '3',
        userId: 'user-3',
        reason: RefundReason.ProductDefective,
        justification: 'Jeden z laptopów jest uszkodzony i nie można go uruchomić',
        supportingDocuments: [
          {
            id: 'doc-2',
            fileName: 'defect-photos.zip',
            fileType: 'application/zip',
            fileSize: 1024000,
            uploadedAt: new Date('2025-03-20'),
            url: 'mock://documents/doc-2'
          },
          {
            id: 'doc-3',
            fileName: 'warranty-claim.pdf',
            fileType: 'application/pdf',
            fileSize: 512000,
            uploadedAt: new Date('2025-03-20'),
            url: 'mock://documents/doc-3'
          }
        ],
        requestedAmount: 800.00,
        status: RequestStatus.Pending,
        createdAt: new Date('2025-03-20'),
        updatedAt: new Date('2025-03-20')
      },
      {
        id: 'rr-3',
        invoiceId: '1',
        userId: 'user-1',
        reason: RefundReason.Duplicate,
        justification: 'Ta faktura została wystawiona dwukrotnie - mamy już podobną z poprzedniego miesiąca',
        supportingDocuments: [],
        requestedAmount: 1250.00,
        status: RequestStatus.Approved,
        adminComments: 'Potwierdzono duplikat faktury. Zwrot został autoryzowany.',
        processedAt: new Date('2025-01-25'),
        processedBy: 'admin-1',
        createdAt: new Date('2025-01-22'),
        updatedAt: new Date('2025-01-25')
      }
    ];

    return of(mockRequests);
  }
}

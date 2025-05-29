import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay, map } from 'rxjs';
import {
  ChangeRequest,
  CreateChangeRequestDto,
  RequestStatus,
  ChangeRequestType,
  ChangeType,
  ApiResponse
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class ChangeRequestService {
  private readonly apiUrl = '/api/change-requests';

  // Signals for state management
  private readonly _changeRequests = signal<ChangeRequest[]>([]);
  private readonly _loading = signal(false);

  // Public readonly signals
  readonly changeRequests = this._changeRequests.asReadonly();
  readonly loading = this._loading.asReadonly();

  constructor(private http: HttpClient) {
    this.loadMockData();
  }

  // Load all change requests
  loadChangeRequests(): Observable<ChangeRequest[]> {
    this._loading.set(true);

    return this.getMockChangeRequests().pipe(
      delay(500),
      map(requests => {
        this._changeRequests.set(requests);
        this._loading.set(false);
        return requests;
      })
    );
  }

  // Get change requests for specific invoice
  getChangeRequestsForInvoice(invoiceId: string): Observable<ChangeRequest[]> {
    return of(this._changeRequests().filter(req => req.invoiceId === invoiceId)).pipe(
      delay(300)
    );
  }

  // Get single change request
  getChangeRequest(id: string): Observable<ChangeRequest | undefined> {
    return of(this._changeRequests().find(req => req.id === id)).pipe(
      delay(300)
    );
  }

  // Create new change request
  createChangeRequest(dto: CreateChangeRequestDto): Observable<ChangeRequest> {
    const newRequest: ChangeRequest = {
      id: this.generateId(),
      invoiceId: dto.invoiceId,
      userId: 'current-user-id', // Would come from auth service
      requestType: dto.requestType,
      status: RequestStatus.Pending,
      justification: dto.justification,
      requestedChanges: dto.requestedChanges.map((change, index) => ({
        ...change,
        itemId: dto.selectedItemIds[index] || this.generateId()
      })),
      createdAt: new Date(),
      updatedAt: new Date()
    };

    // Mock API call
    return of(newRequest).pipe(
      delay(1000),
      map(request => {
        this._changeRequests.update(requests => [...requests, request]);
        return request;
      })
    );
  }

  // Update change request status (admin function)
  updateChangeRequestStatus(id: string, status: RequestStatus, adminComments?: string): Observable<ChangeRequest> {
    return of(this._changeRequests().find(req => req.id === id)).pipe(
      delay(500),
      map(request => {
        if (!request) {
          throw new Error('Change request not found');
        }

        const updatedRequest: ChangeRequest = {
          ...request,
          status,
          adminComments,
          processedAt: new Date(),
          processedBy: 'admin-user-id',
          updatedAt: new Date()
        };

        this._changeRequests.update(requests =>
          requests.map(req => req.id === id ? updatedRequest : req)
        );

        return updatedRequest;
      })
    );
  }

  // Cancel change request (user function)
  cancelChangeRequest(id: string): Observable<ChangeRequest> {
    return this.updateChangeRequestStatus(id, RequestStatus.Cancelled);
  }

  // Approve change request
  approveChangeRequest(id: string, adminComments?: string): Observable<ChangeRequest> {
    return this.updateChangeRequestStatus(id, RequestStatus.Approved, adminComments);
  }

  // Reject change request
  rejectChangeRequest(id: string, adminComments?: string): Observable<ChangeRequest> {
    return this.updateChangeRequestStatus(id, RequestStatus.Rejected, adminComments);
  }

  // Get user's change requests
  getUserChangeRequests(userId: string): Observable<ChangeRequest[]> {
    return of(this._changeRequests().filter(req => req.userId === userId)).pipe(
      delay(300)
    );
  }

  // Private helper methods
  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  private loadMockData(): void {
    this.loadChangeRequests().subscribe();
  }

  private getMockChangeRequests(): Observable<ChangeRequest[]> {
    const mockRequests: ChangeRequest[] = [
      {
        id: 'cr-1',
        invoiceId: '1',
        userId: 'user-1',
        requestType: ChangeRequestType.LineItemModification,
        status: RequestStatus.Pending,
        justification: 'Błędnie naliczona ilość godzin wsparcia technicznego',
        requestedChanges: [
          {
            itemId: '1-2',
            changeType: ChangeType.Quantity,
            currentValue: 5,
            requestedValue: 3,
            field: 'quantity',
            justification: 'Rzeczywista ilość godzin to 3, nie 5'
          }
        ],
        createdAt: new Date('2025-01-20'),
        updatedAt: new Date('2025-01-20')
      },
      {
        id: 'cr-2',
        invoiceId: '2',
        userId: 'user-2',
        requestType: ChangeRequestType.LineItemModification,
        status: RequestStatus.Approved,
        justification: 'Zmiana opisu kampanii reklamowej',
        requestedChanges: [
          {
            itemId: '2-1',
            changeType: ChangeType.Description,
            currentValue: 'Kampania reklamowa online',
            requestedValue: 'Kampania reklamowa online - Google Ads',
            field: 'description',
            justification: 'Doprecyzowanie rodzaju kampanii'
          }
        ],
        adminComments: 'Zmiana zatwierdzona - opis został zaktualizowany',
        processedAt: new Date('2025-02-05'),
        processedBy: 'admin-1',
        createdAt: new Date('2025-02-02'),
        updatedAt: new Date('2025-02-05')
      },
      {
        id: 'cr-3',
        invoiceId: '3',
        userId: 'user-3',
        requestType: ChangeRequestType.LineItemModification,
        status: RequestStatus.UnderReview,
        justification: 'Otrzymano laptopy z wyższą specyfikacją niż zamówione',
        requestedChanges: [
          {
            itemId: '3-1',
            changeType: ChangeType.UnitPrice,
            currentValue: 800.00,
            requestedValue: 900.00,
            field: 'unitPrice',
            justification: 'Cena powinna odzwierciedlać wyższą specyfikację'
          }
        ],
        createdAt: new Date('2025-03-15'),
        updatedAt: new Date('2025-03-16')
      }
    ];

    return of(mockRequests);
  }
}

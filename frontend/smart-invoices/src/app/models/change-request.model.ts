export interface ChangeRequest {
  id: string;
  invoiceId: string;
  invoiceNumber?: string; // Added for display purposes
  userId: string;
  requestedBy?: string; // Added for user display name
  requestType: ChangeRequestType;
  status: RequestStatus;
  justification: string;
  reason?: string; // Added for template compatibility
  requestedChanges: LineItemChange[];
  originalItem?: any; // Added for template compatibility
  requestedItem?: any; // Added for template compatibility
  adminComments?: string;
  createdAt: Date;
  updatedAt: Date;
  processedAt?: Date;
  processedBy?: string;
}

export interface LineItemChange {
  itemId: string;
  changeType: ChangeType;
  currentValue: any;
  requestedValue: any;
  field: string; // 'quantity', 'unitPrice', 'description'
  justification?: string;
}

export enum ChangeRequestType {
  LineItemModification = 'line_item_modification',
  ItemAddition = 'item_addition',
  ItemRemoval = 'item_removal'
}

export enum ChangeType {
  Quantity = 'quantity',
  UnitPrice = 'unit_price',
  Description = 'description',
  Addition = 'addition',
  Removal = 'removal'
}

export enum RequestStatus {
  Pending = 'pending',
  UnderReview = 'under_review',
  Approved = 'approved',
  Rejected = 'rejected',
  Cancelled = 'cancelled',
  Processing = 'processing',
  Completed = 'completed'
}

export interface CreateChangeRequestDto {
  invoiceId: string;
  requestType: ChangeRequestType;
  justification: string;
  requestedChanges: Omit<LineItemChange, 'itemId'>[];
  selectedItemIds: string[];
}

export interface ChangeRequestFilter {
  invoiceNumber?: string;
  status?: RequestStatus;
  dateFrom?: Date;
  dateTo?: Date;
}

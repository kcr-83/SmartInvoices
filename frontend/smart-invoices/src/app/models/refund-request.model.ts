import { RequestStatus } from './change-request.model';

export interface RefundRequest {
  id: string;
  invoiceId: string;
  invoiceNumber?: string; // Added for display purposes
  userId: string;
  requestedBy?: string; // Added for user display name
  reason: RefundReason;
  type?: RefundReason; // Alias for reason for backward compatibility
  customReason?: string;
  justification: string;
  supportingDocuments: SupportingDocument[];
  documents?: SupportingDocument[]; // Alias for supportingDocuments for backward compatibility
  requestedAmount: number;
  amount?: number; // Alias for requestedAmount for backward compatibility
  status: RequestStatus;
  adminComments?: string;
  createdAt: Date;
  requestDate?: Date; // Alias for createdAt for backward compatibility
  updatedAt: Date;
  processedAt?: Date;
  processedBy?: string;
}

export interface SupportingDocument {
  id: string;
  fileName: string;
  name?: string; // Added for template compatibility
  fileType: string;
  type?: string; // Added for template compatibility
  fileSize: number;
  size?: number; // Added for template compatibility
  uploadedAt: Date;
  url?: string;
}

export enum RefundReason {
  ProductDefective = 'product_defective',
  ServiceNotProvided = 'service_not_provided',
  Overcharged = 'overcharged',
  Duplicate = 'duplicate',
  Cancelled = 'cancelled',
  Other = 'other'
}

export interface CreateRefundRequestDto {
  invoiceId: string;
  reason: RefundReason;
  customReason?: string;
  justification: string;
  supportingDocuments?: File[];
}

export interface RefundRequestFilter {
  status?: RequestStatus;
  reason?: RefundReason;
  dateFrom?: Date;
  dateTo?: Date;
  minAmount?: number;
  maxAmount?: number;
}

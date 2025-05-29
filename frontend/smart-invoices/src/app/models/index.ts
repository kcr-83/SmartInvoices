// Re-export all models for convenient importing
export * from './invoice.model';
export * from './change-request.model';
export * from './refund-request.model';
export * from './user.model';

// Common types and utilities
export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
  errors?: string[];
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface PaginationOptions {
  pageNumber: number;
  pageSize: number;
}

// Common enums re-exported for convenience
export { RequestStatus, ChangeRequestType, ChangeType } from './change-request.model';
export { RefundReason } from './refund-request.model';
export { InvoiceStatus } from './invoice.model';
export { UserRole } from './user.model';

// Import for type aliases
import { RequestStatus } from './change-request.model';
import { RefundReason } from './refund-request.model';

// Additional type aliases for backward compatibility
export type ChangeRequestStatus = RequestStatus;
export type RefundStatus = RequestStatus;
export type RefundType = RefundReason;

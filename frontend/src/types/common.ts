// Common Helper DTOs
export interface OperationResult {
  isSuccess: boolean;
  statusCode: number;
  message?: string;
}

export interface OperationResult<T> {
  data?: T;
  isSuccess: boolean;
  statusCode: number;
  message?: string;
}

export interface PageResult<T> {
  items: T;
  totalCount: number;
  pageIndex: number;
  pageSize: number;
  totalPages: number;
}

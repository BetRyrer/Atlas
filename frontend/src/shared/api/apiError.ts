export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  errors?: Record<string, string[]>;
}

export class ApiError extends Error {
  readonly status: number;
  readonly title: string;
  readonly errors?: Record<string, string[]>;

  constructor(problem: ProblemDetails, status: number) {
    super(problem.detail ?? problem.title ?? 'An unexpected error occurred.');
    this.name = 'ApiError';
    this.status = status;
    this.title = problem.title ?? 'Error';
    this.errors = problem.errors;
  }

  get isValidationError(): boolean {
    return this.status === 400 && this.errors !== undefined;
  }

  get isNotFound(): boolean {
    return this.status === 404;
  }

  get isConflict(): boolean {
    return this.status === 409;
  }
}

export function getErrorMessage(error: unknown): string {
  return error instanceof Error ? error.message : 'An unexpected error occurred.';
}

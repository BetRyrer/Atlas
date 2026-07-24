export interface LoginRequest {
  username: string;
  password: string;
}

export interface AuthUser {
  username: string;
  displayName: string;
}

export interface AuthResult extends AuthUser {
  token: string;
  expiresAt: string;
}

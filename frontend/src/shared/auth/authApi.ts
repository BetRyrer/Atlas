import { httpClient } from '../api/httpClient';
import type { AuthResult, LoginRequest } from './types';

export const authApi = {
  async login(request: LoginRequest): Promise<AuthResult> {
    const { data } = await httpClient.post<AuthResult>('/auth/login', request);
    return data;
  },
};

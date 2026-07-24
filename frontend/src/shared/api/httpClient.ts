import axios from 'axios';
import { getToken } from '../auth/tokenStorage';
import { ApiError, type ProblemDetails } from './apiError';

export const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
});

httpClient.interceptors.request.use((config) => {
  const token = getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

httpClient.interceptors.response.use(
  (response) => response,
  (error: unknown) => {
    if (axios.isAxiosError(error) && error.response) {
      const isLoginRequest = error.config?.url?.includes('/auth/login') ?? false;
      if (error.response.status === 401 && !isLoginRequest) {
        window.dispatchEvent(new Event('atlas:unauthorized'));
      }

      const problem = (error.response.data ?? {}) as ProblemDetails;
      return Promise.reject(new ApiError(problem, error.response.status));
    }

    return Promise.reject(error);
  },
);

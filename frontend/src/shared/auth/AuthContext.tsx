import { createContext, useContext, useEffect, useState, type PropsWithChildren } from 'react';
import { authApi } from './authApi';
import { clearStoredAuth, readStoredAuth, writeStoredAuth } from './tokenStorage';
import type { AuthUser, LoginRequest } from './types';

interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isInitializing: boolean;
  login: (request: LoginRequest) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: PropsWithChildren) {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [isInitializing, setIsInitializing] = useState(true);

  useEffect(() => {
    const stored = readStoredAuth();
    if (stored) {
      setUser({ username: stored.username, displayName: stored.displayName });
    }
    setIsInitializing(false);
  }, []);

  useEffect(() => {
    function handleUnauthorized() {
      clearStoredAuth();
      setUser(null);
    }

    window.addEventListener('atlas:unauthorized', handleUnauthorized);
    return () => window.removeEventListener('atlas:unauthorized', handleUnauthorized);
  }, []);

  async function login(request: LoginRequest) {
    const result = await authApi.login(request);
    writeStoredAuth(result);
    setUser({ username: result.username, displayName: result.displayName });
  }

  function logout() {
    clearStoredAuth();
    setUser(null);
  }

  return (
    <AuthContext.Provider value={{ user, isAuthenticated: user !== null, isInitializing, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}

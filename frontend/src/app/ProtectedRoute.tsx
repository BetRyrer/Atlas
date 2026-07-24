import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { useAuth } from '../shared/auth/AuthContext';
import { Spinner } from '../shared/components/ui/Spinner';

export function ProtectedRoute() {
  const { isAuthenticated, isInitializing } = useAuth();
  const location = useLocation();

  if (isInitializing) {
    return <Spinner label="Chargement" />;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  return <Outlet />;
}

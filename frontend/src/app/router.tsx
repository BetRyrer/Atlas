import { lazy, Suspense, type ReactElement } from 'react';
import { createBrowserRouter, Navigate } from 'react-router-dom';
import { AppLayout } from '../shared/components/layout/AppLayout';
import { Spinner } from '../shared/components/ui/Spinner';
import { NotFoundPage } from './NotFoundPage';
import { ProtectedRoute } from './ProtectedRoute';
import { RouteErrorBoundary } from './RouteErrorBoundary';

const LoginPage = lazy(() => import('../features/auth').then((m) => ({ default: m.LoginPage })));
const DashboardPage = lazy(() =>
  import('../features/dashboard').then((m) => ({ default: m.DashboardPage })),
);
const ToolsListPage = lazy(() =>
  import('../features/tools').then((m) => ({ default: m.ToolsListPage })),
);
const ToolDetailPage = lazy(() =>
  import('../features/tools').then((m) => ({ default: m.ToolDetailPage })),
);
const DepartmentsListPage = lazy(() =>
  import('../features/departments').then((m) => ({ default: m.DepartmentsListPage })),
);
const DepartmentDetailPage = lazy(() =>
  import('../features/departments').then((m) => ({ default: m.DepartmentDetailPage })),
);
const MatrixPage = lazy(() => import('../features/matrix').then((m) => ({ default: m.MatrixPage })));

function withSuspense(element: ReactElement) {
  return <Suspense fallback={<Spinner />}>{element}</Suspense>;
}

export const router = createBrowserRouter([
  {
    path: '/login',
    element: withSuspense(<LoginPage />),
    errorElement: <RouteErrorBoundary />,
  },
  {
    path: '/',
    element: <ProtectedRoute />,
    errorElement: <RouteErrorBoundary />,
    children: [
      {
        element: <AppLayout />,
        children: [
          { index: true, element: <Navigate to="/dashboard" replace /> },
          { path: 'dashboard', element: withSuspense(<DashboardPage />) },
          { path: 'tools', element: withSuspense(<ToolsListPage />) },
          { path: 'tools/:toolId', element: withSuspense(<ToolDetailPage />) },
          { path: 'departments', element: withSuspense(<DepartmentsListPage />) },
          { path: 'departments/:departmentId', element: withSuspense(<DepartmentDetailPage />) },
          { path: 'matrix', element: withSuspense(<MatrixPage />) },
          { path: '*', element: <NotFoundPage /> },
        ],
      },
    ],
  },
]);

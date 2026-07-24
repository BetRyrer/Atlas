import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../../auth/AuthContext';
import { Button } from '../ui/Button';

const links = [
  { to: '/dashboard', label: 'Tableau de bord' },
  { to: '/tools', label: 'Outils' },
  { to: '/departments', label: 'Départements' },
  { to: '/matrix', label: 'Matrice' },
];

export function Sidebar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate('/login', { replace: true });
  }

  return (
    <aside className="flex w-56 shrink-0 flex-col border-r border-neutral-200 bg-white dark:border-neutral-800 dark:bg-neutral-950">
      <div className="px-4 py-5">
        <span className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">Atlas</span>
      </div>
      <nav aria-label="Main navigation" className="flex flex-1 flex-col gap-1 px-2">
        {links.map((link) => (
          <NavLink
            key={link.to}
            to={link.to}
            className={({ isActive }) =>
              `rounded-md px-3 py-2 text-sm font-medium transition-colors ${
                isActive
                  ? 'bg-accent-50 text-accent-700 dark:bg-accent-600/10 dark:text-accent-500'
                  : 'text-neutral-600 hover:bg-neutral-100 dark:text-neutral-400 dark:hover:bg-neutral-900'
              }`
            }
          >
            {link.label}
          </NavLink>
        ))}
      </nav>
      <div className="border-t border-neutral-200 px-4 py-4 dark:border-neutral-800">
        <p className="truncate text-sm font-medium text-neutral-700 dark:text-neutral-300">
          {user?.displayName}
        </p>
        <Button variant="ghost" onClick={handleLogout} className="mt-2 w-full justify-start px-0">
          Se déconnecter
        </Button>
      </div>
    </aside>
  );
}

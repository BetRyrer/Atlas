import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useLocation, useNavigate, type Location } from 'react-router-dom';
import { getErrorMessage } from '../../../shared/api/apiError';
import { useAuth } from '../../../shared/auth/AuthContext';
import { Button } from '../../../shared/components/ui/Button';
import { Input } from '../../../shared/components/ui/Input';
import { ThemeToggle } from '../../../shared/components/ui/ThemeToggle';
import { loginFormSchema, type LoginFormSchema } from '../types/loginFormSchema';

export function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [serverError, setServerError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormSchema>({
    resolver: zodResolver(loginFormSchema),
    defaultValues: { username: '', password: '' },
  });

  async function onSubmit(values: LoginFormSchema) {
    setServerError(null);
    setIsSubmitting(true);

    try {
      await login(values);
      const redirectTo = (location.state as { from?: Location } | null)?.from?.pathname ?? '/dashboard';
      navigate(redirectTo, { replace: true });
    } catch (error) {
      setServerError(getErrorMessage(error));
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="flex min-h-screen">
      <div className="relative hidden w-1/2 flex-col justify-between overflow-hidden bg-gradient-to-br from-accent-600 to-accent-800 p-12 text-white md:flex">
        <div aria-hidden="true" className="absolute -right-24 -top-24 h-72 w-72 rounded-full bg-white/10" />
        <div aria-hidden="true" className="absolute -bottom-32 -left-16 h-96 w-96 rounded-full bg-black/10" />

        <span className="text-2xl font-semibold tracking-tight">Atlas</span>

        <div className="max-w-md">
          <h1 className="text-4xl font-semibold leading-tight">Catalogue d'outillage technique</h1>
          <p className="mt-4 text-accent-100">
            Recensez les outils utilisés par chaque service, suivez leur adoption et visualisez la
            couverture départements × outils en un coup d'œil.
          </p>
        </div>

        <p className="text-sm text-accent-200">© {new Date().getFullYear()} Atlas</p>
      </div>

      <div className="relative flex w-full flex-col items-center justify-center bg-neutral-50 p-8 dark:bg-neutral-950 md:w-1/2">
        <ThemeToggle className="absolute right-4 top-4" />
        <div className="w-full max-w-sm">
          <div className="mb-8 md:hidden">
            <span className="text-2xl font-semibold text-neutral-900 dark:text-neutral-100">Atlas</span>
          </div>

          <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">Connexion</h2>
          <p className="mt-1 text-sm text-neutral-600 dark:text-neutral-400">
            Accédez au catalogue d'outillage de votre organisation.
          </p>

          <form onSubmit={handleSubmit(onSubmit)} className="mt-6 flex flex-col gap-4" noValidate>
            <Input
              id="login-username"
              label="Nom d'utilisateur"
              autoComplete="username"
              {...register('username')}
              error={errors.username?.message}
            />
            <Input
              id="login-password"
              label="Mot de passe"
              type="password"
              autoComplete="current-password"
              {...register('password')}
              error={errors.password?.message}
            />

            {serverError ? (
              <p role="alert" className="text-sm text-red-600 dark:text-red-400">
                {serverError}
              </p>
            ) : null}

            <Button type="submit" disabled={isSubmitting} className="mt-2 w-full">
              {isSubmitting ? 'Connexion…' : 'Se connecter'}
            </Button>
          </form>

          <div className="mt-6 rounded-md border border-neutral-200 bg-white p-3 text-xs text-neutral-500 dark:border-neutral-800 dark:bg-neutral-900 dark:text-neutral-400">
            Démo : <span className="font-medium text-neutral-700 dark:text-neutral-300">admin</span> /{' '}
            <span className="font-medium text-neutral-700 dark:text-neutral-300">Atlas2024!</span>
          </div>
        </div>
      </div>
    </div>
  );
}

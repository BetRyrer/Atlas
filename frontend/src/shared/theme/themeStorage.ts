export type Theme = 'light' | 'dark';

// Kept in sync manually with the inline bootstrap script in index.html: that
// script runs before the bundle loads (to avoid a flash of the wrong theme),
// so it can't import this module and re-implements the same key/logic.
// Renaming STORAGE_KEY requires updating index.html's script too.
const STORAGE_KEY = 'atlas.theme';

export function readStoredTheme(): Theme | null {
  const raw = localStorage.getItem(STORAGE_KEY);
  return raw === 'light' || raw === 'dark' ? raw : null;
}

export function writeStoredTheme(theme: Theme): void {
  localStorage.setItem(STORAGE_KEY, theme);
}

export function getSystemTheme(): Theme {
  return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
}

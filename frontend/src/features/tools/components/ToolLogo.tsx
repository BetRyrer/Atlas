import { useState } from 'react';

interface ToolLogoProps {
  name: string;
  logoUrl: string | null;
  size?: number;
}

export function ToolLogo({ name, logoUrl, size = 32 }: ToolLogoProps) {
  const [hasError, setHasError] = useState(false);

  if (!logoUrl || hasError) {
    return (
      <span
        aria-hidden="true"
        style={{ width: size, height: size }}
        className="flex shrink-0 items-center justify-center rounded-md bg-neutral-100 text-xs font-semibold text-neutral-500 dark:bg-neutral-800 dark:text-neutral-400"
      >
        {name.charAt(0).toUpperCase()}
      </span>
    );
  }

  return (
    <img
      src={logoUrl}
      alt=""
      width={size}
      height={size}
      className="shrink-0 rounded-md object-contain"
      onError={() => setHasError(true)}
    />
  );
}

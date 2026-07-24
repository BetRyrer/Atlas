function toEmbedUrl(url: string): string | null {
  try {
    const parsed = new URL(url);

    if (parsed.hostname.endsWith('youtu.be')) {
      return `https://www.youtube.com/embed${parsed.pathname}`;
    }

    if (parsed.hostname.endsWith('youtube.com')) {
      const videoId = parsed.searchParams.get('v');
      if (videoId) {
        return `https://www.youtube.com/embed/${videoId}`;
      }
      if (parsed.pathname.startsWith('/embed/')) {
        return url;
      }
    }

    return null;
  } catch {
    return null;
  }
}

interface YoutubeEmbedProps {
  url: string;
  title: string;
}

export function YoutubeEmbed({ url, title }: YoutubeEmbedProps) {
  const embedUrl = toEmbedUrl(url);

  if (!embedUrl) {
    return null;
  }

  return (
    <div className="aspect-video w-full max-w-2xl overflow-hidden rounded-lg border border-neutral-200 dark:border-neutral-800">
      <iframe
        src={embedUrl}
        title={title}
        className="h-full w-full"
        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
        allowFullScreen
      />
    </div>
  );
}

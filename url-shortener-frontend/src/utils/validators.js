/**
 * Returns true if `value` looks like a usable URL once a scheme is assumed.
 * Accepts input with or without "http(s)://" already on it.
 */
export function isLikelyUrl(value) {
  const trimmed = value.trim();
  if (!trimmed) return false;

  try {
    const withScheme = /^https?:\/\//i.test(trimmed) ? trimmed : `https://${trimmed}`;
    const url = new URL(withScheme);
    return url.hostname.includes(".");
  } catch {
    return false;
  }
}

/**
 * Normalizes a raw input string into a fully-qualified URL string,
 * prefixing "https://" if no scheme was provided.
 */
export function normalizeUrl(value) {
  const trimmed = value.trim();
  return /^https?:\/\//i.test(trimmed) ? trimmed : `https://${trimmed}`;
}

/**
 * Strips the "https://" or "http://" prefix for compact display,
 * e.g. "https://sqz.to/abc123" -> "sqz.to/abc123".
 */
export function displayUrl(href) {
  return href.replace(/^https?:\/\//i, "");
}

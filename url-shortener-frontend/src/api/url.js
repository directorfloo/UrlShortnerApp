import { normalizeUrl } from "../utils/validators.js";
const BASE_URL = import.meta.env.VITE_API_URL || "http://localhost:5000/api";

export async function shortenUrl(originalUrl) {
  const normalized = normalizeUrl(originalUrl);

  const response = await fetch(`${BASE_URL}/urls`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ originalUrl: normalized }),
  });

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    throw new Error(body?.error || "Could not shorten that link. Please try again.");
  }

  const data = await response.json();
  return {
    shortCode: data.shortCode,
    href: data.shortUrl,
    originalUrl: normalized,
    createdAtUtc: data.createdAtUtc,
  };
}

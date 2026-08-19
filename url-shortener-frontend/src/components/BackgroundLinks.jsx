const DECORATIVE_CODES = [
  "x9F2q", "aB3kZ", "pQ7mR", "L4tYw", "9jH2c", "vN6dE", "rT8sA", "k3WxF",
  "mZ1qT", "G7yUb", "h2NcX", "eV5dJ", "w8QoP", "S3rLk", "tY6fM", "c4BnH",
  "D9zAq", "j1KxV",
];

/**
 * Purely decorative background texture: faded, rotated example short links
 * scattered around the page behind the main card. Not interactive.
 */
export default function BackgroundLinks() {
  return (
    <div className="bg-links" aria-hidden="true">
      {DECORATIVE_CODES.map((code, i) => (
        <span key={code} className={`bg-link bg-link-${i + 1}`}>
          sqz.to/{code}
        </span>
      ))}
    </div>
  );
}

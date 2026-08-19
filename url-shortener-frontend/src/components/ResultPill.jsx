import { Copy, Check, AlertCircle } from "lucide-react";
import { displayUrl } from "../utils/validators.js";

/**
 * Shows the freshly-shortened link with a copy button. `copied` toggles
 * the button's icon/color briefly; `copyError` surfaces a fallback
 * message when the clipboard write failed outright.
 */
export default function ResultPill({ result, copied, copyError, onCopy }) {
  if (!result) return null;

  return (
    <>
      <div className="result-pill" aria-live="polite">
        <a className="result-link" href={result.href} target="_blank" rel="noreferrer">
          {displayUrl(result.href)}
        </a>
        <button
          type="button"
          className={`copy-btn ${copied ? "copy-btn-done" : ""}`}
          onClick={onCopy}
          aria-label="Copy short link"
        >
          {copied ? <Check size={16} strokeWidth={2.6} /> : <Copy size={16} strokeWidth={2.4} />}
        </button>
      </div>

      {copyError && (
        <p className="error-msg" role="alert">
          <AlertCircle size={14} strokeWidth={2.4} />
          {copyError}
        </p>
      )}
    </>
  );
}

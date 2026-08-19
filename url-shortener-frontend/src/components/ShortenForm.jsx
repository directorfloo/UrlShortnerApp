import { forwardRef } from "react";
import { AlertCircle } from "lucide-react";

/**
 * The paste-a-link input paired with the round "Shorten" button, plus
 * an inline validation error. `ref` is forwarded to the <input> so the
 * parent page can refocus it after a validation failure.
 */
const ShortenForm = forwardRef(function ShortenForm(
  { value, onChange, onSubmit, error, loading },
  ref
) {
  return (
    <>
      <div className="row">
        <div className="input-pill">
          <input
            ref={ref}
            type="text"
            inputMode="url"
            autoComplete="off"
            spellCheck="false"
            placeholder="paste your link here"
            value={value}
            onChange={(e) => onChange(e.target.value)}
            aria-label="Long URL to shorten"
            aria-invalid={Boolean(error)}
            className="url-input"
          />
        </div>

        <button type="button" className="shorten-btn" onClick={onSubmit} disabled={loading}>
          {loading ? <span className="spinner" aria-hidden="true" /> : "Shorten"}
        </button>
      </div>

      {error && (
        <p className="error-msg" role="alert">
          <AlertCircle size={14} strokeWidth={2.4} />
          {error}
        </p>
      )}
    </>
  );
});

export default ShortenForm;

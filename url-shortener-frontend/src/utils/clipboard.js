/**
 * Copies `text` to the clipboard, preferring the async Clipboard API and
 * falling back to the classic hidden-textarea + execCommand trick when
 * navigator.clipboard is unavailable or blocked (e.g. in sandboxed iframes
 * or non-secure contexts).
 *
 * Returns true on success, false on failure. Never throws.
 */
export async function copyToClipboard(text) {
  try {
    if (navigator.clipboard && window.isSecureContext) {
      await navigator.clipboard.writeText(text);
      return true;
    }
    throw new Error("Clipboard API unavailable");
  } catch {
    try {
      const textarea = document.createElement("textarea");
      textarea.value = text;
      textarea.style.position = "fixed";
      textarea.style.opacity = "0";
      document.body.appendChild(textarea);
      textarea.focus();
      textarea.select();
      const succeeded = document.execCommand("copy");
      document.body.removeChild(textarea);
      return succeeded;
    } catch {
      return false;
    }
  }
}

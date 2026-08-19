import { useState, useRef, useEffect } from "react";

import BackgroundLinks from "../components/BackgroundLinks.jsx";
import ShortenForm from "../components/ShortenForm.jsx";
import ResultPill from "../components/ResultPill.jsx";
import { shortenUrl } from "../api/url.js";
import { isLikelyUrl } from "../utils/validators.js";
import { copyToClipboard } from "../utils/clipboard.js";

export default function ShortenerPage() {
  const [longUrl, setLongUrl] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState(null);
  const [copied, setCopied] = useState(false);
  const [copyError, setCopyError] = useState("");
  const inputRef = useRef(null);

  useEffect(() => {
    if (!copied) return;
    const t = setTimeout(() => setCopied(false), 1800);
    return () => clearTimeout(t);
  }, [copied]);

  const handleChange = (value) => {
    setLongUrl(value);
    if (error) setError("");
  };

  const handleShorten = async () => {
    if (!longUrl.trim()) {
      setError("Paste a link first.");
      inputRef.current?.focus();
      return;
    }
    if (!isLikelyUrl(longUrl)) {
      setError("That doesn't look like a valid link.");
      inputRef.current?.focus();
      return;
    }

    setError("");
    setCopyError("");
    setLoading(true);
    setResult(null);

    try {
      const shortened = await shortenUrl(longUrl);
      setResult(shortened);
    } catch (err) {
      setError(err.message || "Something went wrong. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  const handleCopy = async () => {
    if (!result) return;
    const succeeded = await copyToClipboard(result.href);
    if (succeeded) {
      setCopied(true);
      setCopyError("");
    } else {
      setCopied(false);
      setCopyError("Couldn't copy automatically. Select the link and copy it manually.");
    }
  };

  return (
    <div className="page">
      <BackgroundLinks />

      <div className="arch">
        <ShortenForm
          ref={inputRef}
          value={longUrl}
          onChange={handleChange}
          onSubmit={handleShorten}
          error={error}
          loading={loading}
        />

        <ResultPill result={result} copied={copied} copyError={copyError} onCopy={handleCopy} />
      </div>
    </div>
  );
}

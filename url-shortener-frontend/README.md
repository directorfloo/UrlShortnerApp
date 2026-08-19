# URL Shortener Frontend

Same page you had before, restructured into the folder layout you used in your
task-manager project (`api/`, `components/`, `pages/`, `utils/`, `App.jsx`,
`index.jsx`).

## Structure

```
url-shortener-frontend/
├── index.html
├── vite.config.js
├── package.json
├── .env.example
└── src/
    ├── index.jsx              – entry point (mounts <App />)
    ├── index.css               – all page styles (moved out of the component)
    ├── App.jsx                 – root component, just renders the page
    ├── api/
    │   └── url.js               – shortenUrl(): calls the real backend if
    │                              VITE_API_URL is set, otherwise falls back
    │                              to a client-side mock
    ├── components/
    │   ├── BackgroundLinks.jsx  – decorative faded links behind the card
    │   ├── ShortenForm.jsx      – input pill + Shorten button + error
    │   └── ResultPill.jsx       – shortened link + copy button + copy error
    ├── pages/
    │   └── ShortenerPage.jsx    – wires state + the above components together
    └── utils/
        ├── validators.js        – isLikelyUrl(), displayUrl()
        ├── codeGenerator.js     – makeCode() (used by the mock only)
        └── clipboard.js         – copyToClipboard() with execCommand fallback
```

No Redux/store here, unlike the task-manager project — this page has no auth
and only a handful of local state values, so a global store would be more
ceremony than it's worth. Everything else (the `api/` / `components/` /
`pages/` / `utils/` split, `App.jsx` as a thin root, `index.jsx` as the entry)
follows the same pattern.

## Wiring it to the real backend

By default (`VITE_API_URL` unset), `api/url.js` fakes a response client-side
so the UI works standalone. To use the actual `UrlShortener.API` backend:

1. Copy `.env.example` to `.env` and set:
   ```
   VITE_API_URL=http://localhost:5000/api
   ```
2. Make sure the backend allows anonymous URL creation — the version built
   earlier has `[Authorize]` on `POST /api/urls`. Since this frontend has no
   login, remove that attribute (or point at a different open endpoint)
   before wiring it up.
3. Also add the frontend's origin to the backend's CORS policy in
   `Program.cs` if you lock down `AddCors` beyond `AllowAnyOrigin`.

## Running it

```bash
npm install
npm run dev
```

Then open the local URL Vite prints (usually `http://localhost:5173`).

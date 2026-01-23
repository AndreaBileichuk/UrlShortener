import './App.css'
import {type FormEvent, useEffect, useState} from "react";

interface UrlItem {
    id: number;
    originalUrl: string;
    shortCode: string;
    createdBy: string;
    isOwner: boolean;
}

const userContext = (window as any).USER_CONTEXT || { isAuthenticated: false, isAdmin: false };

function App() {
    const [urls, setUrls] = useState<UrlItem[]>([]);
    const [newUrl, setNewUrl] = useState<string>();
    const [error, setError] = useState("");

    async function handleAdd(e: FormEvent) {
        e.preventDefault();
        setError("");

        const res = await fetch('/api/urls', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ originalUrl: newUrl })
        });

        if (res.ok) {
            const createdUrl = await res.json();
            setUrls([...urls, { ...createdUrl, isOwner: true, createdBy: userContext.userName }]);
            setNewUrl("");
        } else {
            const msg = await res.text();
            setError(msg || "Error adding URL");
        }
    }

    async function handleDelete(id: number) {
        const res = await fetch(`/api/urls/${id}`, { method: 'DELETE' });
        if (res.ok) {
            setUrls(prev => prev.filter(u => u.id !== id));
        } else {
            alert("Failed to delete");
        }
    }

    useEffect(() => {
        fetch('/api/urls')
            .then(res => res.json())
            .then(data => setUrls(data));
    }, [])

    return (
        <div className="container mt-4">
            <h2>Short URLs</h2>

            {userContext.isAuthenticated && (
                <div className="card mb-3 p-3">
                    <form onSubmit={handleAdd} className="d-flex gap-2">
                        <input
                            className="form-control"
                            value={newUrl}
                            onChange={e => setNewUrl(e.target.value)}
                            placeholder="Enter long URL..."
                            required
                        />
                        <button className="btn btn-primary" type="submit">Shorten</button>
                    </form>
                    {error && <div className="text-danger mt-2">{error}</div>}
                </div>
            )}

            <table className="table table-striped table-hover">
                <thead>
                <tr>
                    <th>Original URL</th>
                    <th>Short URL</th>
                    <th>Owner</th>
                    <th>Actions</th>
                </tr>
                </thead>
                <tbody>
                {urls.map(url => (
                    <tr key={url.id}>
                        <td className="text-truncate" style={{maxWidth: '300px'}}>
                            <a href={url.originalUrl} target="_blank">{url.originalUrl}</a>
                        </td>
                        <td>
                            <a href={`/UrlInfo/${url.id}`} className="fw-bold">
                                {window.location.origin}/{url.shortCode}
                            </a>
                        </td>
                        <td>{url.createdBy}</td>
                        <td>
                            {(userContext.isAdmin || url.isOwner) && (
                                <button
                                    className="btn btn-sm btn-danger"
                                    onClick={() => handleDelete(url.id)}
                                >
                                    Delete
                                </button>
                            )}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    )
}

export default App

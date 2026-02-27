import { useState } from "react";
import { getSiteById } from "../services/siteService";
import type { SiteEditDto } from "../services/siteService";

const SiteEdit = () => {
  const [inputId, setInputId] = useState("");
  const [site, setSite] = useState<SiteEditDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleFetch = async () => {
    if (!inputId) {
      setError("Please enter Site Id");
      return;
    }

    try {
      setLoading(true);
      setError("");
      const data = await getSiteById(Number(inputId));
      setSite(data);
    } catch {
      setError("Failed to load site");
      setSite(null);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className="d-flex justify-content-center align-items-center"
      style={{
        minHeight: "100vh",
        width: "100vw",
        background: "#f8f9fa",
      }}
    >
      <div
        className="card shadow-lg p-5"
        style={{ width: "650px", borderRadius: "12px" }}
      >
        <h2 className="mb-4 text-center">Edit Site</h2>

        <div className="mb-3">
          <label className="form-label">Enter Site Id</label>
          <input
            type="number"
            className="form-control"
            value={inputId}
            onChange={(e) => setInputId(e.target.value)}
          />
        </div>

        <button className="btn btn-primary mb-4" onClick={handleFetch}>
          Load Site
        </button>

        {loading && <h5>Loading...</h5>}
        {error && <h5 className="text-danger">{error}</h5>}

        {site && (
          <>
            <div className="mb-3">
              <label className="form-label">Started Date</label>
              <input
                type="date"
                className="form-control"
                value={site.startedDate?.slice(0, 10) ?? ""}
                readOnly
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Site Name</label>
              <input
                type="text"
                className="form-control"
                value={site.name}
                readOnly
              />
            </div>

            <div className="mb-3">
              <label className="form-label">Site Status</label>
              <input
                type="text"
                className="form-control"
                value={site.status}
                readOnly
              />
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default SiteEdit;

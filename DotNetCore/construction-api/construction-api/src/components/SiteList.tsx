import { useEffect, useState } from "react";
import { getAllSites } from "../services/getSiteService";
import type { SiteDto } from "../services/getSiteService";

const SiteList = () => {
  const [sites, setSites] = useState<SiteDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchSites = async () => {
      try {
        const data = await getAllSites();
        setSites(data);
      } catch {
        setError("Failed to load sites");
      } finally {
        setLoading(false);
      }
    };

    fetchSites();
  }, []);

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
        style={{ width: "900px", borderRadius: "12px" }}
      >
        <h3 className="text-center mb-4">Site List</h3>

        {loading && <h5 className="text-center">Loading...</h5>}
        {error && <h5 className="text-danger text-center">{error}</h5>}

        {!loading && !error && (
          <table className="table table-bordered table-striped">
            <thead className="table-primary">
              <tr>
                <th>SL</th>
                <th>Site Name</th>
                <th>Started Date</th>
                <th>Status</th>
                <th>Note</th>
              </tr>
            </thead>
            <tbody>
              {sites.map((site, index) => (
                <tr key={site.id}>
                  <td>{index + 1}</td>
                  <td>{site.name}</td>
                  <td>{new Date(site.startedDate).toLocaleDateString()}</td>
                  <td>{site.status}</td>
                  <td>{site.note ?? "---"}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
};

export default SiteList;

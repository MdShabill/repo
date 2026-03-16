import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
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
      style={{
        width: "100%",
        margin: "0 auto",
        marginTop: "5px",
        marginLeft: "110px",
      }}
    >
      <h2 style={{ textAlign: "center", marginBottom: "20px" }}>Site List</h2>

      <div style={{ marginBottom: "15px" }}>
        <Link
          to="/site-add"
          style={{
            fontWeight: "bold",
            color: "#0d6efd",
            textDecoration: "none",
          }}
        >
          Add Site
        </Link>
      </div>

      {loading && <h5 style={{ textAlign: "center" }}>Loading...</h5>}
      {error && <h5 style={{ textAlign: "center", color: "red" }}>{error}</h5>}

      {!loading && !error && (
        <div className="table-responsive">
          <table className="table table-bordered table-striped table-hover">
            <thead className="table-primary">
              <tr style={{ textAlign: "center" }}>
                <th style={{ width: "60px" }}>SL</th>
                <th>Site Name</th>
                <th style={{ width: "150px" }}>Started Date</th>
                <th style={{ width: "120px" }}>Status</th>
                <th>Address</th>
                <th style={{ width: "120px" }}>Action</th>
              </tr>
            </thead>

            <tbody>
              {sites.map((site, index) => (
                <tr key={site.id} style={{ textAlign: "center" }}>
                  <td>{index + 1}</td>

                  <td>
                    <Link
                      to={`/site/${site.id}`}
                      style={{
                        textDecoration: "none",
                        fontWeight: "500",
                        color: "#0d6efd",
                      }}
                    >
                      {site.name}
                    </Link>
                  </td>

                  <td>{new Date(site.startedDate).toLocaleDateString()}</td>

                  <td>{site.status ?? "---"}</td>

                  <td>
                    {site.addressTypes ?? "---"} - {site.addressLine1 ?? "---"}{" "}
                    - {site.countryName ?? "---"} - {site.pinCode ?? "---"}
                  </td>

                  <td>
                    <Link
                      to={`/site-edit/${site.id}`}
                      style={{
                        color: "green",
                        fontWeight: "bold",
                        textDecoration: "none",
                      }}
                    >
                      Edit
                    </Link>
                    {" | "}
                    <span
                      style={{
                        color: "red",
                        fontWeight: "bold",
                        cursor: "pointer",
                      }}
                    >
                      Delete
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default SiteList;

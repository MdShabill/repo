// Path: src/components/SiteList.tsx

import { useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { getAllSites, deleteSite } from "../services/siteService";
import type { SiteListDto } from "../services/siteService";

function SiteList() {
  const location = useLocation();

  const [sites, setSites] = useState<SiteListDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [expandAddressColumn, setExpandAddressColumn] = useState(false);

  const successMessage =
    (location.state as { success?: string })?.success ?? "";

  useEffect(() => {
    getAllSites()
      .then(setSites)
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  const handleDelete = async (siteId: number) => {
    if (!window.confirm("Are you sure you want to delete this site?")) return;

    try {
      await deleteSite(siteId);
      setSites((prev) => prev.filter((s) => s.id !== siteId));
    } catch (err) {
      setError((err as Error).message);
    }
  };

  const fmt = (d: string) =>
    new Date(d).toLocaleDateString("en-GB", {
      day: "2-digit",
      month: "short",
      year: "numeric",
    });

  return (
    <div
      style={{
        background: "linear-gradient(135deg, #1E3A5F, #0F172A)",
        minHeight: "100vh",
      }}
    >
      <div style={{ padding: "30px" }}>
        <div
          style={{
            background: "#fff",
            borderRadius: "18px",
            padding: "25px",
            boxShadow: "0 10px 25px rgba(0,0,0,0.15)",
          }}
        >
          {/* Header */}
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "1fr 1fr 1fr",
              alignItems: "center",
              marginBottom: "20px",
            }}
          >
            <div>
              <div
                style={{
                  display: "inline-block",
                  background: "#1E3A5F",
                  color: "#fff",
                  padding: "8px 16px",
                  borderRadius: "20px",
                  fontWeight: 500,
                }}
              >
                Total Sites
                <span
                  style={{
                    background: "#F59E0B",
                    padding: "4px 10px",
                    borderRadius: "12px",
                    marginLeft: "6px",
                  }}
                >
                  {sites.length}
                </span>
              </div>
            </div>

            <div
              style={{
                textAlign: "center",
                fontSize: "26px",
                fontWeight: 600,
                color: "#1E293B",
              }}
            >
              Site List
            </div>

            <div style={{ textAlign: "right" }}>
              <Link
                to="/site-add"
                style={{
                  background: "#F59E0B",
                  color: "#fff",
                  padding: "10px 18px",
                  borderRadius: "10px",
                  textDecoration: "none",
                  fontWeight: 500,
                }}
              >
                + Add New
              </Link>
            </div>
          </div>

          {successMessage && (
            <div
              style={{
                color: "#16A34A",
                marginBottom: "10px",
                fontWeight: 500,
              }}
            >
              {successMessage}
            </div>
          )}

          {error && (
            <div style={{ color: "#DC2626", marginBottom: "10px" }}>
              {error}
            </div>
          )}

          {loading ? (
            <p style={{ textAlign: "center", padding: "30px" }}>
              Loading...
            </p>
          ) : (
            <table
              style={{
                width: "100%",
                borderCollapse: "collapse",
                borderRadius: "12px",
                overflow: "hidden",
                tableLayout: "fixed",
              }}
            >
          <thead style={{ background: "#1E3A5F", color: "#fff",}}>
            <tr>
              <th style={{ padding: "12px", textAlign: "left", borderRight: "3px solid rgba(255,255,255,0.9)" }}>
                Site Name
              </th>

              <th style={{ padding: "12px", textAlign: "left", borderRight: "3px solid rgba(255,255,255,0.9)" }}>
                Started Date
              </th>

              <th style={{ padding: "12px", textAlign: "left", borderRight: "3px solid rgba(255,255,255,0.9)" }}>
                Status
              </th>

              <th style={{ padding: "12px", textAlign: "left", borderRight: "3px solid rgba(255,255,255,0.9)" }}>
                Contact Name
              </th>

              <th style={{ padding: "12px", textAlign: "left", borderRight: "3px solid rgba(255,255,255,0.9)" }}>
                Contact Number
              </th>

              <th
                onClick={() => setExpandAddressColumn(!expandAddressColumn)}
                style={{
                  padding: "12px",
                  textAlign: "left",
                  borderRight: "3px solid rgba(255,255,255,0.9)",
                  cursor: "pointer",
                  width: expandAddressColumn ? "30%" : "15%",
                }}
              >
              Address {expandAddressColumn ? "▲" : "▼"}
              </th>

              <th style={{ padding: "12px", textAlign: "left" }}>
                Action
              </th>
            </tr>
          </thead>

              <tbody>
                {sites.length > 0 ? (
                  sites.map((site, index) => (
                    <tr
                      key={site.id}
                      style={{
                        borderBottom: "1px solid #eee",
                        background:
                          index % 2 === 1 ? "#f3f4f6" : "#fff",
                      }}
                      onMouseEnter={(e) =>
                        (e.currentTarget.style.background = "#f9fafb")
                      }
                      onMouseLeave={(e) =>
                        (e.currentTarget.style.background =
                          index % 2 === 1 ? "#f3f4f6" : "#fff")
                      }
                    >
                      <td style={{ padding: "12px", textAlign: "left" }}>
                        <b>{site.name}</b>
                      </td>

                      <td style={{ padding: "12px", textAlign: "left" }}>
                        {site.startedDate
                          ? fmt(site.startedDate)
                          : "—"}
                      </td>

                      <td style={{ padding: "12px", textAlign: "left" }}>
                        {site.status ?? "—"}
                      </td>

                      <td style={{ padding: "12px", textAlign: "left" }}>
                        {site.contactName ?? "—"}
                      </td>

                      <td style={{ padding: "12px", textAlign: "left" }}>
                        {site.contactNumber ?? "—"}
                      </td>

                      <td
                        style={{
                          padding: "8px 12px",
                          textAlign: "left",
                          color: "#64748B",
                          width: expandAddressColumn ? "30%" : "15%",
                          whiteSpace: expandAddressColumn ? "normal" : "nowrap",
                          overflow: "hidden",
                          textOverflow: "ellipsis",
                          borderRight: "1px solid #E5E7EB",
                        }}>
                        {[
                          site.addressTypes,
                          site.addressLine1,
                          site.countryName,
                          site.pinCode,
                        ]
                        .filter(Boolean)
                        .join(" - ") || "—"}
                      </td>

                      <td style={{padding: "12px", textAlign: "left", width: "10%", whiteSpace: "nowrap",}}>
                        <Link
                          to={`/site-edit/${site.id}`}
                          style={{
                            color: "#16A34A",
                            fontWeight: 500,
                            textDecoration: "none",
                            marginRight: "8px",
                          }}
                        >
                          Edit
                        </Link>

                        {" | "}

                        <button
                          onClick={() => handleDelete(site.id)}
                          style={{
                            color: "#DC2626",
                            fontWeight: 500,
                            background: "none",
                            border: "none",
                            cursor: "pointer",
                            padding: 0,
                            marginLeft: "8px",
                          }}
                        >
                          Delete
                        </button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td
                      colSpan={7}
                      style={{
                        textAlign: "center",
                        padding: "20px",
                        color: "#888",
                      }}
                    >
                      No site records found.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </div>
  );
}

export default SiteList;
import { useEffect, useState }   from "react";
import { Link, useLocation }     from "react-router-dom";
import { getServiceProviders, deleteServiceProvider }
  from "../../services/serviceProviderService";
import type { ServiceProviderDto } from "../../services/serviceProviderService";

function ServiceProviderList() {
  const location = useLocation();
  const [records,    setRecords]    = useState<ServiceProviderDto[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [loading,    setLoading]    = useState(true);
  const [error,      setError]      = useState("");

  const successMessage =
    (location.state as { success?: string })?.success ?? "";

  useEffect(() => {
    getServiceProviders()
      .then((res) => { setRecords(res.items); setTotalCount(res.totalCount); })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this service Provider?")) return;
    try {
      await deleteServiceProvider(id);
      setRecords((prev) => prev.filter((r) => r.serviceProviderId !== id));
      setTotalCount((c) => c - 1);
    } catch (err) {
      setError((err as Error).message);
    }
  };

  const formatAddress = (item: ServiceProviderDto) => {
    const parts = [item.addressTypes, item.addressLine1, item.countryName, item.pinCode];
    return parts.filter(Boolean).join(" - ") || "—";
  };

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh" }}>
      <div style={{ padding: "30px" }}>
        <div style={{
          background: "#fff", borderRadius: "18px",
          padding: "25px", boxShadow: "0 10px 25px rgba(0,0,0,0.15)",
        }}>

          {/* Header — 3 column grid matching MVC */}
          <div style={{
            display: "grid", gridTemplateColumns: "1fr auto 1fr",
            alignItems: "center", marginBottom: "20px",
          }}>
            <div>
              <div style={{
                display: "inline-block", background: "#1E3A5F", color: "#fff",
                padding: "8px 16px", borderRadius: "20px", fontWeight: 500,
              }}>
                Total Service Providers&nbsp;
                <span style={{
                  background: "#F59E0B", padding: "4px 10px",
                  borderRadius: "12px", marginLeft: "4px",
                }}>
                  {totalCount}
                </span>
              </div>
            </div>

            <div style={{ fontSize: "28px", fontWeight: 600, color: "#1E293B" }}>
              Service Providers / Vendor
            </div>

            <div style={{ textAlign: "right" }}>
              <Link to="/service-provider/add" style={{
                background: "#F59E0B", color: "#fff", padding: "10px 18px",
                borderRadius: "10px", textDecoration: "none", fontWeight: 500,
              }}>
                + Add New
              </Link>
            </div>
          </div>

          {/* Messages */}
          {successMessage && (
            <div style={{ color: "#16A34A", marginBottom: "12px", fontWeight: 500 }}>
              {successMessage}
            </div>
          )}
          {error && (
            <div style={{ color: "#DC2626", marginBottom: "12px" }}>{error}</div>
          )}

          {/* Table */}
          {loading ? (
            <p style={{ textAlign: "center", padding: "30px" }}>Loading...</p>
          ) : (
            <table style={{ width: "100%", borderCollapse: "collapse", borderRadius: "12px", overflow: "hidden" }}>
              <thead style={{ background: "#1E3A5F", color: "#fff" }}>
                <tr>
                  {["Service Type","Name","Gender","Mobile","Referred By","Address","Action"].map((h) => (
                    <th key={h} style={{ padding: "14px", textAlign: "left", fontSize: "14px" }}>{h}</th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {records.length > 0 ? records.map((item) => (
                  <tr key={item.serviceProviderId}
                    style={{ borderBottom: "1px solid #eee" }}
                    onMouseEnter={(e) => (e.currentTarget.style.background = "#f9fafb")}
                    onMouseLeave={(e) => (e.currentTarget.style.background = "")}
                  >
                    <td style={{ padding: "12px", fontSize: "16px" }}>{item.serviceTypes}</td>
                    <td style={{ padding: "12px", fontSize: "16px" }}>{item.serviceProviderName}</td>
                    <td style={{ padding: "12px", fontSize: "16px" }}>{item.gender}</td>
                    <td style={{ padding: "12px", fontSize: "16px" }}>{item.mobileNumber}</td>
                    <td style={{ padding: "12px", fontSize: "16px" }}>{item.referredBy}</td>
                    <td style={{ padding: "12px", fontSize: "16px", color: "#64748B" }}>
                      {formatAddress(item)}
                    </td>
                    <td style={{ padding: "12px", width: "140px" }}>
                      <Link
                        to={`/service-provider/edit/${item.serviceProviderId}`}
                        style={{ color: "#16A34A", fontWeight: 500, textDecoration: "none", marginRight: "8px" }}
                      >
                        Edit
                      </Link>
                      <button
                        onClick={() => handleDelete(item.serviceProviderId)}
                        style={{
                          color: "#DC2626", fontWeight: 500,
                          background: "none", border: "none", cursor: "pointer", padding: 0,
                        }}
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                )) : (
                  <tr>
                    <td colSpan={7} style={{ textAlign: "center", padding: "20px", color: "#888" }}>
                      No Service Providers found.
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

export default ServiceProviderList;
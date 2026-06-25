// Path: src/pages/CostMaster/CostMasterList.tsx
import { useEffect, useRef, useState }    from "react";
import { Link, useLocation }              from "react-router-dom";
import { getCostMasters, deleteCostMaster } from "../../services/costMasterService";
import type { CostMasterDto, ServiceTypeOption } from "../../services/costMasterService";
import { useSite }                        from "../../context/Sitecontext";

function CostMasterList() {
  const location = useLocation();
  const { selectedSite } = useSite();
  const siteId   = selectedSite?.id ?? 0;

  const [costMasters,   setCostMasters]   = useState<CostMasterDto[]>([]);
  const [serviceTypes,  setServiceTypes]  = useState<ServiceTypeOption[]>([]);
  const [serviceTypeId, setServiceTypeId] = useState<number | null>(null);
  const [loading,       setLoading]       = useState(true);
  const [error,         setError]         = useState("");
  const isFirstLoad = useRef(true);

  const successMessage = (location.state as { success?: string })?.success;

  // Initial load
  useEffect(() => {
    getCostMasters(siteId)
      .then((result) => {
        setServiceTypes(result.serviceTypes);
        setCostMasters(result.items);
        setServiceTypeId(result.selectedServiceTypeId);
      })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, [siteId]);

  // Filter change
  useEffect(() => {
    if (isFirstLoad.current) { isFirstLoad.current = false; return; }
    if (!serviceTypeId) return;
    setLoading(true);
    getCostMasters(siteId, serviceTypeId)
      .then((result) => setCostMasters(result.items))
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, [serviceTypeId]);

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this record?")) return;
    try {
      await deleteCostMaster(siteId, id);
      setCostMasters((prev) => prev.filter((x) => x.id !== id));
    } catch (err) {
      setError((err as Error).message);
    }
  };

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh" }}>
      <div style={{ padding: "30px", maxWidth: "1000px", margin: "0 auto" }}>
        <div style={{
          background: "#fff", borderRadius: "16px",
          padding: "25px", boxShadow: "0 10px 30px rgba(0,0,0,0.2)",
        }}>

          {/* Header */}
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "15px" }}>
            <div />
            <h3 style={{ fontWeight: 700, color: "#1E293B", margin: 0 }}>Cost Master</h3>
            <Link
              to="/cost-master/add"
              style={{
                background: "#F59E0B", color: "#fff", fontWeight: 600,
                borderRadius: "10px", padding: "8px 20px", textDecoration: "none",
              }}
            >
              + Add New
            </Link>
          </div>

          {/* Success */}
          {successMessage && (
            <div style={{
              background: "#dcfce7", color: "#166534", padding: "10px",
              borderRadius: "8px", textAlign: "center", marginBottom: "15px",
            }}>
              {successMessage}
            </div>
          )}

          {/* Error */}
          {error && (
            <div style={{
              background: "#fdecea", color: "#c62828", padding: "10px",
              borderRadius: "8px", textAlign: "center", marginBottom: "15px",
            }}>
              {error}
            </div>
          )}

          {/* Service Type Filter */}
          <div style={{ marginBottom: "15px", display: "flex", alignItems: "center", gap: "10px" }}>
            <label style={{ fontWeight: 600 }}>Service Type:</label>
            <select
              value={serviceTypeId ?? ""}
              onChange={(e) => setServiceTypeId(Number(e.target.value))}
              style={{
                borderRadius: "8px", padding: "5px 10px",
                width: "200px", border: "1px solid #ddd",
              }}
            >
              {serviceTypes.map((s) => (
                <option key={s.id} value={s.id}>{s.name}</option>
              ))}
            </select>
          </div>

          {/* Table */}
          {loading ? (
            <p style={{ textAlign: "center", padding: "30px" }}>Loading...</p>
          ) : costMasters.length > 0 ? (
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead style={{ background: "#1E3A5F", color: "#fff" }}>
                <tr>
                  <th style={{ padding: "14px", textAlign: "center" }}>Date</th>
                  <th style={{ padding: "14px", textAlign: "center" }}>Service Type</th>
                  <th style={{ padding: "14px", textAlign: "center" }}>Cost</th>
                  <th style={{ padding: "14px", textAlign: "center", width: "140px" }}>Action</th>
                </tr>
              </thead>
              <tbody>
                {costMasters.map((item) => (
                  <tr key={item.id} style={{ borderBottom: "1px solid #eee" }}>
                    <td style={{ padding: "12px", textAlign: "center" }}>
                      {new Date(item.date).toLocaleDateString("en-GB", {
                        day: "2-digit", month: "short", year: "numeric",
                      })}
                    </td>
                    <td style={{ padding: "12px", textAlign: "center" }}>{item.name}</td>
                    <td style={{ padding: "12px", textAlign: "center" }}>
                      <span style={{
                        background: "#FEF3C7", color: "#92400E",
                        border: "1px solid #FDE68A", padding: "4px 12px", borderRadius: "8px",
                      }}>
                        ₹ {item.cost}
                      </span>
                    </td>
                    <td style={{ padding: "12px", textAlign: "center" }}>
                      <Link
                        to={`/cost-master/edit/${item.id}`}
                        style={{ color: "#16A34A", fontWeight: 500, textDecoration: "none", marginRight: "12px" }}
                      >
                        Edit
                      </Link>
                      <button
                        onClick={() => handleDelete(item.id)}
                        style={{
                          color: "#DC2626", fontWeight: 500, background: "none",
                          border: "none", cursor: "pointer", padding: 0,
                        }}
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <div style={{ textAlign: "center", padding: "30px", fontWeight: 600, color: "#6c757d" }}>
              🚫 No records found for selected service type
            </div>
          )}

        </div>
      </div>
    </div>
  );
}

export default CostMasterList;
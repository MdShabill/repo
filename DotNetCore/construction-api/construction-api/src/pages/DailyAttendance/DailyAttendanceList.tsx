// Path: src/pages/DailyAttendance/DailyAttendanceList.tsx
import { useEffect, useState }    from "react";
import { Link, useLocation }      from "react-router-dom";
import { getDailyAttendances }    from "../../services/dailyAttendanceService";
import type { DailyAttendanceDto } from "../../services/dailyAttendanceService";
import { useSite }                from "../../context/Sitecontext";

function DailyAttendanceList() {
  const location = useLocation();
  const { selectedSite } = useSite();
  const siteId = selectedSite?.id ?? 0;

  const [records,  setRecords]  = useState<DailyAttendanceDto[]>([]);
  const [dateFrom, setDateFrom] = useState("");
  const [dateTo,   setDateTo]   = useState("");
  const [loading,  setLoading]  = useState(true);
  const [error,    setError]    = useState("");

  const successMessage = (location.state as { success?: string })?.success;

  const loadData = (from?: string, to?: string) => {
    setLoading(true);
    getDailyAttendances(siteId, from, to)
      .then(setRecords)
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  };

  useEffect(() => { loadData(); }, [siteId]);

  const handleGo = (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    const currentDate = new Date().toISOString().split("T")[0];
    const fromDate = dateFrom ? new Date(dateFrom) : null;
    const toDate   = dateTo   ? new Date(dateTo)   : null;

    if (fromDate && toDate && fromDate > toDate) {
      setError("FROM DATE cannot be greater than TO DATE.");
      return;
    }
    if ((dateTo && dateFrom > currentDate) || (dateFrom && dateTo > currentDate)) {
      setError("FROM DATE and TO DATE cannot be in the future.");
      return;
    }

    loadData(dateFrom || undefined, dateTo || undefined);
  };

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh" }}>
      <div style={{ padding: "30px" }}>
        <div style={{
          background: "#fff", borderRadius: "16px", padding: "25px",
          boxShadow: "0 10px 30px rgba(0,0,0,0.2)",
        }}>

          {/* Header */}
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "15px" }}>
            <div />
            <h3 style={{ fontWeight: 700, color: "#1E293B", margin: 0 }}>Attendance</h3>
            <Link
              to="/attendance/add"
              style={{
                background: "#F59E0B", color: "#fff", fontWeight: 600,
                borderRadius: "10px", padding: "8px 18px", textDecoration: "none",
              }}
            >
              + Add New
            </Link>
          </div>

          {successMessage && (
            <div style={{
              background: "#dcfce7", color: "#166534", padding: "10px",
              borderRadius: "8px", textAlign: "center", marginBottom: "15px",
            }}>
              {successMessage}
            </div>
          )}

          {/* Filter */}
          <form onSubmit={handleGo} style={{ display: "flex", alignItems: "center", gap: "10px", marginBottom: "15px" }}>
            <label style={{ fontWeight: 600 }}>From:</label>
            <input
              type="date" value={dateFrom}
              onChange={(e) => setDateFrom(e.target.value)}
              style={{ width: "150px", borderRadius: "8px", padding: "6px 10px", border: "1px solid #ddd" }}
            />
            <label style={{ fontWeight: 600 }}>To:</label>
            <input
              type="date" value={dateTo}
              onChange={(e) => setDateTo(e.target.value)}
              style={{ width: "150px", borderRadius: "8px", padding: "6px 10px", border: "1px solid #ddd" }}
            />
            <button
              type="submit"
              style={{
                background: "#F59E0B", color: "#fff", fontWeight: 600,
                borderRadius: "10px", padding: "8px 18px", border: "none", cursor: "pointer",
              }}
            >
              Go
            </button>
          </form>

          {error && (
            <p style={{ color: "red", fontWeight: 500, marginTop: "0" }}>{error}</p>
          )}

          {/* Table */}
          {loading ? (
            <p style={{ textAlign: "center", padding: "30px" }}>Loading...</p>
          ) : records.length > 0 ? (
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead style={{ background: "#1E3A5F", color: "#fff" }}>
                <tr>
                  {["Date", "Service Type", "Service Providers", "Total Worker", "Amount Per Worker", "Total Amount"].map((h) => (
                    <th key={h} style={{ padding: "14px", textAlign: "center" }}>{h}</th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {records.map((item) => (
                  <tr key={item.id} style={{ borderBottom: "1px solid #eee", textAlign: "center" }}>
                    <td style={{ padding: "12px" }}>
                      {new Date(item.date).toLocaleDateString("en-GB", { day: "2-digit", month: "short", year: "numeric" })}
                    </td>
                    <td style={{ padding: "12px" }}>{item.name}</td>
                    <td style={{ padding: "12px" }}>{item.serviceProviderName}</td>
                    <td style={{ padding: "12px" }}>
                      <span style={{ background: "#FEF3C7", color: "#92400E", border: "1px solid #FDE68A", padding: "4px 12px", borderRadius: "8px" }}>
                        {item.totalWorker}
                      </span>
                    </td>
                    <td style={{ padding: "12px" }}>
                      <span style={{ background: "#FEF3C7", color: "#92400E", border: "1px solid #FDE68A", padding: "4px 12px", borderRadius: "8px" }}>
                        {item.amountPerWorker.toFixed(2)}
                      </span>
                    </td>
                    <td style={{ padding: "12px" }}>
                      <span style={{ background: "#FEF3C7", color: "#92400E", border: "1px solid #FDE68A", padding: "4px 12px", borderRadius: "8px" }}>
                        ₹ {item.totalAmount.toFixed(2)}
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <div style={{ textAlign: "center", padding: "30px", fontWeight: 600, color: "#6c757d" }}>
              🚫 No Record Found
            </div>
          )}

        </div>
      </div>
    </div>
  );
}

export default DailyAttendanceList;
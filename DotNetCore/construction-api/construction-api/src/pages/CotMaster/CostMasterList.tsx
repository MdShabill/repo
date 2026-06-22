import { useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { getCostMasters, getServiceTypes } from "../../services/costMasterService";
import type { CostMasterDto, ServiceTypeOption } from "../../services/costMasterService";

function CostMasterList() {
  const location = useLocation();
  const [costMasters, setCostMasters] = useState<CostMasterDto[]>([]);
  const [serviceTypes, setServiceTypes] = useState<ServiceTypeOption[]>([]);
  const [serviceTypeId, setServiceTypeId] = useState<number | "">("");
  const [loading, setLoading] = useState(true);
  const successMessage = (location.state as { success?: string })?.success;

  useEffect(() => {
    getServiceTypes().then((types) => {
      setServiceTypes(types);
      if (types.length > 0) setServiceTypeId(types[0].id);
    });
  }, []);

  useEffect(() => {
    if (serviceTypeId === "") return;
    setLoading(true);
    getCostMasters(Number(serviceTypeId)).then(setCostMasters).finally(() => setLoading(false));
  }, [serviceTypeId]);

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh" }}>
      <div style={{ padding: "30px", maxWidth: "900px", margin: "0 auto" }}>
        <div style={{ background: "#fff", borderRadius: "16px", padding: "25px", boxShadow: "0 10px 30px rgba(0,0,0,0.2)" }}>

          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "15px" }}>
            <div />
            <h3 style={{ fontWeight: 700, color: "#1E293B", margin: 0 }}>Cost Master</h3>
            <Link to="/cost-master/add" style={{ background: "#F59E0B", color: "#fff", fontWeight: 600, borderRadius: "10px", padding: "8px 20px", textDecoration: "none" }}>
              + Add New
            </Link>
          </div>

          {successMessage && (
            <div style={{ background: "#dcfce7", color: "#166534", padding: "10px", borderRadius: "8px", textAlign: "center", marginBottom: "15px" }}>
              {successMessage}
            </div>
          )}

          <div style={{ marginBottom: "15px", display: "flex", alignItems: "center", gap: "10px" }}>
            <label style={{ fontWeight: 600 }}>Service Type:</label>
            <select value={serviceTypeId} onChange={(e) => setServiceTypeId(Number(e.target.value))} style={{ borderRadius: "8px", padding: "5px 10px", width: "180px" }}>
              {serviceTypes.map((s) => (<option key={s.id} value={s.id}>{s.name}</option>))}
            </select>
          </div>

          {loading ? (
            <p style={{ textAlign: "center", padding: "30px" }}>Loading...</p>
          ) : costMasters.length > 0 ? (
            <table style={{ width: "100%", borderCollapse: "collapse", borderRadius: "12px", overflow: "hidden" }}>
              <thead style={{ background: "#1E3A5F", color: "#fff" }}>
                <tr>
                  <th style={{ padding: "12px" }}>Date</th>
                  <th style={{ padding: "12px" }}>Service Type</th>
                  <th style={{ padding: "12px" }}>Cost</th>
                </tr>
              </thead>
              <tbody>
                {costMasters.map((item) => (
                  <tr key={item.id} style={{ borderBottom: "1px solid #eee", textAlign: "center" }}>
                    <td style={{ padding: "12px" }}>{new Date(item.date).toLocaleDateString("en-GB", { day: "2-digit", month: "short", year: "numeric" })}</td>
                    <td style={{ padding: "12px" }}>{item.name}</td>
                    <td style={{ padding: "12px" }}>
                      <span style={{ background: "#FEF3C7", color: "#92400E", border: "1px solid #FDE68A", padding: "4px 12px", borderRadius: "8px" }}>₹ {item.cost}</span>
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
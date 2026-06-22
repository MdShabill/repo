import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { getServiceTypes, getActiveCost, addCostMaster } from "../../services/costMasterService";
import type { ServiceTypeOption } from "../../services/costMasterService";

function CostMasterAdd() {
  const navigate = useNavigate();
  const [serviceTypes, setServiceTypes] = useState<ServiceTypeOption[]>([]);
  const [serviceTypeId, setServiceTypeId] = useState("");
  const [cost, setCost] = useState("");
  const [date, setDate] = useState(new Date().toISOString().slice(0, 10));
  const [errorMessage, setErrorMessage] = useState("");
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => { getServiceTypes().then(setServiceTypes); }, []);

  const handleServiceTypeChange = async (id: string) => {
    setServiceTypeId(id);
    setCost("");
    if (!id) return;
    const active = await getActiveCost(Number(id)).catch(() => null);
    if (active) setCost(String(active.cost));
  };

  const validate = (): string | null => {
    if (!date || !serviceTypeId) return "Page not submitted, please enter correct Inputs";
    if (!cost || Number(cost) <= 0 || !/^\d+$/.test(cost)) {
      return "Cost must be a positive integer and cannot contain alphabets, decimals, or special characters.";
    }
    return null;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const validationError = validate();
    if (validationError) { setErrorMessage(validationError); return; }
    setSubmitting(true);
    try {
      await addCostMaster({ serviceTypeId: Number(serviceTypeId), cost: Number(cost), date });
      navigate("/cost-master", { state: { success: "Add New Cost Master Successful" } });
    } catch (err: any) {
      setErrorMessage(err.message || "Failed to add cost master");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh", padding: "30px" }}>
      <div style={{ background: "#fff", borderRadius: "16px", padding: "30px", boxShadow: "0 10px 30px rgba(0,0,0,0.2)", maxWidth: "500px", margin: "auto" }}>
        <h3 style={{ textAlign: "center", fontWeight: 700, color: "#1E293B", marginBottom: "20px" }}>Add Cost</h3>

        {errorMessage && (
          <div style={{ background: "#fdecea", color: "#c62828", padding: "10px", borderRadius: "8px", textAlign: "center", marginBottom: "15px" }}>
            <b>{errorMessage}</b>
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: "15px" }}>
            <label style={{ fontWeight: 600, display: "block", marginBottom: "5px" }}>Date</label>
            <input type="date" value={date} onChange={(e) => setDate(e.target.value)} style={{ width: "100%", borderRadius: "10px", padding: "10px", border: "1px solid #ddd" }} />
          </div>

          <div style={{ marginBottom: "15px" }}>
            <label style={{ fontWeight: 600, display: "block", marginBottom: "5px" }}>Service Type</label>
            <select value={serviceTypeId} onChange={(e) => handleServiceTypeChange(e.target.value)} style={{ width: "100%", borderRadius: "10px", padding: "10px", border: "1px solid #ddd", height: "40px" }}>
              <option value="">-- Select --</option>
              {serviceTypes.map((s) => (<option key={s.id} value={s.id}>{s.name}</option>))}
            </select>
          </div>

          <div style={{ marginBottom: "20px" }}>
            <label style={{ fontWeight: 600, display: "block", marginBottom: "5px" }}>Cost</label>
            <input value={cost} onChange={(e) => setCost(e.target.value)} style={{ width: "100%", borderRadius: "10px", padding: "10px", border: "1px solid #ddd" }} />
          </div>

          <div style={{ display: "flex", justifyContent: "flex-end", gap: "8px" }}>
            <Link to="/cost-master" style={{ background: "transparent", color: "#64748B", fontWeight: 600, borderRadius: "10px", padding: "10px 20px", border: "1px solid #CBD5E1", textDecoration: "none" }}>Cancel</Link>
            <button type="submit" disabled={submitting} style={{ background: "#F59E0B", color: "#fff", fontWeight: 600, borderRadius: "10px", padding: "10px 20px", border: "none", cursor: submitting ? "not-allowed" : "pointer" }}>
              {submitting ? "Adding..." : "Add"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default CostMasterAdd;
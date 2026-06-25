// Path: src/pages/CostMaster/CostMasterAdd.tsx
import { useEffect, useState }          from "react";
import { useNavigate, Link }            from "react-router-dom";
import { getCostMasters, getActiveCost, addCostMaster } from "../../services/costMasterService";
import type { ServiceTypeOption }       from "../../services/costMasterService";
import { useSite }                      from "../../context/Sitecontext";

function CostMasterAdd() {
  const navigate  = useNavigate();
  const { selectedSite } = useSite();
  const siteId    = selectedSite?.id ?? 0;

  const [serviceTypes,  setServiceTypes]  = useState<ServiceTypeOption[]>([]);
  const [serviceTypeId, setServiceTypeId] = useState("");
  const [cost,          setCost]          = useState("");
  const [date,          setDate]          = useState(new Date().toISOString().slice(0, 10));
  const [errorMessage,  setErrorMessage]  = useState("");
  const [submitting,    setSubmitting]    = useState(false);

  // Reuse Index endpoint to get service types — no separate endpoint needed
  useEffect(() => {
    getCostMasters(siteId)
      .then((result)=>{setServiceTypes(result.serviceTypes);

      if(result.serviceTypes.length>0){
      const firstId=result.serviceTypes[0].id;
      setServiceTypeId(String(firstId));
      getActiveCost(siteId,firstId)
      .then(x=>{
      if(x)
      setCost(String(x.cost));
   });
}

})
      .catch(() => setErrorMessage("Failed to load service types. Check connection."));
  }, [siteId]);

  // Auto-fill cost when service type changes (mirrors MVC AJAX updateCost())
  const handleServiceTypeChange = async (id: string) => {
    setServiceTypeId(id);
    setCost("");
    setErrorMessage("");
    if (!id) return;
    const active = await getActiveCost(siteId, Number(id)).catch(() => null);
    if (active) setCost(String(active.cost));
  };

  const validate = (): string | null => {
    if (!date || !serviceTypeId)
      return "Page not submitted, please enter correct Inputs";
    if (!cost || Number(cost)<=0 || !/^\d+(\.\d{1,2})?$/.test(cost))
      return "Cost must be a positive number and cannot contain alphabets or special characters.";
    return null;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const validationError = validate();
    if (validationError) { setErrorMessage(validationError); return; }
    setSubmitting(true);
    try {
      await addCostMaster(siteId, {
        serviceTypeId: Number(serviceTypeId),
        cost:          Number(cost),
        date,
      });
      navigate("/cost-master", { state: { success: "Add New Cost Master Successful" } });
    } catch (err: unknown) {
      setErrorMessage((err as Error).message || "Failed to add cost master");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh", padding: "80px 30px 30px" }}>
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
            <input
              type="date" value={date}
              onChange={(e) => setDate(e.target.value)}
              style={{ width: "100%", borderRadius: "10px", padding: "10px", border: "1px solid #ddd" }}
            />
          </div>

          <div style={{ marginBottom: "15px" }}>
            <label style={{ fontWeight: 600, display: "block", marginBottom: "5px" }}>Service Type</label>
            <select
              value={serviceTypeId}
              onChange={(e) => handleServiceTypeChange(e.target.value)}
              style={{ width: "100%", borderRadius: "10px", padding: "10px", border: "1px solid #ddd", height: "40px" }}
            >
              <option value="">-- Select --</option>
              {serviceTypes.map((s) => (
                <option key={s.id} value={s.id}>{s.name}</option>
              ))}
            </select>
          </div>

          <div style={{ marginBottom: "20px" }}>
            <label style={{ fontWeight: 600, display: "block", marginBottom: "5px" }}>Cost</label>
            <input
              type="number"
              step="0.01"
              onChange={(e) => setCost(e.target.value)}
              placeholder="Auto-filled from last active cost"
              style={{ width: "100%", borderRadius: "10px", padding: "10px", border: "1px solid #ddd" }}
            />
          </div>

          <div style={{ display: "flex", justifyContent: "flex-end", gap: "8px" }}>
            <Link to="/cost-master" style={{ background: "transparent", color: "#64748B", fontWeight: 600, borderRadius: "10px", padding: "10px 20px", border: "1px solid #CBD5E1", textDecoration: "none" }}>
              Cancel
            </Link>
            <button
              type="submit" disabled={submitting}
              style={{ background: "#F59E0B", color: "#fff", fontWeight: 600, borderRadius: "10px", padding: "10px 20px", border: "none", cursor: submitting ? "not-allowed" : "pointer" }}
            >
              {submitting ? "Adding..." : "Add"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default CostMasterAdd;
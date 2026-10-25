// Path: src/pages/MaterialPurchase/MaterialPurchaseList.tsx
import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { getMaterialPurchases, getDropdownData, deleteMaterialPurchase } from "../../services/materialPurchaseService";
import type { MaterialPurchaseDto, DropdownOption } from "../../services/materialPurchaseService";
import { useSite } from "../../context/Sitecontext";
import PageHeader from "../../components/PageHeader";

const HEADERS = ["Date", "Supplier", "Supplier Contact", "Brand", 
  "Material", "Unit Price", "Quantity", "Material Cost", "Delivery Charge", "Total Amount", "Action"];
const pill = (v: React.ReactNode) => (
  <span style={{ 
    background: "#FEF3C7", color: "#92400E", border: "1px solid #FDE68A", 
    padding: "4px 12px", borderRadius: "8px" }}>{v}</span>
);
const fmt = (d: string) => new Date(d).toLocaleDateString("en-GB", { day: "2-digit", month: "short", year: "numeric" });
const inputStyle: React.CSSProperties = { 
  borderRadius: "8px", padding: "6px 10px", fontSize: "14px", width: "150px", border: "1px solid #ddd" };

function MaterialPurchaseList() {
  const location = useLocation();
  const { selectedSite } = useSite();
  const siteId = selectedSite?.id ?? 0;

  const [records, setRecords] = useState<MaterialPurchaseDto[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [materials, setMaterials] = useState<DropdownOption[]>([]);
  const [suppliers, setSuppliers] = useState<DropdownOption[]>([]);
  const [brands, setBrands] = useState<DropdownOption[]>([]);
  const [dateFrom, setDateFrom] = useState("");
  const [dateTo, setDateTo] = useState("");
  const [materialId, setMaterialId] = useState("");
  const [supplierId, setSupplierId] = useState("");
  const [brandId, setBrandId] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const successMessage = (location.state as { success?: string })?.success;

  useEffect(() => {
    getDropdownData()
      .then((dd) => { setMaterials(dd.materials); setSuppliers(dd.suppliers); setBrands(dd.brands); })
      .catch(() => {});
  }, []);

  const loadData = (from?: string, to?: string, mat?: string, sup?: string, brd?: string) => {
    setLoading(true);
    setError("");
    getMaterialPurchases(
      siteId, from || undefined, to || 
      undefined, mat ? Number(mat) : undefined, sup ? Number(sup) : undefined, brd ? Number(brd) : undefined)
      .then((res) => { setRecords(res.items); setTotalCount(res.totalCount); })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  };

  useEffect(() => { loadData(); }, [siteId]);

  const handleGo = (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    const today = new Date().toISOString().split("T")[0];
    if (dateFrom && dateTo && dateFrom > dateTo) return setError("FROM DATE cannot be greater than TO DATE.");
    if ((dateTo && dateTo > today) || (dateFrom && dateFrom > today)) return setError("FROM DATE and TO DATE cannot be in the future.");
    loadData(dateFrom, dateTo, materialId, supplierId, brandId);
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this record?")) return;
    try {
      await deleteMaterialPurchase(siteId, id);
      setRecords((prev) => prev.filter((r) => r.id !== id));
      setTotalCount((c) => c - 1);
    } catch (err) {
      setError((err as Error).message);
    }
  };

  const sel = (lbl: string, val: string, set: (v: string) => void, opts: DropdownOption[]) => (
    <label className="fl">{lbl}
      <select value={val} onChange={(e) => set(e.target.value)} style={inputStyle}>
        <option value="">-- Select --</option>
        {opts.map((o) => <option key={o.id} value={o.id}>{o.name}</option>)}
      </select>
    </label>
  );

  const cell = { padding: "8px 12px" };

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh" }}>
      <div className="page-pad" style={{ padding: "30px 10px" }}>
        <div className="page-card" style={{ 
          background: "#fff", borderRadius: "16px", 
          padding: "25px", boxShadow: "0 10px 30px rgba(0,0,0,0.2)", 
          maxWidth: "1400px", margin: "auto" }}>

          {successMessage && <div style={{ 
            color: "#166534", textAlign: "center", 
            marginBottom: "10px", fontWeight: 600 }}>{successMessage}
            </div>}

          <PageHeader
            title="Material Purchase"
            addTo="/material/add"
            badge={<span className="badge-pill">Total Purchase <b>{totalCount}</b></span>}
          />

          <form onSubmit={handleGo} className="filter-form">
            <label className="fl">From: <
              input type="date" value={dateFrom} onChange={(e) => setDateFrom(e.target.value)} style={inputStyle} /></label>
            <label className="fl">To: <
              input type="date" value={dateTo} onChange={(e) => setDateTo(e.target.value)} style={inputStyle} /></label>
            {sel("Material:", materialId, setMaterialId, materials)}
            {sel("Supplier:", supplierId, setSupplierId, suppliers)}
            {sel("Brand:", brandId, setBrandId, brands)}
            <button type="submit" style={{ 
              background: "#F59E0B", color: "#fff", 
              borderRadius: "8px", padding: "6px 15px", 
              border: "none", cursor: "pointer", fontWeight: 600 }}
              >Go</button>
          </form>

          {error && <p style={{ color: "red", fontWeight: 500 }}>{error}</p>}

          {loading ? (
            <p style={{ textAlign: "center", padding: "30px" }}>Loading...</p>
          ) : records.length ? (
            <div style={{ overflowX: "auto" }}>
              <table className="stack-table" style={{ width: "100%", borderCollapse: "collapse", whiteSpace: "nowrap" }}>
                <thead style={{ background: "#1E3A5F", color: "#fff" }}>
                  <tr>{HEADERS.map((h) => <th key={h} style={{ padding: "8px 12px", fontSize: "14px", textAlign: "center" }}>{h}</th>)}</tr>
                </thead>
                <tbody>
                  {records.map((r, i) => {
                    const bg = i % 2 ? "#f8fafc" : "#fff";
                    return (
                      <tr key={r.id} style={{ background: bg, textAlign: "center" }}
                        onMouseEnter={(e) => (e.currentTarget.style.background = "#eef2f7")}
                        onMouseLeave={(e) => (e.currentTarget.style.background = bg)}>
                        <td data-label={HEADERS[0]} style={cell}>{fmt(r.date)}</td>
                        <td data-label={HEADERS[1]} style={cell}>{r.supplierName}</td>
                        <td data-label={HEADERS[2]} style={cell}>{r.phoneNumber}</td>
                        <td data-label={HEADERS[3]} style={cell}>{r.brandName}</td>
                        <td data-label={HEADERS[4]} style={cell}>{r.materialName}</td>
                        <td data-label={HEADERS[5]} style={cell}>{r.unitPrice.toFixed(2)} / <i>{r.unitOfMeasure}</i></td>
                        <td data-label={HEADERS[6]} style={cell}>{r.quantity.toFixed(2)} / <i>{r.unitOfMeasure}</i></td>
                        <td data-label={HEADERS[7]} style={cell}>{pill(r.materialCost.toFixed(2))}</td>
                        <td data-label={HEADERS[8]} style={cell}>{pill(r.deliveryCharge.toFixed(2))}</td>
                        <td data-label={HEADERS[9]} style={cell}>{pill(`₹ ${(r.materialCost + r.deliveryCharge).toFixed(2)}`)}</td>
                        <td data-label={HEADERS[10]} style={cell}>
                          <button onClick={() => handleDelete(r.id)} 
                          style={{ color: "#DC2626", 
                          background: "none", border: "none", 
                          cursor: "pointer", fontWeight: 500 }}
                          >Delete</button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : (
            <div style={{ textAlign: "center", padding: "30px", fontWeight: 600, color: "#6c757d" }}>🚫 No Record Found</div>
          )}
        </div>
      </div>
    </div>
  );
}

export default MaterialPurchaseList;
// Path: src/pages/MaterialPurchase/MaterialPurchaseList.tsx
import { useEffect, useState }  from "react";
import { Link, useLocation }    from "react-router-dom";
import {
  getMaterialPurchases, getDropdownData, deleteMaterialPurchase,
} from "../../services/materialPurchaseService";
import type {
  MaterialPurchaseDto, DropdownOption,
} from "../../services/materialPurchaseService";
import { useSite } from "../../context/Sitecontext";

function MaterialPurchaseList() {
  const location = useLocation();
  const { selectedSite } = useSite();
  const siteId = selectedSite?.id ?? 0;

  const [records,    setRecords]    = useState<MaterialPurchaseDto[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [materials,  setMaterials]  = useState<DropdownOption[]>([]);
  const [suppliers,  setSuppliers]  = useState<DropdownOption[]>([]);
  const [brands,     setBrands]     = useState<DropdownOption[]>([]);

  const [dateFrom,   setDateFrom]   = useState("");
  const [dateTo,     setDateTo]     = useState("");
  const [materialId, setMaterialId] = useState("");
  const [supplierId, setSupplierId] = useState("");
  const [brandId,    setBrandId]    = useState("");

  const [loading,  setLoading]  = useState(true);
  const [error,    setError]    = useState("");

  const successMessage = (location.state as { success?: string })?.success;

  // Load dropdowns once
  useEffect(() => {
    getDropdownData()
      .then((dd) => {
        setMaterials(dd.materials);
        setSuppliers(dd.suppliers);
        setBrands(dd.brands);
      })
      .catch(() => {});
  }, []);

  const loadData = (
    from?: string, to?: string,
    mat?: string, sup?: string, brd?: string
  ) => {
    setLoading(true);
    setError("");
    getMaterialPurchases(
      siteId,
      from || undefined, to || undefined,
      mat ? Number(mat) : undefined,
      sup ? Number(sup) : undefined,
      brd ? Number(brd) : undefined
    )
      .then((res) => { setRecords(res.items); setTotalCount(res.totalCount); })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  };

  useEffect(() => { loadData(); }, [siteId]);

  const handleGo = (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    const today    = new Date().toISOString().split("T")[0];
    const fromDate = dateFrom ? new Date(dateFrom) : null;
    const toDate   = dateTo   ? new Date(dateTo)   : null;

    if (fromDate && toDate && fromDate > toDate) {
      setError("FROM DATE cannot be greater than TO DATE.");
      return;
    }
    if ((dateTo && dateTo > today) || (dateFrom && dateFrom > today)) {
      setError("FROM DATE and TO DATE cannot be in the future.");
      return;
    }
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

  const fmt = (d: string) =>
    new Date(d).toLocaleDateString("en-GB", {
      day: "2-digit", month: "short", year: "numeric",
    });

  const inputStyle: React.CSSProperties = {
    borderRadius: "8px", padding: "6px 10px",
    fontSize: "14px", width: "150px", border: "1px solid #ddd",
  };

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh" }}>
      <div style={{ padding: "30px 10px" }}>
        <div style={{
          background: "#fff", borderRadius: "16px", padding: "25px",
          boxShadow: "0 10px 30px rgba(0,0,0,0.2)", maxWidth: "1400px", margin: "auto",
        }}>

          {/* Success / Error */}
          {successMessage && (
            <div style={{ color: "#166534", textAlign: "center", marginBottom: "10px", fontWeight: 600 }}>
              {successMessage}
            </div>
          )}

          {/* Top bar */}
          <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", gap: "12px", marginBottom: "20px" }}>
            <div style={{
              background: "#1E3A5F", color: "#cbd5e1", padding: "6px 14px",
              borderRadius: "20px", fontSize: "13px", display: "inline-flex", alignItems: "center", gap: "8px",
            }}>
              Total Purchase
              <span style={{ background: "#F59E0B", color: "#fff", padding: "2px 10px", borderRadius: "999px", fontWeight: 700 }}>
                {totalCount}
              </span>
            </div>

            <h3 style={{ fontWeight: 700, color: "#1E293B", margin: 0, flex: 1, textAlign: "center" }}>
              Material Purchase
            </h3>

            <Link to="/material/add" style={{
              background: "#F59E0B", color: "#fff", fontWeight: 600,
              borderRadius: "10px", padding: "8px 18px", textDecoration: "none",
            }}>
              + Add New
            </Link>
          </div>

          {/* Filter row */}
          <form onSubmit={handleGo} style={{ display: "flex", alignItems: "center", gap: "10px", flexWrap: "wrap", marginBottom: "15px" }}>
            <label style={{ fontWeight: 600 }}>From:</label>
            <input type="date" value={dateFrom} onChange={(e) => setDateFrom(e.target.value)} style={inputStyle} />

            <label style={{ fontWeight: 600 }}>To:</label>
            <input type="date" value={dateTo} onChange={(e) => setDateTo(e.target.value)} style={inputStyle} />

            <label style={{ fontWeight: 600 }}>Material:</label>
            <select value={materialId} onChange={(e) => setMaterialId(e.target.value)} style={inputStyle}>
              <option value="">-- Select --</option>
              {materials.map((m) => <option key={m.id} value={m.id}>{m.name}</option>)}
            </select>

            <label style={{ fontWeight: 600 }}>Supplier:</label>
            <select value={supplierId} onChange={(e) => setSupplierId(e.target.value)} style={inputStyle}>
              <option value="">-- Select --</option>
              {suppliers.map((s) => <option key={s.id} value={s.id}>{s.name}</option>)}
            </select>

            <label style={{ fontWeight: 600 }}>Brand:</label>
            <select value={brandId} onChange={(e) => setBrandId(e.target.value)} style={inputStyle}>
              <option value="">-- Select --</option>
              {brands.map((b) => <option key={b.id} value={b.id}>{b.name}</option>)}
            </select>

            <button type="submit" style={{
              background: "#F59E0B", color: "#fff", borderRadius: "8px",
              padding: "6px 15px", border: "none", cursor: "pointer", fontWeight: 600,
            }}>
              Go
            </button>
          </form>

          {error && <p style={{ color: "red", fontWeight: 500 }}>{error}</p>}

          {/* Table */}
          {loading ? (
            <p style={{ textAlign: "center", padding: "30px" }}>Loading...</p>
          ) : records.length > 0 ? (
            <div style={{ overflowX: "auto" }}>
              <table style={{ width: "100%", borderCollapse: "collapse", whiteSpace: "nowrap" }}>
                <thead style={{ background: "#1E3A5F", color: "#fff" }}>
                  <tr>
                    {["Date","Supplier","Supplier Contact","Brand","Material","Unit Price","Quantity","Material Cost","Delivery Charge","Total Amount","Action"]
                      .map((h) => (
                        <th key={h} style={{ padding: "8px 12px", fontSize: "14px", textAlign: "center" }}>{h}</th>
                      ))}
                  </tr>
                </thead>
                <tbody>
                  {records.map((item, i) => {
                    const total = item.materialCost + item.deliveryCharge;
                    return (
                      <tr key={item.id}
                        style={{ background: i % 2 === 1 ? "#f8fafc" : "#fff", textAlign: "center" }}
                        onMouseEnter={(e) => (e.currentTarget.style.background = "#eef2f7")}
                        onMouseLeave={(e) => (e.currentTarget.style.background = i % 2 === 1 ? "#f8fafc" : "#fff")}
                      >
                        <td style={{ padding: "8px 12px" }}>{fmt(item.date)}</td>
                        <td style={{ padding: "8px 12px" }}>{item.supplierName}</td>
                        <td style={{ padding: "8px 12px" }}>{item.phoneNumber}</td>
                        <td style={{ padding: "8px 12px" }}>{item.brandName}</td>
                        <td style={{ padding: "8px 12px" }}>{item.materialName}</td>
                        <td style={{ padding: "8px 12px" }}>
                          {item.unitPrice.toFixed(2)} / <i>{item.unitOfMeasure}</i>
                        </td>
                        <td style={{ padding: "8px 12px" }}>
                          {item.quantity.toFixed(2)} / <i>{item.unitOfMeasure}</i>
                        </td>
                        <td style={{ padding: "8px 12px" }}>
                          <span style={{ background: "#FEF3C7", color: "#92400E", border: "1px solid #FDE68A", padding: "4px 12px", borderRadius: "8px" }}>
                            {item.materialCost.toFixed(2)}
                          </span>
                        </td>
                        <td style={{ padding: "8px 12px" }}>
                          <span style={{ background: "#FEF3C7", color: "#92400E", border: "1px solid #FDE68A", padding: "4px 12px", borderRadius: "8px" }}>
                            {item.deliveryCharge.toFixed(2)}
                          </span>
                        </td>
                        <td style={{ padding: "8px 12px" }}>
                          <span style={{ background: "#FEF3C7", color: "#92400E", border: "1px solid #FDE68A", padding: "4px 12px", borderRadius: "8px" }}>
                            ₹ {total.toFixed(2)}
                          </span>
                        </td>
                        <td style={{ padding: "8px 12px" }}>
                          <button
                            onClick={() => handleDelete(item.id)}
                            style={{ color: "#DC2626", background: "none", border: "none", cursor: "pointer", fontWeight: 500 }}
                          >
                            Delete
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
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

export default MaterialPurchaseList;
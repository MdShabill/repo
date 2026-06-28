// Path: src/components/SiteAdd.tsx
import { useEffect, useState }     from "react";
import { useNavigate, Link }       from "react-router-dom";
import { getDropdownData, getServiceProviders, createSite } from "../services/siteService";
import type { DropdownItem, ProviderOption } from "../services/siteService";

interface CheckboxGroupProps {
  label:       string;
  options:     ProviderOption[];
  selectedIds: number[];
  onChange:    (ids: number[]) => void;
}

function CheckboxDropdown({ label, options, selectedIds, onChange }: CheckboxGroupProps) {
  const [open, setOpen] = useState(false);
  const toggle = (id: number) =>
    onChange(selectedIds.includes(id) ? selectedIds.filter((x) => x !== id) : [...selectedIds, id]);

  return (
    <div style={{ position: "relative", marginBottom: "12px" }}>
      <button
        type="button"
        onClick={() => setOpen(!open)}
        style={{
          width: "100%", textAlign: "left", borderRadius: "10px",
          padding: "8px 12px", height: "40px", fontSize: "14px",
          border: "1px solid #ced4da", background: "#fff", cursor: "pointer",
        }}
      >
        {label} ▾
      </button>
      {open && (
        <div style={{
          position: "absolute", zIndex: 1000, background: "#fff",
          border: "1px solid #ddd", borderRadius: "8px", padding: "8px 12px",
          maxHeight: "250px", overflowY: "auto", width: "100%", boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
        }}>
          {options.length === 0 && <p style={{ color: "#888", fontSize: "13px" }}>No options</p>}
          {options.map((o) => (
            <div key={o.id} style={{ display: "flex", alignItems: "center", gap: "8px", padding: "4px 0" }}>
              <input
                type="checkbox"
                checked={selectedIds.includes(o.id)}
                onChange={() => toggle(o.id)}
                id={`${label}-${o.id}`}
              />
              <label htmlFor={`${label}-${o.id}`} style={{ fontSize: "14px", cursor: "pointer" }}>
                {o.name}
              </label>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

function SiteAdd() {
  const navigate = useNavigate();

  const [name,         setName]         = useState("");
  const [startedDate,  setStartedDate]  = useState(new Date().toISOString().slice(0, 10));
  const [siteStatusId, setSiteStatusId] = useState(0);
  const [note,         setNote]         = useState("");
  const [showAddress,  setShowAddress]  = useState(false);
  const [addressLine1, setAddressLine1] = useState("");
  const [addressTypeId,setAddressTypeId]= useState(0);
  const [countryId,    setCountryId]    = useState(0);
  const [pinCode,      setPinCode]      = useState("");

  const [statuses,     setStatuses]     = useState<DropdownItem[]>([]);
  const [addressTypes, setAddressTypes] = useState<DropdownItem[]>([]);
  const [countries,    setCountries]    = useState<DropdownItem[]>([]);

  const [masonIds,       setMasonIds]       = useState<number[]>([]);
  const [labourIds,      setLabourIds]      = useState<number[]>([]);
  const [electricianIds, setElectricianIds] = useState<number[]>([]);
  const [plumberIds,     setPlumberIds]     = useState<number[]>([]);
  const [painterIds,     setPainterIds]     = useState<number[]>([]);
  const [carpenterIds,   setCarpenterIds]   = useState<number[]>([]);
  const [tilerIds,       setTilerIds]       = useState<number[]>([]);

  const [masons,       setMasons]       = useState<ProviderOption[]>([]);
  const [labours,      setLabours]      = useState<ProviderOption[]>([]);
  const [electricians, setElectricians] = useState<ProviderOption[]>([]);
  const [plumbers,     setPlumbers]     = useState<ProviderOption[]>([]);
  const [painters,     setPainters]     = useState<ProviderOption[]>([]);
  const [carpenters,   setCarpenters]   = useState<ProviderOption[]>([]);
  const [tilers,       setTilers]       = useState<ProviderOption[]>([]);

  const [errorMessage, setErrorMessage] = useState("");
  const [submitting,   setSubmitting]   = useState(false);

  useEffect(() => {
    Promise.all([getDropdownData(), getServiceProviders()]).then(([dd, sp]) => {
      setStatuses(dd.statuses);
      setAddressTypes(dd.addressTypes);
      setCountries(dd.countries);
      setMasons(sp.masterMasons);
      setLabours(sp.labours);
      setElectricians(sp.electricians);
      setPlumbers(sp.plumbers);
      setPainters(sp.painters);
      setCarpenters(sp.carpenters);
      setTilers(sp.tilers);
    }).catch((err: Error) => setErrorMessage(err.message));
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) { setErrorMessage("Please enter Site Name"); return; }
    if (!siteStatusId) { setErrorMessage("Please Select Site Status"); return; }
    setSubmitting(true);
    try {
      await createSite({
        name, startedDate, siteStatusId, note,
        addressLine1: addressLine1 || undefined,
        addressTypeId: addressTypeId || undefined,
        countryId:    countryId    || undefined,
        pinCode:      pinCode ? Number(pinCode) : undefined,
        selectedMasterMasonIds:  masonIds,
        selectedElectricianIds:  electricianIds,
        selectedLabourIds:       labourIds,
        selectedPlumberIds:      plumberIds,
        selectedPainterIds:      painterIds,
        selectedCarpenterIds:    carpenterIds,
        selectedTilerIds:        tilerIds,
      });
      navigate("/sites", { state: { success: "Add New Site Successful" } });
    } catch (err) {
      setErrorMessage((err as Error).message);
    } finally {
      setSubmitting(false);
    }
  };

  const inputStyle: React.CSSProperties = {
    width: "100%", borderRadius: "10px", padding: "8px 12px",
    height: "40px", fontSize: "14px", border: "1px solid #ddd", boxSizing: "border-box",
  };
  const labelStyle: React.CSSProperties = {
    fontWeight: 600, marginBottom: "4px", fontSize: "14px", display: "block",
  };

  return (
    <div style={{ background: "linear-gradient(135deg, #1E3A5F, #0F172A)", minHeight: "100vh", padding: "30px" }}>
      <div style={{
        background: "#fff", borderRadius: "20px", padding: "35px",
        maxWidth: "900px", margin: "auto", boxShadow: "0 12px 30px rgba(0,0,0,0.15)",
      }}>
        <h3 style={{ textAlign: "center", fontWeight: 700, color: "#1E293B", marginBottom: "25px" }}>
          Add Site
        </h3>

        {errorMessage && (
          <div style={{
            background: "#fdecea", color: "#c62828", padding: "10px 14px",
            borderRadius: "8px", marginBottom: "15px", textAlign: "center",
          }}>
            <b>{errorMessage}</b>
          </div>
        )}

        <form onSubmit={handleSubmit}>
          {/* Row 1 */}
          <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "20px", marginBottom: "16px" }}>
            <div>
              <label style={labelStyle}>Created Date</label>
              <input type="date" value={startedDate} onChange={(e) => setStartedDate(e.target.value)} style={inputStyle} />
            </div>
            <div>
              <label style={labelStyle}>Site Name</label>
              <input value={name} onChange={(e) => setName(e.target.value)} placeholder="Enter site name" style={inputStyle} />
            </div>
          </div>

          {/* Row 2 */}
          <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "20px", marginBottom: "20px" }}>
            <div>
              <label style={labelStyle}>Site Status</label>
              <select value={siteStatusId} onChange={(e) => setSiteStatusId(Number(e.target.value))} style={inputStyle}>
                <option value={0}>-- SELECT STATUS --</option>
                {statuses.map((s) => <option key={s.id} value={s.id}>{s.name}</option>)}
              </select>
            </div>
            <div>
              <label style={labelStyle}>Note</label>
              <input value={note} onChange={(e) => setNote(e.target.value)} placeholder="Enter Note" style={inputStyle} />
            </div>
          </div>

          {/* Service Provider dropdowns — 2 column grid */}
          <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "12px", marginBottom: "20px" }}>
            <CheckboxDropdown label="Masons"       options={masons}       selectedIds={masonIds}       onChange={setMasonIds} />
            <CheckboxDropdown label="Labours"      options={labours}      selectedIds={labourIds}      onChange={setLabourIds} />
            <CheckboxDropdown label="Electricians" options={electricians} selectedIds={electricianIds} onChange={setElectricianIds} />
            <CheckboxDropdown label="Plumbers"     options={plumbers}     selectedIds={plumberIds}     onChange={setPlumberIds} />
            <CheckboxDropdown label="Painters"     options={painters}     selectedIds={painterIds}     onChange={setPainterIds} />
            <CheckboxDropdown label="Carpenters"   options={carpenters}   selectedIds={carpenterIds}   onChange={setCarpenterIds} />
            <CheckboxDropdown label="Tilers"       options={tilers}       selectedIds={tilerIds}       onChange={setTilerIds} />
          </div>

          {/* Address toggle */}
          <div style={{ textAlign: "center", marginBottom: "16px" }}>
            <button
              type="button"
              onClick={() => setShowAddress(!showAddress)}
              style={{
                background: "#1E3A5F", color: "#fff", borderRadius: "12px",
                padding: "8px 18px", border: "none", cursor: "pointer", fontWeight: 500,
              }}
            >
              {showAddress ? "Tap to Hide Address" : "Tap for Address"}
            </button>
          </div>

          {/* Address section */}
          {showAddress && (
            <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "16px", marginBottom: "20px" }}>
              <div>
                <label style={labelStyle}>Address</label>
                <input value={addressLine1} onChange={(e) => setAddressLine1(e.target.value)} style={inputStyle} />
              </div>
              <div>
                <label style={labelStyle}>Address Type</label>
                <select value={addressTypeId} onChange={(e) => setAddressTypeId(Number(e.target.value))} style={inputStyle}>
                  <option value={0}>-- Select --</option>
                  {addressTypes.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                </select>
              </div>
              <div>
                <label style={labelStyle}>Country Name</label>
                <select value={countryId} onChange={(e) => setCountryId(Number(e.target.value))} style={inputStyle}>
                  <option value={0}>-- Select --</option>
                  {countries.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
                </select>
              </div>
              <div>
                <label style={labelStyle}>Pin Code</label>
                <input value={pinCode} onChange={(e) => setPinCode(e.target.value)} style={inputStyle} />
              </div>
            </div>
          )}

          {/* Buttons */}
          <div style={{ display: "flex", justifyContent: "flex-end", gap: "10px", marginTop: "16px" }}>
            <Link to="/sites" style={{
              borderRadius: "12px", padding: "8px 20px", border: "1px solid #CBD5E1",
              color: "#64748B", fontWeight: 600, textDecoration: "none",
            }}>
              Cancel
            </Link>
            <button
              type="submit" disabled={submitting}
              style={{
                background: "#F59E0B", color: "#fff", borderRadius: "12px",
                padding: "8px 20px", border: "none", fontWeight: 600,
                cursor: submitting ? "not-allowed" : "pointer",
              }}
            >
              {submitting ? "Adding..." : "Add"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default SiteAdd;
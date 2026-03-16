import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  getSiteById,
  getServiceProviders,
  getDropdownData,
} from "../services/siteService";

import type {
  SiteEditDto,
  ServiceProviderResponse,
  DropdownResponse,
} from "../services/siteService";

import MultiSelectDropdown from "./MultiSelectDropdown";

const SiteEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [site, setSite] = useState<SiteEditDto | null>(null);
  const [providers, setProviders] = useState<ServiceProviderResponse | null>(
    null,
  );
  const [dropdowns, setDropdowns] = useState<DropdownResponse | null>(null);

  const [showAddress, setShowAddress] = useState(false);
  const [loading, setLoading] = useState(false);

  const fetchSite = async () => {
    if (!id) return;

    setLoading(true);

    try {
      const siteData = await getSiteById(Number(id));
      const providerData = await getServiceProviders();
      const dropdownData = await getDropdownData();

      setSite(siteData);
      setProviders(providerData);
      setDropdowns(dropdownData);
    } catch {
      alert("Site not found");
    }

    setLoading(false);
  };

  useEffect(() => {
    fetchSite();
  }, [id]);

  const cancelEdit = () => {
    navigate("/"); // redirect to Site List
  };

  if (loading || !site) {
    return (
      <div
        className="d-flex justify-content-center align-items-center"
        style={{ minHeight: "100vh", width: "100vw", background: "#f8f9fa" }}
      >
        <h4>Loading...</h4>
      </div>
    );
  }

  return (
    <div
      className="d-flex justify-content-center align-items-center"
      style={{ minHeight: "100vh", width: "100vw", background: "#f8f9fa" }}
    >
      <div
        className="card shadow-lg p-5"
        style={{ width: "650px", borderRadius: "12px" }}
      >
        <h2 className="text-center mb-4">Edit Site</h2>

        {/* Started Date */}

        <div className="mb-3">
          <label>
            <b>Started Date</b>
          </label>
          <input
            type="date"
            className="form-control"
            value={site.startedDate.slice(0, 10)}
            readOnly
          />
        </div>

        {/* Site Name */}

        <div className="mb-3">
          <label>
            <b>Site Name</b>
          </label>
          <input className="form-control" value={site.name} readOnly />
        </div>

        {/* Status Dropdown */}

        <div className="mb-3">
          <label>
            <b>Status</b>
          </label>

          <select
            className="form-control"
            value={site.status}
            onChange={(e) => setSite({ ...site, status: e.target.value })}
          >
            {dropdowns?.statuses.map((s) => (
              <option key={s.id} value={s.name}>
                {s.name}
              </option>
            ))}
          </select>
        </div>

        {/* SERVICE TYPES */}

        {providers && (
          <div className="row g-2 mb-3">
            <div className="col-md-6">
              <MultiSelectDropdown
                label="Masons"
                options={providers.masterMasons}
                selectedIds={site.masterMasonIds}
                onChange={(ids) => setSite({ ...site, masterMasonIds: ids })}
              />
            </div>

            <div className="col-md-6">
              <MultiSelectDropdown
                label="Labours"
                options={providers.labours}
                selectedIds={site.labourIds}
                onChange={(ids) => setSite({ ...site, labourIds: ids })}
              />
            </div>

            <div className="col-md-6">
              <MultiSelectDropdown
                label="Electricians"
                options={providers.electricians}
                selectedIds={site.electricianIds}
                onChange={(ids) => setSite({ ...site, electricianIds: ids })}
              />
            </div>

            <div className="col-md-6">
              <MultiSelectDropdown
                label="Plumbers"
                options={providers.plumbers}
                selectedIds={site.plumberIds}
                onChange={(ids) => setSite({ ...site, plumberIds: ids })}
              />
            </div>

            <div className="col-md-6">
              <MultiSelectDropdown
                label="Painters"
                options={providers.painters}
                selectedIds={site.painterIds}
                onChange={(ids) => setSite({ ...site, painterIds: ids })}
              />
            </div>

            <div className="col-md-6">
              <MultiSelectDropdown
                label="Carpenters"
                options={providers.carpenters}
                selectedIds={site.carpenterIds}
                onChange={(ids) => setSite({ ...site, carpenterIds: ids })}
              />
            </div>

            <div className="col-md-6">
              <MultiSelectDropdown
                label="Tilers"
                options={providers.tilers}
                selectedIds={site.tilerIds}
                onChange={(ids) => setSite({ ...site, tilerIds: ids })}
              />
            </div>
          </div>
        )}

        {/* ADDRESS */}

        <button
          className="btn btn-secondary mb-3"
          onClick={() => setShowAddress(!showAddress)}
        >
          {showAddress ? "Hide Address" : "Show Address"}
        </button>

        {showAddress && (
          <>
            <div className="mb-3">
              <label>
                <b>Address</b>
              </label>
              <textarea
                className="form-control"
                value={site.addressLine1 ?? ""}
                readOnly
              />
            </div>

            {/* Address Type Dropdown */}

            <div className="mb-3">
              <label>
                <b>Address Type</b>
              </label>

              <select
                className="form-control"
                value={site.addressTypes ?? ""}
                onChange={(e) =>
                  setSite({ ...site, addressTypes: e.target.value })
                }
              >
                {dropdowns?.addressTypes.map((a) => (
                  <option key={a.id} value={a.name}>
                    {a.name}
                  </option>
                ))}
              </select>
            </div>

            {/* Country Dropdown */}

            <div className="mb-3">
              <label>
                <b>Country</b>
              </label>

              <select
                className="form-control"
                value={site.countryName ?? ""}
                onChange={(e) =>
                  setSite({ ...site, countryName: e.target.value })
                }
              >
                {dropdowns?.countries.map((c) => (
                  <option key={c.id} value={c.name}>
                    {c.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="mb-3">
              <label>
                <b>Pin Code</b>
              </label>
              <input
                className="form-control"
                value={site.pinCode ?? ""}
                readOnly
              />
            </div>
          </>
        )}

        {/* ACTION BUTTONS */}

        <div className="d-flex gap-2">
          <button className="btn btn-primary">Update</button>

          <button className="btn btn-secondary" onClick={cancelEdit}>
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};

export default SiteEdit;

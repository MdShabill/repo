import { useState, useEffect } from "react";
import { addSite } from "../services/addSiteService";
import { getDropdownData } from "../services/dropdownService";

const SiteAdd = () => {
  const [startedDate, setStartedDate] = useState("");
  const [name, setName] = useState("");
  const [note, setNote] = useState("");

  const [siteStatusId, setSiteStatusId] = useState<number | undefined>();
  const [addressTypeId, setAddressTypeId] = useState<number | undefined>();
  const [countryId, setCountryId] = useState<number | undefined>();
  const [addressLine1, setAddressLine1] = useState("");
  const [pinCode, setPinCode] = useState<number | undefined>();

  const [statuses, setStatuses] = useState<any[]>([]);
  const [addressTypes, setAddressTypes] = useState<any[]>([]);
  const [countries, setCountries] = useState<any[]>([]);
  const [message, setMessage] = useState("");

  // Load dropdown on page load
  useEffect(() => {
    getDropdownData().then((data) => {
      setStatuses(data.statuses);
      setAddressTypes(data.addressTypes);
      setCountries(data.countries);
    });
  }, []);

  const handleSubmit = async () => {
    if (!name || !startedDate || !siteStatusId) {
      setMessage("Please fill all required fields");
      return;
    }

    try {
      const newSite = {
        name,
        startedDate,
        siteStatusId,
        note,
        addressLine1,
        addressTypeId,
        countryId,
        pinCode,
      };

      console.log("Sending:", newSite);

      const insertedId = await addSite(newSite);
      setMessage(`Site inserted successfully. Id: ${insertedId}`);
    } catch (error) {
      console.log(error);
      setMessage("Insert failed");
    }
  };

  return (
    <div
      className="d-flex justify-content-center align-items-center"
      style={{
        minHeight: "100vh",
        width: "100vw",
        background: "#f8f9fa",
      }}
    >
      <div
        className="card shadow-lg p-5"
        style={{ width: "650px", borderRadius: "12px" }}
      >
        <h3 className="text-center mb-4">Add New Site</h3>

        <div className="mb-3">
          <label>
            <b>Started Date</b>
          </label>
          <input
            type="date"
            className="form-control"
            value={startedDate}
            onChange={(e) => setStartedDate(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label>
            <b>Site Name</b>
          </label>
          <input
            type="text"
            className="form-control"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label>
            <b>Status</b>
          </label>
          <select
            className="form-control"
            value={siteStatusId ?? ""}
            onChange={(e) =>
              setSiteStatusId(
                e.target.value ? Number(e.target.value) : undefined,
              )
            }
          >
            <option value="">Select Status</option>
            {statuses.map((s) => (
              <option key={s.id} value={s.id}>
                {s.name}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-3">
          <label>
            <b>Address Line</b>
          </label>
          <input
            type="text"
            className="form-control"
            value={addressLine1}
            onChange={(e) => setAddressLine1(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label>
            <b>Address Type</b>
          </label>
          <select
            className="form-control"
            value={addressTypeId}
            onChange={(e) => setAddressTypeId(Number(e.target.value))}
          >
            <option value="">Select Address Type</option>
            {addressTypes.map((a) => (
              <option key={a.id} value={a.id}>
                {a.name}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-3">
          <label>
            <b>Country</b>
          </label>
          <select
            className="form-control"
            value={countryId}
            onChange={(e) => setCountryId(Number(e.target.value))}
          >
            <option value="">Select Country</option>
            {countries.map((c) => (
              <option key={c.id} value={c.id}>
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
            type="number"
            className="form-control"
            value={pinCode ?? ""}
            onChange={(e) => setPinCode(Number(e.target.value))}
          />
        </div>

        <div className="mb-3">
          <label>
            <b>Note</b>
          </label>
          <textarea
            className="form-control"
            value={note}
            onChange={(e) => setNote(e.target.value)}
          />
        </div>

        <div className="text-center">
          <button className="btn btn-success" onClick={handleSubmit}>
            Add
          </button>
        </div>

        {message && (
          <div className="mt-3 text-center text-primary">{message}</div>
        )}
      </div>
    </div>
  );
};

export default SiteAdd;

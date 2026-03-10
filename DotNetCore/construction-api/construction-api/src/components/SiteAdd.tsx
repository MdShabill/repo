// import { useState, useEffect } from "react";
// import { addSite } from "../services/addSiteService";
// import { getDropdownData } from "../services/dropdownService";
// import { getServiceProviders } from "../services/serviceProviderService";
// import CheckboxDropdown from "./CheckboxDropdown";

// const SiteAdd = () => {
//   const [startedDate, setStartedDate] = useState("");
//   const [name, setName] = useState("");
//   const [note, setNote] = useState("");

//   const [siteStatusId, setSiteStatusId] = useState<number>();
//   const [addressTypeId, setAddressTypeId] = useState<number>();
//   const [countryId, setCountryId] = useState<number>();
//   const [addressLine1, setAddressLine1] = useState("");
//   const [pinCode, setPinCode] = useState<number>();

//   const [showAddress, setShowAddress] = useState(false);

//   const [statuses, setStatuses] = useState<any[]>([]);
//   const [addressTypes, setAddressTypes] = useState<any[]>([]);
//   const [countries, setCountries] = useState<any[]>([]);

//   const [masterMasons, setMasterMasons] = useState<any[]>([]);
//   const [electricians, setElectricians] = useState<any[]>([]);
//   const [labours, setLabours] = useState<any[]>([]);
//   const [plumbers, setPlumbers] = useState<any[]>([]);
//   const [painters, setPainters] = useState<any[]>([]);
//   const [carpenters, setCarpenters] = useState<any[]>([]);
//   const [tilers, setTilers] = useState<any[]>([]);

//   const [selectedMasterMasonIds, setSelectedMasterMasonIds] = useState<
//     number[]
//   >([]);
//   const [selectedElectricianIds, setSelectedElectricianIds] = useState<
//     number[]
//   >([]);
//   const [selectedLabourIds, setSelectedLabourIds] = useState<number[]>([]);
//   const [selectedPlumberIds, setSelectedPlumberIds] = useState<number[]>([]);
//   const [selectedPainterIds, setSelectedPainterIds] = useState<number[]>([]);
//   const [selectedCarpenterIds, setSelectedCarpenterIds] = useState<number[]>(
//     [],
//   );
//   const [selectedTilerIds, setSelectedTilerIds] = useState<number[]>([]);

//   const [message, setMessage] = useState("");

//   useEffect(() => {
//     getDropdownData().then((data) => {
//       setStatuses(data.statuses);
//       setAddressTypes(data.addressTypes);
//       setCountries(data.countries);
//     });

//     getServiceProviders().then((data) => {
//       setMasterMasons(data.masterMasons);
//       setElectricians(data.electricians);
//       setLabours(data.labours);
//       setPlumbers(data.plumbers);
//       setPainters(data.painters);
//       setCarpenters(data.carpenters);
//       setTilers(data.tilers);
//     });
//   }, []);

//   const handleSubmit = async () => {
//     if (!name || !startedDate || !siteStatusId) {
//       setMessage("Please fill required fields");
//       return;
//     }

//     const newSite = {
//       name,
//       startedDate,
//       siteStatusId,
//       note,
//       addressLine1,
//       addressTypeId,
//       countryId,
//       pinCode,

//       selectedMasterMasonIds,
//       selectedElectricianIds,
//       selectedLabourIds,
//       selectedPlumberIds,
//       selectedPainterIds,
//       selectedCarpenterIds,
//       selectedTilerIds,
//     };

//     try {
//       const insertedId = await addSite(newSite);
//       setMessage(`Site inserted successfully. Id: ${insertedId}`);
//     } catch {
//       setMessage("Insert failed");
//     }
//   };

//   return (
//     <div
//       className="d-flex justify-content-center align-items-center"
//       style={{ minHeight: "100vh", width: "100vw", background: "#f8f9fa" }}
//     >
//       <div
//         className="card shadow-lg p-5"
//         style={{ width: "650px", borderRadius: "12px" }}
//       >
//         <h3 className="text-center mb-4">Add Site</h3>

//         <input
//           type="date"
//           className="form-control mb-3"
//           onChange={(e) => setStartedDate(e.target.value)}
//         />

//         <input
//           type="text"
//           placeholder="Site Name"
//           className="form-control mb-3"
//           onChange={(e) => setName(e.target.value)}
//         />

//         <select
//           className="form-control mb-3"
//           onChange={(e) => setSiteStatusId(Number(e.target.value))}
//         >
//           <option>Select Status</option>
//           {statuses.map((s) => (
//             <option key={s.id} value={s.id}>
//               {s.name}
//             </option>
//           ))}
//         </select>

//         <textarea
//           className="form-control mb-3"
//           placeholder="Note"
//           onChange={(e) => setNote(e.target.value)}
//         />

//         <div className="row">
//           <div className="col-md-6">
//             <CheckboxDropdown
//               label="Masons"
//               items={masterMasons}
//               selectedIds={selectedMasterMasonIds}
//               setSelectedIds={setSelectedMasterMasonIds}
//             />
//           </div>

//           <div className="col-md-6">
//             <CheckboxDropdown
//               label="Electricians"
//               items={electricians}
//               selectedIds={selectedElectricianIds}
//               setSelectedIds={setSelectedElectricianIds}
//             />
//           </div>

//           <div className="col-md-6">
//             <CheckboxDropdown
//               label="Labours"
//               items={labours}
//               selectedIds={selectedLabourIds}
//               setSelectedIds={setSelectedLabourIds}
//             />
//           </div>

//           <div className="col-md-6">
//             <CheckboxDropdown
//               label="Plumbers"
//               items={plumbers}
//               selectedIds={selectedPlumberIds}
//               setSelectedIds={setSelectedPlumberIds}
//             />
//           </div>

//           <div className="col-md-6">
//             <CheckboxDropdown
//               label="Painters"
//               items={painters}
//               selectedIds={selectedPainterIds}
//               setSelectedIds={setSelectedPainterIds}
//             />
//           </div>

//           <div className="col-md-6">
//             <CheckboxDropdown
//               label="Carpenters"
//               items={carpenters}
//               selectedIds={selectedCarpenterIds}
//               setSelectedIds={setSelectedCarpenterIds}
//             />
//           </div>

//           <div className="col-md-6">
//             <CheckboxDropdown
//               label="Tilers"
//               items={tilers}
//               selectedIds={selectedTilerIds}
//               setSelectedIds={setSelectedTilerIds}
//             />
//           </div>
//         </div>

//         <button
//           type="button"
//           className="btn btn-secondary mb-3"
//           onClick={() => setShowAddress(!showAddress)}
//         >
//           {showAddress ? "Tap to Hide Address" : "Tap for Address"}
//         </button>

//         {showAddress && (
//           <>
//             <input
//               type="text"
//               placeholder="Address Line"
//               className="form-control mb-3"
//               onChange={(e) => setAddressLine1(e.target.value)}
//             />

//             <select
//               className="form-control mb-3"
//               onChange={(e) => setAddressTypeId(Number(e.target.value))}
//             >
//               <option>Select Address Type</option>
//               {addressTypes.map((a) => (
//                 <option key={a.id} value={a.id}>
//                   {a.name}
//                 </option>
//               ))}
//             </select>

//             <select
//               className="form-control mb-3"
//               onChange={(e) => setCountryId(Number(e.target.value))}
//             >
//               <option>Select Country</option>
//               {countries.map((c) => (
//                 <option key={c.id} value={c.id}>
//                   {c.name}
//                 </option>
//               ))}
//             </select>

//             <input
//               type="number"
//               placeholder="Pin Code"
//               className="form-control mb-3"
//               onChange={(e) => setPinCode(Number(e.target.value))}
//             />
//           </>
//         )}

//         <button className="btn btn-success w-100" onClick={handleSubmit}>
//           Add
//         </button>

//         {message && <p className="text-center mt-3">{message}</p>}
//       </div>
//     </div>
//   );
// };

// export default SiteAdd;

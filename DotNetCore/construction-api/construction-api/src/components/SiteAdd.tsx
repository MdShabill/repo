// import { useState } from "react";
// import { addSite } from "../services/addSiteService";

// const SiteAdd = () => {
//   const [startedDate, setStartedDate] = useState("");
//   const [name, setName] = useState("");
//   const [status, setStatus] = useState("");
//   const [note, setNote] = useState("");
//   const [message, setMessage] = useState("");

//   const handleSubmit = async () => {
//     try {
//       const newSite = {
//         name,
//         startedDate,
//         siteStatusId: 1,
//         status,
//         note,
//       };

//       const insertedId = await addSite(newSite);
//       setMessage(`Site inserted successfully. Id: ${insertedId}`);

//       // reset form
//       setStartedDate("");
//       setName("");
//       setStatus("");
//       setNote("");
//     } catch {
//       setMessage("Insert failed");
//     }
//   };

//   return (
//     <div
//       className="d-flex justify-content-center align-items-center"
//       style={{
//         minHeight: "100vh",
//         width: "100vw",
//         background: "#f8f9fa",
//       }}
//     >
//       <div
//         className="card shadow-lg p-5"
//         style={{ width: "650px", borderRadius: "12px" }}
//       >
//         <h3 className="text-center mb-4">Add New Site</h3>

//         <div className="mb-3">
//           <label>Started Date</label>
//           <input
//             type="date"
//             className="form-control"
//             value={startedDate}
//             onChange={(e) => setStartedDate(e.target.value)}
//           />
//         </div>

//         <div className="mb-3">
//           <label>Site Name</label>
//           <input
//             type="text"
//             className="form-control"
//             value={name}
//             onChange={(e) => setName(e.target.value)}
//           />
//         </div>

//         <div className="mb-3">
//           <label>Status</label>
//           <input
//             type="text"
//             className="form-control"
//             value={status}
//             onChange={(e) => setStatus(e.target.value)}
//           />
//         </div>

//         <div className="mb-3">
//           <label>Note</label>
//           <textarea
//             className="form-control"
//             value={note}
//             onChange={(e) => setNote(e.target.value)}
//           />
//         </div>

//         <div className="text-center">
//           <button className="btn btn-success" onClick={handleSubmit}>
//             Add
//           </button>
//         </div>

//         {message && (
//           <div className="mt-3 text-center text-primary">{message}</div>
//         )}
//       </div>
//     </div>
//   );
// };

// export default SiteAdd;

// import { useEffect, useState } from "react";
// import { useParams, useNavigate } from "react-router-dom";
// import { getSiteById } from "../services/getSiteService";
// import type { SiteDto } from "../services/getSiteService";

// const SiteDetail = () => {
//   const { id } = useParams();
//   const navigate = useNavigate();

//   const [site, setSite] = useState<SiteDto | null>(null);
//   const [loading, setLoading] = useState(true);
//   const [error, setError] = useState("");

//   useEffect(() => {
//     const fetchSite = async () => {
//       try {
//         if (id) {
//           const data = await getSiteById(Number(id));
//           setSite(data);
//         }
//       } catch {
//         setError("Failed to load site detail");
//       } finally {
//         setLoading(false);
//       }
//     };

//     fetchSite();
//   }, [id]);

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
//         style={{ width: "900px", borderRadius: "12px" }}
//       >
//         <h3 className="text-center mb-4">Site Detail</h3>

//         {loading && <h5 className="text-center">Loading...</h5>}
//         {error && <h5 className="text-danger text-center">{error}</h5>}

//         {site && (
//           <>
//             <div className="mb-3">
//               <strong>Name:</strong> {site.name}
//             </div>

//             <div className="mb-3">
//               <strong>Started Date:</strong>{" "}
//               {new Date(site.startedDate).toLocaleDateString()}
//             </div>

//             <div className="mb-3">
//               <strong>Status:</strong> {site.status}
//             </div>

//             <div className="mb-3">
//               <strong>Address:</strong> {site.addressLine1 ?? "---"}
//             </div>

//             <div className="mb-3">
//               <strong>Address Type:</strong> {site.addressTypes ?? "---"}
//             </div>

//             <div className="mb-3">
//               <strong>Country:</strong> {site.countryName ?? "---"}
//             </div>

//             <div className="mb-3">
//               <strong>Pin Code:</strong> {site.pinCode ?? "---"}
//             </div>

//             <div className="text-center mt-4">
//               <button className="btn btn-primary" onClick={() => navigate("/")}>
//                 Back
//               </button>
//             </div>
//           </>
//         )}
//       </div>
//     </div>
//   );
// };

// export default SiteDetail;

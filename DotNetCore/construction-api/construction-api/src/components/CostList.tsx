// import { useEffect, useState } from "react";
// import { getStaticCosts } from "../services/costService";
// import type { StaticCost } from "../services/costService";

// const CostList = () => {
//   const [costData, setCostData] = useState<StaticCost[]>([]);
//   const [loading, setLoading] = useState(true);
//   const [error, setError] = useState<string | null>(null);

//   useEffect(() => {
//     const fetchData = async () => {
//       try {
//         const data = await getStaticCosts();
//         console.log("API Data:", data);
//         setCostData(data);
//       } catch (err) {
//         setError("Failed to load data");
//       } finally {
//         setLoading(false);
//       }
//     };

//     fetchData();
//   }, []);

//   if (loading) return <p>Loading construction data...</p>;
//   if (error) return <p>{error}</p>;

//   return (
//     <div style={{ padding: "20px" }}>
//       <h2>Construction Cost Data</h2>

//       {costData.map((item) => (
//         <div key={item.id} style={{ marginBottom: "10px" }}>
//           <strong>{item.name}</strong> - ₹{item.cost} -{" "}
//           {new Date(item.date).toLocaleDateString()}
//         </div>
//       ))}
//     </div>
//   );
// };

// export default CostList;

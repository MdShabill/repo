// export interface StaticCost {
//   id: number;
//   name: string;
//   cost: number;
//   date: string;
// }

// const API_URL =
//   "https://constructionapp1.azurewebsites.net/api/CostMasterAPI/GetStaticCost";

// export const getStaticCosts = async (): Promise<StaticCost[]> => {
//   const response = await fetch(API_URL);

//   if (!response.ok) {
//     throw new Error("Failed to fetch cost data");
//   }

//   return response.json();
// };
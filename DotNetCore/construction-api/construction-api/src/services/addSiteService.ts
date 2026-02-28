// export interface SiteCreateDto {
//   startedDate: string;
//   name: string;
//   status: string;
// }

// const ADD_API_URL =
//   "https://localhost:7036/api/SiteAPI/add";

// export const addSite = async (
//   site: SiteCreateDto
// ): Promise<number> => {
//   const response = await fetch(ADD_API_URL, {
//     method: "POST",
//     headers: {
//       "Content-Type": "application/json",
//     },
//     body: JSON.stringify(site),
//   });

//   if (!response.ok) {
//     throw new Error("Failed to add site");
//   }

//   return response.json();
// };
// export interface SiteDto {
//   id: number;
//   name: string;
//   startedDate: string;
//   status: string;
//   //note: string | null;

//   addressLine1: string;
//   addressTypes: string;
//   countryName: string;
//   pinCode: string;
// }

// /* ================= GET ALL SITE ================= */

// const GET_ALL_URL =
//   "https://localhost:7036/api/SiteAPI/GetAllSites";

// export const getAllSites = async (): Promise<SiteDto[]> => {
//   const response = await fetch(GET_ALL_URL);

//   if (!response.ok) {
//     throw new Error("Failed to fetch sites");
//   }

//   return response.json();
// };

// /* ================= GET SITE BY ID ================= */

// const GET_BY_ID_URL =
//   "https://localhost:7036/api/SiteAPI/select-site";

// export const getSiteById = async (id: number): Promise<SiteDto> => {
//   const response = await fetch(`${GET_BY_ID_URL}?id=${id}`);

//   if (!response.ok) {
//     throw new Error("Failed to fetch site detail");
//   }

//   return response.json();
// };
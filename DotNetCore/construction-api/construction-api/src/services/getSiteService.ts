export interface SiteDto {
  id: number;
  siteId: number;
  name: string;
  startedDate: string;
  siteStatusId: number;
  status: string;
  note: string | null;
}

const GET_ALL_URL =
  "https://localhost:7036/api/SiteAPI/GetAllSites";

export const getAllSites = async (): Promise<SiteDto[]> => {
  const response = await fetch(GET_ALL_URL);

  if (!response.ok) {
    throw new Error("Failed to fetch sites");
  }

  return response.json();
};
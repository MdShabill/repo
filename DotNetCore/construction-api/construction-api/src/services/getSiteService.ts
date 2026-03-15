export interface SiteDto {
  id: number;
  name: string;
  startedDate: string;
  siteStatusId: number;
  status: string;
  addressLine1?: string;
  addressTypes?: string;
  countryName?: string;
  pinCode?: number;
}

const API_BASE = "https://constructionapp1.azurewebsites.net/api/SiteAPI";

export const getAllSites = async (): Promise<SiteDto[]> => {
  const response = await fetch(`${API_BASE}/GetAllSites`);

  if (!response.ok) {
    throw new Error("Failed to fetch sites");
  }

  return await response.json();
};

export const getSiteById = async (id: number) => {
  const response = await fetch(`${API_BASE}/select-site?id=${id}`);

  if (!response.ok) {
    throw new Error("Site not found");
  }

  return await response.json();
};

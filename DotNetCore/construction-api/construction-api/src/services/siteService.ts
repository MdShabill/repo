export interface SiteEditDto {
  id: number;
  startedDate: string;
  name: string;
  status: string;
}

const API_URL =
  "https://constructionapp1.azurewebsites.net/api/SiteAPI/edit/";

export const getSiteById = async (id: number): Promise<SiteEditDto> => {
  const response = await fetch(`${API_URL}${id}`);

  if (!response.ok) {
    throw new Error("Failed to fetch site");
  }

  return response.json();
};
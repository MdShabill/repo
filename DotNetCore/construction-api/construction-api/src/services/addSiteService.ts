export interface SiteCreateDto {
  name: string;
  startedDate: string;
  siteStatusId: number;
  note: string;

  addressLine1?: string;
  addressTypeId?: number;
  countryId?: number;
  pinCode?: number;
}

const ADD_API_URL =
  "https://localhost:7036/api/SiteAPI/add";

export const addSite = async (
  site: SiteCreateDto
): Promise<number> => {
  const response = await fetch(ADD_API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(site),
  });

  if (!response.ok) {
  const errorText = await response.text();
  console.error("Server error:", errorText);
  throw new Error(errorText);
}

  const data = await response.json();
  return data.siteId;
};
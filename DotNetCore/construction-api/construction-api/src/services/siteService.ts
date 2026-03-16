export interface ProviderOption {
  id: number;
  name: string;
}

export interface ServiceProviderResponse {
  masterMasons: ProviderOption[];
  electricians: ProviderOption[];
  labours: ProviderOption[];
  plumbers: ProviderOption[];
  painters: ProviderOption[];
  carpenters: ProviderOption[];
  tilers: ProviderOption[];
}

export interface SiteEditDto {
  id: number;
  startedDate: string;
  name: string;
  status: string;

  addressLine1?: string;
  addressTypes?: string;
  countryName?: string;
  pinCode?: number;

  labourIds: number[];
  masterMasonIds: number[];
  electricianIds: number[];
  plumberIds: number[];
  painterIds: number[];
  carpenterIds: number[];
  tilerIds: number[];
}

const BASE = "https://localhost:7036/api/SiteAPI";

export const getSiteById = async (id: number): Promise<SiteEditDto> => {

  const res = await fetch(`${BASE}/edit/${id}`);

  if (!res.ok) throw new Error("Failed to fetch site");

  return res.json();
};

export const getServiceProviders = async (): Promise<ServiceProviderResponse> => {

  const res = await fetch(`${BASE}/service-providers`);

  if (!res.ok) throw new Error("Failed to fetch providers");

  return res.json();
};




/* ================================
   NEW CODE FOR DROPDOWN DATA
================================ */

export interface DropdownItem {
  id: number;
  name: string;
}

export interface DropdownResponse {
  statuses: DropdownItem[];
  addressTypes: DropdownItem[];
  countries: DropdownItem[];
}

export const getDropdownData = async (): Promise<DropdownResponse> => {

  const res = await fetch(`${BASE}/dropdown-data`);

  if (!res.ok) throw new Error("Failed to fetch dropdown data");

  return res.json();
};
// Path: src/services/siteService.ts
const BASE = "https://localhost:7036/api/SiteAPI";

export interface SiteDropdownDto   { id: number; name: string; }
export interface DropdownItem      { id: number; name: string; }
export interface ProviderOption    { id: number; name: string; }

export interface SiteListDto {
  id:           number;
  name:         string;
  startedDate:  string;
  siteStatusId: number;
  status?:      string;
  addressLine1?: string;
  addressTypes?: string;
  countryName?:  string;
  pinCode?:      number;
}

export interface SiteEditDto {
  id:            number;
  name:          string;
  startedDate:   string;
  status?:       string;
  note?:         string;
  addressLine1?: string;
  addressTypes?: string;
  countryName?:  string;
  pinCode?:      number;
  masterMasonIds:  number[];
  electricianIds:  number[];
  labourIds:       number[];
  plumberIds:      number[];
  painterIds:      number[];
  carpenterIds:    number[];
  tilerIds:        number[];
}

export interface SiteCreateDto {
  name:          string;
  startedDate:   string;
  siteStatusId:  number;
  note?:         string;
  addressLine1?: string;
  addressTypeId?: number;
  countryId?:    number;
  pinCode?:      number;
  selectedMasterMasonIds:  number[];
  selectedElectricianIds:  number[];
  selectedLabourIds:       number[];
  selectedPlumberIds:      number[];
  selectedPainterIds:      number[];
  selectedCarpenterIds:    number[];
  selectedTilerIds:        number[];
}

export interface DropdownResponse {
  statuses:      DropdownItem[];
  addressTypes:  DropdownItem[];
  countries:     DropdownItem[];
}

export interface ServiceProviderResponse {
  masterMasons: ProviderOption[];
  electricians: ProviderOption[];
  labours:      ProviderOption[];
  plumbers:     ProviderOption[];
  painters:     ProviderOption[];
  carpenters:   ProviderOption[];
  tilers:       ProviderOption[];
}

// Navbar site dropdown
export const getNavbarSites = async (): Promise<SiteDropdownDto[]> => {
  const res = await fetch(`${BASE}/GetAllSites`);
  if (!res.ok) throw new Error("Failed to fetch sites");
  return res.json();
};

// Site list
export const getAllSites = async (): Promise<SiteListDto[]> => {
  const res = await fetch(`${BASE}/GetAllSites`);
  if (!res.ok) throw new Error("Failed to fetch sites");
  return res.json();
};

// Site by id for edit
export const getSiteById = async (id: number): Promise<SiteEditDto> => {
  const res = await fetch(`${BASE}/edit/${id}`);
  if (!res.ok) throw new Error("Site not found");
  return res.json();
};

// Dropdowns — status, address type, country
export const getDropdownData = async (): Promise<DropdownResponse> => {
  const res = await fetch(`${BASE}/dropdown-data`);
  if (!res.ok) throw new Error("Failed to fetch dropdown data");
  return res.json();
};

// All service providers grouped by type
export const getServiceProviders = async (): Promise<ServiceProviderResponse> => {
  const res = await fetch(`${BASE}/service-providers`);
  if (!res.ok) throw new Error("Failed to fetch service providers");
  return res.json();
};

// Create site
export const createSite = async (payload: SiteCreateDto): Promise<number> => {
  const res = await fetch(`${BASE}/add`, {
    method:  "POST",
    headers: { "Content-Type": "application/json" },
    body:    JSON.stringify(payload),
  });
  if (!res.ok) {
    const data = await res.json().catch(() => ({}));
    throw new Error((data as any).message || "Failed to create site");
  }
  const data = await res.json();
  return data.siteId;
};

// Update site
export const updateSite = async (payload: SiteCreateDto & { id: number }): Promise<void> => {
  const res = await fetch(`${BASE}/update`, {
    method:  "POST",
    headers: { "Content-Type": "application/json" },
    body:    JSON.stringify(payload),
  });
  if (!res.ok) {
    const data = await res.json().catch(() => ({}));
    throw new Error((data as any).message || "Failed to update site");
  }
};

// Delete site
export const deleteSite = async (siteId: number): Promise<void> => {
  const res = await fetch(`${BASE}/${siteId}`, { method: "DELETE" });
  if (!res.ok) throw new Error("Failed to delete site");
};
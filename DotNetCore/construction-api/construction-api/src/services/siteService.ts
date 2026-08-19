// Path: src/services/siteService.ts
const BASE = "https://localhost:7036/api/SiteAPI";

export interface SiteDropdownDto   { id: number; name: string; }
export interface DropdownItem      { id: number; name: string; }
export interface ProviderOption    { id: number; name: string; }

// NEW
export interface ScopeMasterOption { id: number; name: string; }
export interface ScopeStatusOption { id: number; name: string; }

export interface SiteScopeVm {
  id:                number;
  siteScopeMasterId: number;
  scopeName:         string;
  scopeStatusId:     number;
  statusName:        string;
  remarks?:          string;
  completedDate?:    string;
}

export interface ScopeItemDto {
  siteScopeMasterId: number;
  scopeStatusId:     number;
  remarks?:          string;
}

export interface SiteListDto {
  id:                     number;
  name:                   string;
  contactName?:           string;
  contactNumber?:         string;
  startedDate:            string;
  siteStatusId?:          number;
  status?:                string;
  addressLine1?:          string;
  addressTypes?:          string;
  countryName?:           string;
  pinCode?:               number;
  expectedBudget?:        number;       // NEW
  expectedCompletionDate?: string;      // NEW
}

export interface SiteEditDto {
  id:                     number;
  name:                   string;
  contactName?:           string;
  contactNumber?:         string;
  startedDate:            string;
  status?:                string;
  siteStatusId?:          number;
  addressLine1?:          string;
  addressTypes?:          string;
  countryName?:           string;
  pinCode?:               number;
  expectedBudget?:        number;       // NEW
  expectedCompletionDate?: string;      // NEW
  masterMasonIds:         number[];
  electricianIds:         number[];
  labourIds:              number[];
  plumberIds:             number[];
  painterIds:             number[];
  carpenterIds:           number[];
  tilerIds:               number[];
  scopes:                 SiteScopeVm[];  // NEW
}

export interface SiteCreateDto {
  name:                   string;
  contactName?:           string;
  contactNumber?:         string;
  startedDate:            string;
  siteStatusId:           number;
  note?:                  string;
  addressLine1?:          string;
  addressTypeId?:         number;
  countryId?:             number;
  pinCode?:               number;
  expectedBudget?:        number;       // NEW
  expectedCompletionDate?: string;      // NEW
  selectedMasterMasonIds: number[];
  selectedElectricianIds: number[];
  selectedLabourIds:      number[];
  selectedPlumberIds:     number[];
  selectedPainterIds:     number[];
  selectedCarpenterIds:   number[];
  selectedTilerIds:       number[];
  selectedScopes:         ScopeItemDto[];  // NEW
}

export interface DropdownResponse {
  statuses:      DropdownItem[];
  addressTypes:  DropdownItem[];
  countries:     DropdownItem[];
  scopeMasters:  ScopeMasterOption[];   // NEW
  scopeStatuses: ScopeStatusOption[];   // NEW
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

export const getNavbarSites = async (): Promise<SiteDropdownDto[]> => {
  const res = await fetch(`${BASE}/GetAllSites`);
  if (!res.ok) throw new Error("Failed to fetch sites");
  return res.json();
};

export const getAllSites = async (): Promise<SiteListDto[]> => {
  const res = await fetch(`${BASE}/GetAllSites`);
  if (!res.ok) throw new Error("Failed to fetch sites");
  return res.json();
};

export const getSiteById = async (id: number): Promise<SiteEditDto> => {
  const res = await fetch(`${BASE}/edit/${id}`);
  if (!res.ok) throw new Error("Site not found");
  return res.json();
};

export const getDropdownData = async (): Promise<DropdownResponse> => {
  const res = await fetch(`${BASE}/dropdown-data`);
  if (!res.ok) throw new Error("Failed to fetch dropdown data");
  return res.json();
};

export const getServiceProviders =
  async (): Promise<ServiceProviderResponse> => {
    const res = await fetch(`${BASE}/service-providers`);
    if (!res.ok) throw new Error("Failed to fetch service providers");
    return res.json();
  };

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

export const updateSite = async (
  payload: SiteCreateDto & { id: number }
): Promise<void> => {
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

export const deleteSite = async (siteId: number): Promise<void> => {
  const res = await fetch(`${BASE}/${siteId}`, { method: "DELETE" });
  if (!res.ok) throw new Error("Failed to delete site");
};

export const updateScopeStatus = async (
  siteScopeId:   number,
  scopeStatusId: number,
  remarks?:      string
): Promise<void> => {
  const res = await fetch(`${BASE}/scope-status`, {
    method:  "PATCH",
    headers: { "Content-Type": "application/json" },
    body:    JSON.stringify({ siteScopeId, scopeStatusId, remarks }),
  });
  if (!res.ok) throw new Error("Failed to update scope status");
};
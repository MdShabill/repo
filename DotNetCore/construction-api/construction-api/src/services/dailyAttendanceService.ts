// Path: src/services/dailyAttendanceService.ts
const BASE = "https://localhost:7036/api/DailyAttendanceAPI";

export interface DailyAttendanceDto {
  id:               number;
  date:             string;
  serviceTypeId:    number;
  name:             string;   // service type name
  serviceProviderId: number;
  serviceProviderName: string;
  totalWorker:      number;
  amountPerWorker:  number;
  totalAmount:      number;
  notes?:           string;
}

export interface AddDailyAttendanceDto {
  date:               string;
  serviceTypeId:      number;
  serviceProviderId:  number;
  totalWorker:        number;
  notes?:             string;
}

export interface ServiceTypeOption    { id: number; name: string; }
export interface ServiceProviderOption { serviceProviderId: number; serviceProviderName: string; }
export interface ServiceTypeCost       { serviceTypeId: number; serviceTypeName: string; cost: number; }

export interface DropdownDataResponse {
  serviceTypes:     ServiceTypeOption[];
  serviceTypeCosts: ServiceTypeCost[];
}

export interface CostByServiceTypeResponse {
  cost:            number;
  serviceProviders: ServiceProviderOption[];
}

function makeHeaders(siteId: number): HeadersInit {
  return {
    "Content-Type": "application/json",
    "X-Site-Id":    String(siteId),
  };
}

// GET /api/DailyAttendanceAPI?dateFrom=&dateTo=
export const getDailyAttendances = async (
  siteId: number,
  dateFrom?: string,
  dateTo?:   string
): Promise<DailyAttendanceDto[]> => {
  const params = new URLSearchParams();
  if (dateFrom) params.append("dateFrom", dateFrom);
  if (dateTo)   params.append("dateTo", dateTo);
  const query = params.toString() ? `?${params.toString()}` : "";

  const res = await fetch(`${BASE}${query}`, { headers: makeHeaders(siteId) });
  if (!res.ok) {
    const err = await res.json().catch(() => ({}));
    throw new Error((err as any).message || "Failed to load attendance records");
  }
  return res.json();
};

// GET /api/DailyAttendanceAPI/dropdown-data
export const getDropdownData = async (siteId: number): Promise<DropdownDataResponse> => {
  const res = await fetch(`${BASE}/dropdown-data`, { headers: makeHeaders(siteId) });
  if (!res.ok) throw new Error("Failed to load dropdown data");
  return res.json();
};

// GET /api/DailyAttendanceAPI/GetCostByServiceType?serviceTypeId=3
export const getCostByServiceType = async (
  siteId:        number,
  serviceTypeId: number
): Promise<CostByServiceTypeResponse> => {
  const res = await fetch(
    `${BASE}/GetCostByServiceType?serviceTypeId=${serviceTypeId}`,
    { headers: makeHeaders(siteId) }
  );
  if (!res.ok) throw new Error("Failed to fetch cost/providers");
  return res.json();
};

// POST /api/DailyAttendanceAPI
export const addDailyAttendance = async (
  siteId:  number,
  payload: AddDailyAttendanceDto
): Promise<void> => {
  const res = await fetch(BASE, {
    method:  "POST",
    headers: makeHeaders(siteId),
    body:    JSON.stringify(payload),
  });
  if (!res.ok) {
    const data = await res.json().catch(() => ({}));
    throw new Error((data as any).message || "Failed to add attendance");
  }
};
// Path: src/services/costMasterService.ts
const BASE = "https://localhost:7036/api/CostMasterAPI";

export interface ServiceTypeOption  { id: number; name: string; }
export interface CostMasterDto      { id: number; serviceTypeId: number; name: string; cost: number; date: string; }
export interface AddCostMasterDto   { serviceTypeId: number; cost: number; date: string; }

// New shape returned by Index endpoint
export interface CostMasterIndexResponse {
  items:                 CostMasterDto[];
  serviceTypes:          ServiceTypeOption[];
  selectedServiceTypeId: number;
}

// Every request must carry X-Site-Id header (replaces MVC session)
function makeHeaders(siteId: number): HeadersInit {
  return {
    "Content-Type": "application/json",
    "X-Site-Id":    String(siteId),
  };
}

// GET /api/CostMasterAPI  OR  /api/CostMasterAPI?serviceTypeId=3
// Returns items + serviceTypes together — no separate service-types call needed
export const getCostMasters = async (
  siteId:        number,
  serviceTypeId?: number
): Promise<CostMasterIndexResponse> => {
  const url = serviceTypeId ? `${BASE}?serviceTypeId=${serviceTypeId}` : BASE;
  const res = await fetch(url, { headers: makeHeaders(siteId) });
  if (!res.ok) {
    const err = await res.json().catch(() => ({}));
    throw new Error((err as any).message || "Failed to load cost masters");
  }
  return await res.json();
};

// GET /api/CostMasterAPI/GetActiveCost?serviceTypeId=3
export const getActiveCost = async (
  siteId:        number,
  serviceTypeId: number
): Promise<CostMasterDto | null> => {
  const res = await fetch(
    `${BASE}/GetActiveCost?serviceTypeId=${serviceTypeId}`,
    { headers: makeHeaders(siteId) }
  );
  if (res.status === 404) return null;
  if (!res.ok)            return null;
  return await res.json();
};

// POST /api/CostMasterAPI  (NOT /Add — controller uses [HttpPost] with no route template)
export const addCostMaster = async (
  siteId:  number,
  payload: AddCostMasterDto
): Promise<void> => {
  const res = await fetch(BASE, {
    method:  "POST",
    headers: makeHeaders(siteId),
    body:    JSON.stringify(payload),
  });
  if (!res.ok) {
    const data = await res.json().catch(() => ({}));
    throw new Error((data as any).message || "Failed to add cost master");
  }
};

export async function getCostMasterById(
siteId:number,
id:number)
{
const res = await fetch(`${BASE}/${id}`,{headers:makeHeaders(siteId)});

return await res.json();}

export async function updateCostMaster(
siteId:number,
id:number,
payload:any)
{
const res=await fetch(`${BASE}/${id}`,{method:"PUT",headers:makeHeaders(siteId),body:JSON.stringify(payload)});

if(!res.ok){

const e=await res.json();
throw new Error(
e.message);}}

export async function deleteCostMaster(
siteId:number,
id:number)
{

const res=await fetch(`${BASE}/${id}`,{method:"DELETE",headers:makeHeaders(siteId)});
if(!res.ok){

const e=await res.json();
throw new Error(
e.message);}}
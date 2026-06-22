const BASE = "https://localhost:7036/api/CostMasterAPI";

export interface ServiceTypeOption {
  id: number;
  name: string;
}

export interface CostMasterDto {
  id: number;
  serviceTypeId: number;
  name: string;
  cost: number;
  date: string;
}

export interface AddCostMasterDto {
  serviceTypeId: number;
  cost: number;
  date: string;
}

export const getServiceTypes = async (): Promise<ServiceTypeOption[]> => {
  const res = await fetch(`${BASE}/service-types`);
  if (!res.ok) throw new Error("Failed to load service types");
  return res.json();
};

export const getCostMasters = async (serviceTypeId?: number): Promise<CostMasterDto[]> => {
  const url = serviceTypeId ? `${BASE}?serviceTypeId=${serviceTypeId}` : BASE;
  const res = await fetch(url);
  if (res.status === 404) return [];
  if (!res.ok) throw new Error("Failed to load cost master list");
  return res.json();
};

export const getActiveCost = async (serviceTypeId: number): Promise<CostMasterDto | null> => {
  const res = await fetch(`${BASE}/GetActiveCost?serviceTypeId=${serviceTypeId}`);
  if (res.status === 404) return null;
  if (!res.ok) throw new Error("Failed to load active cost");
  return res.json();
};

export const addCostMaster = async (payload: AddCostMasterDto): Promise<void> => {
  const res = await fetch(`${BASE}/Add`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || "Failed to add cost master");
  }
};
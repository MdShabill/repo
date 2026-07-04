// Path: src/services/materialPurchaseService.ts
const BASE = "https://localhost:7036/api/MaterialPurchaseAPI";

export interface MaterialPurchaseDto {
  id:             number;
  date:           string;
  supplierName:   string;
  phoneNumber:    string;
  brandName:      string;
  materialName:   string;
  unitPrice:      number;
  unitOfMeasure:  string;
  quantity:       number;
  materialCost:   number;
  deliveryCharge: number;
}

export interface AddMaterialPurchaseDto {
  date:           string;
  supplierId:     number;
  phoneNumber:    string;
  brandId:        number;
  materialId:     number;
  unitOfMeasure:  string;
  unitPrice:      number;
  quantity:       number;
  materialCost:   number;
  deliveryCharge: number;
}

export interface DropdownOption { id: number; name: string; }

export interface DropdownDataResponse {
  suppliers: DropdownOption[];
  materials: DropdownOption[];
  brands:    DropdownOption[];
}

export interface MaterialInfoResponse {
  unitOfMeasure: string;
  unitPrice:     number;
}

export interface MaterialPurchaseListResponse {
  items:      MaterialPurchaseDto[];
  totalCount: number;
}

function makeHeaders(siteId: number): HeadersInit {
  return {
    "Content-Type": "application/json",
    "X-Site-Id":    String(siteId),
  };
}

// GET list with optional filters
export const getMaterialPurchases = async (
  siteId:     number,
  dateFrom?:  string,
  dateTo?:    string,
  materialId?: number,
  supplierId?: number,
  brandId?:    number
): Promise<MaterialPurchaseListResponse> => {
  const params = new URLSearchParams();
  if (dateFrom)   params.append("dateFrom",   dateFrom);
  if (dateTo)     params.append("dateTo",     dateTo);
  if (materialId) params.append("materialId", String(materialId));
  if (supplierId) params.append("supplierId", String(supplierId));
  if (brandId)    params.append("brandId",    String(brandId));

  const query = params.toString() ? `?${params.toString()}` : "";
  const res   = await fetch(`${BASE}${query}`, { headers: makeHeaders(siteId) });

  if (!res.ok) {
    const err = await res.json().catch(() => ({}));
    throw new Error((err as any).message || "Failed to load purchases");
  }
  return res.json();
};

// GET dropdown options
export const getDropdownData = async (): Promise<DropdownDataResponse> => {
  const res = await fetch(`${BASE}/dropdown-data`);
  if (!res.ok) throw new Error("Failed to load dropdown data");
  return res.json();
};

// GET material unit price + unit of measure on material select
export const getMaterialInfo = async (
  materialId: number
): Promise<MaterialInfoResponse> => {
  const res = await fetch(`${BASE}/material-info?materialId=${materialId}`);
  if (!res.ok) throw new Error("Failed to load material info");
  return res.json();
};

// POST add
export const addMaterialPurchase = async (
  siteId:  number,
  payload: AddMaterialPurchaseDto
): Promise<void> => {
  const res = await fetch(BASE, {
    method:  "POST",
    headers: makeHeaders(siteId),
    body:    JSON.stringify(payload),
  });
  if (!res.ok) {
    const data = await res.json().catch(() => ({}));
    throw new Error((data as any).message || "Failed to add purchase");
  }
};

// DELETE
export const deleteMaterialPurchase = async (
  siteId: number,
  id:     number
): Promise<void> => {
  const res = await fetch(`${BASE}/${id}`, {
    method:  "DELETE",
    headers: makeHeaders(siteId),
  });
  if (!res.ok) {
    const data = await res.json().catch(() => ({}));
    throw new Error((data as any).message || "Failed to delete");
  }
};
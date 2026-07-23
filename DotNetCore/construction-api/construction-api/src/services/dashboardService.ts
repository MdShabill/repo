// Path: src/services/dashboardService.ts

const BASE = "https://localhost:7036/api/DashboardAPI";

export interface DashboardStatsDto {
  totalMaterialSpend: number;
  totalPurchases: number;
  totalAttendanceWorkers: number;
}

// siteId = 0  -> All Sites
// siteId > 0  -> Selected Site
export const getDashboardStats = async (
  siteId: number = 0
): Promise<DashboardStatsDto> => {

  const response = await fetch(
    `${BASE}/stats?siteId=${siteId}`
  );

  if (!response.ok) {
    throw new Error("Failed to load dashboard stats");
  }

  return response.json();
};
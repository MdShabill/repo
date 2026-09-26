// Path: src/services/dashboardService.ts
const BASE = "https://localhost:7036/api/DashboardAPI";

export interface DashboardStatsDto {
  // All-time
  totalMaterialSpend:       number;
  totalPurchases:           number;
  totalAttendanceWorkers:   number;

  // Overview
  totalSites:               number;
  activeSites:              number;
  totalPendingScopes:       number;

  // This month
  thisMonthSpend:           number;
  thisMonthPurchases:       number;
  thisMonthAttendance:      number;

  // Site info
  hasSiteInfo:              boolean;
  siteName?:                string;
  siteStatus?:              string;
  siteStartedDate?:         string;
  siteExpectedCompletion?:  string;

  // Budget
  expectedBudget?:          number;
  budgetRemaining?:         number;
  budgetUtilizationPercent?: number;

  // Timeline
  daysElapsed?:             number;
  daysRemaining?:           number;
  timelineProgressPercent?: number;
  isOverdue:                boolean;

  // Scopes
  totalScopes:              number;
  completedScopes:          number;
  inProgressScopes:         number;
  pendingScopes:            number;
  onHoldScopes:             number;
  notStartedScopes:         number;
  scopeCompletionPercent:   number;

  // Last activity
  lastMaterialActivity?:    string;
  lastAttendanceActivity?:  string;
}

export interface ScopeSummaryItemDto {
  id:             number;
  scopeName:      string;
  scopeStatusId:  number;
  statusName:     string;
  remarks?:       string;
  completedDate?: string;
}

export interface MonthlyActivityDto {
  month:            number;
  year:             number;
  monthlySpend:     number;
  monthlyPurchases: number;
  monthlyAttendance:number;
}

// siteId = 0 → all sites, siteId > 0 → specific site
export const getDashboardStats = async (
  siteId: number = 0
): Promise<DashboardStatsDto> => {
  const res = await fetch(`${BASE}/stats?siteId=${siteId}`);
  if (!res.ok) throw new Error("Failed to load dashboard stats");
  return res.json();
};

export const getDashboardScopeItems = async (
  siteId: number
): Promise<ScopeSummaryItemDto[]> => {
  const res = await fetch(`${BASE}/scope-items?siteId=${siteId}`);
  if (!res.ok) throw new Error("Failed to load scope items");
  return res.json();
};

// NEW — specific month + year ka activity
export const getMonthlyActivity = async (
  siteId: number,
  month:  number,
  year:   number
): Promise<MonthlyActivityDto> => {
  const res = await fetch(
    `${BASE}/monthly-activity?siteId=${siteId}&month=${month}&year=${year}`
  );
  if (!res.ok) throw new Error("Failed to load monthly activity");
  return res.json();
};
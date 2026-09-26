// Path: src/pages/Home.tsx

import { useEffect, useState } from "react";
import {getDashboardStats, getDashboardScopeItems, getMonthlyActivity, } from "../services/dashboardService";
import type { DashboardStatsDto, ScopeSummaryItemDto, MonthlyActivityDto, } from "../services/dashboardService";
import { useSite } from "../context/Sitecontext";
import { updateScopeStatus } from "../services/siteService";

function Home() {
  const { selectedSite } = useSite();
  const siteId = selectedSite?.id ?? 0;

  const [stats, setStats] = useState<DashboardStatsDto | null>(null);
  const [scopeItems, setScopeItems] = useState<ScopeSummaryItemDto[]>([]);
  const [showScopeTable, setShowScopeTable] = useState(false);
  const [statsLoading, setStatsLoading] = useState(true);

  const [selectedMonth, setSelectedMonth] = useState<number>(
    new Date().getMonth() + 1
  );
  const [selectedYear, setSelectedYear] = useState<number>(
    new Date().getFullYear()
  );
  const [monthlyActivity, setMonthlyActivity] =
    useState<MonthlyActivityDto | null>(null);
  const [monthlyLoading, setMonthlyLoading] = useState(false);

  useEffect(() => {
    setStatsLoading(true);
    setShowScopeTable(false);

    const calls: Promise<any>[] = [getDashboardStats(siteId)];

    if (siteId > 0) {
      calls.push(getDashboardScopeItems(siteId));
    }

    Promise.all(calls)
      .then(([statsData, scopeData]) => {
        setStats(statsData);
        setScopeItems(scopeData ?? []);
      })
      .catch(() => {
        setStats(null);
        setScopeItems([]);
      })
      .finally(() => setStatsLoading(false));
  }, [siteId]);

  // ==================================================
  // MONTHLY ACTIVITY
  // ==================================================

  const MONTHS = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ];

  // Reset month/year whenever selected site changes
  useEffect(() => {
    setSelectedMonth(new Date().getMonth() + 1);
    setSelectedYear(new Date().getFullYear());
    setMonthlyActivity(null);
  }, [siteId]);

  // Load selected month's activity
  useEffect(() => {
    if (!stats?.hasSiteInfo || !siteId) {
      return;
    }

    setMonthlyLoading(true);

    getMonthlyActivity(siteId, selectedMonth, selectedYear)
      .then(setMonthlyActivity)
      .catch(() => setMonthlyActivity(null))
      .finally(() => setMonthlyLoading(false));
  }, [
    selectedMonth,
    selectedYear,
    siteId,
    stats?.hasSiteInfo,
  ]);

  // Previous month
  const prevMonth = () => {
    if (selectedMonth === 1) {
      setSelectedMonth(12);
      setSelectedYear((y) => y - 1);
    } else {
      setSelectedMonth((m) => m - 1);
    }
  };

  // Next month
  const nextMonth = () => {
    const now = new Date();

    // Do not allow future month
    if (
      selectedYear === now.getFullYear() &&
      selectedMonth === now.getMonth() + 1
    ) {
      return;
    }

    if (selectedMonth === 12) {
      setSelectedMonth(1);
      setSelectedYear((y) => y + 1);
    } else {
      setSelectedMonth((m) => m + 1);
    }
  };

  // Previous year
  const prevYear = () => {
    setSelectedYear((y) => y - 1);
  };

  // Next year
  const nextYear = () => {
    if (selectedYear < new Date().getFullYear()) {
      setSelectedYear((y) => y + 1);
    }
  };

  const isCurrentMonth =
    selectedMonth === new Date().getMonth() + 1 &&
    selectedYear === new Date().getFullYear();

  // Monthly filtered display values
  const displaySpend =
    monthlyActivity?.monthlySpend ??
    stats?.thisMonthSpend ??
    0;

  const displayPurchases =
    monthlyActivity?.monthlyPurchases ??
    stats?.thisMonthPurchases ??
    0;

  const displayAttendance =
    monthlyActivity?.monthlyAttendance ??
    stats?.thisMonthAttendance ??
    0;

  const handleScopeStatusChange = async (
    id: number,
    newStatusId: number
  ) => {
    try {
      await updateScopeStatus(id, newStatusId);

      setScopeItems((prev) =>
        prev.map((s) =>
          s.id === id
            ? {
                ...s,
                scopeStatusId: newStatusId,
                statusName:
                  newStatusId === 3
                    ? "Completed"
                    : newStatusId === 2
                    ? "In Progress"
                    : newStatusId === 1
                    ? "Pending"
                    : newStatusId === 4
                    ? "On Hold"
                    : "Not Started",
              }
            : s
        )
      );
    } catch {
      alert("Failed to update scope status");
    }
  };

  // ── Helpers ─────────────────────────────────────────────────

  const fmt = (n: number) =>
    "₹" +
    n.toLocaleString("en-IN", {
      maximumFractionDigits: 0,
    });

  const siteStatusColor = (status?: string) => {
    if (status === "Completed") return "#16A34A";
    if (status === "Partially Completed") return "#3B82F6";
    if (status === "On Hold") return "#DC2626";

    return "#F59E0B";
  };

  const scopeStatusStyle = (statusId: number) => {
    if (statusId === 3) {
      return {
        bg: "#dcfce7",
        color: "#16A34A",
      };
    }

    if (statusId === 2) {
      return {
        bg: "#FEF3C7",
        color: "#92400E",
      };
    }

    if (statusId === 4) {
      return {
        bg: "#fef2f2",
        color: "#DC2626",
      };
    }

    if (statusId === 5) {
      return {
        bg: "#f8fafc",
        color: "#94A3B8",
      };
    }

    return {
      bg: "#f1f5f9",
      color: "#64748B",
    };
  };

  // ── Shared styles ────────────────────────────────────────────

  const miniCard: React.CSSProperties = {
    background: "rgba(255,255,255,0.06)",
    border: "0.5px solid rgba(255,255,255,0.12)",
    borderRadius: "10px",
    padding: "12px 14px",
    margin: 0,
    boxSizing: "border-box",
  };

  const miniLabel: React.CSSProperties = {
    fontSize: "10px",
    color: "#94A3B8",
    textTransform: "uppercase",
    letterSpacing: "0.7px",
    margin: "0 0 4px 0",
  };

  const inlineCard: React.CSSProperties = {
    background: "rgba(255,255,255,0.06)",
    border: "0.5px solid rgba(255,255,255,0.12)",
    borderRadius: "10px",
    padding: "12px 14px",
    flex: 1,
    boxSizing: "border-box",
  };

  const inlineLabel: React.CSSProperties = {
    fontSize: "9px",
    color: "#94A3B8",
    textTransform: "uppercase",
    letterSpacing: "0.7px",
    margin: "0 0 4px 0",
  };

  // ════════════════════════════════════════════════════════════
  return (
    <div
      style={{
        margin: 0,
        padding: 0,
      }}
    >
      {/* ══════════════════════════════════════════
          HERO + DASHBOARD — dark background
      ══════════════════════════════════════════ */}

      <section
        style={{
          background:
            "linear-gradient(135deg, #1c2e46 0%, #0F172A 100%)",
          padding: "32px 24px 28px 24px",
        }}
      >
        <div
          style={{
            maxWidth: "1100px",
            margin: "0 auto",
          }}
        >
          {/* ── Hero row ── */}

          <div
            style={{
              display: "flex",
              alignItems: "flex-start",
              gap: "28px",
            }}
          >
            {/* LEFT — Tagline + Dashboard stats */}

            <div
              style={{
                flex: 1,
                minWidth: 0,
              }}
            >
              {/* Badge */}

              <div
                style={{
                  display: "inline-flex",
                  alignItems: "center",
                  gap: "7px",
                  background: "rgba(245,158,11,0.15)",
                  border: "0.5px solid rgba(245,158,11,0.35)",
                  color: "#FCD34D",
                  fontSize: "12px",
                  padding: "4px 12px",
                  borderRadius: "20px",
                  marginBottom: "16px",
                }}
              >
                <span
                  style={{
                    width: "7px",
                    height: "7px",
                    background: "#F59E0B",
                    borderRadius: "50%",
                    display: "inline-block",
                  }}
                />

                Construction management platform
              </div>

              <h1
                style={{
                  fontSize: "32px",
                  fontWeight: 700,
                  color: "#fff",
                  lineHeight: 1.2,
                  margin: "0 0 24px 0",
                  whiteSpace: "nowrap",
                }}
              >
                Control every rupee,{" "}
                <span style={{ color: "#F59E0B" }}>
                  every site, every day.
                </span>
              </h1>

              {stats?.hasSiteInfo && !statsLoading && (
                <div>
                  {/* Site name + status badge */}

                  <div
                    style={{
                      display: "flex",
                      alignItems: "center",
                      gap: "10px",
                      marginBottom: "10px",
                    }}
                  >
                    <h3
                      style={{
                        fontSize: "17px",
                        fontWeight: 700,
                        color: "#fff",
                        margin: 0,
                      }}
                    >
                      {stats.siteName}
                    </h3>

                    <span
                      style={{
                        background: siteStatusColor(stats.siteStatus),
                        color: "#fff",
                        fontSize: "11px",
                        fontWeight: 600,
                        padding: "2px 10px",
                        borderRadius: "20px",
                      }}
                    >
                      {stats.siteStatus ?? "—"}
                    </span>
                  </div>

                  {/* Row 1 — Timeline 4 chips */}

                  <div
                    style={{
                      display: "flex",
                      gap: "10px",
                      marginBottom: "12px",
                    }}
                  >
                    <div style={inlineCard}>
                      <p style={inlineLabel}>Started</p>

                      <p
                        style={{
                          fontSize: "13px",
                          fontWeight: 700,
                          color: "#fff",
                          margin: 0,
                        }}
                      >
                        {stats.siteStartedDate ?? "—"}
                      </p>
                    </div>

                    <div style={inlineCard}>
                      <p style={inlineLabel}>Due</p>

                      <p
                        style={{
                          fontSize: "13px",
                          fontWeight: 700,
                          margin: 0,
                          color: stats.siteExpectedCompletion
                            ? stats.isOverdue
                              ? "#F87171"
                              : "#fff"
                            : "#64748B",
                        }}
                      >
                        {stats.siteExpectedCompletion ?? "Not Set"}
                      </p>
                    </div>

                    <div style={inlineCard}>
                      <p style={inlineLabel}>Days Active</p>

                      <p
                        style={{
                          fontSize: "20px",
                          fontWeight: 700,
                          color: "#fff",
                          margin: 0,
                        }}
                      >
                        {stats.daysElapsed ?? "—"}
                      </p>
                    </div>

                    <div style={inlineCard}>
                      <p style={inlineLabel}>
                        {stats.isOverdue ? "Overdue By" : "Days Left"}
                      </p>

                      <p
                        style={{
                          fontSize: "20px",
                          fontWeight: 700,
                          margin: 0,
                          color:
                            stats.daysRemaining == null
                              ? "#94A3B8"
                              : stats.isOverdue
                              ? "#F87171"
                              : "#4ADE80",
                        }}
                      >
                        {stats.daysRemaining == null
                          ? "—"
                          : Math.abs(stats.daysRemaining)}
                      </p>

                      {stats.timelineProgressPercent != null && (
                        <div
                          style={{
                            background: "rgba(255,255,255,0.1)",
                            height: "3px",
                            borderRadius: "2px",
                            marginTop: "6px",
                            overflow: "hidden",
                          }}
                        >
                          <div
                            style={{
                              height: "100%",
                              borderRadius: "2px",
                              background: stats.isOverdue
                                ? "#F87171"
                                : "#F59E0B",
                              width: `${stats.timelineProgressPercent}%`,
                              transition: "width 0.6s ease",
                            }}
                          />
                        </div>
                      )}
                    </div>
                  </div>

                  {/* ==================================================
                      Row 2 — Budget + Scope + This Month
                      ================================================== */}

                  <div
                    style={{
                      display: "flex",
                      gap: "10px",
                      marginBottom: "12px",
                      alignItems: "stretch",
                    }}
                  >
                    {/* ==================================================
                        BUDGET
                        ================================================== */}

                    <div
                      style={{
                        ...inlineCard,
                        flex: "1.2",
                        padding: "12px 14px",
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "flex-start",
                      }}
                    >
                      <p style={inlineLabel}>Budget</p>

                      {stats.expectedBudget ? (
                        <>
                          <div
                            style={{
                              display: "flex",
                              gap: "14px",
                              marginBottom: "8px",
                              alignItems: "flex-start",
                            }}
                          >
                            {[
                              {
                                label: "Expected",
                                value: fmt(stats.expectedBudget),
                                color: "#fff",
                              },
                              {
                                label: "Spent",
                                value: fmt(stats.totalMaterialSpend),
                                color: "#fff",
                              },
                              {
                                label:
                                  (stats.budgetRemaining ?? 0) < 0
                                    ? "Over"
                                    : "Left",
                                value:
                                  stats.budgetRemaining != null
                                    ? fmt(
                                        Math.abs(
                                          stats.budgetRemaining
                                        )
                                      )
                                    : "—",
                                color:
                                  (stats.budgetRemaining ?? 0) < 0
                                    ? "#F87171"
                                    : "#4ADE80",
                              },
                            ].map((item) => (
                              <div key={item.label}>
                                <p
                                  style={{
                                    fontSize: "9px",
                                    color: "#94A3B8",
                                    margin: "0 0 2px 0",
                                  }}
                                >
                                  {item.label}
                                </p>

                                <p
                                  style={{
                                    fontSize: "13px",
                                    fontWeight: 700,
                                    color: item.color,
                                    margin: 0,
                                  }}
                                >
                                  {item.value}
                                </p>
                              </div>
                            ))}
                          </div>

                          <div
                            style={{
                              background: "rgba(255,255,255,0.1)",
                              height: "5px",
                              borderRadius: "3px",
                              overflow: "hidden",
                              marginBottom: "4px",
                            }}
                          >
                            <div
                              style={{
                                height: "100%",
                                borderRadius: "3px",
                                background:
                                  (stats.budgetUtilizationPercent ??
                                    0) >= 90
                                    ? "#F87171"
                                    : (stats.budgetUtilizationPercent ??
                                        0) >= 70
                                    ? "#F59E0B"
                                    : "#4ADE80",
                                width: `${Math.min(
                                  100,
                                  stats.budgetUtilizationPercent ?? 0
                                )}%`,
                                transition: "width 0.6s ease",
                              }}
                            />
                          </div>

                          <p
                            style={{
                              fontSize: "10px",
                              color: "#94A3B8",
                              margin: 0,
                            }}
                          >
                            {(
                              stats.budgetUtilizationPercent ?? 0
                            ).toFixed(1)}
                            % utilized
                          </p>
                        </>
                      ) : (
                        <>
                          <p
                            style={{
                              fontSize: "20px",
                              fontWeight: 700,
                              color: "#fff",
                              margin: "0 0 3px 0",
                            }}
                          >
                            {fmt(stats.totalMaterialSpend)}
                          </p>

                          <p
                            style={{
                              fontSize: "11px",
                              color: "#F59E0B",
                              margin: 0,
                            }}
                          >
                            Total spend · Budget not set
                          </p>
                        </>
                      )}
                    </div>

                    {/* ==================================================
                        SCOPE
                        ================================================== */}

                    <div
                      style={{
                        ...inlineCard,
                        flex: 1,
                        padding: "12px 14px",
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "flex-start",
                      }}
                    >
                      <p style={inlineLabel}>Work Scope</p>

                      {stats.totalScopes > 0 ? (
                        <>
                          <div
                            style={{
                              display: "flex",
                              gap: "6px",
                              marginBottom: "8px",
                            }}
                          >
                            {[
                              {
                                label: "Done",
                                count: stats.completedScopes,
                                color: "#4ADE80",
                              },
                              {
                                label: "Active",
                                count: stats.inProgressScopes,
                                color: "#F59E0B",
                              },
                              {
                                label: "Pending",
                                count: stats.pendingScopes,
                                color: "#94A3B8",
                              },
                              {
                                label: "Hold",
                                count: stats.onHoldScopes,
                                color: "#F87171",
                              },
                            ].map((item) => (
                              <div
                                key={item.label}
                                style={{
                                  textAlign: "center",
                                  flex: 1,
                                }}
                              >
                                <p
                                  style={{
                                    fontSize: "18px",
                                    fontWeight: 700,
                                    color: item.color,
                                    margin: 0,
                                  }}
                                >
                                  {item.count}
                                </p>

                                <p
                                  style={{
                                    fontSize: "9px",
                                    color: "#94A3B8",
                                    margin: "1px 0 0 0",
                                  }}
                                >
                                  {item.label}
                                </p>
                              </div>
                            ))}
                          </div>

                          <div
                            style={{
                              background: "rgba(255,255,255,0.1)",
                              height: "5px",
                              borderRadius: "3px",
                              overflow: "hidden",
                              marginBottom: "4px",
                            }}
                          >
                            <div
                              style={{
                                height: "100%",
                                borderRadius: "3px",
                                background: "#4ADE80",
                                width: `${stats.scopeCompletionPercent}%`,
                                transition: "width 0.6s ease",
                              }}
                            />
                          </div>

                          <div
                            style={{
                              display: "flex",
                              justifyContent: "space-between",
                              alignItems: "center",
                            }}
                          >
                            <p
                              style={{
                                fontSize: "10px",
                                color: "#94A3B8",
                                margin: 0,
                              }}
                            >
                              {stats.completedScopes}/{stats.totalScopes}{" "}
                              complete
                            </p>

                            <button
                              onClick={() =>
                                setShowScopeTable(!showScopeTable)
                              }
                              style={{
                                background: "none",
                                border: "none",
                                color: "#F59E0B",
                                fontSize: "11px",
                                cursor: "pointer",
                                padding: 0,
                                fontWeight: 600,
                              }}
                            >
                              {showScopeTable
                                ? "▲ Hide"
                                : "▼ Details"}
                            </button>
                          </div>
                        </>
                      ) : (
                        <p
                          style={{
                            fontSize: "12px",
                            color: "#64748B",
                            margin: "8px 0 0 0",
                          }}
                        >
                          No scopes defined
                        </p>
                      )}
                    </div>

                    {/* ==================================================
                        THIS MONTH — Month + Year Navigation
                        Material LEFT / Labour RIGHT
                        ================================================== */}

                    <div
                      style={{
                        ...inlineCard,
                        flex: 1,
                        padding: "12px 14px",
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "flex-start",
                      }}
                    >
                      {/* Header */}
                      <p style={inlineLabel}>
                        {isCurrentMonth
                          ? "This Month"
                          : "Selected Month"}
                      </p>

                      {/* Month Navigation */}
                      <div
                        style={{
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "space-between",
                          marginBottom: "2px",
                        }}
                      >
                        <button
                          onClick={prevMonth}
                          style={{
                            border: "none",
                            background: "none",
                            fontSize: "14px",
                            color: "#F59E0B",
                            padding: "0 4px",
                            cursor: "pointer",
                            fontWeight: 700,
                          }}
                        >
                          ‹
                        </button>

                        <span
                          style={{
                            fontSize: "11px",
                            fontWeight: 600,
                            color: "#fff",
                          }}
                        >
                          {MONTHS[selectedMonth - 1]}
                        </span>

                        <button
                          onClick={nextMonth}
                          disabled={isCurrentMonth}
                          style={{
                            background: "none",
                            border: "none",
                            color: isCurrentMonth
                              ? "#4B5563"
                              : "#F59E0B",
                            fontSize: "14px",
                            cursor: isCurrentMonth
                              ? "not-allowed"
                              : "pointer",
                            padding: "0 4px",
                            fontWeight: 700,
                          }}
                        >
                          ›
                        </button>
                      </div>

                      {/* Year Navigation */}
                      <div
                        style={{
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "space-between",
                          marginBottom: "7px",
                        }}
                      >
                        <button
                          onClick={prevYear}
                          style={{
                            background: "none",
                            border: "none",
                            color: "#F59E0B",
                            fontSize: "14px",
                            cursor: "pointer",
                            padding: "0 4px",
                            fontWeight: 700,
                          }}
                        >
                          ‹
                        </button>

                        <span
                          style={{
                            fontSize: "11px",
                            fontWeight: 600,
                            color: "#fff",
                          }}
                        >
                          {selectedYear}
                        </span>

                        <button
                          onClick={nextYear}
                          disabled={
                            selectedYear >= new Date().getFullYear()
                          }
                          style={{
                            background: "none",
                            border: "none",
                            color:
                              selectedYear >=
                              new Date().getFullYear()
                                ? "#4B5563"
                                : "#F59E0B",
                            fontSize: "14px",
                            cursor:
                              selectedYear >=
                              new Date().getFullYear()
                                ? "not-allowed"
                                : "pointer",
                            padding: "0 4px",
                            fontWeight: 700,
                          }}
                        >
                          ›
                        </button>
                      </div>

                      {/* Divider */}
                      <div
                        style={{
                          borderTop:
                            "0.5px solid rgba(255,255,255,0.1)",
                          marginBottom: "8px",
                        }}
                      />

                      {/* Monthly Data */}
                      {monthlyLoading ? (
                        <p
                          style={{
                            color: "#94A3B8",
                            fontSize: "12px",
                            margin: 0,
                          }}
                        >
                          Loading...
                        </p>
                      ) : (
                        <div
                          style={{
                            display: "flex",
                            width: "100%",
                            minHeight: "58px",
                          }}
                        >
                          {/* MATERIAL - LEFT */}
                          <div
                            style={{
                              flex: 1,
                              minWidth: 0,
                              paddingRight: "12px",
                              boxSizing: "border-box",
                            }}
                          >
                            <p
                              style={{
                                fontSize: "9px",
                                color: "#94A3B8",
                                margin: "0 0 2px 0",
                              }}
                            >
                              MATERIAL
                            </p>

                            <p
                              style={{
                                fontSize: "14px",
                                fontWeight: 700,
                                color: "#fff",
                                margin: 0,
                                whiteSpace: "nowrap",
                              }}
                            >
                              {fmt(displaySpend)}
                            </p>

                            <p
                              style={{
                                fontSize: "10px",
                                color: "#94A3B8",
                                margin: "1px 0 0 0",
                              }}
                            >
                              {displayPurchases} purchase
                              {displayPurchases !== 1
                                ? "s"
                                : ""}
                            </p>
                          </div>

                          {/* VERTICAL SEPARATOR */}
                          <div
                            style={{
                              width: "1px",
                              background:
                                "rgba(255,255,255,0.12)",
                              alignSelf: "stretch",
                              flexShrink: 0,
                            }}
                          />

                          {/* LABOUR - RIGHT */}
                          <div
                            style={{
                              flex: 1,
                              minWidth: 0,
                              paddingLeft: "12px",
                              boxSizing: "border-box",
                            }}
                          >
                            <p
                              style={{
                                fontSize: "9px",
                                color: "#94A3B8",
                                margin: "0 0 2px 0",
                              }}
                            >
                              LABOUR
                            </p>

                            <p
                              style={{
                                fontSize: "14px",
                                fontWeight: 700,
                                color: "#fff",
                                margin: 0,
                              }}
                            >
                              {displayAttendance}
                            </p>

                            <p
                              style={{
                                fontSize: "10px",
                                color: "#94A3B8",
                                margin: "1px 0 0 0",
                              }}
                            >
                              workers this month
                            </p>
                          </div>
                        </div>
                      )}
                    </div>
                  </div>

                  {/* Scope detail table — expandable */}

                  {showScopeTable && scopeItems.length > 0 && (
                    <div
                      style={{
                        background: "rgba(255,255,255,0.04)",
                        border:
                          "0.5px solid rgba(255,255,255,0.1)",
                        borderRadius: "10px",
                        overflow: "hidden",
                      }}
                    >
                      <table
                        style={{
                          width: "100%",
                          borderCollapse: "collapse",
                        }}
                      >
                        <thead>
                          <tr
                            style={{
                              background:
                                "rgba(255,255,255,0.08)",
                            }}
                          >
                            {[
                              "Work",
                              "Status",
                              "Remarks",
                              "Update",
                            ].map((h) => (
                              <th
                                key={h}
                                style={{
                                  padding: "10px 14px",
                                  textAlign: "left",
                                  fontSize: "11px",
                                  color: "#94A3B8",
                                  fontWeight: 600,
                                  textTransform: "uppercase",
                                  letterSpacing: "0.5px",
                                }}
                              >
                                {h}
                              </th>
                            ))}
                          </tr>
                        </thead>

                        <tbody>
                          {scopeItems.map((scope, i) => {
                            const { bg, color } =
                              scopeStatusStyle(
                                scope.scopeStatusId
                              );

                            return (
                              <tr
                                key={scope.id}
                                style={{
                                  borderTop:
                                    "0.5px solid rgba(255,255,255,0.06)",
                                  background:
                                    i % 2 === 0
                                      ? "transparent"
                                      : "rgba(255,255,255,0.02)",
                                }}
                              >
                                <td
                                  style={{
                                    padding: "9px 14px",
                                    fontSize: "13px",
                                    color: "#fff",
                                  }}
                                >
                                  {scope.scopeName}
                                </td>

                                <td
                                  style={{
                                    padding: "9px 14px",
                                  }}
                                >
                                  <span
                                    style={{
                                      background: bg,
                                      color,
                                      fontWeight: 600,
                                      padding: "2px 8px",
                                      borderRadius: "20px",
                                      fontSize: "11px",
                                    }}
                                  >
                                    {scope.statusName}
                                  </span>
                                </td>

                                <td
                                  style={{
                                    padding: "9px 14px",
                                    fontSize: "12px",
                                    color: "#64748B",
                                  }}
                                >
                                  {scope.remarks || "—"}
                                </td>

                                <td
                                  style={{
                                    padding: "9px 14px",
                                  }}
                                >
                                  <select
                                    value={scope.scopeStatusId}
                                    onChange={(e) =>
                                      handleScopeStatusChange(
                                        scope.id,
                                        Number(e.target.value)
                                      )
                                    }
                                    style={{
                                      borderRadius: "6px",
                                      padding: "3px 7px",
                                      fontSize: "11px",
                                      border:
                                        "1px solid rgba(255,255,255,0.15)",
                                      background: "#1c2e46",
                                      color: "#fff",
                                      cursor: "pointer",
                                    }}
                                  >
                                    <option value={1}>
                                      Pending
                                    </option>

                                    <option value={2}>
                                      In Progress
                                    </option>

                                    <option value={3}>
                                      Completed
                                    </option>

                                    <option value={4}>
                                      On Hold
                                    </option>

                                    <option value={5}>
                                      Not Started
                                    </option>
                                  </select>
                                </td>
                              </tr>
                            );
                          })}
                        </tbody>
                      </table>
                    </div>
                  )}
                </div>
              )}
            </div>

            {/* ==================================================
                RIGHT — 3 COMPACT STAT CARDS
                ================================================== */}

            <div
              style={{
                flexShrink: 0,
                width: "180px",
                display: "flex",
                flexDirection: "column",
                gap: "10px",
                alignSelf: "flex-start",
                marginTop: "68px",
              }}
            >
              {/* TOTAL SPEND */}

              <div
                style={{
                  ...miniCard,
                  minHeight: "92px",
                  display: "flex",
                  flexDirection: "column",
                  justifyContent: "space-between",
                }}
              >
                <div>
                  <p style={miniLabel}>
                    {siteId > 0
                      ? "Total Spend — This Site"
                      : "Total Material Spend"}
                  </p>

                  {statsLoading ? (
                    <p
                      style={{
                        color: "#94A3B8",
                        fontSize: "13px",
                        margin: 0,
                      }}
                    >
                      Loading...
                    </p>
                  ) : (
                    <p
                      style={{
                        fontSize: "22px",
                        fontWeight: 700,
                        color: "#fff",
                        margin: 0,
                      }}
                    >
                      {stats
                        ? fmt(stats.totalMaterialSpend)
                        : "—"}
                    </p>
                  )}
                </div>

                {!statsLoading && (
                  <p
                    style={{
                      fontSize: "11px",
                      color: "#F59E0B",
                      margin: "8px 0 0 0",
                    }}
                  >
                    all-time material cost
                  </p>
                )}
              </div>

              {/* TOTAL PURCHASES */}

              <div
                style={{
                  ...miniCard,
                  minHeight: "92px",
                  display: "flex",
                  flexDirection: "column",
                  justifyContent: "space-between",
                }}
              >
                <div>
                  <p style={miniLabel}>Total Purchases</p>

                  {statsLoading ? (
                    <p
                      style={{
                        color: "#94A3B8",
                        fontSize: "13px",
                        margin: 0,
                      }}
                    >
                      Loading...
                    </p>
                  ) : (
                    <p
                      style={{
                        fontSize: "22px",
                        fontWeight: 700,
                        color: "#fff",
                        margin: 0,
                      }}
                    >
                      {stats?.totalPurchases ?? "—"}
                    </p>
                  )}
                </div>

                {!statsLoading && (
                  <p
                    style={{
                      fontSize: "11px",
                      color: "#F59E0B",
                      margin: "8px 0 0 0",
                    }}
                  >
                    all-time material entries
                  </p>
                )}
              </div>

              {/* TOTAL ATTENDANCE */}

              <div
                style={{
                  ...miniCard,
                  minHeight: "92px",
                  display: "flex",
                  flexDirection: "column",
                  justifyContent: "space-between",
                }}
              >
                <div>
                  <p style={miniLabel}>Total Attendance</p>

                  {statsLoading ? (
                    <p
                      style={{
                        color: "#94A3B8",
                        fontSize: "13px",
                        margin: 0,
                      }}
                    >
                      Loading...
                    </p>
                  ) : (
                    <p
                      style={{
                        fontSize: "22px",
                        fontWeight: 700,
                        color: "#fff",
                        margin: 0,
                      }}
                    >
                      {stats?.totalAttendanceWorkers ?? "—"}
                    </p>
                  )}
                </div>

                {!statsLoading && (
                  <p
                    style={{
                      fontSize: "11px",
                      color: "#4ADE80",
                      margin: "8px 0 0 0",
                    }}
                  >
                    all-time workers
                  </p>
                )}
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* ══════════════════════════════════════════
          FEATURES SECTION — UPDATED UI ONLY
      ══════════════════════════════════════════ */}

      <section
        style={{
          position: "relative",
          overflow: "hidden",
          background:
            "radial-gradient(circle at 12% 5%, rgba(30,136,229,0.10), transparent 28%), radial-gradient(circle at 88% 15%, rgba(30,136,229,0.08), transparent 28%), linear-gradient(180deg, #ffffff 0%, #f5f9fd 100%)",
          padding: "64px 24px 78px",
        }}
      >
        {/* Decorative background circles */}

        <div
          style={{
            position: "absolute",
            width: "220px",
            height: "220px",
            borderRadius: "50%",
            background: "rgba(30,136,229,0.07)",
            top: "-100px",
            left: "-100px",
          }}
        />

        <div
          style={{
            position: "absolute",
            width: "280px",
            height: "280px",
            borderRadius: "50%",
            background: "rgba(30,136,229,0.045)",
            bottom: "-160px",
            right: "-100px",
          }}
        />

        <div
          style={{
            position: "relative",
            maxWidth: "1100px",
            margin: "0 auto",
          }}
        >
          {/* Section Heading */}

          <div
            style={{
              marginBottom: "34px",
            }}
          >
            <p
              style={{
                fontSize: "12px",
                fontWeight: 700,
                color: "#3678B7",
                textTransform: "uppercase",
                letterSpacing: "1.8px",
                margin: "0 0 10px 0",
              }}
            >
              WHAT BUILDERLEDGER DOES
            </p>

            <div
              style={{
                width: "48px",
                height: "4px",
                borderRadius: "4px",
                background: "#F59E0B",
                marginBottom: "14px",
              }}
            />

            <h2
              style={{
                fontSize: "32px",
                fontWeight: 700,
                color: "#102E52",
                margin: "0 0 10px 0",
                lineHeight: 1.2,
              }}
            >
              Everything your site needs, in one system
            </h2>

            <p
              style={{
                fontSize: "15px",
                color: "#47627F",
                maxWidth: "650px",
                lineHeight: 1.65,
                margin: 0,
              }}
            >
              Designed for construction teams managing multiple sites,
              vendors, and budgets simultaneously.
            </p>
          </div>

          {/* Feature Cards */}

          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(3, 1fr)",
              gap: "22px",
            }}
          >
            <FeatureCard
              type="cost"
              title="Cost master"
              description="Define standard rates for materials, labour, and equipment. All transactions reference these rates automatically."
            />

            <FeatureCard
              type="attendance"
              title="Attendance tracking"
              description="Record daily worker attendance per site. Calculate wages, overtime, and absenteeism across your workforce."
            />

            <FeatureCard
              type="material"
              title="Material reports"
              description="Track every purchase, quantity, brand, supplier, and cost. Identify overspend and compare against budget."
            />

            <FeatureCard
              type="service"
              title="Service providers"
              description="Manage your contractors, workers, and service providers in one centralized system."
            />

            <FeatureCard
              type="reports"
              title="Site-wise reports"
              description="View construction activity and financial information site by site."
            />

            <FeatureCard
              type="expense"
              title="Expense control"
              description="Track expenses and maintain better control over your construction budget."
            />
          </div>
        </div>
      </section>

      {/* ══════════════════════════════════════════
          FOOTER
      ══════════════════════════════════════════ */}

      <footer
        style={{
          position: "relative",
          background:
            "linear-gradient(135deg, #1c3554 0%, #10233D 100%)",
          padding: "34px 24px",
          textAlign: "center",
          overflow: "hidden",
        }}
      >
        <div
          style={{
            position: "absolute",
            width: "300px",
            height: "80px",
            background: "rgba(255,255,255,0.03)",
            borderRadius: "50%",
            left: "-100px",
            top: "30px",
            transform: "rotate(-8deg)",
          }}
        />

        <div
          style={{
            position: "relative",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            gap: "12px",
          }}
        >
          <span
            style={{
              width: "22px",
              height: "22px",
              borderRadius: "50%",
              border: "2px solid #F59E0B",
              display: "inline-flex",
              alignItems: "center",
              justifyContent: "center",
              color: "#F59E0B",
              fontSize: "11px",
              fontWeight: 700,
            }}
          >
            ✓
          </span>

          <p
            style={{
              color: "#AFC0D3",
              fontSize: "13px",
              margin: 0,
            }}
          >
            BuilderLedger © 2026. Construction management made simple.
          </p>
        </div>
      </footer>
    </div>
  );
}

/* ==================================================
   FEATURE CARD
================================================== */

function FeatureCard({
  type,
  title,
  description,
}: {
  type:
    | "cost"
    | "attendance"
    | "material"
    | "service"
    | "reports"
    | "expense";
  title: string;
  description: string;
}) {
  const cardConfig = {
    cost: {
      accent: "#1687E8",
      soft: "rgba(22,135,232,0.10)",
    },

    attendance: {
      accent: "#2FB985",
      soft: "rgba(47,185,133,0.11)",
    },

    material: {
      accent: "#F2A900",
      soft: "rgba(242,169,0,0.12)",
    },

    service: {
      accent: "#4C35D8",
      soft: "rgba(76,53,216,0.10)",
    },

    reports: {
      accent: "#0C9CB0",
      soft: "rgba(12,156,176,0.10)",
    },

    expense: {
      accent: "#E23B43",
      soft: "rgba(226,59,67,0.10)",
    },
  };

  const config = cardConfig[type];

  return (
    <div
      style={{
        position: "relative",
        display: "flex",
        alignItems: "flex-start",
        gap: "18px",
        minHeight: "178px",
        padding: "27px 22px 24px 28px",
        borderRadius: "14px",
        border: `1px solid ${config.accent}25`,
        borderLeft: `5px solid ${config.accent}`,
        background:
          "linear-gradient(135deg, rgba(255,255,255,0.98), rgba(248,251,255,0.94))",
        boxShadow:
          "0 8px 24px rgba(25,55,90,0.07)",
        transition:
          "transform 0.2s ease, box-shadow 0.2s ease",
      }}
      onMouseEnter={(e) => {
        e.currentTarget.style.transform = "translateY(-4px)";
        e.currentTarget.style.boxShadow =
          "0 14px 32px rgba(25,55,90,0.12)";
      }}
      onMouseLeave={(e) => {
        e.currentTarget.style.transform = "translateY(0)";
        e.currentTarget.style.boxShadow =
          "0 8px 24px rgba(25,55,90,0.07)";
      }}
    >
      {/* Icon */}

      <div
        style={{
          flexShrink: 0,
          width: "62px",
          height: "62px",
          borderRadius: "50%",
          background: config.soft,
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          color: config.accent,
        }}
      >
        <FeatureIcon type={type} color={config.accent} />
      </div>

      {/* Content */}

      <div
        style={{
          flex: 1,
          minWidth: 0,
          paddingRight: "28px",
        }}
      >
        <h3
          style={{
            fontSize: "17px",
            fontWeight: 700,
            color: "#12345A",
            margin: "3px 0 10px 0",
            lineHeight: 1.25,
          }}
        >
          {title}
        </h3>

        <p
          style={{
            fontSize: "13.5px",
            color: "#466482",
            lineHeight: 1.65,
            margin: 0,
          }}
        >
          {description}
        </p>
      </div>

      {/* Arrow */}

      <div
        style={{
          position: "absolute",
          right: "18px",
          top: "27px",
          width: "34px",
          height: "34px",
          borderRadius: "50%",
          background: config.soft,
          color: config.accent,
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          fontSize: "20px",
          fontWeight: 500,
        }}
      >
        →
      </div>
    </div>
  );
}

/* ==================================================
   FEATURE ICONS
================================================== */

function FeatureIcon({
  type,
  color,
}: {
  type:
    | "cost"
    | "attendance"
    | "material"
    | "service"
    | "reports"
    | "expense";
  color: string;
}) {
  if (type === "cost") {
    return (
      <span
        style={{
          fontSize: "30px",
          fontWeight: 700,
          lineHeight: 1,
        }}
      >
        ₹
      </span>
    );
  }

  if (type === "attendance") {
    return (
      <svg
        width="34"
        height="34"
        viewBox="0 0 40 40"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <circle cx="20" cy="11" r="6" fill={color} />

        <path
          d="M8 31C8 24.9249 13.3726 20 20 20C26.6274 20 32 24.9249 32 31"
          fill={color}
        />

        <circle
          cx="7"
          cy="17"
          r="4"
          fill={color}
          opacity="0.65"
        />

        <circle
          cx="33"
          cy="17"
          r="4"
          fill={color}
          opacity="0.65"
        />
      </svg>
    );
  }

  if (type === "material") {
    return (
      <svg
        width="34"
        height="34"
        viewBox="0 0 40 40"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M20 5L33 12L20 19L7 12L20 5Z"
          fill={color}
        />

        <path
          d="M7 12V27L20 34V19L7 12Z"
          fill={color}
          opacity="0.72"
        />

        <path
          d="M33 12V27L20 34V19L33 12Z"
          fill={color}
          opacity="0.9"
        />

        <path
          d="M20 19V34"
          stroke="#fff"
          strokeWidth="1.5"
          opacity="0.5"
        />
      </svg>
    );
  }

  if (type === "service") {
    return (
      <svg
        width="34"
        height="34"
        viewBox="0 0 40 40"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <circle cx="20" cy="10" r="6" fill={color} />

        <path
          d="M9 34C9 26.8203 13.9249 21 20 21C26.0751 21 31 26.8203 31 34"
          fill={color}
        />

        <path
          d="M14 20L20 25L26 20"
          stroke="#fff"
          strokeWidth="2"
          opacity="0.55"
        />
      </svg>
    );
  }

  if (type === "reports") {
    return (
      <svg
        width="34"
        height="34"
        viewBox="0 0 40 40"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <rect
          x="6"
          y="20"
          width="6"
          height="13"
          rx="1"
          fill={color}
          opacity="0.65"
        />

        <rect
          x="17"
          y="13"
          width="6"
          height="20"
          rx="1"
          fill={color}
          opacity="0.85"
        />

        <rect
          x="28"
          y="6"
          width="6"
          height="27"
          rx="1"
          fill={color}
        />
      </svg>
    );
  }

  return (
    <svg
      width="34"
      height="34"
      viewBox="0 0 40 40"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        d="M20 4L33 9V18C33 26 27.5 32.5 20 36C12.5 32.5 7 26 7 18V9L20 4Z"
        stroke={color}
        strokeWidth="3"
        strokeLinejoin="round"
      />

      <path
        d="M14 20L18 24L26 15"
        stroke={color}
        strokeWidth="3"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  );
}

export default Home;
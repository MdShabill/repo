import { useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { getAllSites, filterSites, getDropdownData, deleteSite, } from "../services/siteService";
import type { SiteListDto, DropdownItem, } from "../services/siteService";

function SiteList() {
  const location = useLocation();

  const [sites, setSites] = useState<SiteListDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [statuses, setStatuses] = useState<DropdownItem[]>([]);

  const [search, setSearch] = useState("");
  const [selectedStatusId, setSelectedStatusId] = useState<
    number | undefined
  >(undefined);

  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");

  const [budgetFrom, setBudgetFrom] = useState("");
  const [budgetTo, setBudgetTo] = useState("");

  const [openFilter, setOpenFilter] = useState<
    "status" | "date" | "budget" | null
  >(null);

  const [expandAddressColumn, setExpandAddressColumn] = useState(false);

  const successMessage =
    (location.state as { success?: string })?.success ?? "";

  /* =========================================================
     LOAD INITIAL SITE DATA
  ========================================================= */

  useEffect(() => {
    loadInitialData();
  }, []);

  /* =========================================================
     LOAD STATUS DROPDOWN
  ========================================================= */

  useEffect(() => {
    getDropdownData()
      .then((data) => {
        setStatuses(data.statuses ?? []);
      })
      .catch((err: Error) => {
        setError(err.message);
      });
  }, []);

  const loadInitialData = async () => {
    try {
      setLoading(true);
      setError("");

      const data = await getAllSites();
      setSites(data);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  /* =========================================================
     APPLY FILTERS
  ========================================================= */

  const handleApplyFilters = async () => {
    setError("");

    /* ---------- DATE VALIDATION ---------- */

    if (fromDate && toDate && fromDate > toDate) {
      setError("From Date cannot be greater than To Date.");
      return;
    }

    /* ---------- BUDGET VALIDATION ---------- */

    const parsedBudgetFrom =
      budgetFrom.trim() !== ""
        ? Number(budgetFrom)
        : undefined;

    const parsedBudgetTo =
      budgetTo.trim() !== ""
        ? Number(budgetTo)
        : undefined;

    if (
      parsedBudgetFrom !== undefined &&
      (Number.isNaN(parsedBudgetFrom) || parsedBudgetFrom < 0)
    ) {
      setError("Budget From cannot be negative.");
      return;
    }

    if (
      parsedBudgetTo !== undefined &&
      (Number.isNaN(parsedBudgetTo) || parsedBudgetTo < 0)
    ) {
      setError("Budget To cannot be negative.");
      return;
    }

    if (
      parsedBudgetFrom !== undefined &&
      parsedBudgetTo !== undefined &&
      parsedBudgetFrom > parsedBudgetTo
    ) {
      setError(
        "Budget From cannot be greater than Budget To."
      );
      return;
    }

    try {
      setLoading(true);

      const data = await filterSites({
        search: search.trim() || undefined,
        statusId: selectedStatusId,
        fromDate: fromDate || undefined,
        toDate: toDate || undefined,
        budgetFrom: parsedBudgetFrom,
        budgetTo: parsedBudgetTo,
      });

      setSites(data);
      setOpenFilter(null);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  /* =========================================================
     CLEAR ALL FILTERS
  ========================================================= */

  const handleClearFilters = async () => {
    setSearch("");
    setSelectedStatusId(undefined);

    setFromDate("");
    setToDate("");

    setBudgetFrom("");
    setBudgetTo("");

    setOpenFilter(null);
    setError("");

    try {
      setLoading(true);

      const data = await getAllSites();
      setSites(data);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  /* =========================================================
     STATUS APPLY
  ========================================================= */

  const handleStatusChange = async (
    statusId: number | undefined
  ) => {
    setSelectedStatusId(statusId);

    setError("");

    try {
      setLoading(true);

      const data = await filterSites({
        search: search.trim() || undefined,
        statusId,
        fromDate: fromDate || undefined,
        toDate: toDate || undefined,
        budgetFrom:
          budgetFrom.trim() !== ""
            ? Number(budgetFrom)
            : undefined,
        budgetTo:
          budgetTo.trim() !== ""
            ? Number(budgetTo)
            : undefined,
      });

      setSites(data);
      setOpenFilter(null);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  /* =========================================================
     DELETE
  ========================================================= */

  const handleDelete = async (siteId: number) => {
    if (!window.confirm("Are you sure you want to delete this site?")) {
      return;
    }

    try {
      await deleteSite(siteId);

      setSites((prev) =>
        prev.filter((s) => s.id !== siteId)
      );
    } catch (err) {
      setError((err as Error).message);
    }
  };

  /* =========================================================
     DATE FORMAT
  ========================================================= */

  const fmt = (d: string) =>
    new Date(d).toLocaleDateString("en-GB", {
      day: "2-digit",
      month: "short",
      year: "numeric",
    });

  /* =========================================================
     ADDRESS
  ========================================================= */

  const getAddressText = (site: SiteListDto) =>
    [
      site.addressTypes,
      site.addressLine1,
      site.countryName,
      site.pinCode,
    ]
      .filter(Boolean)
      .join(" - ") || "—";

  /* =========================================================
     SELECTED STATUS NAME
  ========================================================= */

  const selectedStatusName =
    statuses.find(
      (status) => status.id === selectedStatusId
    )?.name ?? "";

  /* =========================================================
     ACTIVE FILTER CHECK
  ========================================================= */

  const hasActiveFilters =
    search.trim() !== "" ||
    selectedStatusId !== undefined ||
    fromDate !== "" ||
    toDate !== "" ||
    budgetFrom !== "" ||
    budgetTo !== "";

  return (
    <div
      style={{
        background:
          "linear-gradient(135deg, #1E3A5F, #0F172A)",
        minHeight: "100vh",
      }}
    >
      <div style={{ padding: "30px" }}>
        <div
          style={{
            background: "#fff",
            borderRadius: "18px",
            padding: "25px",
            boxShadow: "0 10px 25px rgba(0,0,0,0.15)",
          }}
        >
          {/* =================================================
              HEADER
          ================================================= */}

          <div
            style={{
              display: "grid",
              gridTemplateColumns: "1fr 1fr 1fr",
              alignItems: "center",
              marginBottom: "20px",
            }}
          >
            {/* TOTAL SITES */}

            <div>
              <div
                style={{
                  display: "inline-block",
                  background: "#1E3A5F",
                  color: "#fff",
                  padding: "8px 16px",
                  borderRadius: "20px",
                  fontWeight: 500,
                }}
              >
                Total Sites

                <span
                  style={{
                    background: "#F59E0B",
                    padding: "4px 10px",
                    borderRadius: "12px",
                    marginLeft: "6px",
                  }}
                >
                  {sites.length}
                </span>
              </div>
            </div>

            {/* TITLE */}

            <div
              style={{
                textAlign: "center",
                fontSize: "26px",
                fontWeight: 600,
                color: "#1E293B",
              }}
            >
              Site List
            </div>

            {/* ADD NEW */}

            <div style={{ textAlign: "right" }}>
              <Link
                to="/site-add"
                style={{
                  display: "inline-block",
                  background: "#F59E0B",
                  color: "#fff",
                  padding: "10px 18px",
                  borderRadius: "10px",
                  textDecoration: "none",
                  fontWeight: 500,
                }}
              >
                + Add New
              </Link>
            </div>
          </div>

          {/* =================================================
              FILTER BAR
          ================================================= */}

          <div
            style={{
              display: "flex",
              alignItems: "center",
              gap: "10px",
              marginBottom: "20px",
              flexWrap: "wrap",
            }}
          >
            {/* ================= SEARCH ================= */}

            <div
              style={{
                flex: "1 1 280px",
                minWidth: "250px",
              }}
            >
              <div
                style={{
                  display: "flex",
                  alignItems: "center",
                  border: "1px solid #CBD5E1",
                  borderRadius: "9px",
                  overflow: "hidden",
                  background: "#fff",
                }}
              >
                <span
                  style={{
                    paddingLeft: "12px",
                    color: "#64748B",
                    fontSize: "16px",
                  }}
                >
                  🔍
                </span>

                <input
                  type="text"
                  value={search}
                  onChange={(e) =>
                    setSearch(e.target.value)
                  }
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      handleApplyFilters();
                    }
                  }}
                  placeholder="Search Site Name or Contact Name"
                  style={{
                    width: "100%",
                    border: "none",
                    outline: "none",
                    padding: "10px 12px 10px 8px",
                    fontSize: "14px",
                  }}
                />
              </div>
            </div>

            {/* ================= STATUS ================= */}

            <div style={{ position: "relative" }}>
              <button
                type="button"
                onClick={() =>
                  setOpenFilter(
                    openFilter === "status"
                      ? null
                      : "status"
                  )
                }
                style={{
                  minWidth: "130px",
                  padding: "10px 14px",
                  borderRadius: "9px",
                  border: "1px solid #CBD5E1",
                  background:
                    selectedStatusId !== undefined
                      ? "#FFF7E6"
                      : "#fff",
                  color: "#334155",
                  cursor: "pointer",
                  fontSize: "14px",
                  textAlign: "left",
                }}
              >
                {selectedStatusName || "Status"}
                <span
                  style={{
                    float: "right",
                    marginLeft: "10px",
                  }}
                >
                  ▾
                </span>
              </button>

              {openFilter === "status" && (
                <div
                  style={{
                    position: "absolute",
                    top: "calc(100% + 6px)",
                    left: 0,
                    zIndex: 20,
                    width: "220px",
                    background: "#fff",
                    border: "1px solid #E2E8F0",
                    borderRadius: "10px",
                    boxShadow:
                      "0 10px 25px rgba(0,0,0,0.15)",
                    padding: "8px",
                  }}
                >
                  <button
                    type="button"
                    onClick={() =>
                      handleStatusChange(undefined)
                    }
                    style={{
                      width: "100%",
                      textAlign: "left",
                      border: "none",
                      background:
                        selectedStatusId === undefined
                          ? "#F8FAFC"
                          : "#fff",
                      padding: "9px 10px",
                      borderRadius: "7px",
                      cursor: "pointer",
                      color: "#475569",
                    }}
                  >
                    All Statuses
                  </button>

                  {statuses.map((status) => (
                    <button
                      key={status.id}
                      type="button"
                      onClick={() =>
                        handleStatusChange(status.id)
                      }
                      style={{
                        width: "100%",
                        textAlign: "left",
                        border: "none",
                        background:
                          selectedStatusId === status.id
                            ? "#FFF7E6"
                            : "#fff",
                        padding: "9px 10px",
                        borderRadius: "7px",
                        cursor: "pointer",
                        color: "#334155",
                      }}
                    >
                      {status.name}
                    </button>
                  ))}
                </div>
              )}
            </div>

            {/* ================= DATE ================= */}

            <div style={{ position: "relative" }}>
              <button
                type="button"
                onClick={() =>
                  setOpenFilter(
                    openFilter === "date"
                      ? null
                      : "date"
                  )
                }
                style={{
                  minWidth: "110px",
                  padding: "10px 14px",
                  borderRadius: "9px",
                  border: "1px solid #CBD5E1",
                  background:
                    fromDate || toDate
                      ? "#FFF7E6"
                      : "#fff",
                  color: "#334155",
                  cursor: "pointer",
                  fontSize: "14px",
                  textAlign: "left",
                }}
              >
                Date
                <span
                  style={{
                    float: "right",
                    marginLeft: "10px",
                  }}
                >
                  ▾
                </span>
              </button>

              {openFilter === "date" && (
                <div
                  style={{
                    position: "absolute",
                    top: "calc(100% + 6px)",
                    left: 0,
                    zIndex: 20,
                    width: "280px",
                    background: "#fff",
                    border: "1px solid #E2E8F0",
                    borderRadius: "10px",
                    boxShadow:
                      "0 10px 25px rgba(0,0,0,0.15)",
                    padding: "15px",
                  }}
                >
                  <div
                    style={{
                      fontWeight: 600,
                      color: "#1E293B",
                      marginBottom: "12px",
                    }}
                  >
                    Date
                  </div>

                  {/* FROM DATE */}

                  <label
                    style={{
                      display: "block",
                      fontSize: "13px",
                      fontWeight: 500,
                      color: "#475569",
                      marginBottom: "5px",
                    }}
                  >
                    From Date
                  </label>

                  <input
                    type="date"
                    value={fromDate}
                    onChange={(e) =>
                      setFromDate(e.target.value)
                    }
                    style={{
                      width: "100%",
                      boxSizing: "border-box",
                      padding: "9px 10px",
                      border:
                        "1px solid #CBD5E1",
                      borderRadius: "7px",
                      outline: "none",
                      marginBottom: "12px",
                    }}
                  />

                  {/* TO DATE */}

                  <label
                    style={{
                      display: "block",
                      fontSize: "13px",
                      fontWeight: 500,
                      color: "#475569",
                      marginBottom: "5px",
                    }}
                  >
                    To Date
                  </label>

                  <input
                    type="date"
                    value={toDate}
                    onChange={(e) =>
                      setToDate(e.target.value)
                    }
                    style={{
                      width: "100%",
                      boxSizing: "border-box",
                      padding: "9px 10px",
                      border:
                        "1px solid #CBD5E1",
                      borderRadius: "7px",
                      outline: "none",
                      marginBottom: "14px",
                    }}
                  />

                  <div
                    style={{
                      display: "flex",
                      justifyContent: "flex-end",
                      gap: "8px",
                    }}
                  >
                    <button
                      type="button"
                      onClick={() => {
                        setFromDate("");
                        setToDate("");
                      }}
                      style={{
                        padding: "8px 12px",
                        borderRadius: "7px",
                        border:
                          "1px solid #CBD5E1",
                        background: "#fff",
                        color: "#475569",
                        cursor: "pointer",
                      }}
                    >
                      Clear
                    </button>

                    <button
                      type="button"
                      onClick={handleApplyFilters}
                      style={{
                        padding: "8px 14px",
                        borderRadius: "7px",
                        border: "none",
                        background: "#F59E0B",
                        color: "#fff",
                        cursor: "pointer",
                        fontWeight: 500,
                      }}
                    >
                      Apply
                    </button>
                  </div>
                </div>
              )}
            </div>

            {/* ================= BUDGET ================= */}

            <div style={{ position: "relative" }}>
              <button
                type="button"
                onClick={() =>
                  setOpenFilter(
                    openFilter === "budget"
                      ? null
                      : "budget"
                  )
                }
                style={{
                  minWidth: "115px",
                  padding: "10px 14px",
                  borderRadius: "9px",
                  border: "1px solid #CBD5E1",
                  background:
                    budgetFrom || budgetTo
                      ? "#FFF7E6"
                      : "#fff",
                  color: "#334155",
                  cursor: "pointer",
                  fontSize: "14px",
                  textAlign: "left",
                }}
              >
                Budget
                <span
                  style={{
                    float: "right",
                    marginLeft: "10px",
                  }}
                >
                  ▾
                </span>
              </button>

              {openFilter === "budget" && (
                <div
                  style={{
                    position: "absolute",
                    top: "calc(100% + 6px)",
                    left: 0,
                    zIndex: 20,
                    width: "280px",
                    background: "#fff",
                    border: "1px solid #E2E8F0",
                    borderRadius: "10px",
                    boxShadow:
                      "0 10px 25px rgba(0,0,0,0.15)",
                    padding: "15px",
                  }}
                >
                  <div
                    style={{
                      fontWeight: 600,
                      color: "#1E293B",
                      marginBottom: "12px",
                    }}
                  >
                    Budget
                  </div>

                  {/* FROM AMOUNT */}

                  <label
                    style={{
                      display: "block",
                      fontSize: "13px",
                      fontWeight: 500,
                      color: "#475569",
                      marginBottom: "5px",
                    }}
                  >
                    From Amount
                  </label>

                  <input
                    type="number"
                    min="0"
                    value={budgetFrom}
                    onChange={(e) =>
                      setBudgetFrom(e.target.value)
                    }
                    placeholder="Minimum budget"
                    style={{
                      width: "100%",
                      boxSizing: "border-box",
                      padding: "9px 10px",
                      border:
                        "1px solid #CBD5E1",
                      borderRadius: "7px",
                      outline: "none",
                      marginBottom: "12px",
                    }}
                  />

                  {/* TO AMOUNT */}

                  <label
                    style={{
                      display: "block",
                      fontSize: "13px",
                      fontWeight: 500,
                      color: "#475569",
                      marginBottom: "5px",
                    }}
                  >
                    To Amount
                  </label>

                  <input
                    type="number"
                    min="0"
                    value={budgetTo}
                    onChange={(e) =>
                      setBudgetTo(e.target.value)
                    }
                    placeholder="Maximum budget"
                    style={{
                      width: "100%",
                      boxSizing: "border-box",
                      padding: "9px 10px",
                      border:
                        "1px solid #CBD5E1",
                      borderRadius: "7px",
                      outline: "none",
                      marginBottom: "14px",
                    }}
                  />

                  <div
                    style={{
                      display: "flex",
                      justifyContent: "flex-end",
                      gap: "8px",
                    }}
                  >
                    <button
                      type="button"
                      onClick={() => {
                        setBudgetFrom("");
                        setBudgetTo("");
                      }}
                      style={{
                        padding: "8px 12px",
                        borderRadius: "7px",
                        border:
                          "1px solid #CBD5E1",
                        background: "#fff",
                        color: "#475569",
                        cursor: "pointer",
                      }}
                    >
                      Clear
                    </button>

                    <button
                      type="button"
                      onClick={handleApplyFilters}
                      style={{
                        padding: "8px 14px",
                        borderRadius: "7px",
                        border: "none",
                        background: "#F59E0B",
                        color: "#fff",
                        cursor: "pointer",
                        fontWeight: 500,
                      }}
                    >
                      Apply
                    </button>
                  </div>
                </div>
              )}
            </div>

            {/* ================= SEARCH/APPLY ================= */}

            <button
              type="button"
              onClick={handleApplyFilters}
              style={{
                padding: "10px 16px",
                borderRadius: "9px",
                border: "none",
                background: "#1E3A5F",
                color: "#fff",
                cursor: "pointer",
                fontWeight: 500,
              }}
            >
              Apply
            </button>

            {/* ================= CLEAR ================= */}

            {hasActiveFilters && (
              <button
                type="button"
                onClick={handleClearFilters}
                style={{
                  padding: "10px 14px",
                  borderRadius: "9px",
                  border: "1px solid #CBD5E1",
                  background: "#fff",
                  color: "#475569",
                  cursor: "pointer",
                  fontWeight: 500,
                }}
              >
                Clear
              </button>
            )}
          </div>

          {/* =================================================
              SUCCESS MESSAGE
          ================================================= */}

          {successMessage && (
            <div
              style={{
                color: "#16A34A",
                marginBottom: "10px",
                fontWeight: 500,
              }}
            >
              {successMessage}
            </div>
          )}

          {/* =================================================
              ERROR MESSAGE
          ================================================= */}

          {error && (
            <div
              style={{
                color: "#DC2626",
                background: "#FEF2F2",
                border: "1px solid #FECACA",
                padding: "9px 12px",
                borderRadius: "7px",
                marginBottom: "12px",
              }}
            >
              {error}
            </div>
          )}

          {/* =================================================
              TABLE
          ================================================= */}

          {loading ? (
            <p
              style={{
                textAlign: "center",
                padding: "20px",
              }}
            >
              Loading...
            </p>
          ) : (
            <table
              style={{
                width: "100%",
                borderCollapse: "collapse",
                borderRadius: "12px",
                overflow: "hidden",
                tableLayout: "fixed",
              }}
            >
              {/* ================= COLUMN WIDTHS ================= */}

              <colgroup>
                <col style={{ width: "14%" }} />
                <col style={{ width: "11%" }} />
                <col style={{ width: "14%" }} />
                <col style={{ width: "13%" }} />
                <col style={{ width: "13%" }} />

                <col
                  style={{
                    width: expandAddressColumn
                      ? "25%"
                      : "15%",
                    transition: "width 0.3s ease",
                  }}
                />

                <col style={{ width: "10%" }} />
              </colgroup>

              {/* ================= TABLE HEADER ================= */}

              <thead
                style={{
                  background: "#1E3A5F",
                  color: "#fff",
                }}
              >
                <tr>
                  <th
                    style={{
                      padding: "10px 12px",
                      textAlign: "left",
                      fontSize: "14px",
                      fontWeight: 600,
                      whiteSpace: "nowrap",
                      borderRight:
                        "3px solid rgba(255,255,255,0.95)",
                    }}
                  >
                    Site Name
                  </th>

                  <th
                    style={{
                      padding: "10px 12px",
                      textAlign: "left",
                      fontSize: "14px",
                      fontWeight: 600,
                      whiteSpace: "nowrap",
                      borderRight:
                        "3px solid rgba(255,255,255,0.95)",
                    }}
                  >
                    Started Date
                  </th>

                  <th
                    style={{
                      padding: "10px 12px",
                      textAlign: "left",
                      fontSize: "14px",
                      fontWeight: 600,
                      whiteSpace: "nowrap",
                      borderRight:
                        "3px solid rgba(255,255,255,0.95)",
                    }}
                  >
                    Status
                  </th>

                  <th
                    style={{
                      padding: "10px 12px",
                      textAlign: "left",
                      fontSize: "14px",
                      fontWeight: 600,
                      whiteSpace: "nowrap",
                      borderRight:
                        "3px solid rgba(255,255,255,0.95)",
                    }}
                  >
                    Contact Name
                  </th>

                  <th
                    style={{
                      padding: "10px 12px",
                      textAlign: "left",
                      fontSize: "14px",
                      fontWeight: 600,
                      whiteSpace: "nowrap",
                      borderRight:
                        "3px solid rgba(255,255,255,0.95)",
                    }}
                  >
                    Contact Number
                  </th>

                  <th
                    style={{
                      padding: "10px 12px",
                      textAlign: "left",
                      fontSize: "14px",
                      fontWeight: 600,
                      whiteSpace: "nowrap",
                      borderRight:
                        "3px solid rgba(255,255,255,0.95)",
                    }}
                  >
                    Address
                  </th>

                  <th
                    style={{
                      padding: "10px 12px",
                      textAlign: "left",
                      fontSize: "14px",
                      fontWeight: 600,
                      whiteSpace: "nowrap",
                    }}
                  >
                    Action
                  </th>
                </tr>
              </thead>

              {/* ================= TABLE BODY ================= */}

              <tbody>
                {sites.length > 0 ? (
                  sites.map((site, index) => (
                    <tr
                      key={site.id}
                      style={{
                        borderBottom:
                          "1px solid #E5E7EB",
                        background:
                          index % 2 === 1
                            ? "#F3F4F6"
                            : "#FFFFFF",
                      }}
                      onMouseEnter={(e) => {
                        e.currentTarget.style.background =
                          "#F9FAFB";
                      }}
                      onMouseLeave={(e) => {
                        e.currentTarget.style.background =
                          index % 2 === 1
                            ? "#F3F4F6"
                            : "#FFFFFF";
                      }}
                    >
                      {/* ================= SITE NAME ================= */}

                      <td
                        style={{
                          padding: "8px 12px",
                          textAlign: "left",
                          verticalAlign: "middle",
                          borderRight:
                            "1px solid #E5E7EB",
                        }}
                      >
                        <Link
                          to={`/site-detail/${site.id}`}
                          style={{
                            color: "inherit",
                            textDecoration: "none",
                            fontWeight: "inherit",
                          }}
                        >
                          <b>{site.name}</b>
                        </Link>
                      </td>

                      {/* ================= STARTED DATE ================= */}

                      <td
                        style={{
                          padding: "8px 12px",
                          textAlign: "left",
                          verticalAlign: "middle",
                          borderRight:
                            "1px solid #E5E7EB",
                          whiteSpace: "nowrap",
                        }}
                      >
                        {site.startedDate
                          ? fmt(site.startedDate)
                          : "—"}
                      </td>

                      {/* ================= STATUS ================= */}

                      <td
                        style={{
                          padding: "8px 12px",
                          textAlign: "left",
                          verticalAlign: "middle",
                          borderRight:
                            "1px solid #E5E7EB",
                        }}
                      >
                        {site.status ?? "—"}
                      </td>

                      {/* ================= CONTACT NAME ================= */}

                      <td
                        style={{
                          padding: "8px 12px",
                          textAlign: "left",
                          verticalAlign: "middle",
                          borderRight:
                            "1px solid #E5E7EB",
                        }}
                      >
                        {site.contactName ?? "—"}
                      </td>

                      {/* ================= CONTACT NUMBER ================= */}

                      <td
                        style={{
                          padding: "8px 12px",
                          textAlign: "left",
                          verticalAlign: "middle",
                          borderRight:
                            "1px solid #E5E7EB",
                          whiteSpace: "nowrap",
                        }}
                      >
                        {site.contactNumber ?? "—"}
                      </td>

                      {/* ================= ADDRESS ================= */}

                      <td
                        onClick={() =>
                          setExpandAddressColumn(
                            (current) => !current
                          )
                        }
                        title={
                          expandAddressColumn
                            ? "Click to collapse Address column"
                            : "Click to expand Address column"
                        }
                        style={{
                          padding: "8px 12px",
                          textAlign: "left",
                          verticalAlign: "middle",
                          color: "#64748B",
                          cursor: "pointer",
                          width: expandAddressColumn
                            ? "25%"
                            : "15%",
                          whiteSpace:
                            expandAddressColumn
                              ? "normal"
                              : "nowrap",
                          overflow: "hidden",
                          textOverflow:
                            expandAddressColumn
                              ? "clip"
                              : "ellipsis",
                          wordBreak:
                            expandAddressColumn
                              ? "break-word"
                              : "normal",
                          borderRight:
                            "1px solid #E5E7EB",
                          transition:
                            "width 0.3s ease",
                        }}
                      >
                        {getAddressText(site)}
                      </td>

                      {/* ================= ACTION ================= */}

                      <td
                        style={{
                          padding: "8px 12px",
                          textAlign: "left",
                          verticalAlign: "middle",
                          whiteSpace: "nowrap",
                        }}
                      >
                        <Link
                          to={`/site-edit/${site.id}`}
                          style={{
                            color: "#16A34A",
                            fontWeight: 500,
                            textDecoration: "none",
                            marginRight: "8px",
                          }}
                        >
                          Edit
                        </Link>

                        {" | "}

                        <button
                          onClick={() =>
                            handleDelete(site.id)
                          }
                          style={{
                            color: "#DC2626",
                            fontWeight: 500,
                            background: "none",
                            border: "none",
                            cursor: "pointer",
                            padding: 0,
                            marginLeft: "8px",
                          }}
                        >
                          Delete
                        </button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td
                      colSpan={7}
                      style={{
                        textAlign: "center",
                        padding: "15px",
                        color: "#888",
                      }}
                    >
                      No site records found.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </div>
  );
}

export default SiteList;
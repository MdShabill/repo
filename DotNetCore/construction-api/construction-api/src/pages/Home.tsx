// Path: src/pages/Home.tsx

import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import { getDashboardStats } from "../services/dashboardService";
import type { DashboardStatsDto } from "../services/dashboardService";

import { useSite } from "../context/Sitecontext";

function Home() {
  // ==================================================
  // 1. SELECTED SITE
  // ==================================================

  const { selectedSite } = useSite();

  /*
    selectedSite = null
      => siteId = 0
      => All Sites ka complete data

    selectedSite = { id: 5, ... }
      => siteId = 5
      => Sirf selected site ka complete data
  */

  const siteId = selectedSite?.id ?? 0;


  // ==================================================
  // 2. DASHBOARD STATE
  // ==================================================

  const [stats, setStats] =
    useState<DashboardStatsDto | null>(null);

  const [statsLoading, setStatsLoading] =
    useState(true);

  const [statsError, setStatsError] =
    useState("");


  // ==================================================
  // 3. LOAD DASHBOARD STATS
  // ==================================================

  useEffect(() => {
    /*
      API call hogi:

      1. Home page open hone par
      2. Navbar se site select karne par
      3. Site change karne par

      siteId change hoga,
      aur dashboard ka data dobara load hoga.
    */

    setStatsLoading(true);
    setStatsError("");

    getDashboardStats(siteId)
      .then((data) => {
        setStats(data);
      })
      .catch((error) => {
        console.error("Dashboard stats error:", error);

        setStats(null);
        setStatsError(
          "Unable to load dashboard statistics."
        );
      })
      .finally(() => {
        setStatsLoading(false);
      });

  }, [siteId]);


  // ==================================================
  // 4. FORMAT CURRENCY
  // ==================================================

  const formatCurrency = (value: number) => {
    return "₹" + value.toLocaleString("en-IN", {
      maximumFractionDigits: 0,
    });
  };


  // ==================================================
  // 5. REUSABLE CARD STYLES
  // ==================================================

  const miniCard: React.CSSProperties = {
    background: "rgba(255,255,255,0.06)",
    border: "1px solid rgba(255,255,255,0.12)",
    borderRadius: "9px",
    padding: "14px",
  };

  const miniLabel: React.CSSProperties = {
    fontSize: "10px",
    color: "#94A3B8",
    textTransform: "uppercase",
    letterSpacing: "0.5px",
    margin: "0 0 5px 0",
  };

  const miniValue: React.CSSProperties = {
    fontSize: "22px",
    fontWeight: 600,
    color: "#fff",
    margin: 0,
  };

  const miniSub: React.CSSProperties = {
    fontSize: "12px",
    margin: "5px 0 0 0",
  };


  // ==================================================
  // UI
  // ==================================================

  return (
    <div
      style={{
        margin: 0,
        padding: 0,
      }}
    >

      {/* ==================================================
          HERO SECTION
      ================================================== */}

      <section
        style={{
          background: "#1c2e46",
          padding: "52px 20px",
        }}
      >

        <div
          style={{
            display: "flex",
            alignItems: "flex-start",
            gap: "40px",
            maxWidth: "1100px",
            margin: "0 auto",
          }}
        >

          {/* ==================================================
              LEFT SIDE: HERO CONTENT
          ================================================== */}

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
                border: "1px solid rgba(245,158,11,0.35)",
                color: "#FCD34D",
                fontSize: "12px",
                padding: "4px 12px",
                borderRadius: "20px",
                marginBottom: "14px",
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


            {/* Heading */}

            <h1
              style={{
                fontSize: "36px",
                fontWeight: 600,
                color: "#fff",
                margin: "0 0 10px 0",
                lineHeight: 1.25,
              }}
            >

              Control every rupee,
              <br />

              <span
                style={{
                  color: "#F59E0B",
                }}
              >
                every site, every day.
              </span>

            </h1>


            {/* Description */}

            <p
              style={{
                fontSize: "13px",
                color: "#94A3B8",
                margin: "0 0 18px 0",
                maxWidth: "600px",
              }}
            >
              BuilderLedger brings your material purchases, labour attendance,
              payments, site expenses into one place, so nothing slips through
              the cracks.
            </p>


            {/* Buttons */}

            <div
              style={{
                display: "flex",
                gap: "10px",
              }}
            >

              <Link
                to="/material-report"
                style={{
                  background: "#F59E0B",
                  color: "#1a2332",
                  fontSize: "13.5px",
                  fontWeight: 600,
                  padding: "10px 22px",
                  borderRadius: "7px",
                  textDecoration: "none",
                  display: "inline-block",
                }}
              >
                View material report
              </Link>


              <Link
                to="/cost-master"
                style={{
                  background: "transparent",
                  color: "#fff",
                  fontSize: "13.5px",
                  padding: "10px 22px",
                  borderRadius: "7px",
                  border: "1px solid rgba(255,255,255,0.3)",
                  textDecoration: "none",
                  display: "inline-block",
                }}
              >
                Open cost master
              </Link>

            </div>

          </div>


          {/* ==================================================
              RIGHT SIDE: DASHBOARD STATS
          ================================================== */}

          <div
            style={{
              width: "235px",
              flexShrink: 0,
              display: "flex",
              flexDirection: "column",
              gap: "10px",
            }}
          >

            {/* ==================================================
                LOADING STATE
            ================================================== */}

            {statsLoading && (
              <div style={miniCard}>

                <p
                  style={{
                    color: "#94A3B8",
                    fontSize: "13px",
                    margin: 0,
                  }}
                >
                  Loading dashboard...
                </p>

              </div>
            )}


            {/* ==================================================
                ERROR STATE
            ================================================== */}

            {!statsLoading && statsError && (
              <div style={miniCard}>

                <p
                  style={{
                    color: "#F87171",
                    fontSize: "13px",
                    margin: 0,
                  }}
                >
                  {statsError}
                </p>

              </div>
            )}


            {/* ==================================================
                DASHBOARD CARDS
            ================================================== */}

            {!statsLoading && !statsError && stats && (
              <>

                {/* ==============================================
                    CARD 1: TOTAL MATERIAL SPEND
                ============================================== */}

                <div style={miniCard}>

                  <p style={miniLabel}>
                    Total Material Spend
                  </p>


                  <p style={miniValue}>
                    {formatCurrency(
                      stats.totalMaterialSpend
                    )}
                  </p>


                  <p
                    style={{
                      ...miniSub,
                      color: "#F59E0B",
                    }}
                  >
                    all-time material cost
                  </p>

                </div>


                {/* ==============================================
                    CARD 2: TOTAL PURCHASES
                ============================================== */}

                <div style={miniCard}>

                  <p style={miniLabel}>
                    Total Purchases
                  </p>


                  <p style={miniValue}>
                    {stats.totalPurchases}
                  </p>


                  <p
                    style={{
                      ...miniSub,
                      color: "#F59E0B",
                    }}
                  >
                    all-time material entries
                  </p>

                </div>


                {/* ==============================================
                    CARD 3: TOTAL ATTENDANCE
                ============================================== */}

                <div style={miniCard}>

                  <p style={miniLabel}>
                    Total Attendance
                  </p>


                  <p style={miniValue}>
                    {stats.totalAttendanceWorkers}
                  </p>


                  <p
                    style={{
                      ...miniSub,
                      color: "#4ADE80",
                    }}
                  >
                    all-time workers
                  </p>

                </div>

              </>
            )}

          </div>

        </div>

      </section>


      {/* ==================================================
          FEATURES SECTION
      ================================================== */}

      <section
        style={{
          background: "#fff",
          padding: "26px 20px 50px",
        }}
      >

        <div
          style={{
            maxWidth: "1030px",
            margin: "0 auto",
          }}
        >

          <p
            style={{
              color: "#94A3B8",
              fontSize: "12px",
              letterSpacing: "1px",
              margin: "0 0 8px 0",
            }}
          >
            WHAT BUILDERLEDGER DOES
          </p>


          <h2
            style={{
              color: "#14243A",
              fontSize: "24px",
              margin: "0 0 8px 0",
            }}
          >
            Everything your site needs, in one system
          </h2>


          <p
            style={{
              color: "#64748B",
              margin: "0 0 22px 0",
            }}
          >
            Designed for construction teams managing multiple sites,
            vendors, and budgets simultaneously.
          </p>


          {/* Feature Cards */}

          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(3, 1fr)",
              gap: "18px",
            }}
          >

            <FeatureCard
              title="Cost master"
              description="Define standard rates for materials, labour, and equipment. All transactions reference these rates automatically."
            />


            <FeatureCard
              title="Attendance tracking"
              description="Record daily worker attendance per site. Calculate wages, overtime, and absenteeism across your workforce."
            />


            <FeatureCard
              title="Material reports"
              description="Track every purchase, quantity, brand, supplier, and cost. Identify overspend and compare against budget."
            />


            <FeatureCard
              title="Service providers"
              description="Manage your contractors, workers, and service providers in one centralized system."
            />


            <FeatureCard
              title="Site-wise reports"
              description="View construction activity and financial information site by site."
            />


            <FeatureCard
              title="Expense control"
              description="Track expenses and maintain better control over your construction budget."
            />

          </div>

        </div>

      </section>


      {/* ==================================================
          FOOTER
      ================================================== */}

      <footer
        style={{
          background: "#1c2e46",
          color: "#94A3B8",
          textAlign: "center",
          padding: "20px",
          fontSize: "12px",
        }}
      >
        BuilderLedger © 2026. Construction management made simple.
      </footer>

    </div>
  );
}


// ==================================================
// FEATURE CARD COMPONENT
// ==================================================

function FeatureCard({
  title,
  description,
}: {
  title: string;
  description: string;
}) {
  return (
    <div
      style={{
        border: "1px solid #E2E8F0",
        borderRadius: "14px",
        padding: "20px",
        minHeight: "125px",
      }}
    >

      <h3
        style={{
          color: "#14243A",
          fontSize: "16px",
          margin: "0 0 10px 0",
        }}
      >
        {title}
      </h3>


      <p
        style={{
          color: "#64748B",
          fontSize: "13px",
          lineHeight: 1.7,
          margin: 0,
        }}
      >
        {description}
      </p>

    </div>
  );
}


export default Home;
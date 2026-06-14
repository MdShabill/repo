import { Link } from "react-router-dom";

function Home() {
  return (
    <div style={{ margin: 0, padding: 0 }}>
      {/* ── HERO ── */}
      <section
        style={{
          background: "#1c2e46",
          padding: "60px 20px",
        }}
      >
        <div
          style={{
            display: "flex",
            alignItems: "flex-start",
            gap: "20px",
            maxWidth: "1100px",
            margin: "0 auto",
          }}
        >
          <div style={{ flex: 1, minWidth: 0 }}>
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
                marginBottom: "14px",
              }}
            >
              <span
                style={{
                  width: "7px",
                  height: "7px",
                  background: "#F59E0B",
                  borderRadius: "50%",
                  flexShrink: 0,
                  display: "inline-block",
                }}
              />
              Construction management platform
            </div>

            {/* Title */}
            <h1
              style={{
                fontSize: "36px",
                fontWeight: 600,
                color: "#fff",
                marginBottom: "10px",
                lineHeight: 1.25,
              }}
            >
              Control every rupee,
              <br />
              <span style={{ color: "#F59E0B" }}>every site, every day.</span>
            </h1>

            {/* Desc */}
            <p
              style={{
                fontSize: "13px",
                color: "#94A3B8",
                marginBottom: "16px",
                maxWidth: "600px",
              }}
            >
              BuilderLedger brings your material purchases, labour attendance,
              payments, site expenses into one place — so nothing slips through
              the cracks.
            </p>

            {/* CTAs */}
            <div style={{ display: "flex", gap: "10px" }}>
              <Link
                to="/material"
                style={{
                  background: "#F59E0B",
                  color: "#1a2332",
                  fontSize: "13.5px",
                  fontWeight: 600,
                  padding: "10px 22px",
                  borderRadius: "7px",
                  border: "none",
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
                  border: "0.5px solid rgba(255,255,255,0.3)",
                  textDecoration: "none",
                  display: "inline-block",
                }}
              >
                Open cost master
              </Link>
            </div>
          </div>
        </div>
      </section>

      {/* ── FEATURES ── */}
      <section
        style={{
          padding: "32px 20px 16px",
          maxWidth: "1100px",
          margin: "0 auto",
        }}
      >
        <p
          style={{
            fontSize: "11px",
            fontWeight: 600,
            color: "#888780",
            textTransform: "uppercase",
            letterSpacing: "0.7px",
            marginBottom: "6px",
          }}
        >
          What BuilderLedger does
        </p>
        <h2 style={{ fontSize: "20px", color: "#1a2332", marginBottom: "4px" }}>
          Everything your site needs, in one system
        </h2>
        <p
          style={{
            fontSize: "13px",
            color: "#64748B",
            marginBottom: "20px",
            lineHeight: 1.65,
          }}
        >
          Designed for construction teams managing multiple sites, vendors, and
          budgets simultaneously.
        </p>

        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(3, 1fr)",
            gap: "14px",
          }}
        >
          {[
            {
              bg: "#FAEEDA",
              icon: (
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <rect
                    x="2"
                    y="2"
                    width="5"
                    height="5"
                    rx="1"
                    fill="#854F0B"
                  />
                  <rect
                    x="9"
                    y="2"
                    width="5"
                    height="5"
                    rx="1"
                    fill="#854F0B"
                  />
                  <rect
                    x="2"
                    y="9"
                    width="5"
                    height="5"
                    rx="1"
                    fill="#854F0B"
                  />
                  <rect
                    x="9"
                    y="9"
                    width="5"
                    height="5"
                    rx="1"
                    fill="#EF9F27"
                  />
                </svg>
              ),
              title: "Cost master",
              desc: "Define standard rates for materials, labour, and equipment. All transactions reference these rates automatically.",
            },
            {
              bg: "#E6F1FB",
              icon: (
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <circle
                    cx="8"
                    cy="6"
                    r="3"
                    stroke="#185FA5"
                    strokeWidth="1.2"
                  />
                  <path
                    d="M3 13c0-2.76 2.24-5 5-5s5 2.24 5 5"
                    stroke="#185FA5"
                    strokeWidth="1.2"
                    strokeLinecap="round"
                  />
                </svg>
              ),
              title: "Attendance tracking",
              desc: "Record daily worker attendance per site. Calculate wages, overtime, and absenteeism across your workforce.",
            },
            {
              bg: "#E1F5EE",
              icon: (
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <path
                    d="M2 13L5 7l3 3 3-4 3 4"
                    stroke="#0F6E56"
                    strokeWidth="1.2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
              ),
              title: "Material reports",
              desc: "Track every purchase — quantity, brand, supplier, and cost. Identify overspend and compare against budget.",
            },
            {
              bg: "#EEEDFE",
              icon: (
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <rect
                    x="2"
                    y="4"
                    width="12"
                    height="9"
                    rx="1.5"
                    stroke="#534AB7"
                    strokeWidth="1.2"
                  />
                  <path
                    d="M5 4V3a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v1"
                    stroke="#534AB7"
                    strokeWidth="1.2"
                  />
                  <path
                    d="M5 8h6M5 11h3"
                    stroke="#534AB7"
                    strokeWidth="1"
                    strokeLinecap="round"
                  />
                </svg>
              ),
              title: "Service providers",
              desc: "Maintain a directory of contractors and vendors. Log work orders, payments, and performance per site.",
            },
            {
              bg: "#EAF3DE",
              icon: (
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <rect
                    x="2"
                    y="2"
                    width="12"
                    height="3"
                    rx="1"
                    fill="#3B6D11"
                  />
                  <rect
                    x="2"
                    y="6.5"
                    width="12"
                    height="3"
                    rx="1"
                    fill="#3B6D11"
                  />
                  <rect
                    x="2"
                    y="11"
                    width="7"
                    height="3"
                    rx="1"
                    fill="#3B6D11"
                  />
                </svg>
              ),
              title: "Site-wise reports",
              desc: "Switch between Shaktinagar, Obra, and Lucknow to see isolated spend summaries, P&L, and material usage.",
            },
            {
              bg: "#FAECE7",
              icon: (
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <path
                    d="M8 2v4M8 10v4M2 8h4M10 8h4"
                    stroke="#993C1D"
                    strokeWidth="1.3"
                    strokeLinecap="round"
                  />
                  <circle cx="8" cy="8" r="2" fill="#F0997B" />
                </svg>
              ),
              title: "Expense control",
              desc: "Set budgets per site and material category. Get visibility into variances before they become problems.",
            },
          ].map((f) => (
            <div
              key={f.title}
              style={{
                background: "#F8F7F4",
                border: "0.5px solid #E2E0D8",
                borderRadius: "12px",
                padding: "14px 16px",
              }}
            >
              <div
                style={{
                  display: "flex",
                  alignItems: "center",
                  gap: "10px",
                  marginBottom: "8px",
                }}
              >
                <div
                  style={{
                    width: "30px",
                    height: "30px",
                    borderRadius: "7px",
                    background: f.bg,
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    flexShrink: 0,
                  }}
                >
                  {f.icon}
                </div>
                <span
                  style={{
                    fontSize: "13.5px",
                    fontWeight: 600,
                    color: "#1a2332",
                  }}
                >
                  {f.title}
                </span>
              </div>
              <p
                style={{
                  fontSize: "12.5px",
                  color: "#64748B",
                  lineHeight: 1.6,
                  margin: 0,
                }}
              >
                {f.desc}
              </p>
            </div>
          ))}
        </div>
      </section>

      {/* ── QUICK ACCESS ── */}
      <section
        style={{
          padding: "16px 32px 36px",
          maxWidth: "1100px",
          margin: "0 auto",
        }}
      >
        <div
          style={{
            fontSize: "15px",
            fontWeight: 600,
            color: "#1a2332",
            marginBottom: "12px",
          }}
        >
          Quick access
        </div>
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(3, 1fr)",
            gap: "10px",
          }}
        >
          {[
            {
              to: "/cost-master",
              dot: "#F59E0B",
              name: "Cost master",
              sub: "Rates & price lists",
            },
            {
              to: "/attendance",
              dot: "#378ADD",
              name: "Attendance",
              sub: "Daily worker log",
            },
            {
              to: "/material",
              dot: "#1D9E75",
              name: "Material report",
              sub: "Purchases & stock",
            },
            {
              to: "/service-provider",
              dot: "#7F77DD",
              name: "Service providers",
              sub: "Vendors & contractors",
            },
            {
              to: "/sites",
              dot: "#639922",
              name: "Sites",
              sub: "All active sites",
            },
            {
              to: "/home",
              dot: "#D85A30",
              name: "Dashboard",
              sub: "Overview & summary",
            },
          ].map((item) => (
            <Link
              key={item.to}
              to={item.to}
              style={{
                background: "#fff",
                border: "0.5px solid #E2E0D8",
                borderRadius: "9px",
                padding: "13px 16px",
                textDecoration: "none",
                display: "flex",
                alignItems: "center",
                gap: "11px",
              }}
            >
              <span
                style={{
                  width: "9px",
                  height: "9px",
                  borderRadius: "3px",
                  background: item.dot,
                  flexShrink: 0,
                  display: "inline-block",
                }}
              />
              <span>
                <span
                  style={{
                    fontSize: "13px",
                    fontWeight: 600,
                    color: "#1a2332",
                    display: "block",
                  }}
                >
                  {item.name}
                </span>
                <span
                  style={{
                    fontSize: "11.5px",
                    color: "#94A3B8",
                    display: "block",
                    marginTop: "1px",
                  }}
                >
                  {item.sub}
                </span>
              </span>
            </Link>
          ))}
        </div>
      </section>

      {/* ── FOOTER ── */}
      <footer style={{ background: "#1a2332", padding: "28px 32px 0" }}>
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "2fr 1fr 1fr 1fr",
            gap: "24px",
            maxWidth: "1100px",
            margin: "0 auto",
            paddingBottom: "20px",
          }}
        >
          {/* Brand */}
          <div>
            <img
              src="/UploadedImage/WebSiteLogo.jpg"
              alt="Builder Ledger"
              style={{ height: "36px", marginBottom: "10px" }}
            />
            <p
              style={{
                fontSize: "12px",
                color: "#475569",
                lineHeight: 1.65,
                maxWidth: "200px",
                margin: 0,
              }}
            >
              Builder Ledger Construction &amp; Financial Management for site
              supervisors, engineers, and project managers.
            </p>
          </div>

          {/* Modules */}
          <div>
            <div
              style={{
                fontSize: "10.5px",
                fontWeight: 600,
                color: "#64748B",
                textTransform: "uppercase",
                letterSpacing: "0.6px",
                marginBottom: "10px",
              }}
            >
              Modules
            </div>
            {[
              { to: "/cost-master", label: "Cost master" },
              { to: "/attendance", label: "Attendance" },
              { to: "/material", label: "Material report" },
              { to: "/service-provider", label: "Service providers" },
              { to: "/sites", label: "Sites" },
            ].map((l) => (
              <Link
                key={l.to}
                to={l.to}
                style={{
                  display: "block",
                  fontSize: "12.5px",
                  color: "#64748B",
                  textDecoration: "none",
                  marginBottom: "6px",
                }}
              >
                {l.label}
              </Link>
            ))}
          </div>

          {/* Account */}
          <div>
            <div
              style={{
                fontSize: "10.5px",
                fontWeight: 600,
                color: "#64748B",
                textTransform: "uppercase",
                letterSpacing: "0.6px",
                marginBottom: "10px",
              }}
            >
              Account
            </div>
            <Link
              to="/home"
              style={{
                display: "block",
                fontSize: "12.5px",
                color: "#64748B",
                textDecoration: "none",
                marginBottom: "6px",
              }}
            >
              Dashboard
            </Link>
            <Link
              to="/login"
              style={{
                display: "block",
                fontSize: "12.5px",
                color: "#64748B",
                textDecoration: "none",
                marginBottom: "6px",
              }}
            >
              Log out
            </Link>
          </div>

          {/* Camp Offices */}
          <div>
            <div
              style={{
                fontSize: "10.5px",
                fontWeight: 600,
                color: "#64748B",
                textTransform: "uppercase",
                letterSpacing: "0.6px",
                marginBottom: "10px",
              }}
            >
              Camp Offices
            </div>
            {[
              { href: "tel:05446233928", label: "Shaktinagar: 05446-233928" },
              { href: "tel:05446262264", label: "Obra: 05446-262264" },
              { href: "tel:05223200260", label: "Lucknow: 0522-3200260" },
            ].map((c) => (
              <a
                key={c.href}
                href={c.href}
                style={{
                  display: "block",
                  fontSize: "12.5px",
                  color: "#64748B",
                  textDecoration: "none",
                  marginBottom: "6px",
                }}
              >
                {c.label}
              </a>
            ))}
          </div>
        </div>

        {/* Footer Bottom */}
        <div
          style={{
            borderTop: "0.5px solid rgba(255,255,255,0.07)",
            padding: "14px 0",
            maxWidth: "1100px",
            margin: "0 auto",
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <span style={{ fontSize: "11.5px", color: "#334155" }}>
            © 2023 Builder Ledger — Construction &amp; Financial Management
          </span>
          <div style={{ display: "flex", gap: "16px" }}>
            <Link
              to="/privacy"
              style={{
                fontSize: "11.5px",
                color: "#334155",
                textDecoration: "none",
              }}
            >
              Privacy
            </Link>
            <Link
              to="/home"
              style={{
                fontSize: "11.5px",
                color: "#334155",
                textDecoration: "none",
              }}
            >
              About
            </Link>
          </div>
        </div>
      </footer>
    </div>
  );
}

export default Home;

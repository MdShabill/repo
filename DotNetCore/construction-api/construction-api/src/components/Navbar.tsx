import { Link } from "react-router-dom";

function Navbar() {
  const user = JSON.parse(localStorage.getItem("user") || "{}");

  const handleLogout = () => {
    localStorage.removeItem("user");

    window.location.href = "/login";
  };

  return (
    <nav
      className="
navbar
navbar-expand-lg
navbar-light
bg-light
shadow-sm
"
    >
      <div
        className="
container-fluid
px-4
"
      >
        {/* Logo */}

        <Link className="navbar-brand" to="/home">
          <img
            src="/UploadedImage/WebSiteLogo.jpg"
            alt="Builder Ledger"
            style={{ height: "50px" }}
          />
        </Link>

        {/* Mobile */}

        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarContent"
        >
          <span className="navbar-toggler-icon" />
        </button>

        {/* Menu */}

        <div
          className="
collapse
navbar-collapse
"
          id="navbarContent"
        >
          <ul
            className="
navbar-nav
me-auto
"
          >
            <li className="nav-item">
              <Link
                className="
nav-link
fw-bold
"
                to="/home"
              >
                Home
              </Link>
            </li>

            <li className="nav-item">
              <Link
                className="
nav-link
fw-bold
"
                to="/cost-master"
              >
                Cost Master
              </Link>
            </li>

            <li className="nav-item">
              <Link
                className="
nav-link
fw-bold
"
                to="/attendance"
              >
                Attendance
              </Link>
            </li>

            <li className="nav-item">
              <Link
                className="
nav-link
fw-bold
"
                to="/material-report"
              >
                Material Report
              </Link>
            </li>

            <li className="nav-item">
              <Link
                className="
nav-link
fw-bold
"
                to="/service-provider"
              >
                Service Provider
              </Link>
            </li>

            <li className="nav-item">
              <Link
                className="
nav-link
fw-bold
"
                to="/sitelist"
              >
                Site
              </Link>
            </li>
          </ul>

          {/* Right */}

          <div
            className="
d-flex
flex-column
flex-lg-row
align-items-lg-center
gap-2
"
          >
            <select
              className="form-select"
              style={{
                minWidth: "220px",
              }}
            >
              <option>-- Select Site --</option>

              <option>Dr Shabbir Sb - Suja</option>
            </select>

            <button
              className="
btn
btn-success
"
            >
              Go
            </button>

            <span
              className="
fw-semibold
text-nowrap
"
            >
              Welcome, {user?.name || user?.email || "User"}
            </span>

            <button
              className="
btn
btn-outline-danger
"
              onClick={handleLogout}
            >
              Logout
            </button>
          </div>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;

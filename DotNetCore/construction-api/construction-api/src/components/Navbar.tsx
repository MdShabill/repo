import { Link } from "react-router-dom";

function Navbar() {
  const user = JSON.parse(localStorage.getItem("user") || "{}");

  const handleLogout = () => {
    localStorage.removeItem("user");
    window.location.href = "/login";
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
      <div className="container-fluid px-4">
        {/* Logo */}
        <Link className="navbar-brand" to="/">
          <img src="/logo.png" height="45" alt="logo" />
        </Link>

        {/* Mobile Toggle */}
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarContent"
        >
          <span className="navbar-toggler-icon"></span>
        </button>

        {/* Menu */}
        <div className="collapse navbar-collapse" id="navbarContent">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <Link className="nav-link fw-bold" to="/">
                Home
              </Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link fw-bold" to="/cost-master">
                Cost Master
              </Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link fw-bold" to="/attendance">
                Attendance
              </Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link fw-bold" to="/material-report">
                Material Report
              </Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link fw-bold" to="/service-provider">
                Service Provider
              </Link>
            </li>

            <li className="nav-item">
              <Link className="nav-link fw-bold" to="/sitelist">
                Site
              </Link>
            </li>
          </ul>

          {/* Right Section */}
          <div className="d-flex flex-column flex-lg-row align-items-lg-center gap-2">
            <select className="form-select" style={{ minWidth: "200px" }}>
              <option>-- Select Site --</option>
              <option>Dr Shabbir Sb - Suja</option>
            </select>

            <button className="btn btn-success">Go</button>

            <span className="fw-semibold text-nowrap">
              Welcome, {user?.name}
            </span>

            <button
              onClick={handleLogout}
              className="btn btn-link text-decoration-none"
            >
              Log Out
            </button>
          </div>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;

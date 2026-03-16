function Home() {
  return (
    <>
      {/* Hero Section */}
      <div
        style={{
          backgroundImage: "url('/UploadedImage/Background-Image.jpg')",
          backgroundSize: "cover",
          backgroundPosition: "center",
          minHeight: "35vh",
          display: "flex",
          alignItems: "center",
        }}
      >
        <div className="container-fluid text-center px-4">
          <h2 className="fw-bold">Your Partner in Construction Management</h2>

          <p className="lead">
            Manage your construction material purchases, track expenses, and
            optimize your resources with ease.
          </p>

          <button className="btn btn-outline-primary btn-lg">Learn More</button>
        </div>
      </div>

      {/* About */}
      <div className="container-fluid mt-4 px-4">
        <div
          className="text-center mx-auto p-4"
          style={{ backgroundColor: "#add8e6", maxWidth: "1100px" }}
        >
          <p>
            India's No.1 tech-enabled construction company that aims to make
            construction simple, transparent, and reliable.
          </p>

          <p>
            We provide end-to-end design-to-build construction services with
            real-time tracking and project updates.
          </p>
        </div>
      </div>

      {/* Features */}
      <div className="container-fluid mt-5 px-4">
        <div className="row g-4 text-center">
          <div className="col-lg-4 col-md-6">
            <div
              className="card shadow-sm h-100"
              style={{ background: "lightblue" }}
            >
              <div className="card-body">
                <i className="fas fa-truck fa-3x mb-3 text-primary"></i>
                <h5 className="card-title">Track Deliveries</h5>
                <p>Monitor material deliveries in real-time.</p>
              </div>
            </div>
          </div>

          <div className="col-lg-4 col-md-6">
            <div
              className="card shadow-sm h-100"
              style={{ background: "lightyellow" }}
            >
              <div className="card-body">
                <i className="fas fa-file-invoice-dollar fa-3x mb-3 text-primary"></i>
                <h5 className="card-title">Manage Costs</h5>
                <p>Keep track of expenses and calculate total costs.</p>
              </div>
            </div>
          </div>

          <div className="col-lg-4 col-md-12">
            <div
              className="card shadow-sm h-100"
              style={{ background: "skyblue" }}
            >
              <div className="card-body">
                <i className="fas fa-tools fa-3x mb-3 text-primary"></i>
                <h5 className="card-title">Material Tracking</h5>
                <p>Manage materials, suppliers, and brands easily.</p>
              </div>
            </div>
          </div>
        </div>

        {/* CTA */}
        <div className="text-center mt-5">
          <h3 className="fw-bold">Ready to Get Started?</h3>

          <p className="lead">
            Experience the most efficient way to manage your construction
            resources.
          </p>

          <button className="btn btn-success btn-lg">Sign Up Now</button>
        </div>
      </div>

      {/* Footer */}
      <footer className="bg-dark text-white mt-5">
        <div className="container-fluid py-4 px-4">
          <div className="row g-4">
            <div className="col-lg-3 col-md-6">
              <h5>Menu</h5>
              <ul className="list-unstyled">
                <li>About Us</li>
                <li>Projects</li>
                <li>Cost Master</li>
                <li>Attendance</li>
                <li>Material Report</li>
                <li>Contact Us</li>
              </ul>
            </div>

            <div className="col-lg-3 col-md-6">
              <h5>Follow Us</h5>
              <ul className="list-unstyled">
                <li>Facebook</li>
                <li>Instagram</li>
                <li>LinkedIn</li>
              </ul>
            </div>

            <div className="col-lg-4 col-md-12">
              <h5>Camp Office Contact</h5>
              <ul className="list-unstyled">
                <li>Shaktinagar: 05446-233928</li>
                <li>Obra: 05446-262264</li>
                <li>Lucknow: 0522-3200260</li>
              </ul>
            </div>
          </div>

          <div className="text-center mt-4">
            <small>
              © 2024 Ashoka Construction Company. All Rights Reserved.
            </small>
          </div>
        </div>
      </footer>
    </>
  );
}

export default Home;

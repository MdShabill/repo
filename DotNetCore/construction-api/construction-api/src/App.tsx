// Path: src/App.tsx
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider, useAuth }  from "./context/Authcontext";
import { SiteProvider }           from "./context/Sitecontext";
import SessionCheck               from "./components/Sessioncheck";
import RequireAuth                from "./components/Requireauth";
import Navbar                     from "./components/Navbar";
import Login                      from "./pages/Login";
import Home                       from "./pages/Home";
import NoSiteSelected             from "./pages/Nositeselected";
import CostMasterList             from "./pages/CostMaster/CostMasterList";
import CostMasterAdd              from "./pages/CostMaster/CostMasterAdd";
import CostMasterEdit             from "./pages/CostMaster/CostMasterEdit";
import SiteList                   from "./components/SiteList";
import SiteAdd                    from "./components/SiteAdd";
import SiteEdit                   from "./components/SiteEdit";

function AppLayout({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();
  if (!user) return <Navigate to="/login" replace />;
  return (
    <>
      <Navbar />
      <main style={{ paddingTop: "68px" }}>{children}</main>
    </>
  );
}

function AppRoutes() {
  const { user } = useAuth();

  return (
    <Routes>
      {/* Root always goes to login — login page redirects to home if already logged in */}
      <Route path="/" element={user ? <Navigate to="/home" replace /> : <Navigate to="/login" replace />}/>

      {/* Login: already logged in → home */}
      <Route
        path="/login"
        element={user ? <Navigate to="/home" replace /> : <Login />}
      />

      {/* Home — only login required, no site required */}
      <Route path="/home"
        element={<AppLayout><RequireAuth><Home /></RequireAuth></AppLayout>}
      />

      {/* Site module — login only, no site selection required */}
      <Route path="/sites"
        element={<AppLayout><RequireAuth><SiteList /></RequireAuth></AppLayout>}
      />
      <Route path="/site-add"
        element={<AppLayout><RequireAuth><SiteAdd /></RequireAuth></AppLayout>}
      />
      <Route path="/site-edit/:id"
        element={<AppLayout><RequireAuth><SiteEdit /></RequireAuth></AppLayout>}
      />
      
      {/* No site selected page */}
      <Route path="/no-site-selected"
        element={<AppLayout><RequireAuth><NoSiteSelected /></RequireAuth></AppLayout>}
      />

      {/* Cost Master — login + site both required */}
      <Route path="/cost-master"
        element={<AppLayout><SessionCheck><CostMasterList /></SessionCheck></AppLayout>}
      />
      <Route path="/cost-master/add"
        element={<AppLayout><SessionCheck><CostMasterAdd /></SessionCheck></AppLayout>}
      />

      <Route path="/cost-master/edit/:id"
        element={<AppLayout><SessionCheck><CostMasterEdit/></SessionCheck></AppLayout>}/>

      {/* Catch-all → always login */}
      <Route path="*" element={<Navigate to="/login" replace />} />
    </Routes>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <SiteProvider>
          <AppRoutes />
        </SiteProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider, useAuth } from "./context/Authcontext";
import { SiteProvider } from "./context/Sitecontext";
import SessionCheck from "./components/Sessioncheck";
import RequireAuth from "./components/Requireauth";
import Navbar from "./components/Navbar";

import Login from "./pages/Login";
import Home from "./pages/Home";
import NoSiteSelected from "./pages/Nositeselected";
import CostMasterList from "./pages/CotMaster/CostMasterList";
import CostMasterAdd from "./pages/CotMaster/CostMasterAdd";
import SiteList from "./components/SiteList";
import SiteAdd from "./components/SiteAdd";
import SiteEdit from "./components/SiteEdit";
import SiteDetail from "./components/SiteDetail";

function AppLayout({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();
  if (!user) return <Navigate to="/login" replace />;
  return (
    <>
      <Navbar />
      <main>{children}</main>
    </>
  );
}

function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />

      <Route path="/home" element={<AppLayout><RequireAuth><Home /></RequireAuth></AppLayout>}/>

      <Route path="/sites"        element={<AppLayout><RequireAuth><SiteList /></RequireAuth></AppLayout>} />
      <Route path="/site-add"     element={<AppLayout><RequireAuth><SiteAdd /></RequireAuth></AppLayout>} />
      <Route path="/site-edit/:id" element={<AppLayout><RequireAuth><SiteEdit /></RequireAuth></AppLayout>} />
      <Route path="/site/:id"     element={<AppLayout><RequireAuth><SiteDetail /></RequireAuth></AppLayout>} />

      {/* No-site-selected landing page - login only */}
      <Route path="/no-site-selected" element={<AppLayout><RequireAuth><NoSiteSelected /></RequireAuth></AppLayout>}/>

      <Route path="/cost-master" element={<AppLayout><SessionCheck><CostMasterList /></SessionCheck></AppLayout>} />
      <Route path="/cost-master/add" element={<AppLayout><SessionCheck><CostMasterAdd /></SessionCheck></AppLayout>} />

      <Route path="*" element={<Navigate to="/home" replace />} />
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
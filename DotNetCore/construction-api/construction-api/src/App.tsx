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
import SiteDetail                 from "./components/SiteDetail";
import SiteAdd                    from "./components/SiteAdd";
import SiteEdit                   from "./components/SiteEdit";
import DailyAttendanceList        from "./pages/DailyAttendance/DailyAttendanceList";
import DailyAttendanceAdd         from "./pages/DailyAttendance/DailyAttendanceAdd";
import MaterialPurchaseList       from "./pages/MaterialPurchase/MaterialPurchaseList";
import MaterialPurchaseAdd        from "./pages/MaterialPurchase/MaterialPurchaseAdd";
import ServiceProviderList        from "./pages/ServiceProvider/ServiceProviderList";
import ServiceProviderAdd         from "./pages/ServiceProvider/ServiceProviderAdd";
import ServiceProviderEdit        from "./pages/ServiceProvider/ServiceProviderEdit";


function AppLayout({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();
  if (!user) return <Navigate to="/login" replace />;
  return (
    <>
      <Navbar />
      {/* paddingTop: 68px — navbar ki height compensate karta hai */}
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
      <Route path="/site-detail/:id"
        element={<AppLayout><RequireAuth><SiteDetail /></RequireAuth></AppLayout>}
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

      <Route path="/attendance"
        element={<AppLayout><SessionCheck><DailyAttendanceList /></SessionCheck></AppLayout>}/>

      <Route path="/attendance/add"
        element={<AppLayout><SessionCheck><DailyAttendanceAdd /></SessionCheck></AppLayout>}/>

      <Route path="/material"
        element={<AppLayout><SessionCheck><MaterialPurchaseList /></SessionCheck></AppLayout>}
      />

      <Route path="/material/add"
       element={<AppLayout><SessionCheck><MaterialPurchaseAdd /></SessionCheck></AppLayout>}
      />  

      <Route path="/service-provider"
       element={<AppLayout><SessionCheck><ServiceProviderList /></SessionCheck></AppLayout>}
      />

      <Route path="/service-provider/add"
       element={<AppLayout><SessionCheck><ServiceProviderAdd /></SessionCheck></AppLayout>}
      />

      <Route path="/service-provider/edit/:id"
       element={<AppLayout><SessionCheck><ServiceProviderEdit /></SessionCheck></AppLayout>}
      />

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
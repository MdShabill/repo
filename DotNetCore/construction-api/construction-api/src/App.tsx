// import CostList from "./components/CostList.tsx";

// function App() {
//   return <CostList />;
// }

// export default App;

////-----------Site Get All Data And Get Site By Id And Site Add And Site Edit-------------------

import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import SiteList from "./components/SiteList";
import SiteDetail from "./components/SiteDetail";
import SiteAdd from "./components/SiteAdd";
import SiteEdit from "./components/SiteEdit";

function App() {
  return (
    <BrowserRouter>
      <Navbar />

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/sitelist" element={<SiteList />} />
        <Route path="/site/:id" element={<SiteDetail />} />
        <Route path="/site-add" element={<SiteAdd />} />
        <Route path="/site-edit/:id" element={<SiteEdit />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

//--------------------------------------

// import { BrowserRouter, Routes, Route } from "react-router-dom";
// import Navbar from "./components/Navbar";
// import Login from "./pages/Login";
// import Home from "./pages/Home";

// function App() {

//   const user = localStorage.getItem("user");

//   return (

//     <BrowserRouter>

//       {user && <Navbar />}

//       <Routes>

//         <Route path="/login" element={<Login />} />

//         <Route path="/" element={<Home />} />

//       </Routes>

//     </BrowserRouter>

//   );
// }

// export default App;

// import CostList from "./components/CostList.tsx";

// function App() {
//   return <CostList />;
// }

// export default App;

////---------------Site Edit------------------------------

// import SiteEdit from "./components/SiteEdit";

// function App() {
//   return <SiteEdit />;
// }

// export default App;

////--------------Site Add-------------------------------

// import { BrowserRouter, Routes, Route } from "react-router-dom";
// import SiteAdd from "./components/SiteAdd";

// function App() {
//   return (
//     <BrowserRouter>
//       <Routes>
//         <Route path="/" element={<SiteAdd />} />
//       </Routes>
//     </BrowserRouter>
//   );
// }

// export default App;

////-----------Site Get All Data And Get Site By Id-------------------

// import { BrowserRouter, Routes, Route } from "react-router-dom";
// import SiteList from "./components/SiteList";
// import SiteDetail from "./components/SiteDetail";

// function App() {
//   return (
//     <BrowserRouter>
//       <Routes>
//         <Route path="/" element={<SiteList />} />
//         <Route path="/site/:id" element={<SiteDetail />} />
//       </Routes>
//     </BrowserRouter>
//   );
// }

// export default App;

////-----------Site Get All Data And Get Site By Id And Site Add-------------------

import { BrowserRouter, Routes, Route } from "react-router-dom";
import SiteList from "./components/SiteList";
import SiteDetail from "./components/SiteDetail";
import SiteAdd from "./components/SiteAdd";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<SiteList />} />
        <Route path="/site/:id" element={<SiteDetail />} />
        <Route path="/site-add" element={<SiteAdd />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

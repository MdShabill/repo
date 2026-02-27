// import CostList from "./components/CostList.tsx";

// function App() {
//   return <CostList />;
// }

// export default App;

////---------------------------------------------

import { BrowserRouter, Routes, Route } from "react-router-dom";
import SiteEdit from "./components/SiteEdit";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<SiteEdit />} />
        <Route path="/site/edit/:id" element={<SiteEdit />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

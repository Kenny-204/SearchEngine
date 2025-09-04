import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import SearchPage from "./pages/search";
import SystemDesign from "./pages/system-design";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<SearchPage />} />
        <Route path="/system-design" element={<SystemDesign />} />
      </Routes>
    </Router>
  );
}

export default App;
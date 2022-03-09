import logo from "./logo.svg";
import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
} from "react-router-dom";
import NavBar from "./components/NavBar/NavBar";
import Account from "./components/Account/Account";
import Login from "./components/Login/Login";
import "./App.css";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/account" element={<Account />}/>
        <Route path="/home/*" element={<Login/>}/>
        <Route path="*" element={<Navigate to="/home/login" />} />
      </Routes>
    </Router>
  );
}

export default App;

import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from './components/SignIn/Signin.jsx'
import AdminLogin from "./components/AdminLogic/AdminLogic.jsx";
import AdminHome from "./components/AdminHome/AdminHome.jsx";
// import AddEmployee from "./components/AddEmployee"; // later
// import Dashboard from "./components/Dashboard"; // later

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
         <Route path="/admin" element={<AdminLogin />} /> 
         <Route path="/admin-home" element={<AdminHome />} />
        {/* <Route path="/add-employee" element={<AddEmployee />} /> */}
        {/* <Route path="/dashboard" element={<Dashboard />} /> */}
      </Routes>
    </Router>
  );
}

export default App;

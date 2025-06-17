import React from "react";
import { useNavigate } from "react-router-dom";
import "./AdminHome.css";

const AdminHome = () => {
  const navigate = useNavigate();

  return (
    <div className="admin-home-container">
      <div className="admin-home-card">
        <h2>Welcome, Admin</h2>
        <p>Select an option:</p>
        <div className="button-group">
          <button onClick={() => navigate("/add-employee")}>
            Register a New Employee
          </button>
          <button onClick={() => navigate("/dashboard")}>
            Go to Dashboard
          </button>
        </div>
      </div>
    </div>
  );
};

export default AdminHome;

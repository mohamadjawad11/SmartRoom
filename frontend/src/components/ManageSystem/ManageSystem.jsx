import React from "react";
import Sidebar from "../Sidebar/Sidebar";
import "./ManageSystem.css";
import {
  FiUserPlus,
  FiEdit,
  FiUserX,
  FiPlusSquare,
  FiEdit3,
  FiTrash2,
} from "react-icons/fi";
import { useNavigate } from "react-router-dom";

export default function ManageSystem() {
  const navigate = useNavigate();
  return (
    <div className="manage-system-page">
      <Sidebar />
      <div className="manage-content">
        <h2>System Management</h2>

        {/* Employee Management */}
        <section className="management-section">
          <h3>Manage Employees</h3>
          <div className="button-group">
            <button className="btn-primary" onClick={() => navigate("/add-employee")}>
              <FiUserPlus className="icon" /> Add Employee
            </button>
            <button className="btn-warning" onClick={() => navigate("/update-employee")}>
              <FiEdit className="icon" /> Modify Employee
            </button>
            <button className="btn-danger" onClick={() => navigate("/delete-employee")}>
              <FiUserX className="icon" /> Delete Employee
            </button>
          </div>
        </section>

        {/* Room Management */}
        <section className="management-section">
          <h3>Manage Rooms</h3>
          <div className="button-group">
            <button className="btn-primary" onClick={() => navigate("/add-room")}>
              <FiPlusSquare className="icon" /> Add Room
            </button>
            <button className="btn-warning" onClick={() => navigate("/update-room")}>
              <FiEdit3 className="icon" /> Modify Room
            </button>
            <button className="btn-danger" onClick={() => navigate("/delete-room")}>
              <FiTrash2 className="icon" /> Delete Room
            </button>
          </div>
        </section>
      </div>
    </div>
  );
}

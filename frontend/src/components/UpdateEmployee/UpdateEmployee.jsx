// src/pages/UpdateEmployee.jsx
import React, { useEffect, useState } from "react";
import axios from "axios";
import Sidebar from "../Sidebar/Sidebar";
import "./UpdateEmployee.css";

const UpdateEmployee = () => {
  const [employees, setEmployees] = useState([]);
  const [selectedEmployeeId, setSelectedEmployeeId] = useState("");
  const [formData, setFormData] = useState({ username: "", email: "", password: "", role: "Employee" });
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const token = localStorage.getItem("token");
        const res = await axios.get("http://localhost:5091/api/User", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setEmployees(res.data);
      } catch (err) {
        console.error("Failed to fetch users.", err);
      }
    };

    fetchUsers();
  }, []);

  const handleSelectChange = async (e) => {
    const userId = e.target.value;
    setSelectedEmployeeId(userId);
    if (userId === "") return;
    try {
      const token = localStorage.getItem("token");
      const res = await axios.get(`http://localhost:5091/api/User/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      const user = res.data;
      setFormData({ username: user.username, email: user.email, password: "", role: user.role });
    } catch (err) {
      console.error("Failed to fetch user details.", err);
    }
  };

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!selectedEmployeeId) return;

    try {
      setLoading(true);
      const token = localStorage.getItem("token");
      await axios.put(`http://localhost:5091/api/User/${selectedEmployeeId}`, formData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setMessage("Employee updated successfully.");
    } catch (err) {
      console.error("Failed to update employee.", err);
      setMessage("Error updating employee.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="form-page">
      <Sidebar />
      <div className="form-container">
        <h2>Update Employee</h2>
        <select onChange={handleSelectChange} value={selectedEmployeeId}>
          <option value="">Select an employee</option>
          {employees.map((emp) => (
            <option key={emp.id} value={emp.id}>
              {emp.username}
            </option>
          ))}
        </select>

        {selectedEmployeeId && (
          <form onSubmit={handleSubmit}>
            <input
              type="text"
              name="username"
              placeholder="Username"
              value={formData.username}
              onChange={handleChange}
              required
            />
            <input
              type="email"
              name="email"
              placeholder="Email"
              value={formData.email}
              onChange={handleChange}
              required
            />
            <input
              type="password"
              name="password"
              placeholder="New Password (optional)"
              value={formData.password}
              onChange={handleChange}
            />
            <select name="role" value={formData.role} onChange={handleChange}>
              <option value="Employee">Employee</option>
              <option value="Admin">Admin</option>
            </select>
            <button type="submit" disabled={loading}>
              {loading ? "Updating..." : "Update Employee"}
            </button>
            {message && <p className="message">{message}</p>}
          </form>
        )}
      </div>
    </div>
  );
};

export default UpdateEmployee;

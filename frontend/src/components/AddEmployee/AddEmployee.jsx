import React, { useState } from "react";
import axios from "axios";
import Sidebar from "../Sidebar/Sidebar";
import "./AddEmployee.css"; // Optional: style file
import toast from "react-hot-toast";


export default function AddEmployee() {
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
    role: "Employee",
  });
 

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
  e.preventDefault();

  try {
    const token = localStorage.getItem("token");

    const response = await axios.post(
      "http://localhost:5091/api/User",
      formData,
      {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      }
    );

    toast.success(response.data.message || "Employee added successfully!");
    setFormData({ username: "", email: "", password: "", role: "Employee" });

  } catch (err) {
    console.error(err);
    toast.error(err.response?.data?.message || "Failed to add employee");
  }
};

  return (
    <div className="page-container">
      <Sidebar />
      <div className="main-content">
        <h2 className="form-title">Add New Employee</h2>
        <form onSubmit={handleSubmit} className="employee-form">
          <label>Username:</label>
          <input
            type="text"
            name="username"
            value={formData.username}
            onChange={handleChange}
            required
          />

          <label>Email:</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />

          <label>Password:</label>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
          />

          <label>Role:</label>
          <select name="role" value={formData.role} onChange={handleChange}>
            <option value="Employee">Employee</option>
            <option value="Admin">Admin</option>
          </select>

          <button type="submit" className="submit-btn">Add Employee</button>
        </form>
      </div>
    </div>
  );
}

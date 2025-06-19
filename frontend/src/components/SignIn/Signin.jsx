import React, { useState } from "react";
import "./Signin.css";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { FiMail, FiLock } from "react-icons/fi";
import loginimage from  '../../assets/images/login.avif';

const Signin = () => {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    email: "",
    password: ""
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const res = await axios.post("http://localhost:5091/api/auth/login", formData);
      const role = res.data.user.role;

      if (role === "Admin") {
        navigate("/admin-home");
      } else {
        alert("login successful");
      }
    } catch (err) {
      alert("Login failed: " + (err.response?.data?.message || "Server error"));
    }
  };

  return (
    <div className="login-wrapper">
      <div className="login-card">
        <div className="login-left">
          <img src={loginimage} alt="Login Visual" />
        </div>

        <div className="login-right">
          <h2>Welcome Back</h2>
          <p className="subtext">Please enter your login details</p>
          <form onSubmit={handleSubmit}>
            <div className="input-icon">
              <FiMail className="icon" />
              <input
                type="email"
                name="email"
                placeholder="Email"
                value={formData.email}
                onChange={handleChange}
                required
              />
            </div>

            <div className="input-icon">
              <FiLock className="icon" />
              <input
                type="password"
                name="password"
                placeholder="Password"
                value={formData.password}
                onChange={handleChange}
                required
              />
            </div>

            <button type="submit">Log In</button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Signin;

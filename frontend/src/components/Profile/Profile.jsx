import React, { useState, useEffect } from "react";
import Sidebar from "../../components/Sidebar/Sidebar";
import "./Profile.css";
import { useSelector } from "react-redux";
import axios from "axios";
import { FiUser, FiMail, FiShield, FiLock } from "react-icons/fi";

export default function Profile() {
  const { currentUser } = useSelector((state) => state.user);

  const [formData, setFormData] = useState({
    username: "",
    email: "",
    role: "",
    currentPassword: "",
    newPassword: "",
    confirmPassword: ""
  });

  const [message, setMessage] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const res = await axios.get("http://localhost:5091/api/User/profile", {
          headers: {
            Authorization: `Bearer ${currentUser.token}`
          }
        });

        setFormData((prev) => ({
          ...prev,
          username: res.data.username,
          email: res.data.email,
          role: res.data.role
        }));
      } catch (err) {
        console.error("Error fetching profile:", err);
      }
    };

    fetchProfile();
  }, [currentUser.token]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage(null);
    setLoading(true);

    try {
      const res = await axios.put(
        "http://localhost:5091/api/User/update-password",
        {
          currentPassword: formData.currentPassword,
          newPassword: formData.newPassword,
          confirmPassword: formData.confirmPassword
        },
        {
          headers: {
            Authorization: `Bearer ${currentUser.token}`
          }
        }
      );

      setMessage(res.data.message);
      setFormData((prev) => ({
        ...prev,
        currentPassword: "",
        newPassword: "",
        confirmPassword: ""
      }));
    } catch (err) {
      if (err.response && err.response.data && err.response.data.message) {
        setMessage(err.response.data.message);
      } else {
        setMessage("An error occurred.");
      }
    }

    setLoading(false);
  };

  return (
    <div className="profile-page">
      <Sidebar />
      <div className="profile-card2">
        <div className="profile-form-wrapper">
          <h2><span role="img" aria-label="user">👤</span> Hello {formData.username}</h2>
          <form onSubmit={handleSubmit}>
            <label>
              Full Name
              <div className="input-with-icon">
                <FiUser className="input-icon" />
                <input
                  type="text"
                  name="username"
                  value={formData.username}
                  disabled
                  className="inside-value"
                />
              </div>
            </label>

            <label>
              Email
              <div className="input-with-icon">
                <FiMail className="input-icon" />
                <input
                  type="email"
                  name="email"
                  value={formData.email}
                  disabled
                  className="inside-value"
                />
              </div>
            </label>

            <label>
              Role
              <div className="input-with-icon">
                <FiShield className="input-icon" />
                <select className="inside-value" name="role" value={formData.role} disabled>
                  <option value="Employee">Employee</option>
                  <option value="Admin">Admin</option>
                </select>
              </div>
            </label>

            <label>
              Current Password
              <div className="input-with-icon">
                <FiLock className="input-icon" />
                <input
                  type="password"
                  name="currentPassword"
                  value={formData.currentPassword}
                  onChange={handleChange}
               
                  className="inside-value"
                />
              </div>
            </label>

            <label>
              New Password
              <div className="input-with-icon">
                <FiLock className="input-icon" />
                <input
                  type="password"
                  name="newPassword"
                  value={formData.newPassword}
                  onChange={handleChange}
                 
                />
              </div>
            </label>

            <label>
              Confirm New Password
              <div className="input-with-icon">
                <FiLock className="input-icon" />
                <input
                  type="password"
                  name="confirmPassword"
                  value={formData.confirmPassword}
                  onChange={handleChange}
              
                />
              </div>
            </label>

            <button type="submit" className="edit-btn" disabled={loading}>
              {loading ? "Saving..." : "Save Changes"}
            </button>

            {message && <p className="form-message">{message}</p>}
          </form>
        </div>
      </div>
    </div>
  );
}

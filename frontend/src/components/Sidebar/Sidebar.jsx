import React, { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import "./Sidebar.css";
import { useSelector, useDispatch } from "react-redux";
import { signOut } from "../../redux/userSlice";
import {
  FiUser,
  FiGrid,
  FiBookOpen,
  FiCalendar,
  FiFileText,
  FiBarChart2,
  FiUsers,
  FiLogOut,
  FiInbox // Added for My Invitations
} from "react-icons/fi";
import companylogo from "../../assets/images/complogo.png";

export default function Sidebar() {
  const { currentUser } = useSelector((state) => state.user);
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [showConfirm, setShowConfirm] = useState(false);

  const handleLogout = () => {
    localStorage.removeItem("token");
    dispatch(signOut());
    navigate("/");
  };

  return (
    <div className="sidebar">
      <div className="sidebar-header">
        <h2>Room Manager</h2>
        <img className="logo-img" src={companylogo} alt="Company Logo" />
      </div>

      <nav className="nav-links">
        <NavLink to="/dashboard" activeclassname="active">
          <FiGrid className="icon" />
          Dashboard
        </NavLink>
        <NavLink to="/profile" activeclassname="active">
          <FiUser className="icon" />
          Profile
        </NavLink>
        <NavLink to="/book-room" activeclassname="active">
          <FiCalendar className="icon" />
          Book Room
        </NavLink>
        <NavLink to="/bookings" activeclassname="active">
          <FiCalendar className="icon" />
          Bookings
        </NavLink>
        <NavLink to="/meetings" activeclassname="active">
          <FiBookOpen className="icon" />
          Meetings
        </NavLink>

        <NavLink to="/view-rooms" activeclassname="active">
          <FiBookOpen className="icon" />
          View Rooms
        </NavLink>


        {/* ✅ My Invitations for all users */}
        <NavLink to="/my-invitations" activeclassname="active">
          <FiInbox className="icon" />
          My Invitations
        </NavLink>

        <NavLink to="/mom" activeclassname="active">
          <FiFileText className="icon" />
          MoM
        </NavLink>
        <NavLink to="/reports" activeclassname="active">
          <FiBarChart2 className="icon" />
          Reports
        </NavLink>

        {currentUser?.role === "Admin" && (
         <NavLink to="/manage-system" activeclassname="active">
            <FiUsers className="icon" />
            Manage System
          </NavLink>

        )}
      </nav>

      <div className="sidebar-footer">
        <span className="role-label">{currentUser?.role}</span>
        <button onClick={() => setShowConfirm(true)} className="logout-btn">
          <FiLogOut className="icon" />
          Logout
        </button>
      </div>

      {showConfirm && (
        <div className="confirm-overlay">
          <div className="confirm-box">
            <p>Are you sure you want to sign out?</p>
            <div className="confirm-buttons">
              <button onClick={handleLogout} className="confirm-yes">Yes</button>
              <button onClick={() => setShowConfirm(false)} className="confirm-no">No</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

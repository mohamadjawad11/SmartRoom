import React, { useEffect, useState } from "react";
import "./Bookings.css";
import Sidebar from "../Sidebar/Sidebar.jsx";
import toast from "react-hot-toast";
import { jwtDecode } from "jwt-decode";
import InviteModal from "../InviteModal/InviteModal";

export default function Bookings() {
  const [bookings, setBookings] = useState([]);
  const [expandedBookingId, setExpandedBookingId] = useState(null);
  const [currentUserId, setCurrentUserId] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [selectedBookingId, setSelectedBookingId] = useState(null);

 useEffect(() => {
  const token = localStorage.getItem("token");
  if (token) {
    const decoded = jwtDecode(token);
    console.log("Decoded token:", decoded);

    const userId =
      decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

    if (userId) {
      setCurrentUserId(Number(userId));
    } else {
      console.warn("User ID not found in token:", decoded);
    }
  }
}, []);


  useEffect(() => {
    const fetchBookings = async () => {
      try {
        const token = localStorage.getItem("token");
        const res = await fetch("http://localhost:5091/api/Booking", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        const data = await res.json();
        setBookings(data);
        
      } catch (err) {
        toast.error("Failed to load bookings.");
        console.error(err);
      }
    };
    fetchBookings();
  }, []);

  const toggleAttendees = (bookingId) => {
    setExpandedBookingId(expandedBookingId === bookingId ? null : bookingId);
  };

  const handleOpenInvite = (bookingId) => {
    setSelectedBookingId(bookingId);
    setShowModal(true);
  };

  return (
    <div className="page-container">
      <div className="sidebar-container">
        <Sidebar />
      </div>
      <div className="main-content">
        <div className="bookings-section">
          <h2 className="section-title">My Bookings</h2>
          <table className="bookings-table">
            <thead>
              <tr>
                <th>Room</th>
                <th>Location</th>
                <th>Start Time</th>
                <th>End Time</th>
                <th>Purpose</th>
                <th>Status</th>
                <th>Attendees</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
      {bookings.map((b) => {
  console.log("booking userId:", b.userId, "current userId:", currentUserId);
  return (
    <React.Fragment key={b.id}>
      <tr>
        <td>{b.roomName}</td>
        <td>{b.roomLocation}</td>
        <td>{new Date(b.startTime).toLocaleString()}</td>
        <td>{new Date(b.endTime).toLocaleString()}</td>
        <td>{b.purpose}</td>
        <td>
          <span className={`status-badge ${b.status.toLowerCase()}`}>
            {b.status}
          </span>
        </td>
        <td>
          <button
            className="attendee-toggle"
            onClick={() => toggleAttendees(b.id)}
          >
            {expandedBookingId === b.id ? "Hide" : "View"}
          </button>
        </td>
        <td>
          {b.userId === currentUserId && (
            <button
              onClick={() => handleOpenInvite(b.id)}
              className="invite-btn"
            >
              Invite
            </button>
          )}
        </td>
      </tr>
      {expandedBookingId === b.id && (
        <tr className="attendees-row">
          <td colSpan="8">
            {!b.attendees || b.attendees.length === 0 ? (
              <p>No attendees yet.</p>
            ) : (
              <ul className="attendee-list">
                {b.attendees.map((a) => (
                  <li key={a.userId}>
                    {a.username} —{" "}
                    <span className={`attendee-badge ${a.status.toLowerCase()}`}>
                      {a.status}
                    </span>
                  </li>
                ))}
              </ul>
            )}
          </td>
        </tr>
      )}
    </React.Fragment>
  );
})}

            </tbody>
          </table>
        </div>
      </div>
      {showModal && (
        <InviteModal
          bookingId={selectedBookingId}
          onClose={() => setShowModal(false)}
        />
      )}
    </div>
  );
}

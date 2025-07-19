import React, { useEffect, useState } from "react";
import "./Bookings.css";
import Sidebar from "../Sidebar/Sidebar.jsx";
import toast from "react-hot-toast";
import { jwtDecode } from "jwt-decode";
import InviteModal from "../InviteModal/InviteModal";
import { useNavigate } from "react-router-dom";

export default function Bookings() {
  const [bookings, setBookings] = useState([]);
  const [expandedBookingId, setExpandedBookingId] = useState(null);
  const [currentUserId, setCurrentUserId] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [selectedBookingId, setSelectedBookingId] = useState(null);

  // Confirmation modal for delete
  const [showConfirmModal, setShowConfirmModal] = useState(false);
  const [bookingToDelete, setBookingToDelete] = useState(null);

  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const decoded = jwtDecode(token);
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

  const requestDeleteBooking = (bookingId) => {
    setBookingToDelete(bookingId);
    setShowConfirmModal(true);
  };

  const confirmDeleteBooking = async () => {
    try {
      const token = localStorage.getItem("token");
      const res = await fetch(`http://localhost:5091/api/Booking/${bookingToDelete}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      const result = await res.json();

      if (res.ok) {
        toast.success("Booking deleted!");
        setBookings((prev) => prev.filter((b) => b.id !== bookingToDelete));
      } else {
        toast.error(result.message || "Delete failed.");
      }
    } catch (err) {
      toast.error("Server error.");
      console.error(err);
    }

    setShowConfirmModal(false);
    setBookingToDelete(null);
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
                <th>Invite</th>
                <th>Modify</th>
                <th>Delete</th>
              </tr>
            </thead>
            <tbody>
              {bookings.map((b) => (
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
                    <td>
                      {b.userId === currentUserId && (
                        <button
                          onClick={() => navigate(`/modify-booking/${b.id}`)}
                          className="modify-btn"
                          style={{ backgroundColor: "#ffc107", color: "black" }}
                        >
                          Modify
                        </button>
                      )}
                    </td>
                    <td>
                      {b.userId === currentUserId && (
                        <button
                          onClick={() => requestDeleteBooking(b.id)}
                          className="delete-btn"
                          style={{ backgroundColor: "#dc3545", color: "white" }}
                        >
                          Delete
                        </button>
                      )}
                    </td>
                  </tr>
                  {expandedBookingId === b.id && (
                    <tr className="attendees-row">
                      <td colSpan="10">
                        {!b.attendees || b.attendees.length === 0 ? (
                          <p>No attendees yet.</p>
                        ) : (
                          <ul className="attendee-list">
                            {b.attendees.map((a) => (
                              <li key={a.userId}>
                                {a.username} —{" "}
                                <span
                                  className={`attendee-badge ${a.status.toLowerCase()}`}
                                >
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
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* Invite Modal */}
      {showModal && (
        <InviteModal
          bookingId={selectedBookingId}
          onClose={() => setShowModal(false)}
        />
      )}

      {/* Delete Confirmation Modal */}
      {showConfirmModal && (
        <div className="confirm-overlay">
          <div className="confirm-box">
            <p>Are you sure you want to delete this booking?</p>
            <div className="confirm-buttons">
              <button onClick={confirmDeleteBooking} className="confirm-yes">
                Yes, Delete
              </button>
              <button
                onClick={() => setShowConfirmModal(false)}
                className="confirm-no"
              >
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

// import React, { useState, useEffect } from "react";
// import axios from "axios";
// import { useSelector } from "react-redux"; // Assuming you use Redux to get the current user
// import { useHistory } from "react-router-dom";

// const Meetings = () => {
//   const [createdMeetings, setCreatedMeetings] = useState([]);
//   const [invitedMeetings, setInvitedMeetings] = useState([]);
//   const [loading, setLoading] = useState(true);
//   const [error, setError] = useState(null);

//   const currentUser = useSelector((state) => state.user.currentUser); // Get current user from Redux

//   const history = useHistory();

//   // Fetch meetings when the component mounts
//   useEffect(() => {
//     const fetchMeetings = async () => {
//       try {
//         // Fetch meetings the user has been invited to
//         const invitationsResponse = await axios.get("/api/meeting/my-invitations");
//         setInvitedMeetings(invitationsResponse.data);

//         // Fetch meetings the user has created
//         const createdResponse = await axios.get("/api/booking/my-created-meetings");
//         setCreatedMeetings(createdResponse.data);

//         setLoading(false);
//       } catch (error) {
//         setError("Failed to load meetings");
//         setLoading(false);
//       }
//     };

//     fetchMeetings();
//   }, [currentUser.id]); // Refetch when user changes (optional)

//   const handleJoinMeeting = async (bookingId) => {
//     try {
//       const response = await axios.post(`/api/meeting/join/${bookingId}`);
//       alert("Successfully joined the meeting.");
//       // Optionally, redirect to the meeting page or show meeting link
//     } catch (error) {
//       alert("Failed to join the meeting.");
//     }
//   };

//   const handleStartMeeting = async (bookingId) => {
//     try {
//       const response = await axios.post(`/api/meeting/start/${bookingId}`);
//       alert("Meeting started successfully.");
//       // Optionally, redirect to the meeting room or update status
//     } catch (error) {
//       alert("Failed to start the meeting.");
//     }
//   };

//   if (loading) {
//     return <div>Loading...</div>;
//   }

//   if (error) {
//     return <div>{error}</div>;
//   }

//   return (
//     <div className="meetings-container">
//       <h1>Meetings</h1>

//       {/* Meetings the user has created */}
//       <div className="meetings-list">
//         <h2>Your Created Meetings</h2>
//         {createdMeetings.length === 0 && <p>You have not created any meetings.</p>}
//         {createdMeetings.map((meeting) => (
//           <div className="meeting-card" key={meeting.Id}>
//             <h3>{meeting.RoomName}</h3>
//             <p>{meeting.Purpose}</p>
//             <p>{meeting.StartTime} - {meeting.EndTime}</p>
//             <p>{meeting.RoomLocation}</p>
//             <button onClick={() => handleStartMeeting(meeting.Id)}>Start Meeting</button>
//           </div>
//         ))}
//       </div>

//       {/* Meetings the user is invited to */}
//       <div className="meetings-list">
//         <h2>Your Invitations</h2>
//         {invitedMeetings.length === 0 && <p>You have no invitations.</p>}
//         {invitedMeetings.map((meeting) => (
//           <div className="meeting-card" key={meeting.Id}>
//             <h3>{meeting.RoomName}</h3>
//             <p>{meeting.Purpose}</p>
//             <p>{meeting.StartTime} - {meeting.EndTime}</p>
//             <p>{meeting.RoomLocation}</p>
//             <button onClick={() => handleJoinMeeting(meeting.Id)}>Join Meeting</button>
//           </div>
//         ))}
//       </div>
//     </div>
//   );
// };

// export default Meetings;

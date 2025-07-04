// src/pages/InvitePage.jsx
import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./InvitePage.css";
import toast from "react-hot-toast";

export default function InvitePage() {
  const { bookingId } = useParams();
  const [users, setUsers] = useState([]);
  const [selectedUserId, setSelectedUserId] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const token = localStorage.getItem("token");
        const res = await fetch("http://localhost:5091/api/User", {
          headers: { Authorization: `Bearer ${token}` },
        });
        const data = await res.json();
        setUsers(data);
      } catch (err) {
        toast.error("Failed to load users.");
        console.log(err);
      }
    };
    fetchUsers();
  }, []);

  const handleInvite = async () => {
    if (!selectedUserId) return toast.error("Please select a user.");
    try {
      const token = localStorage.getItem("token");
      const res = await fetch("http://localhost:5091/api/MeetingAttendee/invite", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ bookingId: parseInt(bookingId), userId: parseInt(selectedUserId) }),
      });

      const result = await res.json();

      if (res.ok) {
        toast.success("User invited!");
        navigate("/bookings");
      } else {
        toast.error(result.message || "Invitation failed.");
      }
    } catch (err) {
      toast.error("Server error.");
      console.log(err);
    }
  };

  return (
    <div className="invite-page">
      <h2>Invite User to Booking #{bookingId}</h2>
      <select value={selectedUserId} onChange={(e) => setSelectedUserId(e.target.value)}>
        <option value="">Select a user</option>
        {users.map((u) => (
          <option key={u.id} value={u.id}>
            {u.username}
          </option>
        ))}
      </select>
      <div className="invite-actions">
        <button onClick={handleInvite} className="btn-primary">Invite</button>
        <button onClick={() => navigate("/bookings")} className="btn-secondary">Cancel</button>
      </div>
    </div>
  );
}

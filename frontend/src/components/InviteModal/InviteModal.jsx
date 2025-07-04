import React, { useEffect, useState } from "react";
import "./InviteModal.css";
import toast from "react-hot-toast";

export default function InviteModal({ bookingId, onClose }) {
  const [users, setUsers] = useState([]);
  const [selectedUserId, setSelectedUserId] = useState("");

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
        console.error(err);
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
        body: JSON.stringify({ bookingId, userId: parseInt(selectedUserId) }),
      });

      const result = await res.json();

      if (res.ok) {
        toast.success("User invited successfully!");
        onClose(); // close modal
      } else {
        toast.error(result.message || "Invitation failed.");
      }
    } catch (err) {
      toast.error("Server error.");
      console.error(err);
    }
  };

  return (
    <div className="modal-backdrop">
      <div className="modal-box">
        <h3>Invite Attendee</h3>
        <select value={selectedUserId} onChange={(e) => setSelectedUserId(e.target.value)}>
          <option value="">Select a user</option>
          {users.map((user) => (
            <option key={user.id} value={user.id}>
              {user.username}
            </option>
          ))}
        </select>
        <div className="modal-actions">
          <button onClick={handleInvite} className="btn-primary">Invite</button>
          <button onClick={onClose} className="btn-secondary">Cancel</button>
        </div>
      </div>
    </div>
  );
}

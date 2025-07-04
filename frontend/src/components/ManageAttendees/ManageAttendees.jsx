import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Sidebar from "../Sidebar/Sidebar.jsx";
import toast from "react-hot-toast";
import "./ManageAttendees.css";

export default function ManageAttendees() {
  const { bookingId } = useParams();
  const [attendees, setAttendees] = useState([]);
  const [newUserId, setNewUserId] = useState("");
  const [loading, setLoading] = useState(true);

  const fetchAttendees = async () => {
    try {
      const token = localStorage.getItem("token");
      const res = await fetch(`http://localhost:5091/api/MeetingAttendees/${bookingId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      const data = await res.json();
      if (res.ok) setAttendees(data);
      else toast.error(data.message || "Failed to load attendees.");
    } catch (err) {
      toast.error("Server error.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAttendees();
  }, [bookingId]);

  const handleInvite = async () => {
    if (!newUserId) return toast.error("Please enter a User ID.");

    try {
      const token = localStorage.getItem("token");
      const res = await fetch(`http://localhost:5091/api/MeetingAttendees`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ bookingId: parseInt(bookingId), userId: parseInt(newUserId) }),
      });

      const result = await res.json();

      if (res.ok) {
        toast.success("Invitation sent.");
        setNewUserId("");
        fetchAttendees();
      } else {
        toast.error(result.message || "Failed to invite.");
      }
    } catch (err) {
      toast.error("Server error.");
      console.error(err);
    }
  };

  return (
    <div className="manage-page">
      <Sidebar />
      <div className="manage-content">
        <h2>Manage Attendees</h2>

        <div className="invite-form">
          <input
            type="number"
            placeholder="Enter User ID"
            value={newUserId}
            onChange={(e) => setNewUserId(e.target.value)}
          />
          <button onClick={handleInvite}>Invite</button>
        </div>

        {loading ? (
          <p>Loading attendees...</p>
        ) : (
          <table className="attendees-table">
            <thead>
              <tr>
                <th>User ID</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {attendees.map((a) => (
                <tr key={a.id}>
                  <td>{a.userId}</td>
                  <td>
                    <span className={`status-badge ${a.status.toLowerCase()}`}>
                      {a.status}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}

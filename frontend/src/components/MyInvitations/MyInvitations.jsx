import React, { useEffect, useState } from "react";
import Sidebar from "../Sidebar/Sidebar.jsx";
import "./MyInvitations.css";
import toast from "react-hot-toast";

export default function MyInvitations() {
  const [invitations, setInvitations] = useState([]);

  useEffect(() => {
    const fetchInvites = async () => {
      try {
        const token = localStorage.getItem("token");
        const res = await fetch("http://localhost:5091/api/MeetingAttendee/my-invitations", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        const data = await res.json();
        setInvitations(data);
      } catch (err) {
        toast.error("Failed to load invitations.");
        console.error(err);
      }
    };
    fetchInvites();
  }, []);

  const updateStatus = async (attendeeId, status) => {
    try {
      const token = localStorage.getItem("token");
      const res = await fetch("http://localhost:5091/api/MeetingAttendee/update-status", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ attendeeId, status }),
      });

      if (res.ok) {
        toast.success(`Invitation ${status}`);
        setInvitations((prev) =>
          prev.map((inv) =>
            inv.id === attendeeId ? { ...inv, status } : inv
          )
        );
      } else {
        toast.error("Failed to update status.");
      }
    } catch (err) {
      toast.error("Error updating status.");
      console.error(err);
    }
  };

  return (
    <div className="invitations-page">
      <Sidebar />
      <div className="invitations-content">
        <h2>My Meeting Invitations</h2>
        {invitations.length === 0 ? (
          <p>No invitations found.</p>
        ) : (
          <table className="invitations-table">
            <thead>
              <tr>
                <th>Room</th>
                <th>Location</th>
                <th>Organizer</th>
                <th>Start</th>
                <th>End</th>
                <th>Purpose</th>
                <th>Status</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {invitations.map((inv) => (
                <tr key={inv.id}>
                  <td>{inv.room}</td>
                  <td>{inv.location}</td>
                  <td>{inv.organizerUsername}</td>
                  <td>{new Date(inv.startTime).toLocaleString()}</td>
                  <td>{new Date(inv.endTime).toLocaleString()}</td>
                  <td>{inv.purpose}</td>
                  <td>
                    <span className={`status-badge ${inv.status.toLowerCase()}`}>
                      {inv.status}
                    </span>
                  </td>
                  <td>
                    {inv.status === "Pending" && (
                      <>
                        <button
                          className="accept-btn"
                          onClick={() => updateStatus(inv.id, "Accepted")}
                        >
                          Accept
                        </button>
                        <button
                          className="reject-btn"
                          onClick={() => updateStatus(inv.id, "Rejected")}
                        >
                          Reject
                        </button>
                      </>
                    )}
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

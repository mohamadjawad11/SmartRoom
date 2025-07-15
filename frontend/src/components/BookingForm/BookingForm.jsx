import React, { useEffect, useState } from "react";
import "./BookingForm.css";
import Sidebar from "../Sidebar/Sidebar.jsx";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
export default function BookingForm() {
  const [rooms, setRooms] = useState([]);
  const [formData, setFormData] = useState({
    roomId: "",
    startTime: "",
    endTime: "",
    purpose: "",
  });

  const navigate=useNavigate();
  const [statusMessage, setStatusMessage] = useState({ type: "", text: "" });

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        const res = await fetch("http://localhost:5091/api/Room");
        const data = await res.json();
        setRooms(data.filter((room) => room.isAvailable));
      } catch (err) {
        toast.error("Failed to load rooms.");
        console.log(err);
      }
    };
    fetchRooms();
  }, []);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (new Date(formData.endTime) <= new Date(formData.startTime)) {
      toast.error("End time must be after start time.");
      setStatusMessage({ type: "error", text: "End time must be after start time." });
      return;
    }

    try {
      const token = localStorage.getItem("token");
      const res = await fetch("http://localhost:5091/api/Booking", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(formData),
      });

      const result = await res.json();

      if (res.ok) {
        toast.success("Booking successful!");
        setStatusMessage({ type: "success", text: "Booking completed successfully." });
        setFormData({
          roomId: "",
          startTime: "",
          endTime: "",
          purpose: "",
        });
        setTimeout(() => {
  navigate("/bookings");
}, 2000);
      } else {
        toast.error(result.message || "Booking failed.");
        setStatusMessage({ type: "error", text: result.message || "Booking failed." });
                 
      }
    } catch (err) {
      toast.error("Server error. Please try again.");
      setStatusMessage({ type: "error", text: "Server error. Please try again." });
      console.log(err);
    }
  };

  return (
    <div className="booking-page">
      <Sidebar />
      <div className="booking-content">
        <form onSubmit={handleSubmit} className="booking-form">
          <h2>Book a Meeting Room</h2>

          <div className="form-group">
            <label htmlFor="roomId">Room</label>
            <select
              id="roomId"
              value={formData.roomId}
              onChange={handleChange}
              required
            >
              <option value="">Select a room</option>
              {rooms.map((room) => (
                <option key={room.id} value={room.id}>
                  {room.name} — {room.location}
                </option>
              ))}
            </select>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="startTime">Start Time</label>
              <input
                type="datetime-local"
                id="startTime"
                value={formData.startTime}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="endTime">End Time</label>
              <input
                type="datetime-local"
                id="endTime"
                value={formData.endTime}
                onChange={handleChange}
                required
              />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="purpose">Agenda</label>
            <textarea
              id="purpose"
              value={formData.purpose}
              onChange={handleChange}
              placeholder="project Planning ..."
              required
            ></textarea>
          </div>

          <div className="form-actions">
            <button type="submit" className="btn-primary">Submit Booking</button>
            <button type="reset" className="btn-secondary">Cancel</button>
          </div>

          {statusMessage.text && (
            <p className={`status-msg ${statusMessage.type}`}>
              {statusMessage.text}
            </p>
          )}
        </form>
      </div>
    </div>
  );
}

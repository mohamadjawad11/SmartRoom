import React, { useEffect, useState } from "react";
import "./ModifyBooking.css";
import Sidebar from "../Sidebar/Sidebar.jsx";
import toast from "react-hot-toast";
import { useParams, useNavigate } from "react-router-dom";

export default function ModifyBooking() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [rooms, setRooms] = useState([]);
  const [formData, setFormData] = useState({
    roomId: "",
    startTime: "",
    endTime: "",
    purpose: ""
  });

  useEffect(() => {
    // Fetch all rooms
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

    // Fetch booking details
    const fetchBooking = async () => {
      try {
        const token = localStorage.getItem("token");
        const res = await fetch("http://localhost:5091/api/Booking", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        const data = await res.json();
        const booking = data.find((b) => b.id === parseInt(id));
        if (!booking) return toast.error("Booking not found.");

        setFormData({
          roomId: booking.roomId,
          startTime: booking.startTime.slice(0, 16), // format to 'yyyy-MM-ddTHH:mm'
          endTime: booking.endTime.slice(0, 16),
          purpose: booking.purpose,
        });
      } catch (err) {
        toast.error("Failed to load booking.");
        console.log(err);
      }
    };

    fetchRooms();
    fetchBooking();
  }, [id]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (new Date(formData.endTime) <= new Date(formData.startTime)) {
      toast.error("End time must be after start time.");
      return;
    }

    try {
      const token = localStorage.getItem("token");
      const res = await fetch(`http://localhost:5091/api/Booking/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          roomId: parseInt(formData.roomId),
          startTime: formData.startTime,
          endTime: formData.endTime,
          purpose: formData.purpose,
        }),
      });

      const result = await res.json();

      if (res.ok) {
        toast.success("Booking updated successfully!");
        navigate("/bookings");
      } else {
        toast.error(result.message || "Update failed.");
      }
    } catch (err) {
      toast.error("Server error.");
      console.error(err);
    }
  };

  return (
    <div className="booking-page">
      <Sidebar />
      <div className="booking-content">
        <form onSubmit={handleSubmit} className="booking-form">
          <h2>Modify Booking</h2>

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
              placeholder="project planning..."
              required
            ></textarea>
          </div>

          <div className="form-actions">
            <button type="submit" className="btn-primary">Update Booking</button>
            <button type="button" onClick={() => navigate("/bookings")} className="btn-secondary">Cancel</button>
          </div>
        </form>
      </div>
    </div>
  );
}

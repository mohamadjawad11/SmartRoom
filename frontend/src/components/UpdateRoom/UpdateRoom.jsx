import React, { useState, useEffect } from "react";
import axios from "axios";
import Sidebar from "../Sidebar/Sidebar";
import "./UpdateRoom.css";
import { useNavigate } from "react-router-dom";

const UpdateRoom = () => {
  const [rooms, setRooms] = useState([]);
  const [selectedRoomId, setSelectedRoomId] = useState("");
  const [loading, setLoading] = useState(false);

  const [formData, setFormData] = useState({
    name: "",
    capacity: "",
    location: "",
    description: "",
    imagePath: ""
  });
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        const res = await axios.get("http://localhost:5091/api/Room");
        setRooms(res.data);
      } catch (err) {
        console.error("Failed to fetch rooms.", err);
      }
    };

    fetchRooms();
  }, []);

  const handleSelectChange = async (e) => {
    const id = e.target.value;
    setSelectedRoomId(id);
    if (!id) return;

    try {
      const res = await axios.get(`http://localhost:5091/api/Room/${id}`);
      const room = res.data;
      setFormData({
        name: room.name,
        capacity: room.capacity,
        location: room.location,
        description: room.description,
        imagePath: room.imagePath || ""
      });
    } catch (err) {
      console.error("Failed to fetch room details.", err);
    }
  };

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // const handleSubmit = async (e) => {
  //   e.preventDefault();
  //   if (!selectedRoomId) return;

  //   try {
  //     await axios.put(`http://localhost:5091/api/Room/${selectedRoomId}`, formData);
  //     setMessage("Room updated successfully!");
  //     navigate('/view-rooms');
  //   } catch (err) {
  //     console.error("Failed to update room.", err);
  //     setMessage("Error updating room.");
  //   }
  // };
  const handleSubmit = async (e) => {
  e.preventDefault();
  setLoading(true); // Start loading
  try {
    const token = localStorage.getItem('token');
    await axios.put(`http://localhost:5091/api/Room/${selectedRoomId}`, formData, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    setMessage("Room updated successfully!");
    navigate('/view-rooms');
  } catch (error) {
    console.error('Error updating room:', error);
  } finally {
    setLoading(false); 
  }
};


  return (
    <div className="update-room-page">
      <Sidebar />
      <div className="update-room-container">
        <h2>Update Room</h2>
        <select onChange={handleSelectChange} value={selectedRoomId}>
          <option value="">-- Select a room --</option>
          {rooms.map((room) => (
            <option key={room.id} value={room.id}>
              {room.name}
            </option>
          ))}
        </select>

        {selectedRoomId && (
          <form className="update-room-form" onSubmit={handleSubmit}>
            <input
              type="text"
              name="name"
              placeholder="Room Name"
              value={formData.name}
              onChange={handleChange}
              required
            />
            <input
              type="number"
              name="capacity"
              placeholder="Capacity"
              value={formData.capacity}
              onChange={handleChange}
              required
            />
            <input
              type="text"
              name="location"
              placeholder="Location"
              value={formData.location}
              onChange={handleChange}
              required
            />
            <textarea
              name="description"
              placeholder="Description"
              value={formData.description}
              onChange={handleChange}
              required
            />
            <input
              type="text"
              name="imagePath"
              placeholder="Image Path (optional)"
              value={formData.imagePath}
              onChange={handleChange}
            />
            <button type="submit" disabled={loading}>
  {loading ? 'Updating...' : 'Update Room'}
</button>

            {message && <p className="message">{message}</p>}
          </form>
        )}
      </div>
    </div>
  );
};

export default UpdateRoom;

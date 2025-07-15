import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Sidebar from '../Sidebar/Sidebar';
import './DeleteRoom.css';

const DeleteRoom = () => {
  const [rooms, setRooms] = useState([]);
  const [selectedRoomId, setSelectedRoomId] = useState('');
  const [confirmDelete, setConfirmDelete] = useState(false);
  const [message, setMessage] = useState('');

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        const token = localStorage.getItem('token');
        const res = await axios.get('http://localhost:5091/api/Room', {
          headers: { Authorization: `Bearer ${token}` }
        });
        setRooms(res.data);
      } catch (error) {
        setMessage('Failed to load rooms.');
        console.error(error);
      }
    };

    fetchRooms();
  }, []);

  const handleDelete = async () => {
    try {
      const token = localStorage.getItem('token');
      await axios.delete(`http://localhost:5091/api/Room/${selectedRoomId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      setMessage('Room deleted successfully.');
      setRooms(rooms.filter(room => room.id !== parseInt(selectedRoomId)));
      setSelectedRoomId('');
      setConfirmDelete(false);
    } catch (error) {
      setMessage(error.response?.data?.message || 'Failed to delete room.');
    }
  };

  return (
    <div className="delete-room-page">
      <Sidebar />
      <div className="delete-room-container">
        <h2>Delete Room</h2>
        <select value={selectedRoomId} onChange={(e) => setSelectedRoomId(e.target.value)}>
          <option value=""> Select a Room </option>
          {rooms.map(room => (
            <option key={room.id} value={room.id}>
              {room.name} ({room.location})
            </option>
          ))}
        </select>

        {selectedRoomId && !confirmDelete && (
          <button className="confirm-btn" onClick={() => setConfirmDelete(true)}>
            Confirm Delete
          </button>
        )}

        {confirmDelete && (
          <div className="confirmation-box">
            <p>Are you sure you want to delete this room?</p>
            <div className="confirmation-buttons">
              <button className="delete-btn" onClick={handleDelete}>Yes, Delete</button>
              <button className="cancel-btn" onClick={() => setConfirmDelete(false)}>Cancel</button>
            </div>
          </div>
        )}

        {message && <p className="message">{message}</p>}
      </div>
    </div>
  );
};

export default DeleteRoom;

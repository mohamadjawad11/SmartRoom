// src/components/ViewRooms.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Sidebar from '../Sidebar/Sidebar.jsx';
import './ViewRooms.css';

const ViewRooms = () => {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        const response = await axios.get('http://localhost:5091/api/Room');
        setRooms(response.data);
      } catch (err) {
        setError('Failed to fetch room data');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchRooms();
  }, []);  // Empty dependency array ensures this runs only once when the component mounts

  if (loading) {
    return <div>Loading rooms...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="view-rooms">
      <Sidebar />
      <div className="room-container">
        <h2>Available Rooms</h2>
        <div className="room-cards">
          {rooms.map((room) => (
            <div key={room.id} className="room-card">
              <h3>{room.name}</h3>
              <p><strong>Capacity:</strong> {room.capacity}</p>
              <p><strong>Location:</strong> {room.location}</p>
              <p>{room.description}</p>
              <img src={room.imagePath || '/default-room-image.jpg'} alt={room.name} className="room-image" />
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ViewRooms;

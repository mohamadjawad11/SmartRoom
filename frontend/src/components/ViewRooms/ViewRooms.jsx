import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Sidebar from '../Sidebar/Sidebar.jsx';
import './ViewRooms.css';
import { FiSearch } from 'react-icons/fi';

const ViewRooms = () => {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [query, setQuery] = useState('');
  const [, setSearching] = useState(false);

  const fetchAllRooms = async () => {
    try {
      const response = await axios.get('http://localhost:5091/api/Room');
      setRooms(response.data);
      setError('');
    } catch (err) {
      setError('Failed to fetch room data');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async () => {
    if (!query.trim()) {
      fetchAllRooms(); // fallback to all rooms
      return;
    }

    try {
      setSearching(true);
      const res = await axios.get(`http://localhost:5091/api/Room/search?query=${query}`);
      setRooms(res.data);
      setError('');
    } catch (err) {
      setRooms([]);
      setError('No rooms found.');
      console.error(err);
    } finally {
      setSearching(false);
    }
  };

  useEffect(() => {
    fetchAllRooms();
  }, []);

  if (loading) {
    return <div>Loading rooms...</div>;
  }
if (error && rooms.length === 0) {
    return <div>{error}</div>;
  }

  return (
    <div className="view-rooms">
      <Sidebar />
      <div className="room-container">
        <h2>Available Rooms</h2>

        {/* 🔍 Search Input */}
        <div className="search-bar">
          <input
            type="text"
            placeholder="Search by room name..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <button onClick={handleSearch} title="Search">
            <FiSearch />
          </button>
        </div>

        {/* 🔽 Room Cards */}
        <div className="room-cards">
          {rooms.length > 0 ? (
            rooms.map((room) => (
              <div key={room.id} className="room-card">
                <h3>{room.name}</h3>
                <p><strong>Capacity:</strong> {room.capacity}</p>
                <p><strong>Location:</strong> {room.location}</p>
                <p>{room.description}</p>
                <img
                  src={room.imagePath || '/default-room-image.jpg'}
                  alt={room.name}
                  className="room-image"
                />
              </div>
            ))
          ) : (
            <p>No rooms to display.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default ViewRooms;

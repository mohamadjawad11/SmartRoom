// src/pages/AddRoom.jsx
import React, { useState } from 'react';
import axios from 'axios';
import Sidebar from '../Sidebar/Sidebar';
import './AddRoom.css';
import { useNavigate } from 'react-router-dom';


const AddRoom = () => {
  const [formData, setFormData] = useState({
    name: '',
    capacity: '',
    location: '',
    description: '',
    imagePath: ''
  });

  const [message, setMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();


  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const token = localStorage.getItem('token');
      const response = await axios.post('http://localhost:5091/api/Room', formData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      console.log(response);
      setMessage('Room added successfully!');
      setFormData({ name: '', capacity: '', location: '', description: '', imagePath: '' });
      navigate('/view-rooms');

    } catch (error) {
      setMessage(error.response?.data?.message || 'Failed to add room.');
    } finally {
      setLoading(false);
    }
  };
// uaps cyqo kpoc cytl
  return (
    <div className="add-room-page">
      <Sidebar />
      <div className="add-room-container">
        <h2>Add New Room</h2>
        <form onSubmit={handleSubmit} className="add-room-form">
          <input type="text" name="name" placeholder="Room Name" value={formData.name} onChange={handleChange} required />
          <input type="number" name="capacity" placeholder="Capacity" value={formData.capacity} onChange={handleChange} required />
          <input type="text" name="location" placeholder="Location" value={formData.location} onChange={handleChange} required />
          <textarea name="description" placeholder="Description" value={formData.description} onChange={handleChange} required></textarea>
          <input type="text" name="imagePath" placeholder="Image Path (optional)" value={formData.imagePath} onChange={handleChange} />

          <button type="submit" disabled={loading}>{loading ? 'Adding...' : 'Add Room'}</button>
          {message && <p className="message">{message}</p>}
        </form>
      </div>
    </div>
  );
};

export default AddRoom;
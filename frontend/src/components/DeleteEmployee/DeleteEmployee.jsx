import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './DeleteEmployee.css';
import Sidebar from "../Sidebar/Sidebar";

const DeleteEmployee = () => {
  const [username, setUsername] = useState('');
  const [users, setUsers] = useState([]);
  const [message, setMessage] = useState('');
  const [showConfirm, setShowConfirm] = useState(false);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const token = localStorage.getItem('token');
        const res = await axios.get("http://localhost:5091/api/User", {
          headers: { Authorization: `Bearer ${token}` }
        });
        setUsers(res.data);
      } catch (err) {
        setMessage('Failed to load users.');
        console.error('Failed to fetch users.', err);
      }
    };

    fetchUsers();
  }, []);

  const handleDelete = async () => {
    try {
      const token = localStorage.getItem('token');
      await axios.delete(`http://localhost:5091/api/User/by-username/${username}`, {
        headers: { Authorization: `Bearer ${token}` }
      });

      setMessage('Employee deleted successfully.');
      setUsername('');
    } catch (error) {
      setMessage(error.response?.data?.message || 'Failed to delete employee.');
    } finally {
      setShowConfirm(false);
    }
  };

  return (
    <div className="delete-employee-container">
      <Sidebar />
      <div className="delete-employee-form">
        <h2>Delete Employee</h2>

        <label>Select Username:</label>
        <select value={username} onChange={(e) => setUsername(e.target.value)}>
          <option value="">-- Choose a user --</option>
          {users.map((user) => (
            <option key={user.id} value={user.username}>{user.username}</option>
          ))}
        </select>

        <button
          onClick={() => {
            if (!username) {
              setMessage("Please select a user.");
            } else {
              setShowConfirm(true);
            }
          }}
        >
          Delete Employee
        </button>

        {message && <p className="message">{message}</p>}

        {/* ✅ Custom confirmation dialog */}
        {showConfirm && (
          <div className="confirm-overlay">
            <div className="confirm-box">
              <p>Are you sure you want to delete user <strong>{username}</strong>?</p>
              <div className="confirm-buttons">
                <button className="confirm-yes" onClick={handleDelete}>Yes</button>
                <button className="confirm-no" id='confirm-no2' onClick={() => setShowConfirm(false)}>No</button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default DeleteEmployee;

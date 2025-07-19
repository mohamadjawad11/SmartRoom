import React, { useState, useEffect } from "react";
import axios from "axios";
import { useSelector } from "react-redux"; 
import Sidebar from "../Sidebar/Sidebar.jsx";
import "./Meeting.css";

const Meetings = () => {
  const [createdMeetings, setCreatedMeetings] = useState([]);
  const [invitedMeetings, setInvitedMeetings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const currentUser = useSelector((state) => state.user.currentUser); // Get current user from Redux

  // Fetch meetings when the component mounts
  useEffect(() => {
    const fetchMeetings = async () => {
      try {
        const token = localStorage.getItem("token");
        const response1 = await fetch("http://localhost:5091/api/MeetingAttendee/my-meetings", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        const data = await response1.json();
        setInvitedMeetings(data);
        setLoading(false);

        const response2 = await fetch("http://localhost:5091/api/Booking/my-created-meetings", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        const data2 = await response2.json();
        setCreatedMeetings(data2);
        setLoading(false);
      } catch (error) {
        setError("Failed to load meetings");
        setLoading(false);
        console.error("Error fetching meetings:", error);
      }
    };

    fetchMeetings();
  }, [currentUser.id]);

const handleJoinMeeting = async (bookingId) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.post(
      `http://localhost:5091/api/MeetingAttendee/join/${bookingId}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log(response.data);
    alert("Successfully joined the meeting.");
  } catch (error) {
    alert("Failed to join the meeting.");
    console.error("Error joining meeting:", error);
  }
};


const handleStartMeeting = async (bookingId) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.post(
      `http://localhost:5091/api/MeetingAttendee/start/${bookingId}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log(response.data);
    alert("Meeting started successfully.");
  } catch (error) {
    alert("Failed to start the meeting.");
    console.error("Error starting meeting:", error);
  }
};

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

 return (
  <div className="meetings-container">
    <Sidebar />

    {/* Your Created Meetings */}
    <div className="meetings-list">
      <h2>Your Created Meetings</h2>
      {createdMeetings.length === 0 && <p>You have not created any meetings.</p>}
      {createdMeetings.map((meeting) => (
        <div className="meeting-card" key={meeting.id}>
          <table>
            <tbody>
              <tr>
                <td><strong>Room:</strong> {meeting.name || "N/A"}</td>
                <td><strong>Location:</strong> {meeting.location || "N/A"}</td>
              </tr>
              <tr>
                <td><strong>Start Time:</strong> {meeting.startTime ? new Date(meeting.startTime).toLocaleString() : 'N/A'}</td>
                <td><strong>End Time:</strong> {meeting.endTime ? new Date(meeting.endTime).toLocaleString() : 'N/A'}</td>
              </tr>
              <tr>
                <td><strong>Purpose:</strong> {meeting.purpose || "N/A"}</td>
                <td><strong>Status:</strong> {meeting.status || "N/A"}</td>
              </tr>
            </tbody>
          </table>
          <button onClick={() => handleStartMeeting(meeting.id)}>Start Meeting</button>
        </div>
      ))}
    </div>

    {/* Your Invitations */}
    <div className="meetings-list">
      <h2>Your Invitations</h2>
      {invitedMeetings.length === 0 && <p>You have no invitations.</p>}
      {invitedMeetings.map((meeting) => (
        <div className="meeting-card" key={meeting.id}>
          <table>
            <tbody>
              <tr>
                <td><strong>Room:</strong> {meeting.room || "N/A"}</td>
                <td><strong>Location:</strong> {meeting.location || "N/A"}</td>
              </tr>
              <tr>
                <td><strong>Start Time:</strong> {meeting.startTime ? new Date(meeting.startTime).toLocaleString() : 'N/A'}</td>
                <td><strong>End Time:</strong> {meeting.endTime ? new Date(meeting.endTime).toLocaleString() : 'N/A'}</td>
              </tr>
              <tr>
                <td><strong>Purpose:</strong> {meeting.purpose || "N/A"}</td>
                <td><strong>Organizer:</strong> {meeting.organizerUsername || "N/A"}</td>
              </tr>
            </tbody>
          </table>
          <button onClick={() => handleJoinMeeting(meeting.id)}>Join Meeting</button>
        </div>
      ))}
    </div>
  </div>
);

};

export default Meetings;

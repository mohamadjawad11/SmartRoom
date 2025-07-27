import React, { useEffect, useState } from "react";
import Sidebar from "../Sidebar/Sidebar";
import "./MeetingInside.css";
import { useParams } from "react-router-dom";

const MeetingInside = () => {
  const { bookingId } = useParams();
  const [attendees, setAttendees] = useState([]);
  const [notes, setNotes] = useState("");
  const [tasks, setTasks] = useState([]);
  const [taskDescription, setTaskDescription] = useState("");
  const [assignedUserId, setAssignedUserId] = useState("");
  const [startTime, setStartTime] = useState(null);
  const [endTime, setEndTime] = useState(null);
  const [timeLeft, setTimeLeft] = useState("Loading...");

  const token = localStorage.getItem("token");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const attendeeRes = await fetch(`http://localhost:5091/api/MeetingAttendee/booking/${bookingId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        const attendeesData = await attendeeRes.json();
        setAttendees(attendeesData);

        const taskRes = await fetch(`http://localhost:5091/api/MeetingTask/${bookingId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        const taskData = await taskRes.json();
        setTasks(taskData);

        const bookingRes = await fetch(`http://localhost:5091/api/Booking/${bookingId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        const bookingData = await bookingRes.json();
        const start = new Date(bookingData.startTime);
        const end = new Date(bookingData.endTime);
        setStartTime(start);
        setEndTime(end);
      } catch (error) {
        console.error("Error fetching meeting data:", error);
      }
    };

    fetchData();
  }, [bookingId, token]);

  // Countdown effect
  useEffect(() => {
    if (startTime && endTime) {
      const updateDuration = () => {
        const now = new Date();
        const remainingMs = endTime - now;

        if (remainingMs <= 0) {
          setTimeLeft("Meeting ended");
          return;
        }

        const minutes = Math.floor(remainingMs / 60000);
        const seconds = Math.floor((remainingMs % 60000) / 1000);
        setTimeLeft(`${minutes}m ${seconds < 10 ? "0" : ""}${seconds}s`);
      };

      updateDuration();
      const interval = setInterval(updateDuration, 1000);

      return () => clearInterval(interval);
    }
  }, [startTime, endTime]);

  const handleNoteSubmit = async () => {
    try {
      await fetch("http://localhost:5091/api/MeetingNotes", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          bookingId: parseInt(bookingId),
          content: notes,
        }),
      });
      setNotes("");
      alert("Note submitted");
    } catch (error) {
      console.error("Failed to submit note:", error);
      alert("Failed to submit note");
    }
  };

  const handleTaskAssign = async () => {
    try {
      await fetch("http://localhost:5091/api/MeetingTask", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          bookingId: parseInt(bookingId),
          assignedUserId: parseInt(assignedUserId),
          taskDescription,
        }),
      });
      setTaskDescription("");
      setAssignedUserId("");

      const taskRes = await fetch(`http://localhost:5091/api/MeetingTask/${bookingId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      const taskData = await taskRes.json();
      setTasks(taskData);
    } catch (error) {
      console.error("Failed to assign task:", error);
      alert("Failed to assign task");
    }
  };

  return (
    <div className="meeting-page2">
      <Sidebar />
      <div className="meeting-content2">
        <h1>Meeting Inside</h1>

        <div className="meeting-section2">
          <h2>Meeting Duration</h2>
          <p>{timeLeft}</p>
        </div>

        <div className="meeting-section2">
          <h2>Attendees</h2>
          <ul>
            {attendees.map((att) => (
              <li key={att.id}>
                {att.username} - <strong>{att.status}</strong>
              </li>
            ))}
          </ul>
        </div>

        <div className="meeting-section2">
          <h2>Meeting Notes</h2>
          <textarea
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            placeholder="Write meeting note..."
          ></textarea>
          <button className="sbmt-btn" onClick={handleNoteSubmit}>Submit Note</button>
        </div>

        <div className="meeting-section2">
          <h2>Assign Task</h2>
          <select
            value={assignedUserId}
            onChange={(e) => setAssignedUserId(e.target.value)}
          >
            <option value="">Select Attendee</option>
            {attendees.map((att) => (
              <option key={att.id} value={att.userId}>
                {att.username}
              </option>
            ))}
          </select>
          <input
            type="text"
            placeholder="Task description"
            value={taskDescription}
            onChange={(e) => setTaskDescription(e.target.value)}
          />
          <button className="sbmt-btn" onClick={handleTaskAssign}>Assign Task</button>

          <ul className="task-list">
            {tasks.map((task) => (
              <li key={task.id}>
                <strong>{task.assignedTo}:</strong> {task.taskDescription}
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
};

export default MeetingInside;

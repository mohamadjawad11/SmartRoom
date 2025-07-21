import React, { useEffect, useState } from "react";
import axios from "axios";
import Sidebar from "../Sidebar/Sidebar";
import "./Dashboard.css";
import {
  FaUsers,
  FaClock,
  FaChartBar,
  FaDoorOpen,
  FaCalendarAlt,
} from "react-icons/fa";
import {
  BarChart,
  Bar,
  LineChart,
  Line,
  PieChart,
  Pie,
  Cell,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  ResponsiveContainer,
  Legend,
} from "recharts";

export default function Dashboard() {
  const [kpis, setKpis] = useState({});
  const [meetingsOverTime, setMeetingsOverTime] = useState([]);
  const [topRooms, setTopRooms] = useState([]);
  const [topUsers, setTopUsers] = useState([]);
  const [hourlyDist, setHourlyDist] = useState([]);

  const token = localStorage.getItem("token");

  useEffect(() => {
    const headers = { Authorization: `Bearer ${token}` };
    axios.get("http://localhost:5091/api/dashboard/kpis", { headers }).then((res) => setKpis(res.data));
    axios.get("http://localhost:5091/api/dashboard/meetings-over-time", { headers }).then((res) => setMeetingsOverTime(res.data));
    axios.get("http://localhost:5091/api/dashboard/top-rooms", { headers }).then((res) => setTopRooms(res.data));
    axios.get("http://localhost:5091/api/dashboard/top-users", { headers }).then((res) => setTopUsers(res.data));
    axios.get("http://localhost:5091/api/dashboard/hourly-distribution", { headers }).then((res) => setHourlyDist(res.data));
  }, []);

  const kpiCards = [
    {
      icon: <FaCalendarAlt />,
      title: "Total Meetings",
      value: kpis.totalMeetings || 0,
    },
    {
      icon: <FaDoorOpen />,
      title: "Total Rooms",
      value: kpis.totalRooms || 0,
    },
    {
      icon: <FaUsers />,
      title: "Active Users",
      value: kpis.activeUsers || 0,
    },
    {
      icon: <FaClock />,
      title: "Avg. Duration (min)",
      value: kpis.averageDurationMinutes || 0,
    },
    {
      icon: <FaChartBar />,
      title: "Utilization Rate",
      value: `${kpis.utilizationRate || 0}%`,
    },
  ];

  return (
    <div className="dashboard-container">
      <Sidebar />
      <div className="dashboard-content">
        <h2>📊 Dashboard Overview</h2>

        <div className="kpi-grid">
          {kpiCards.map((card, i) => (
            <div className="kpi-card" key={i}>
              <div className="kpi-icon">{card.icon}</div>
              <div className="kpi-info">
                <h4>{card.title}</h4>
                <p>{card.value}</p>
              </div>
            </div>
          ))}
        </div>

        <div className="charts-grid">
          <div className="chart-box">
            <h3>Meetings Over Time</h3>
            <ResponsiveContainer width="100%" height={250}>
              <LineChart data={meetingsOverTime}>
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="date" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="meetingCount" stroke="#8884d8" />
              </LineChart>
            </ResponsiveContainer>
          </div>

          <div className="chart-box">
            <h3>Top Booked Rooms</h3>
            <ResponsiveContainer width="100%" height={250}>
              <BarChart data={topRooms}>
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="roomName" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Bar dataKey="bookingCount" fill="#82ca9d" />
              </BarChart>
            </ResponsiveContainer>
          </div>

          <div className="chart-box">
            <h3>Top Users</h3>
            <ResponsiveContainer width="100%" height={250}>
              <BarChart data={topUsers}>
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="username" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Bar dataKey="bookingCount" fill="#ffc658" />
              </BarChart>
            </ResponsiveContainer>
          </div>

          <div className="chart-box">
            <h3>Hourly Distribution</h3>
            <ResponsiveContainer width="100%" height={250}>
              <BarChart data={hourlyDist}>
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="hour" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Bar dataKey="bookingCount" fill="#8884d8" />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>
    </div>
  );
}

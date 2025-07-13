import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

// Auth & Admin
import Login from "./components/SignIn/Signin.jsx";
import AdminLogin from "./components/AdminLogic/AdminLogic.jsx";
import AdminHome from "./components/AdminHome/AdminHome.jsx";

// User-related
import Profile from "./components/Profile/Profile.jsx";
import BookingForm from "./components/BookingForm/BookingForm.jsx";
import Bookings from "./components/Booking/Bookings.jsx";
import ManageAttendees from "./components/ManageAttendees/ManageAttendees.jsx";
import MyInvitations from "./components/MyInvitations/MyInvitations.jsx";
import ViewRooms from "./components/ViewRooms/ViewRooms.jsx";
import ModifyBooking from "./components/ModifyBooking/ModifyBooking.jsx"; // ✅ NEW

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/admin" element={<AdminLogin />} />
        <Route path="/admin-home" element={<AdminHome />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/book-room" element={<BookingForm />} />
        <Route path="/bookings" element={<Bookings />} />
        <Route path="/modify-booking/:id" element={<ModifyBooking />} /> {/* ✅ NEW ROUTE */}
        <Route path="/manage-attendees/:bookingId" element={<ManageAttendees />} />
        <Route path="/my-invitations" element={<MyInvitations />} />
        <Route path="/view-rooms" element={<ViewRooms />} />
      </Routes>
    </Router>
  );
}

export default App;

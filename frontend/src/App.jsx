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
import ModifyBooking from "./components/ModifyBooking/ModifyBooking.jsx";
import ManageAttendees from "./components/ManageAttendees/ManageAttendees.jsx";
import MyInvitations from "./components/MyInvitations/MyInvitations.jsx";
import ViewRooms from "./components/ViewRooms/ViewRooms.jsx";

// System Management
import ManageSystem from "./components/ManageSystem/ManageSystem.jsx";
import AddEmployee from "./components/AddEmployee/AddEmployee.jsx";
import UpdateEmployee from "./components/UpdateEmployee/UpdateEmployee.jsx";
import DeleteEmployee from "./components/DeleteEmployee/DeleteEmployee.jsx";
import AddRoom from "./components/AddRoom/AddRoom.jsx";
import UpdateRoom from "./components/UpdateRoom/UpdateRoom.jsx";
import DeleteRoom from "./components/DeleteRoom/DeleteRoom.jsx";
import Meeting from "./components/Meeting/Meeting.jsx";

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
        <Route path="/modify-booking/:id" element={<ModifyBooking />} />
        <Route path="/manage-attendees/:bookingId" element={<ManageAttendees />} />
        <Route path="/my-invitations" element={<MyInvitations />} />
        <Route path="/view-rooms" element={<ViewRooms />} />
        <Route path="/manage-system" element={<ManageSystem />} />
        <Route path="/add-employee" element={<AddEmployee />} />
        <Route path="/update-employee" element={<UpdateEmployee />} />
        <Route path="/delete-employee" element={<DeleteEmployee />} />
        <Route path="/add-room" element={<AddRoom />} />
        <Route path="/update-room" element={<UpdateRoom />} />
        <Route path="/delete-room" element={<DeleteRoom />} />
        <Route path="/meetings" element={<Meeting />} />
        
        
      </Routes>
    </Router>
  );
}

export default App;


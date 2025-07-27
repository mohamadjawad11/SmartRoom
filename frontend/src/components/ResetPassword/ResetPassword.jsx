import React, { useState } from 'react';
import './ResetPassword.css';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const ResetPassword = () => {
  const [step, setStep] = useState(1);
  const [email, setEmail] = useState('');
  const [otp, setOtp] = useState('');
  const [newPass, setNewPass] = useState('');
  const [confirmPass, setConfirmPass] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSendOtp = async () => {
    try {
      await axios.post('http://localhost:5091/Api/auth/request-reset', { email });
      setStep(2);
      setError('');
    } catch (err) {
      setError(err.response?.data?.message || 'Something went wrong');
    }
  };

  const handleVerifyOtp = async () => {
    try {
      await axios.post('http://localhost:5091/api/Auth/verify-otp', { email, code: otp });
      setStep(3);
      setError('');
    } catch (err) {
      setError(err.response?.data?.message || 'Invalid or expired code');
    }
  };

  const handleUpdatePassword = async () => {
    if (newPass !== confirmPass) {
      setError('Passwords do not match');
      return;
    }
    try {
      await axios.post('http://localhost:5091/api/Auth/reset-password', {
        email,
        code: otp,
        newPassword: newPass
      });
      setError('');
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to update password');
    }
  };

  return (
    <div className="reset-container">
      <div className="reset-box">
        <h2>Reset Password</h2>
        {step === 1 && (
          <>
            <input
              type="email"
              placeholder="Enter your email"
              className="reset-input"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <button className="reset-button" onClick={handleSendOtp}>
              Reset Password
            </button>
          </>
        )}

        {step === 2 && (
          <>
            <input
              type="text"
              placeholder="Enter OTP"
              className="reset-input"
              value={otp}
              onChange={(e) => setOtp(e.target.value)}
              maxLength={6}
            />
            <button className="reset-button" onClick={handleVerifyOtp}>
              Verify Code
            </button>
          </>
        )}

        {step === 3 && (
          <>
            <input
              type="password"
              placeholder="New Password"
              className="reset-input"
              value={newPass}
              onChange={(e) => setNewPass(e.target.value)}
            />
            <input
              type="password"
              placeholder="Confirm Password"
              className="reset-input"
              value={confirmPass}
              onChange={(e) => setConfirmPass(e.target.value)}
            />
            <button className="reset-button" onClick={handleUpdatePassword}>
              Update Password
            </button>
          </>
        )}

        {error && <p className="reset-error">{error}</p>}
      </div>
    </div>
  );
};

export default ResetPassword;

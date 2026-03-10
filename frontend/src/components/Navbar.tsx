import React, { useState, useRef, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Conversations from '../pages/Conversations';
import '../styles/Navbar.css';

const Navbar: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [showProfileMenu, setShowProfileMenu] = useState(false);
  const [showConversations, setShowConversations] = useState(false);
  const profileMenuRef = useRef<HTMLDivElement>(null);
  const conversationsRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (profileMenuRef.current && !profileMenuRef.current.contains(event.target as Node)) {
        setShowProfileMenu(false);
      }
      if (conversationsRef.current && !conversationsRef.current.contains(event.target as Node)) {
        setShowConversations(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <div className="navbar-brand">
          <h1>📚 Reading Community</h1>
        </div>

        <div className="navbar-icons">
          {/* Conversations Icon */}
          <div className="navbar-icon-wrapper" ref={conversationsRef}>
            <button
              className="navbar-icon-button"
              onClick={() => setShowConversations(!showConversations)}
              title="Messages"
            >
              💬
            </button>
            {showConversations && <Conversations />}
          </div>

          {/* Profile Icon */}
          <div className="navbar-icon-wrapper" ref={profileMenuRef}>
            <button
              className="navbar-icon-button"
              onClick={() => setShowProfileMenu(!showProfileMenu)}
              title="Profile"
            >
              👤
            </button>

            {showProfileMenu && (
              <div className="profile-menu">
                <div className="profile-menu-header">
                  <p className="user-name">{user?.name}</p>
                  <p className="user-email">{user?.email}</p>
                </div>
                <a href="/profile" className="menu-item">
                  View Profile
                </a>
                <a href="/settings" className="menu-item">
                  Settings
                </a>
                <hr />
                <button onClick={handleLogout} className="menu-item logout">
                  Logout
                </button>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;

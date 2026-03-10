import React, { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import apiService from '../services/apiService';
import { UserDetailDTO } from '../types';
import '../styles/Profile.css';

const Profile: React.FC = () => {
  const { user } = useAuth();
  const [profile, setProfile] = useState<UserDetailDTO | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    try {
      setIsLoading(true);
      const response = await apiService.getUserProfile();
      if (response.data.data) {
        setProfile(response.data.data);
      }
    } catch (err: any) {
      setError('Failed to load profile');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="profile-container">
      <div className="profile-card">
        <div className="profile-header">
          <div className="profile-avatar">👤</div>
          <div className="profile-info">
            <h1>{profile?.userName || user?.name}</h1>
            <p>{user?.email}</p>
          </div>
        </div>

        {error && <div className="error-message">{error}</div>}

        {isLoading ? (
          <div className="loading">Loading profile...</div>
        ) : profile ? (
          <>
            <div className="profile-details">
              <div className="profile-section">
                <h3>Followers</h3>
                <p>{profile.numberFollowers}</p>
              </div>

              <div className="profile-section">
                <h3>Following</h3>
                <p>{profile.numberFollowing}</p>
              </div>

              {profile.wishLists && profile.wishLists.length > 0 && (
                <div className="profile-section">
                  <h3>Wish Lists</h3>
                  <ul>
                    {profile.wishLists.map((list) => (
                      <li key={list.id}>{list.name} ({list.numberOfItems} items)</li>
                    ))}
                  </ul>
                </div>
              )}
            </div>

            <button className="edit-profile-button">Edit Profile</button>
          </>
        ) : null}
      </div>
    </div>
  );
};

export default Profile;

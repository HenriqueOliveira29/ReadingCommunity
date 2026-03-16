import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiService from '../services/apiService';
import { AuthorCreateDTO } from '../types';
import '../styles/CreateAuthor.css';

const CreateAuthor: React.FC = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<AuthorCreateDTO>({
    name: '',
    biography: '',
    birthDate: null,
    deathDate: null,
    nationality: '',
    profileImageUrl: '',
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'birthDate' || name === 'deathDate' 
        ? (value ? new Date(value + 'T00:00:00').toISOString() : null)
        : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.name.trim() || !formData.nationality.trim()) {
      setError('Please fill in all required fields');
      return;
    }

    try {
      setIsLoading(true);
      setError('');
      const response = await apiService.createAuthor(formData);
      if (response.data.isSuccess) {
        navigate('/authors');
      } else {
        setError(response.data.message || 'Failed to create author');
      }
    } catch (err: any) {
      setError('Failed to create author');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="create-author-container">
      <h1>Create New Author</h1>

      {error && <div className="error-message">{error}</div>}

      <form onSubmit={handleSubmit} className="create-author-form">
        <div className="form-group">
          <label htmlFor="name">Name *</label>
          <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="nationality">Nationality *</label>
          <input
            type="text"
            id="nationality"
            name="nationality"
            value={formData.nationality}
            onChange={handleInputChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="biography">Biography</label>
          <textarea
            id="biography"
            name="biography"
            value={formData.biography}
            onChange={handleInputChange}
            rows={6}
          />
        </div>

        <div className="form-row">
          <div className="form-group">
            <label htmlFor="birthDate">Birth Date</label>
            <input
              type="date"
              id="birthDate"
              name="birthDate"
              value={formData.birthDate ? formData.birthDate.split('T')[0] : ''}
              onChange={handleInputChange}
            />
          </div>

          <div className="form-group">
            <label htmlFor="deathDate">Death Date</label>
            <input
              type="date"
              id="deathDate"
              name="deathDate"
              value={formData.deathDate ? formData.deathDate.split('T')[0] : ''}
              onChange={handleInputChange}
            />
          </div>
        </div>

        <div className="form-group">
          <label htmlFor="profileImageUrl">Profile Image URL</label>
          <input
            type="url"
            id="profileImageUrl"
            name="profileImageUrl"
            value={formData.profileImageUrl}
            onChange={handleInputChange}
          />
        </div>

        <button type="submit" disabled={isLoading} className="submit-button">
          {isLoading ? 'Creating...' : 'Create Author'}
        </button>
      </form>
    </div>
  );
};

export default CreateAuthor;
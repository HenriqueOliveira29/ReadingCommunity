import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiService from '../services/apiService';
import { AuthorListDTO } from '../types';
import '../styles/Authors.css';

const Authors: React.FC = () => {
  const navigate = useNavigate();
  const [authors, setAuthors] = useState<AuthorListDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchAuthors();
  }, []);

  const fetchAuthors = async () => {
    try {
      setIsLoading(true);
      const response = await apiService.getAuthors();
      if (response.data.data?.items) {
        setAuthors(response.data.data.items);
      }
    } catch (err: any) {
      setError('Failed to load authors');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="authors-container">
      <div className="authors-header">
        <h1>Authors Collection</h1>
        <button className="add-author-button" onClick={() => navigate('/create-author')}>
          Add New Author
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {isLoading ? (
        <div className="loading">Loading authors...</div>
      ) : authors.length === 0 ? (
        <div className="no-authors">No authors available yet.</div>
      ) : (
        <div className="authors-grid">
          {authors.map((author) => (
            <div key={author.id} className="author-card" onClick={() => navigate(`/authors/${author.id}`)}>
              <div className="author-image">
                <img src={author.profileImageUrl || "https://via.placeholder.com/200x250?text=Author"} alt={author.name} />
              </div>
              <div className="author-content">
                <h3>{author.name}</h3>
                <p className="nationality">{author.nationality}</p>
                <p className="books-count">{author.numberOfBooks} books</p>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Authors;
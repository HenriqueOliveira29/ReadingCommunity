import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import apiService from '../services/apiService';
import { AuthorDetailDTO } from '../types';
import '../styles/AuthorDetail.css';

const AuthorDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [author, setAuthor] = useState<AuthorDetailDTO | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (id) {
      fetchAuthorDetail(parseInt(id));
    }
  }, [id]);

  const fetchAuthorDetail = async (authorId: number) => {
    try {
      setIsLoading(true);
      const response = await apiService.getAuthorById(authorId);
      if (response.data.isSuccess && response.data.data) {
        setAuthor(response.data.data);
      } else {
        setError(response.data.message || 'Failed to load author details');
      }
    } catch (err: any) {
      setError('Failed to load author details');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    return (
      <div className="author-detail-container">
        <div className="loading">Loading author details...</div>
      </div>
    );
  }

  if (error || !author) {
    return (
      <div className="author-detail-container">
        <div className="error-message">{error || 'Author not found'}</div>
        <button onClick={() => navigate('/authors')} className="back-button">
          Back to Authors
        </button>
      </div>
    );
  }

  return (
    <div className="author-detail-container">
      <div className="author-detail-header">
        <button onClick={() => navigate('/authors')} className="back-button">
          ← Back to Authors
        </button>
      </div>

      <div className="author-detail-content">
        <div className="author-profile">
          <div className="author-image-large">
            <img src={author.profileImageUrl || "https://via.placeholder.com/300x400?text=Author"} alt={author.name} />
          </div>
          <div className="author-info">
            <h1>{author.name}</h1>
            <div className="author-meta">
              <p className="nationality">🇺🇸 {author.nationality}</p>
              <p className="age-status">
                {author.age} years old • {author.isAlive ? 'Alive' : 'Deceased'}
              </p>
              {author.birthDate && (
                <p className="birth-date">
                  Born: {new Date(author.birthDate).toLocaleDateString()}
                </p>
              )}
              {author.deathDate && (
                <p className="death-date">
                  Died: {new Date(author.deathDate).toLocaleDateString()}
                </p>
              )}
            </div>
          </div>
        </div>

        <div className="author-biography">
          <h2>Biography</h2>
          <p>{author.biography || 'No biography available.'}</p>
        </div>

        <div className="author-books">
          <h2>Books ({author.books.length})</h2>
          {author.books.length > 0 ? (
            <div className="books-grid">
              {author.books.map((book) => (
                <div key={book.id} className="book-card-small" onClick={() => navigate(`/books/${book.id}`)}>
                  <div className="book-image-small">
                    <img src={book.coverImageUrl || "https://via.placeholder.com/150x200?text=Book"} alt={book.title} />
                  </div>
                  <div className="book-info-small">
                    <h4>{book.title}</h4>
                    <p className="book-year">
                      {book.publicationDate ? new Date(book.publicationDate).getFullYear() : 'Unknown year'}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <p>No books found for this author.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default AuthorDetail;
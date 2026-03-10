import React, { useEffect, useState } from 'react';
import apiService from '../services/apiService';
import { BookListDTO } from '../types';
import '../styles/Home.css';

const Home: React.FC = () => {
  const [books, setBooks] = useState<BookListDTO[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchBooks();
  }, []);

  const fetchBooks = async () => {
    try {
      setIsLoading(true);
      const response = await apiService.getBooks();
      if (response.data.data?.items) {
        setBooks(response.data.data.items);
      }
    } catch (err: any) {
      setError('Failed to load books');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="home-container">
      <h1>Books Collection</h1>

      {error && <div className="error-message">{error}</div>}

      {isLoading ? (
        <div className="loading">Loading books...</div>
      ) : books.length === 0 ? (
        <div className="no-books">No books available yet.</div>
      ) : (
        <div className="books-grid">
          {books.map((book) => (
            <div key={book.id} className="book-card">
              <div className="book-image">
                <img src="https://via.placeholder.com/200x300?text=Book" alt={book.title} />
              </div>
              <div className="book-content">
                <h3>{book.title}</h3>
                <p className="author">{book.authorName}</p>
                <p className="description">{book.description}</p>
                {book.categories && book.categories.length > 0 && (
                  <div className="categories">
                    {book.categories.join(', ')}
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Home;

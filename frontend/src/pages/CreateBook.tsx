import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiService from '../services/apiService';
import { BookCreateDTO, AuthorListDTO } from '../types';
import '../styles/CreateBook.css';

const CreateBook: React.FC = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<BookCreateDTO>({
    title: '',
    authorId: 0,
    description: '',
    publicationDate: '',
    coverImageUrl: '',
    numberOfPages: 0,
    width: 0,
    height: 0,
    depth: 0,
  });
  const [authors, setAuthors] = useState<AuthorListDTO[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingAuthors, setIsLoadingAuthors] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchAuthors();
  }, []);

  const fetchAuthors = async () => {
    try {
      setIsLoadingAuthors(true);
      const response = await apiService.getAuthors();
      if (response.data.data?.items) {
        setAuthors(response.data.data.items);
      }
    } catch (err: any) {
      setError('Failed to load authors');
      console.error(err);
    } finally {
      setIsLoadingAuthors(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'authorId' || name === 'numberOfPages' ? parseInt(value) || 0 : 
               name === 'width' || name === 'height' || name === 'depth' ? parseFloat(value) || 0 :
               name === 'publicationDate' ? (value ? new Date(value + 'T00:00:00').toISOString() : '') : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.title.trim() || !formData.authorId || !formData.description.trim()) {
      setError('Please fill in all required fields');
      return;
    }

    try {
      setIsLoading(true);
      setError('');
      const response = await apiService.createBook(formData);
      if (response.data.isSuccess) {
        navigate('/');
      } else {
        setError(response.data.message || 'Failed to create book');
      }
    } catch (err: any) {
      setError('Failed to create book');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="create-book-container">
      <h1>Create New Book</h1>

      {error && <div className="error-message">{error}</div>}

      <form onSubmit={handleSubmit} className="create-book-form">
        <div className="form-group">
          <label htmlFor="title">Title *</label>
          <input
            type="text"
            id="title"
            name="title"
            value={formData.title}
            onChange={handleInputChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="authorId">Author *</label>
          <select
            id="authorId"
            name="authorId"
            value={formData.authorId}
            onChange={handleInputChange}
            required
            disabled={isLoadingAuthors}
          >
            <option value={0}>Select an author</option>
            {authors.map(author => (
              <option key={author.id} value={author.id}>
                {author.name}
              </option>
            ))}
          </select>
          {isLoadingAuthors && <div className="loading-authors">Loading authors...</div>}
        </div>

        <div className="form-group">
          <label htmlFor="description">Description *</label>
          <textarea
            id="description"
            name="description"
            value={formData.description}
            onChange={handleInputChange}
            rows={4}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="publicationDate">Publication Date</label>
          <input
            type="date"
            id="publicationDate"
            name="publicationDate"
            value={formData.publicationDate ? formData.publicationDate.split('T')[0] : ''}
            onChange={handleInputChange}
          />
        </div>

        <div className="form-group">
          <label htmlFor="coverImageUrl">Cover Image URL</label>
          <input
            type="url"
            id="coverImageUrl"
            name="coverImageUrl"
            value={formData.coverImageUrl}
            onChange={handleInputChange}
          />
        </div>

        <div className="form-group">
          <label htmlFor="numberOfPages">Number of Pages</label>
          <input
            type="number"
            id="numberOfPages"
            name="numberOfPages"
            value={formData.numberOfPages}
            onChange={handleInputChange}
            min="1"
          />
        </div>

        <div className="dimensions-group">
          <h3>Dimensions (cm)</h3>
          <div className="form-group">
            <label htmlFor="width">Width</label>
            <input
              type="number"
              id="width"
              name="width"
              value={formData.width}
              onChange={handleInputChange}
              step="0.1"
              min="0"
            />
          </div>
          <div className="form-group">
            <label htmlFor="height">Height</label>
            <input
              type="number"
              id="height"
              name="height"
              value={formData.height}
              onChange={handleInputChange}
              step="0.1"
              min="0"
            />
          </div>
          <div className="form-group">
            <label htmlFor="depth">Depth</label>
            <input
              type="number"
              id="depth"
              name="depth"
              value={formData.depth}
              onChange={handleInputChange}
              step="0.1"
              min="0"
            />
          </div>
        </div>

        <button type="submit" disabled={isLoading} className="submit-button">
          {isLoading ? 'Creating...' : 'Create Book'}
        </button>
      </form>
    </div>
  );
};

export default CreateBook;
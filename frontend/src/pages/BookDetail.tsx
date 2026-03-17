import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import apiService from '../services/apiService';
import { BookDetailDTO, ReviewCreateDTO } from '../types';
import { useAuth } from '../context/AuthContext';
import '../styles/BookDetail.css';

const BookDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [book, setBook] = useState<BookDetailDTO | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [reviewRating, setReviewRating] = useState(5);
  const [reviewComment, setReviewComment] = useState('');
  const [isSubmittingReview, setIsSubmittingReview] = useState(false);
  const [showReviewForm, setShowReviewForm] = useState(false);
  const [followingStatuses, setFollowingStatuses] = useState<Record<number, boolean>>({});

  useEffect(() => {
    if (id) {
      fetchBookDetail(parseInt(id));
    }
  }, [id]);

  const fetchBookDetail = async (bookId: number) => {
    try {
      setIsLoading(true);
      const response = await apiService.getBookById(bookId);
      if (response.data.isSuccess && response.data.data) {
        setBook(response.data.data);
        // Fetch following statuses for reviewers
        if (user) {
          await fetchFollowingStatuses(response.data.data.reviews);
        }
      } else {
        setError(response.data.message || 'Failed to load book details');
      }
    } catch (err: any) {
      setError('Failed to load book details');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchFollowingStatuses = async (reviews: any[]) => {
    const statuses: Record<number, boolean> = {};
    for (const review of reviews) {
      try {
        const response = await apiService.isFollowing(review.userId);
        if (response.data.isSuccess) {
          statuses[review.userId] = response.data.data;
        }
      } catch (err) {
        console.error(`Failed to check following status for user ${review.userId}`, err);
      }
    }
    setFollowingStatuses(statuses);
  };

  const handleSubmitReview = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id) return;

    try {
      setIsSubmittingReview(true);
      const reviewData: ReviewCreateDTO = {
        rating: reviewRating,
        comment: reviewComment,
        bookId: parseInt(id)
      };

      const response = await apiService.createReview(reviewData);
      if (response.data.isSuccess) {
        // Refresh book details to show the new review
        await fetchBookDetail(parseInt(id));
        // Reset form
        setReviewRating(5);
        setReviewComment('');
        setShowReviewForm(false);
      } else {
        setError(response.data.message || 'Failed to add review');
      }
    } catch (err: any) {
      setError('Failed to add review');
      console.error(err);
    } finally {
      setIsSubmittingReview(false);
    }
  };

  const handleFollowUser = async (userId: number) => {
    try {
      const isCurrentlyFollowing = followingStatuses[userId];
      const response = isCurrentlyFollowing 
        ? await apiService.unFollowUser(userId)
        : await apiService.followUser(userId);
      
      if (response.data.isSuccess) {
        setError(''); // Clear any previous error
        // Update the following status
        setFollowingStatuses(prev => ({
          ...prev,
          [userId]: !isCurrentlyFollowing
        }));
      } else {
        setError(response.data.message || `Failed to ${isCurrentlyFollowing ? 'unfollow' : 'follow'} user`);
      }
    } catch (err: any) {
      setError(`Failed to ${followingStatuses[userId] ? 'unfollow' : 'follow'} user`);
      console.error(err);
    }
  };

  if (isLoading) {
    return (
      <div className="book-detail-container">
        <div className="loading">Loading book details...</div>
      </div>
    );
  }

  if (error || !book) {
    return (
      <div className="book-detail-container">
        <div className="error-message">{error || 'Book not found'}</div>
        <button onClick={() => navigate('/')} className="back-button">
          Back to Books
        </button>
      </div>
    );
  }

  return (
    <div className="book-detail-container">
      <div className="book-detail-header">
        <button onClick={() => navigate('/')} className="back-button">
          ← Back to Books
        </button>
      </div>

      <div className="book-detail-content">
        <div className="book-profile">
          <div className="book-image-large">
            <img src={book.coverImageUrl || "https://via.placeholder.com/300x400?text=Book"} alt={book.title} />
          </div>
          <div className="book-info">
            <h1>{book.title}</h1>
            <div className="book-meta">
              <p className="author">by <span className="author-link" onClick={() => navigate(`/authors/${book.authorId}`)}>{book.authorName}</span></p>
              <p className="publication-date">
                Published: {new Date(book.publicationDate).toLocaleDateString()}
              </p>
              <p className="pages">📖 {book.numberOfPages} pages</p>
              <p className="dimensions">📏 {book.dimensions}</p>
            </div>
            {book.categories && book.categories.length > 0 && (
              <div className="categories">
                {book.categories.map((category, index) => (
                  <span key={index} className="category-tag">{category}</span>
                ))}
              </div>
            )}
          </div>
        </div>

        <div className="book-description">
          <h2>Description</h2>
          <p>{book.description || 'No description available.'}</p>
        </div>

        {book.images && book.images.length > 0 && (
          <div className="book-images">
            <h2>Additional Images</h2>
            <div className="images-grid">
              {book.images.map((imageUrl, index) => (
                <div key={index} className="image-item">
                  <img src={imageUrl} alt={`${book.title} - Image ${index + 1}`} />
                </div>
              ))}
            </div>
          </div>
        )}

        <div className="add-review-section">
          {!showReviewForm ? (
            <button onClick={() => setShowReviewForm(true)} className="add-review-button">
              ✍️ Write a Review
            </button>
          ) : (
            <div className="review-form-container">
              <h3>Write a Review</h3>
              <form onSubmit={handleSubmitReview} className="review-form">
                <div className="rating-section">
                  <label>Rating:</label>
                  <div className="star-rating">
                    {[1, 2, 3, 4, 5].map((star) => (
                      <span
                        key={star}
                        className={`star ${star <= reviewRating ? 'selected' : ''}`}
                        onClick={() => setReviewRating(star)}
                      >
                        ★
                      </span>
                    ))}
                  </div>
                </div>
                <div className="comment-section">
                  <label htmlFor="comment">Comment:</label>
                  <textarea
                    id="comment"
                    value={reviewComment}
                    onChange={(e) => setReviewComment(e.target.value)}
                    placeholder="Share your thoughts about this book..."
                    required
                    rows={4}
                  />
                </div>
                <div className="form-buttons">
                  <button type="submit" disabled={isSubmittingReview} className="submit-review-button">
                    {isSubmittingReview ? 'Submitting...' : 'Submit Review'}
                  </button>
                  <button type="button" onClick={() => setShowReviewForm(false)} className="cancel-button">
                    Cancel
                  </button>
                </div>
              </form>
            </div>
          )}
        </div>

        {book.reviews && book.reviews.length > 0 && (
          <div className="book-reviews">
            <h2>Reviews ({book.reviews.length})</h2>
            <div className="reviews-list">
              {book.reviews.map((review) => (
                <div key={review.id} className="review-item">
                  <div className="review-header">
                    <img 
                      src={review.userProfileImageUrl || "https://via.placeholder.com/40x40?text=U"} 
                      alt={`${review.userName}'s profile`} 
                      className="reviewer-avatar" 
                    />
                    <span className="reviewer-name">{review.userName}</span>
                    <span className="review-rating">⭐ {review.rating}/5</span>
                    {user && parseInt(user.id) !== review.userId && (
                      <button 
                        className={`follow-button ${followingStatuses[review.userId] ? 'following' : ''}`} 
                        onClick={() => handleFollowUser(review.userId)}
                      >
                        {followingStatuses[review.userId] ? 'Following' : 'Follow'}
                      </button>
                    )}
                  </div>
                  <p className="review-comment">{review.comment}</p>
                  <p className="review-date">
                    {new Date(review.datePosted).toLocaleDateString()}
                  </p>
                </div>
              ))}
            </div>
          </div>
        )}

        {book.reviews && book.reviews.length === 0 && (
          <div className="book-reviews">
            <h2>Reviews</h2>
            <p className="no-reviews">No reviews yet. Be the first to review this book!</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default BookDetail;
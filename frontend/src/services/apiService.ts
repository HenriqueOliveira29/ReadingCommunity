import axios, { AxiosInstance } from 'axios';
import {
  LoginDTO,
  RegisterDTO,
  AuthResponseDTO,
  BookListDTO,
  BookDetailDTO,
  BookCreateDTO,
  AuthorListDTO,
  AuthorDetailDTO,
  AuthorCreateDTO,
  ReviewDetailDTO,
  ReviewCreateDTO,
  UserDetailDTO,
  UserListDTO,
  WishlistCollectionListDTO,
  WishlistCollectionDetailDTO,
  WishlistCollectionCreateDTO,
  WishlistItemListDTO,
  WishlistItemCreateDTO,
  OperationResult,
  PageResult,
} from '../types';

const API_BASE_URL = 'https://localhost:5001/api';

class ApiService {
  private api: AxiosInstance;

  constructor() {
    this.api = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Add token to requests
    this.api.interceptors.request.use((config) => {
      const token = localStorage.getItem('token');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });
  }

  // Auth endpoints
  async login(credentials: LoginDTO) {
    return this.api.post<OperationResult<AuthResponseDTO>>('/auth/login', credentials);
  }

  async register(credentials: RegisterDTO) {
    return this.api.post<OperationResult<AuthResponseDTO>>('/auth/register', credentials);
  }

  // Book endpoints
  async getBooks(page?: number, pageSize?: number) {
    return this.api.get<OperationResult<PageResult<BookListDTO[]>>>('/book', {
      params: { page, pageSize },
    });
  }

  async getBookById(id: number) {
    return this.api.get<OperationResult<BookDetailDTO>>(`/book/${id}`);
  }

  async createBook(data: BookCreateDTO) {
    return this.api.post<OperationResult<BookDetailDTO>>('/book/add', data);
  }

  async updateBook(id: number, data: BookCreateDTO) {
    return this.api.put<OperationResult<BookDetailDTO>>(`/book/${id}`, data);
  }

  async deleteBook(id: number) {
    return this.api.delete<OperationResult<void>>(`/book/${id}`);
  }

  // Author endpoints
  async getAuthors(page?: number, pageSize?: number) {
    return this.api.get<OperationResult<PageResult<AuthorListDTO[]>>>('/author', {
      params: { page, pageSize },
    });
  }

  async getAuthorById(id: number) {
    return this.api.get<OperationResult<AuthorDetailDTO>>(`/author/${id}`);
  }

  async createAuthor(data: AuthorCreateDTO) {
    return this.api.post<OperationResult<AuthorDetailDTO>>('/author/add', data);
  }

  async updateAuthor(id: number, data: AuthorCreateDTO) {
    return this.api.put<OperationResult<AuthorDetailDTO>>(`/author/${id}`, data);
  }

  async deleteAuthor(id: number) {
    return this.api.delete<OperationResult<void>>(`/author/${id}`);
  }

  // Review endpoints
  async getReviewsByBook(bookId: number) {
    return this.api.get<OperationResult<ReviewDetailDTO[]>>(`/reviews/book/${bookId}`);
  }

  async createReview(data: ReviewCreateDTO) {
    return this.api.post<OperationResult<ReviewDetailDTO>>('/reviews', data);
  }

  async updateReview(id: number, data: ReviewCreateDTO) {
    return this.api.put<OperationResult<ReviewDetailDTO>>(`/reviews/${id}`, data);
  }

  async deleteReview(id: number) {
    return this.api.delete<OperationResult<void>>(`/reviews/${id}`);
  }

  // User endpoints
  async getUserProfile() {
    return this.api.get<OperationResult<UserDetailDTO>>('/users/profile');
  }

  async getUserById(id: number) {
    return this.api.get<OperationResult<UserDetailDTO>>(`/users/${id}`);
  }

  async updateUserProfile(data: Partial<UserDetailDTO>) {
    return this.api.put<OperationResult<UserDetailDTO>>('/users/profile', data);
  }

  async getUsers(page?: number, pageSize?: number) {
    return this.api.get<OperationResult<PageResult<UserListDTO[]>>>('/users', {
      params: { page, pageSize },
    });
  }

  // Wishlist endpoints
  async getWishlistCollections() {
    return this.api.get<OperationResult<WishlistCollectionListDTO[]>>('/wishlists');
  }

  async getWishlistCollection(id: number) {
    return this.api.get<OperationResult<WishlistCollectionDetailDTO>>(`/wishlists/${id}`);
  }

  async createWishlistCollection(data: WishlistCollectionCreateDTO) {
    return this.api.post<OperationResult<WishlistCollectionDetailDTO>>('/wishlists', data);
  }

  async updateWishlistCollection(id: number, data: WishlistCollectionCreateDTO) {
    return this.api.put<OperationResult<WishlistCollectionDetailDTO>>(`/wishlists/${id}`, data);
  }

  async deleteWishlistCollection(id: number) {
    return this.api.delete<OperationResult<void>>(`/wishlists/${id}`);
  }

  // Wishlist Items endpoints
  async addWishlistItem(data: WishlistItemCreateDTO) {
    return this.api.post<OperationResult<WishlistItemListDTO>>('/wishlist-items', data);
  }

  async removeWishlistItem(id: number) {
    return this.api.delete<OperationResult<void>>(`/wishlist-items/${id}`);
  }

  async updateWishlistItem(id: number, data: Partial<WishlistItemCreateDTO>) {
    return this.api.put<OperationResult<WishlistItemListDTO>>(`/wishlist-items/${id}`, data);
  }

  // Conversation endpoints
  async getConversations() {
    return this.api.get<any>('/conversations');
  }

  async getConversationMessages(conversationId: string) {
    return this.api.get<any>(`/conversations/${conversationId}/messages`);
  }

  async sendMessage(conversationId: string, message: string) {
    return this.api.post<OperationResult<void>>(`/conversations/${conversationId}/messages`, { message });
  }
}

export default new ApiService();

import { BookListDTO } from './book';

// Wishlist DTOs
export interface WishlistCollectionListDTO {
  id: number;
  name: string;
  description: string;
  isPublic: boolean;
  numberOfItems: number;
}

export interface WishlistCollectionDetailDTO {
  id: number;
  name: string;
  description: string;
  isPublic: boolean;
  isDefault: boolean;
  items: WishlistItemListDTO[];
}

export interface WishlistCollectionCreateDTO {
  name: string;
  description: string;
  isPublic: boolean;
  isDefault: boolean;
}

export interface WishlistItemListDTO {
  id: number;
  book: BookListDTO;
  addedAt: string;
  priority: number;
  isPurchased: boolean;
}

export interface WishlistItemCreateDTO {
  bookId: number;
  collectionId: number;
  priority: number;
}

import { ReviewDetailDTO } from './review';

// Book DTOs
export interface BookListDTO {
  id: number;
  title: string;
  description: string;
  publicationDate: string;
  authorName: string;
  categories: string[];
}

export interface BookDetailDTO {
  id: number;
  title: string;
  authorId: number;
  authorName: string;
  description: string;
  publicationDate: string;
  coverImageUrl: string;
  numberOfPages: number;
  dimensions: string;
  reviews: ReviewDetailDTO[];
  images: string[];
  categories: string[];
}

export interface BookCreateDTO {
  title: string;
  authorId: number;
  description: string;
  publicationDate: string;
  coverImageUrl: string;
  numberOfPages: number;
  width: number;
  height: number;
  depth: number;
}

import { BookListDTO } from './book';

// Author DTOs
export interface AuthorListDTO {
  id: number;
  name: string;
  nationality: string;
  numberOfBooks: number;
  profileImageUrl: string;
}

export interface AuthorDetailDTO {
  id: number;
  name: string;
  biography: string;
  birthDate: string | null;
  deathDate: string | null;
  age: number;
  isAlive: boolean;
  nationality: string;
  profileImageUrl: string;
  books: BookListDTO[];
}

export interface AuthorCreateDTO {
  name: string;
  biography: string;
  birthDate: string | null;
  deathDate: string | null;
  nationality: string;
  profileImageUrl: string;
}

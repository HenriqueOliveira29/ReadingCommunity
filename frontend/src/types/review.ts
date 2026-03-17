// Review DTOs
export interface ReviewDetailDTO {
  id: number;
  rating: number;
  comment: string;
  datePosted: string;
  bookId: number;
  bookName: string;
  userId: number;
  userName: string;
  userProfileImageUrl: string;
}

export interface ReviewCreateDTO {
  rating: number;
  comment: string;
  bookId: number;
}

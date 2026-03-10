import { WishlistCollectionListDTO } from './wishlist';

// User DTOs
export interface UserDetailDTO {
  id: number;
  userName: string;
  profileImageUrl: string;
  numberFollowers: number;
  numberFollowing: number;
  wishLists: WishlistCollectionListDTO[];
}

export interface UserListDTO {
  id: number;
  userName: string;
  profileImageUrl: string;
}

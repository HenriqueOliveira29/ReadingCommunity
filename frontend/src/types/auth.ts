// Auth DTOs
export interface LoginDTO {
  email: string;
  password: string;
}

export interface RegisterDTO {
  username: string;
  email: string;
  password: string;
}

export interface AuthResponseDTO {
  userId: number;
  username: string;
  email: string;
  token: string;
  expiresAt: string;
}

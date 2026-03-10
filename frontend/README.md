# Reading Community Frontend

A modern React + TypeScript frontend for the Reading Community application.

## Features

- **Authentication**: Login page with JWT token management
- **Navbar**: Navigation bar with profile and messaging icons
- **Profile Menu**: Quick access to user profile and settings
- **Conversations**: View all user conversations with real-time updates
- **Books List**: Browse and view all available books

## Project Structure

```
frontend/
├── public/               # Static assets
├── src/
│   ├── components/      # Reusable UI components
│   │   └── Navbar.tsx   # Navigation bar with profile & chat
│   ├── pages/           # Page components
│   │   ├── Login.tsx    # Login page
│   │   ├── Home.tsx     # Books listing page
│   │   └── Conversations.tsx  # Conversations view
│   ├── context/         # React Context
│   │   └── AuthContext.tsx    # Authentication state management
│   ├── services/        # API services
│   │   └── apiService.ts      # Backend API client
│   ├── styles/          # CSS files
│   ├── App.tsx          # Main app component with routing
│   └── main.tsx         # Application entry point
├── package.json
├── tsconfig.json
├── vite.config.ts       # Vite configuration
└── index.html
```

## Getting Started

### Prerequisites

- Node.js 16+ 
- npm or yarn

### Installation

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Create a `.env.local` file with your configuration:
```
VITE_API_BASE_URL=http://localhost:5000/api
```

### Development

Start the development server:
```bash
npm run dev
```

The application will be available at `http://localhost:3000`

### Build

Build for production:
```bash
npm run build
```

Preview the production build:
```bash
npm run preview
```

## Configuration

The frontend is pre-configured to work with the Reading Community API running on `http://localhost:5000`. 

**API Proxy** is enabled in `vite.config.ts` to handle CORS issues during development.

## Features Overview

### 1. Login Page
- Email and password authentication
- Form validation
- Error handling and user feedback
- Redirects to home page on successful login

### 2. Navbar
- Sticky navigation bar
- Profile icon (👤) with dropdown menu
- Messages/Conversations icon (💬) with conversation list
- User information display
- Logout functionality

### 3. Conversations Panel
- View all user conversations
- Display last message preview
- Show unread message count
- Display last message timestamp

### 4. Books Page (Home)
- Grid layout of all available books
- Book card with:
  - Cover image
  - Title
  - Author
  - Description
  - Star rating
- Loading state while fetching data
- Error handling

### 5. Authentication
- JWT token-based authentication
- Persistent login (token stored in localStorage)
- Protected routes (redirect to login if not authenticated)
- Automatic token inclusion in API requests

## Authentication Flow

1. User enters credentials on login page
2. Frontend sends request to backend `/api/auth/login`
3. Backend returns JWT token
4. Token is stored in localStorage and context
5. Token is included in all subsequent API requests
6. User is redirected to home page
7. Protected routes check authentication status

## API Integration

The frontend integrates with the following backend endpoints:

### Authentication
- `POST /api/auth/login` - Login with email and password
- `POST /api/auth/register` - Create new account

### Books
- `GET /api/books` - List all books
- `GET /api/books/:id` - Get book details

### Conversations
- `GET /api/conversations` - List user conversations
- `GET /api/conversations/:id/messages` - Get conversation messages
- `POST /api/conversations/:id/messages` - Send a message

### Users
- `GET /api/users/profile` - Get current user profile
- `PUT /api/users/profile` - Update user profile

## Styling

The application uses vanilla CSS with a modern design system:

- **Color Scheme**: Purple gradient (#667eea to #764ba2)
- **Primary Background**: #2c3e50
- **Light Background**: #f5f7fa
- **Responsive Design**: Mobile-first approach with media queries

## Future Enhancements

- [ ] User profile page
- [ ] Settings page
- [ ] Real-time messaging with WebSockets
- [ ] Book reviews and ratings
- [ ] Search and filter functionality
- [ ] User wishlist
- [ ] Following/follower system
- [ ] Dark mode
- [ ] PWA support

## Troubleshooting

### CORS Issues
If you encounter CORS errors, ensure:
1. Backend is running on `http://localhost:5000`
2. Backend CORS is configured to allow `http://localhost:3000`
3. Vite proxy is correctly configured in `vite.config.ts`

### Token Issues
If login fails:
1. Check backend has `/api/auth/login` endpoint
2. Verify response includes `token` and `user` objects
3. Check browser localStorage for token storage

### API Connection Issues
- Ensure backend server is running
- Check API_BASE_URL in `apiService.ts` matches your backend
- Verify all required endpoints are implemented

## License

This project is part of the Reading Community application.

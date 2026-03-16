import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext';
import Navbar from './components/Navbar';
import Login from './pages/Login';
import SignUp from './pages/SignUp';
import Home from './pages/Home';
import Profile from './pages/Profile';
import CreateBook from './pages/CreateBook';
import Authors from './pages/Authors';
import CreateAuthor from './pages/CreateAuthor';
import AuthorDetail from './pages/AuthorDetail';
import './styles/App.css';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <>{children}</> : <Navigate to="/login" />;
};

function App() {
  const { isAuthenticated } = useAuth();

  return (
    <BrowserRouter>
      {isAuthenticated && <Navbar />}
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<SignUp />} />
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <Home />
            </ProtectedRoute>
          }
        />
        <Route
          path="/profile"
          element={
            <ProtectedRoute>
              <Profile />
            </ProtectedRoute>
          }
        />
        <Route
          path="/create-book"
          element={
            <ProtectedRoute>
              <CreateBook />
            </ProtectedRoute>
          }
        />
        <Route
          path="/authors"
          element={
            <ProtectedRoute>
              <Authors />
            </ProtectedRoute>
          }
        />
        <Route
          path="/create-author"
          element={
            <ProtectedRoute>
              <CreateAuthor />
            </ProtectedRoute>
          }
        />
        <Route
          path="/authors/:id"
          element={
            <ProtectedRoute>
              <AuthorDetail />
            </ProtectedRoute>
          }
        />
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

import React, { useEffect, useState } from 'react';
import apiService from '../services/apiService';
import { UserListDTO } from '../types';
import '../styles/Conversations.css';
import ChatModal from './ChatModal';
import { useAuth } from '../context/AuthContext';

interface Conversation {
  id: string;
  participantName: string;
  lastMessage: string;
  lastMessageTime: string;
  unreadCount: number;
}

const Conversations: React.FC = () => {
  const [conversations, setConversations] = useState<Conversation[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [followedUsers, setFollowedUsers] = useState<UserListDTO[]>([]);
  const [isLoadingUsers, setIsLoadingUsers] = useState(false);
  const { user } = useAuth();
  const [selectedConversationId, setSelectedConversationId] = useState<string | null>(null);
  const [isChatModalOpen, setIsChatModalOpen] = useState(false);

  useEffect(() => {
    fetchConversations();
  }, []);

  const fetchConversations = async () => {
    try {
      setIsLoading(true);
      const response = await apiService.getConversations();
      const data = response.data?.data;

      if (!Array.isArray(data)) {
        setError('Received invalid conversation payload from server');
        setConversations([]);
        return;
      }

      const normalized = data.map((item: any): Conversation => ({
        id: String(item.id ?? ''),
        participantName:
          item.name ??
          item.description ??
          item.participants?.find((p: any) => p.userName)?.userName ??
          `Conversation #${item.id}`,
        lastMessage:
          item.messages?.length > 0
            ? item.messages[item.messages.length - 1].content ?? 'No messages'
            : 'No messages yet',
        lastMessageTime:
          item.lastMessageAt || item.createdAt
            ? new Date(item.lastMessageAt ?? item.createdAt).toLocaleString()
            : '',
        unreadCount: item.unreadCount ?? 0,
      }));

      setConversations(normalized);
    } catch (err: any) {
      setError('Failed to load conversations');
      console.error(err);
      setConversations([]);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchFollowedUsers = async () => {
    try {
      setIsLoadingUsers(true);
      const response = await apiService.getFollowedUsers();
      if (response.data.isSuccess) {
        setFollowedUsers(response.data.data || []);
      }
    } catch (err: any) {
      console.error('Failed to load followed users', err);
    } finally {
      setIsLoadingUsers(false);
    }
  };

  const handleCreateMessage = async (userId: number) => {
    try {
      const response = await apiService.createConversation(userId);
      if (response.data.isSuccess) {
        setShowCreateModal(true);
        // Refresh conversations
        await fetchConversations();
      } else {
        setError(response.data.message || 'Failed to create conversation');
      }
    } catch (err: any) {
      setError('Failed to create conversation');
      console.error(err);
    }
  };

  const openCreateModal = () => {
    setShowCreateModal(true);
    fetchFollowedUsers();
  };

  return (
    <>
    <ChatModal 
    isOpen={isChatModalOpen} 
    onClose={() => setIsChatModalOpen(false)} 
    token={user?.token || ""} 
    conversationId={selectedConversationId || ""} 
    currentUserId={user?.id?.toString() || ""} 
        />
    <div className="conversations-modal">
      <div className="conversations-header">
        <h2>Messages</h2>
        <button className="create-message-button" onClick={openCreateModal}>
          Create Message
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {isLoading ? (
        <div className="loading">Loading conversations...</div>
      ) : conversations.length === 0 ? (
        <div className="no-conversations">No conversations yet.</div>
      ) : (
        <div className="conversations-list">
          {conversations.map((conversation) => (
            <div key={conversation.id} className="conversation-item" onClick={()=> {
              console.log("Selected conversation ID:", conversation.id);
              setSelectedConversationId(conversation.id);
              setIsChatModalOpen(true);
            }}>
              <div className="conversation-info">
                <h4>{conversation.participantName}</h4>
                <p>{conversation.lastMessage}</p>
              </div>
              <div className="conversation-meta">
                <span className="time">{conversation.lastMessageTime}</span>
                {conversation.unreadCount > 0 && (
                  <span className="unread-badge">{conversation.unreadCount}</span>
                )}
              </div>
            </div>
          ))}
        </div>
      )}

      {showCreateModal && (
        <div className="modal-overlay" onClick={() => setShowCreateModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <h3>Select a user to message</h3>
            {isLoadingUsers ? (
              <div className="loading">Loading users...</div>
            ) : followedUsers.length === 0 ? (
              <p>You are not following anyone yet.</p>
            ) : (
              <div className="users-list">
                {followedUsers.map((user) => (
                  <div key={user.id} className="user-item" onClick={() => handleCreateMessage(user.id)}>
                    <img 
                      src={user.profileImageUrl || "https://via.placeholder.com/40x40?text=U"} 
                      alt={`${user.userName}'s profile`} 
                      className="user-avatar" 
                    />
                    <span>{user.userName}</span>
                  </div>
                ))}
              </div>
            )}
            <button className="close-modal-button" onClick={() => setShowCreateModal(false)}>
              Close
            </button>
          </div>
        </div>
      )}
    </div>
    </>
  );
};

export default Conversations;

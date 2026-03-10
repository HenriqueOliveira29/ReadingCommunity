import React, { useEffect, useState } from 'react';
import apiService from '../services/apiService';
import '../styles/Conversations.css';

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

  useEffect(() => {
    fetchConversations();
  }, []);

  const fetchConversations = async () => {
    try {
      setIsLoading(true);
      const response = await apiService.getConversations();
      setConversations(response.data);
    } catch (err: any) {
      setError('Failed to load conversations');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="conversations-modal">
      <div className="conversations-header">
        <h2>Messages</h2>
      </div>

      {error && <div className="error-message">{error}</div>}

      {isLoading ? (
        <div className="loading">Loading conversations...</div>
      ) : conversations.length === 0 ? (
        <div className="no-conversations">No conversations yet.</div>
      ) : (
        <div className="conversations-list">
          {conversations.map((conversation) => (
            <div key={conversation.id} className="conversation-item">
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
    </div>
  );
};

export default Conversations;

import React, { useState, useEffect, useRef } from 'react';
import chatService from '../services/chatService'; // Adjust path
import apiService from '../services/apiService';

// Define types for our internal state
interface Message {
  senderId: string;
  text: string;
  timestamp: Date;
}

interface ChatModalProps {
  isOpen: boolean;
  onClose: () => void;
  token: string;
  conversationId: string;
  currentUserId: string;
}

const ChatModal: React.FC<ChatModalProps> = ({ 
  isOpen, 
  onClose, 
  token, 
  conversationId, 
  currentUserId 
}) => {
  const [messages, setMessages] = useState<Message[]>([]);
  const [inputValue, setInputValue] = useState<string>("");
  const [isConnecting, setIsConnecting] = useState<boolean>(false);
  
  // Ref for auto-scrolling
  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!isOpen || !token) return;

    let handlerId: string;

    const setupChat = async () => {
        
      const setupChat = async () => {
        setIsConnecting(true);
        try {
            // 1. Fetch History (Example API call)
            const response = await apiService.getConversationMessages(conversationId);
            const history = await response.data.json();
            setMessages(history.map((m: any) => ({
            senderId: m.senderId.toString(), // Match your Message interface
            text: m.text,
            timestamp: new Date(m.createdAt)
            })));

            // 2. Connect SignalR (Your existing logic)
            if (!chatService.isConnected) {
            await chatService.startConnection(token);
            }
            await chatService.joinConversation(conversationId);
        // Subscribing to the singleton service
        handlerId = chatService.onMessageReceived((convId, senderId, message) => {
          if (convId === conversationId) {
            const newMessage: Message = {
              senderId,
              text: message,
              timestamp: new Date()
            };
            setMessages((prev) => [...prev, newMessage]);
          }
        });
      } catch (err) {
        console.error("SignalR Connection Error:", err);
      } finally {
        setIsConnecting(false);
      }
    };

    setupChat();

    // Cleanup: Remove this specific component's listener when unmounting/closing
    return () => {
      if (handlerId) {
        chatService.offMessageReceived(handlerId);
      }
    };
  }, [isOpen, token, conversationId]);

  // Scroll to bottom whenever messages array changes
  useEffect(() => {
    scrollRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const handleSend = async () => {
    if (!inputValue.trim() || isConnecting) return;

    try {
      console.log("Sending message to conversation:", conversationId, "Message:", inputValue);
      await chatService.sendMessage(conversationId, inputValue);
      setInputValue("");
    } catch (err) {
      console.error("Failed to send message:", err);
    }
  };

  if (!isOpen) return null;

  return (
    <div style={styles.overlay}>
      <div style={styles.modal}>
        <div style={styles.header}>
          <span>{isConnecting ? "Connecting..." : `Conversation: ${conversationId}`}</span>
          <button onClick={onClose} style={styles.closeBtn}>&times;</button>
        </div>
        
        <div style={styles.chatWindow}>
          {messages.map((msg, i) => (
            <div key={i} style={{
              ...styles.message,
              alignSelf: msg.senderId === currentUserId ? 'flex-end' : 'flex-start',
              backgroundColor: msg.senderId === currentUserId ? '#007bff' : '#f1f1f1',
              color: msg.senderId === currentUserId ? 'white' : 'black'
            }}>
              <div style={{ fontSize: '0.7rem', marginBottom: '2px', opacity: 0.7 }}>
                {msg.senderId === currentUserId ? 'You' : msg.senderId}
              </div>
              {msg.text}
            </div>
          ))}
          <div ref={scrollRef} />
        </div>

        <div style={styles.footer}>
          <input 
            type="text"
            value={inputValue}
            onChange={(e) => setInputValue(e.target.value)}
            onKeyDown={(e) => e.key === 'Enter' && handleSend()}
            placeholder="Type a message..."
            style={styles.input}
          />
          <button 
            onClick={handleSend} 
            disabled={isConnecting}
            style={styles.sendBtn}
          >
            Send
          </button>
        </div>
      </div>
    </div>
  );
};

// Simple Styles Object (You can move these to a .css or .scss file)
const styles: { [key: string]: React.CSSProperties } = {
  overlay: { position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0,0,0,0.5)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 1000 },
  modal: { width: '400px', height: '550px', backgroundColor: 'white', borderRadius: '12px', display: 'flex', flexDirection: 'column', overflow: 'hidden', boxShadow: '0 8px 30px rgba(0,0,0,0.12)' },
  header: { padding: '16px', background: '#007bff', color: 'white', display: 'flex', justifyContent: 'space-between', alignItems: 'center' },
  closeBtn: { background: 'none', border: 'none', color: 'white', fontSize: '20px', cursor: 'pointer' },
  chatWindow: { flex: 1, padding: '16px', overflowY: 'auto', display: 'flex', flexDirection: 'column', gap: '12px' },
  message: { padding: '10px 14px', borderRadius: '12px', maxWidth: '80%', wordWrap: 'break-word' },
  footer: { padding: '16px', borderTop: '1px solid #eee', display: 'flex', gap: '8px' },
  input: { flex: 1, padding: '10px', borderRadius: '6px', border: '1px solid #ddd', outline: 'none' },
  sendBtn: { padding: '10px 20px', backgroundColor: '#007bff', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer' }
};

export default ChatModal;
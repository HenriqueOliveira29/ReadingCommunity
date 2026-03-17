import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

class ChatService {
  private connection: HubConnection | null = null;
  private messageHandlers: { [key: string]: (conversationId: string, senderId: string, message: string) => void } = {};

  async startConnection(token: string): Promise<void> {
    this.connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/chathub', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    this.connection.on('ReceiveMessage', (conversationId: string, senderId: string, message: string) => {
      // Call all registered message handlers
      Object.values(this.messageHandlers).forEach(handler => {
        handler(conversationId, senderId, message);
      });
    });

    await this.connection.start();
    console.log('SignalR connection started');
  }

  async stopConnection(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
      console.log('SignalR connection stopped');
    }
  }

  async joinConversation(conversationId: string): Promise<void> {
    if (this.connection) {
      await this.connection.invoke('JoinConversation', conversationId);
    }
  }

  async sendMessage(conversationId: string, message: string): Promise<void> {
    if (this.connection) {
      await this.connection.invoke('SendMessage', conversationId, message);
    }
  }

  onMessageReceived(handler: (conversationId: string, senderId: string, message: string) => void): string {
    const id = Math.random().toString(36).substr(2, 9);
    this.messageHandlers[id] = handler;
    return id;
  }

  offMessageReceived(id: string): void {
    delete this.messageHandlers[id];
  }

  get isConnected(): boolean {
    return this.connection?.state === 'Connected';
  }
}

export default new ChatService();
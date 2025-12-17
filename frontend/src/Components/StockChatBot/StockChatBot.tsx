import React, { useState } from 'react';
import { FaUser, FaRobot, FaPaperPlane } from 'react-icons/fa'; // Importing icons
import './StockChatBot.css';  // Ensure styling is in place

const StockChatBot: React.FC = () => {
  const [userInput, setUserInput] = useState('');
  const [chatHistory, setChatHistory] = useState<{ sender: string; message: string }[]>([]);
  const [loading, setLoading] = useState(false);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setUserInput(e.target.value);
  };

  const handleSendMessage = async () => {
    if (!userInput.trim()) return;

    // Add user's message to the chat
    setChatHistory(prev => [...prev, { sender: 'User', message: userInput }]);
    
    // Clear the input
    setUserInput('');

    // Call backend API to get the AI response
    setLoading(true);

    try {
      const response = await fetch('https://localhost:5002/api/Chat/query', {  // Ensure correct endpoint
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ query: userInput }),
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      const data = await response.json();
      setChatHistory(prev => [...prev, { sender: 'AI', message: data.response }]);
    } catch (error) {
      console.error('Error fetching AI response:', error);
      setChatHistory(prev => [...prev, { sender: 'AI', message: 'Error getting response from AI.' }]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="chatbot-wrapper">
      <div className="chatbot-container">
        <div className="chat-header">
          <h2>Stock ChatBot</h2>
        </div>
        <div className="chat-history">
          {chatHistory.map((chat, index) => (
            <div key={index} className={`chat-message ${chat.sender === 'User' ? 'user-message' : 'ai-message'}`}>
              {chat.sender === 'User' ? <FaUser className="chat-icon" /> : <FaRobot className="chat-icon" />}
              <div className="chat-bubble">
                <strong>{chat.sender === 'User' ? 'You' : 'AI'}: </strong>
                {chat.message}
              </div>
            </div>
          ))}
        </div>
        <div className="chat-input">
          <input
            type="text"
            value={userInput}
            onChange={handleInputChange}
            placeholder="Ask about stock news..."
            disabled={loading}
          />
          <button onClick={handleSendMessage} disabled={loading || !userInput.trim()}>
            {loading ? 'Sending...' : <FaPaperPlane />}
          </button>
        </div>
      </div>
    </div>
  );
};

export default StockChatBot;

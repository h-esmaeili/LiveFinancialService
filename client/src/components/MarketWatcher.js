import React, { useState, useEffect } from 'react';

const MarketWatcher = () => {
  const [messages, setMessages] = useState([]);
  const [connectionStatus, setConnectionStatus] = useState("Connecting...");

  useEffect(() => {
    // Replace this with your WebSocket URL
    const socket = new WebSocket('ws://localhost:5077/ws');

    // Event listener for when the connection opens
    socket.onopen = () => {
      setConnectionStatus("Connected");
      console.log('WebSocket connection opened');
    };

    // Event listener for incoming messages
    socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      
      const { data: items = [] } = data;
      const market = items[1];
      const price = items[5];
      const lastUpdated = items[2];

      const message = { market, price: price.toFixed(2), lastUpdated };
      if (market === 'btcusdt' || market === 'ethusdt' || market === 'solusdt') {
        console.log('WebSocket message received:', data);

        
      setMessages((prevMessages) => {
        const existingMessageIndex = prevMessages.findIndex(msg => msg.market === market);
        if (existingMessageIndex !== -1) {
          // Update the existing message
          const updatedMessages = [...prevMessages];
          updatedMessages[existingMessageIndex] = message;
          return updatedMessages;
        } else {
          // Add the new message
          return [message, ...prevMessages];
        }
      });
      }

    };

    // Event listener for connection errors
    socket.onerror = (error) => {
      console.error('WebSocket error:', error);
      setConnectionStatus("Error");
    };

    // Event listener for when the connection closes
    socket.onclose = () => {
      setConnectionStatus("Disconnected");
      console.log('WebSocket connection closed');
    };

    // Clean up the WebSocket connection when the component unmounts
    return () => {
      socket.close();
    };
  }, []); // Empty dependency array to only run on mount and unmount

  return (

    <div className="px-4 sm:px-6 lg:px-8">
      <div className="sm:flex sm:items-center">
        <div className="sm:flex-auto">
          <h1 className="text-base font-semibold leading-6 text-gray-900">Markets</h1>
          <p className="mt-2 text-sm text-gray-700">
            A list of all the markets.
          </p>
        </div>
        
      </div>
      <div className="mt-8 flow-root">
        <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
          <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
            <table className="min-w-full divide-y divide-gray-300">
              <thead>
                <tr>
                  <th scope="col" className="py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-gray-900 sm:pl-0">
                    Name
                  </th>
                  <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">
                    Title
                  </th>
                  <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">
                    Price
                  </th>
                  <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">
                    Last Updated
                  </th>
                  <th scope="col" className="relative py-3.5 pl-3 pr-4 sm:pr-0">
                    <span className="sr-only">Detail</span>
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-200">
                {messages.map((message) => (
                  <tr key={message.market}>
                    <td className="whitespace-nowrap py-4 pl-4 pr-3 text-sm font-medium text-gray-900 sm:pl-0">
                      {message.market}
                    </td>
                    <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-500">{message.market}</td>
                    <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-500">{message.price}</td>
                    <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-500">{message.lastUpdated}</td>
                    <td className="relative whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-medium sm:pr-0">
                      <a href="#" className="text-indigo-600 hover:text-indigo-900">
                        Detail<span className="sr-only">, {message.market}</span>
                      </a>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

  );
};

export default MarketWatcher;

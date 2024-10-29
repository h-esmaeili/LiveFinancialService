import React, { useState, useEffect } from 'react';
import { format } from 'date-fns';
import { DataService } from '../services';

const MarketWatcher = () => {
  const [markets, setMarkets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [connectionStatus, setConnectionStatus] = useState("Connecting...");
  const socket = new WebSocket('ws://localhost:50600/ws');

  const getMarkets = () => {
    DataService.get(`api/Instruments?filter=`)
      .then(function (response) {
        const { data } = response;
        setMarkets(data);
        setLoading(false);
      })
      .catch((e) => {
        
      });
  }


  useEffect(() => {
    getMarkets();

    if(!loading) {
      // Event listener for when the connection opens
      socket.onopen = () => {
        setConnectionStatus("Connected");
        console.log('WebSocket connection opened');
      };

      // Event listener for incoming messages
      socket.onmessage = (event) => {
        const data = JSON.parse(event.data);
        const { data: items = [] } = data;
        const name = items[1];
        const price = items[5];
        const lastUpdated = format(new Date(items[2]), 'MMMM dd, yyyy HH:mm:ss');

        const existingMarket = markets.find((market) => market.name === name);
        if (!existingMarket) {
          return;
        }

        const market = { ...existingMarket, name, price: price.toFixed(2), lastUpdated };

        console.log('WebSocket message received:', data);
    
        setMarkets((prevMarkets) => {
          const existingMarketIndex = prevMarkets.findIndex((market) => market.name === name);
          if (existingMarketIndex !== -1) {
            // Update the existing message
            const updatedMarkets = [...prevMarkets];
            updatedMarkets[existingMarketIndex] = market;
            return updatedMarkets;
          } else {
            // Add the new item
            return [market, ...prevMarkets];
          }
        });

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

    }

    // Clean up the WebSocket connection when the component unmounts
    return () => {
      socket.close();
    };
  }, [loading]); // Empty dependency array to only run on mount and unmount

  return (
    <div className="px-4 sm:px-6 lg:px-8">
      <div className="sm:flex sm:items-center">
        <div className="sm:flex-auto">
          <h1 className="text-base font-semibold text-gray-900">Markets</h1>
          <p className="mt-2 text-sm text-gray-700">
            A list of all available markets and their current prices.
          </p>
        </div>
        
      </div>
      <div className="mt-8 flow-root">
        <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
          <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
            <table className="min-w-full divide-y divide-gray-300">
              <thead>
                <tr>
                  <th
                    scope="col"
                    className="whitespace-nowrap py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-gray-900 sm:pl-0"
                  >
                    Name
                  </th>
                  <th
                    scope="col"
                    className="whitespace-nowrap px-2 py-3.5 text-left text-sm font-semibold text-gray-900"
                  >
                    Base Currency
                  </th>
                  <th
                    scope="col"
                    className="whitespace-nowrap px-2 py-3.5 text-left text-sm font-semibold text-gray-900"
                  >
                    Quote Currency
                  </th>
                  <th
                    scope="col"
                    className="whitespace-nowrap px-2 py-3.5 text-left text-sm font-semibold text-gray-900"
                  >
                    Price
                  </th>
                  <th
                    scope="col"
                    className="whitespace-nowrap px-2 py-3.5 text-left text-sm font-semibold text-gray-900"
                  >
                    Description
                  </th>
                  <th
                    scope="col"
                    className="whitespace-nowrap px-2 py-3.5 text-left text-sm font-semibold text-gray-900"
                  >
                    Last Updated
                  </th>            
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-200 bg-white">
                {markets.map((market) => (
                  <tr key={market.name}>
                    <td className="whitespace-nowrap px-2 py-2 text-sm font-medium text-gray-900 sm:pl-0">
                      {market.name.toUpperCase()}
                    </td>
                    <td className="whitespace-nowrap px-2 py-2 text-sm text-gray-900">{market.baseCurrency}</td>
                    <td className="whitespace-nowrap px-2 py-2 text-sm text-gray-900">{market.quoteCurrency}</td>
                    <td className="whitespace-nowrap px-2 py-2 text-sm text-gray-900">{market.price}</td>
                    <td className="whitespace-nowrap px-2 py-2 text-sm text-gray-500">{market.description}</td>
                    <td className="whitespace-nowrap px-2 py-2 text-sm text-gray-500">{market.lastUpdated}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  )
}

export default MarketWatcher;

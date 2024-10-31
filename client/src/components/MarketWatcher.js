import React, { useState, useEffect } from 'react';
import { format } from 'date-fns';
import { DataService } from '../services';

const MarketWatcher = () => {
  const [tickets, setTickets] = useState([]);
  const [availableMarkets, setAvailableMarkets] = useState([]);

// Fetch the list of available markets only once
useEffect(() => {
  const fetchMarkets = async () => {
    try {
      const response = await DataService.get('api/Instruments?filter='); 
      const { data, success } = await response.json();
      debugger;
      setAvailableMarkets(data);
    } catch (error) {
      console.error("Failed to fetch markets:", error);
    }
  };

  fetchMarkets();
}, []); // Empty dependency array ensures this runs only once

// Set up the WebSocket connection once available markets are fetched
useEffect(() => {
  if (availableMarkets.length === 0) return; // Ensure markets are loaded before setting up WebSocket

  const ws = new WebSocket('ws://localhost:50600/ws');

  ws.onopen = () => {
    console.log("Connected to WebSocket server");
  };

  ws.onmessage = (event) => {
    const newTickets = JSON.parse(event.data);
    console.log("Received tickets:", newTickets);

    const { data: items = [] } = newTickets;
    const name = items[1];
    const price = items[5];
    const lastUpdated = format(new Date(items[2]), 'MMMM dd, yyyy HH:mm:ss');
    const existingMarket = availableMarkets.find((market) => market.name === name);
    if (!existingMarket) {
      return;
    }

    const market = { ...existingMarket, name, price: price.toFixed(2), lastUpdated };
    
    setTickets((prevTickets) => {
      const existingTicketIndex = prevTickets.findIndex((ticket) => ticket.name === name);
      if (existingTicketIndex !== -1) {
        // Update the existing message
        const updatedTickets = [...prevTickets];
        updatedTickets[existingTicketIndex] = market;
        return updatedTickets;
      } else {
        // Add the new item
        return [market, ...prevTickets];
      }
    });
  };

  ws.onclose = () => {
    console.log("Disconnected from WebSocket server");
  };

  return () => ws.close();
}, [availableMarkets]); // This useEffect depends on availableMarkets


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
                {tickets.map((market) => (
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
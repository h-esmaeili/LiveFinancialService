# MarketPulse

**A .NET 8 Web API and React project for real-time financial data streaming.**

## Project Overview
MarketPulse is designed to provide REST API and WebSocket endpoints for live financial instrument prices sourced from public data providers, specifically Tiingo or Binance. This service efficiently handles high subscriber demand, streaming real-time updates for financial data.

## Key Elements
- **REST API**:
  - Retrieve a list of financial instruments (e.g., EURUSD, USDJPY, BTCUSD).
  - Get the current price for a specific financial instrument.
- **WebSocket Service**:
  - Subscribe to live price updates for specific financial instruments.
  - Broadcast updates to all subscribed clients.
- **Data Source**:
  - Public APIs like [Tiingo](https://www.tiingo.com/documentation/websockets/forex) or Binance WebSocket for BTCUSD.
- **Performance**:
  - Built to efficiently manage over 1,000 WebSocket subscribers.
- **Logging and Error Reporting**:
  - Event and error logging streamed to `stdout` for easy monitoring.

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js & npm](https://nodejs.org/) (for running the React front-end)
- [Git](https://git-scm.com/)
- API Key for Tiingo (if using Tiingo as the data provider)

## Getting Started
Follow these steps to set up and run the project locally.

### 1. Clone the repository
```bash
git clone https://github.com/h-esmaeili/MarketPulse.git
cd MarketPulse
```
### 2. Backend Setup(API)
```bash
cd server
dotnet restore
dotnet build
```
### Configuration
**Tiingo API Key:** Add your API key for Tiingo in the configuration file.
### Run the Backend
The API will be available at `http://localhost:50600`

### 3. Frontend Setup(React)
```bash
cd client
npm install
```
### Run the Frontend
```bash
npm start
```
The React app will be available at `http://localhost:3000`
### Usage
**API Endpoints**:
  - List Instruments.
      - Endpoint: `/api/instruments`
      - Method: `GET`
      - Description: Retrieves a list of available instruments.
  - Get Instrument Price
      - Endpoint: '/api/instruments/{ticker}/price`
      - Method: `GET`
      - Description: Gets the current price of a specific financial instrument.
**Web Socket**
To subscribe to live price updates, connect to the `WebSocket at ws://localhost:50600/ws`
### Logging & Error Handling
  - All events and errors are logged to the console (stdout) for easy tracking.
  - Error messages provide detailed information about connection and data issues.
**Project Structure**
```bash
MarketPulse/
├── clientapp/              # Frontend (React)
├── server/                 # Backend (ASP.NET Core Web API)
├── README.md
└── LICENSE
```
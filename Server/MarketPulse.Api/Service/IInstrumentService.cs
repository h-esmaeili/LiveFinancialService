﻿using MarketPulse.Api.Models;

namespace MarketPulse.Api.Service
{
    public interface IInstrumentService
    {
        Task<List<InstrumentModel>> List();
        Task<List<InstrumentDataModel>> GetPrice(string instrument);
    }
}
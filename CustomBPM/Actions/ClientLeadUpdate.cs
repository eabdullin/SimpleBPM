// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientLeadUpdate.cs" company="Ingenius Systems">
//   Copyright (c) Ingenius Systems
//   Create on 16:33:03 by Еламан Абдуллин
// </copyright>
// <summary>
//   Defines the ClientLeadUpdate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("ClientLeadUpdate")]
    public class ClientLeadUpdate : IAction
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly IClientsRepository _clientsRepository;

        public ClientLeadUpdate(IDealsRepository dealsRepository, IClientsRepository clientsRepository)
        {
            _dealsRepository = dealsRepository;
            _clientsRepository = clientsRepository;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            Deal deal = _dealsRepository.Find(dealId);
            if (deal == null)
                throw new Exception("Неподдерживаемый тип сделки");
            var client = deal.Dossier.Client;
            if (client.IsLead)
            {
                client.IsLead = false;
            }
            _clientsRepository.Update(client);
        }
    }
}
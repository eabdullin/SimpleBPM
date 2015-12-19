using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("ClearCheckedFlags")]
    class ClearCheckedFlags : IAction
    {
        private readonly IDealsRepository _dealsRepository;

        public ClearCheckedFlags(IDealsRepository dealsRepository)
        {
            _dealsRepository = dealsRepository;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            var dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            var deal = _dealsRepository.Find(dealId);
            if (deal == null)
                throw new Exception("Сделка не найдена");

            deal.IsChecklistCompleted = false;
            deal.IsClientDataChecked = false;
            _dealsRepository.Update(deal);
        }
    }
}

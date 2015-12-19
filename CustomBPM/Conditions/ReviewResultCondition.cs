using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ReviewResultCondition")]
    class ReviewResultCondition : ICondition
    {
        private readonly IDealsRepository _dealsRepository;

        public ReviewResultCondition(IDealsRepository dealsRepository)
        {
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            var dealString = parameters[ProcessConstants.DealId];
            if (dealString == null)
                throw new ArgumentNullException(ProcessConstants.DealId);
            long dealId = long.Parse(dealString);
            BuyDeal deal = _dealsRepository.Find(dealId) as BuyDeal;
            if (deal == null)
                throw new Exception("Неподдерживаемый тип сделки");
            if (deal.Review != null && deal.Review.Result == ReviewResult.Approved)
            {
                reasons = "Лист оценки уже согласован";
                return false;
            }

            reasons = null;
            return true;
        }
    }
}

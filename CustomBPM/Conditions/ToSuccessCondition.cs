using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ToSuccessCondition")]
    class ToSuccessCondition : ICondition
    {
        private IDealsRepository _dealsRepository;

        public ToSuccessCondition(IDealsRepository dealsRepository)
        {
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            var dealString = parameters[ProcessConstants.DealId];
            if (dealString == null)
                throw new ArgumentNullException(ProcessConstants.DealId);
            long dealId = long.Parse(dealString);
            var deal = _dealsRepository.Find(dealId);
            var dealResult = deal.Result as SuccessDealResult;
            if (dealResult == null || !dealResult.GiveCar || !dealResult.PrintCheckList || !dealResult.SignDocs)
            {
                reasons = "Заполните чеклист";
                return false;
            }
            reasons = null;
            return true;
        }
    }

}

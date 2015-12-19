using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ToInJobFromApprovalOrToIncomeCondition")]
    class ToInJobFromApprovalOrToIncomeCondition : ICondition
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly ILogService _logService;
        public ToInJobFromApprovalOrToIncomeCondition(IDealsRepository dealsRepository, ILogService logService)
        {
            _dealsRepository = dealsRepository;
            _logService = logService;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            var dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            var deal = _dealsRepository.Find(dealId) as BuyDeal;
            if (deal == null) throw new NotSupportedException("Неподдерживаемый тип сделки");

            if (deal.Review.NeedNewAgreement)
            {
                reasons = "Необходимо создать новую договоренность";
                return false;
            }
            reasons = null;
            //_logService.ReviewAgreementLog(dealId).Wait();
            return true;
        }
    }
}

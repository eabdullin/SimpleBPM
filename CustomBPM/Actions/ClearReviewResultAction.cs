using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("ClearReviewResultAction")]
    class ClearReviewResultAction : IAction
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly ILogService _logService;

        public ClearReviewResultAction(IDealsRepository dealsRepository, ILogService logService)
        {
            _dealsRepository = dealsRepository;
            _logService = logService;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            BuyDeal deal = _dealsRepository.Find(dealId) as BuyDeal;
            if (deal == null)
                throw new Exception("Неподдерживаемый тип сделки");
            if (deal.Review == null)
                return;
            deal.Review.Result = ReviewResult.Undefined;
            _dealsRepository.Update(deal);
            _logService.ReworkReviewAgreementLog(deal.Id, deal.Valuation.CarId);
        }
    }
}

using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("CreateReviewAgreementAction")]
    class CreateReviewAgreementAction : IAction
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly ILogService _logService;
        private readonly IReviewAgreementService _reviewAgreementService;
        private readonly IReviewAgreementsRepository _reviewAgreementsRepository;

        public CreateReviewAgreementAction(IDealsRepository dealsRepository, ILogService logService, IReviewAgreementService reviewAgreementService, IReviewAgreementsRepository reviewAgreementsRepository)
        {
            _dealsRepository = dealsRepository;
            _logService = logService;
            _reviewAgreementService = reviewAgreementService;
            _reviewAgreementsRepository = reviewAgreementsRepository;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            long userId = parameters.GetParameter<long>(ProcessConstants.UserId);
            BuyDeal deal = _dealsRepository.Find(dealId) as BuyDeal;
            if (deal == null)
                throw new NotSupportedException("Неподдерживаемый тип сделки");
            var review = deal.Review;
            if (review == null)
            {
                review = _reviewAgreementService.CreateReviewAgreement(userId, userId, deal.Valuation.Id);
            }
            else
            {
                review.Result = ReviewResult.Undefined;
                _reviewAgreementsRepository.Update(review);
            }
            _logService.StartReviewAgreementLog(deal.DossierId, deal.Valuation.CarId);
        }
    }
}

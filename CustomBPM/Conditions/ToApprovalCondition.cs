using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ToApprovalCondition")]
    class ToApprovalCondition: ICondition
    {
        private readonly IDealsRepository _dealsRepository;
        public ToApprovalCondition(IDealsRepository dealsRepository)
        {
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            BuyDeal deal = _dealsRepository.Find(dealId) as BuyDeal;
            if (deal == null)
                throw new NotSupportedException("Неподдерживаемый тип сделки");
            if (deal.Valuation == null)
            {
                reasons = "Необходимо заполнить лист оценки";
                return false;
            }
            reasons = null;
            return true;
        }
    }
    [Code("CheckValuationFillingCondition")]
    class CheckValuationFillingCondition : ICondition
    {
        private readonly IDealsRepository _dealsRepository;
        public CheckValuationFillingCondition(IDealsRepository dealsRepository)
        {
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            BuyDeal deal = _dealsRepository.Find(dealId) as BuyDeal;
            if (deal == null)
                throw new NotSupportedException("Неподдерживаемый тип сделки");
            if (deal.Valuation != null && !deal.Valuation.ValuationIsFill  )
            {
                reasons = "Необходимо дозаполнить лист оценки";
                return false;
            }
            reasons = null;
            return true;
        }
    }
    [Code("FromApprovalCondition")]
    class FromApprovalCondition : ICondition
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly IReviewAgreementsRepository _reviewAgreementsRepository;
        public FromApprovalCondition(IDealsRepository dealsRepository, IReviewAgreementsRepository reviewAgreementsRepository)
        {
            _dealsRepository = dealsRepository;
            _reviewAgreementsRepository = reviewAgreementsRepository;

        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            BuyDeal deal = _dealsRepository.Find(dealId) as BuyDeal;
            if (deal == null)
                throw new NotSupportedException("Неподдерживаемый тип сделки");
            var review = deal.Review;
            if (review != null && review.Result == ReviewResult.Undefined)
            {
                reasons = "Согласование не завершено";
                return false;
            }
            reasons = null;
            return true;
        }
    }
}

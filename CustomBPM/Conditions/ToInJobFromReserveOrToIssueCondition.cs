using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ToInJobFromReserveOrToIssueCondition")]
    class ToInJobFromReserveOrToIssueCondition : ICondition
    {
        private readonly IDealsRepository _dealsRepository;

        public ToInJobFromReserveOrToIssueCondition(IDealsRepository dealsRepository)
        {
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            var dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            var deal = _dealsRepository.Find(dealId) as SaleDeal;
            if (deal == null)
                throw new NotSupportedException("Неподдерживаемый тип сделки");

            if (deal.Reserve != null)
            {
                reasons = "Необходимо создать новую договоренность и удалить резерв";
                return false;
            }
            reasons = null;
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("ToIssueChangeCarStatusAction")]
    class ToIssueChangeCarStatusAction : IAction
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly ICarsRepository _carsRepository;

        public ToIssueChangeCarStatusAction(IDealsRepository dealsRepository, ICarsRepository carsRepository)
        {
            _dealsRepository = dealsRepository;
            _carsRepository = carsRepository;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            SaleDeal deal = _dealsRepository.Find(dealId) as SaleDeal;
            if (deal == null)
                throw new Exception("Неподдерживаемый тип сделки");
            if (deal.Reserve == null)
                return;

            deal.Reserve.Car.Status = CarStatus.ToIssue;
            _carsRepository.Update(deal.Reserve.Car);
            _dealsRepository.Update(deal);
        }
    }
}

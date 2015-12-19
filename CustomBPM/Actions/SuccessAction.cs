using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("SuccessAction")]
    class SuccessAction :IAction
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly ICarsRepository _carsRepository;
        public SuccessAction(IDealsRepository dealsRepository, ICarsRepository carsRepository)
        {
            _dealsRepository = dealsRepository;
            _carsRepository = carsRepository;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            var dealString = parameters[ProcessConstants.DealId];
            if (dealString == null)
                throw new ArgumentNullException(ProcessConstants.DealId);
            long dealId = long.Parse(dealString);
            SaleDeal deal = _dealsRepository.Find(dealId) as SaleDeal;
            if (deal == null)
                throw new Exception("Не найден резерв");
            Car car = deal.Reserve.Car;
            car.Status = CarStatus.Saled;
            _carsRepository.Update(car);
        }
    }
}

using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("ClearCarSelectionAction")]
    class ClearCarSelectionAction : IAction
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly ILogService _logService;

        public ClearCarSelectionAction(IDealsRepository dealsRepository, ILogService logService)
        {
            _dealsRepository = dealsRepository;
            _logService = logService;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            SaleDeal deal = _dealsRepository.Find(dealId) as SaleDeal;
            if (deal == null)
                throw new Exception("Неподдерживаемый тип сделки");

            var cars = deal.Dossier.CarsSelection.ToList();
            foreach (var carInSelection in cars)
            {
                deal.Dossier.CarsSelection.Remove(carInSelection);
                _logService.RemoveCarFromSelectionLog(deal.DossierId, carInSelection.Id);
            }
            _dealsRepository.Update(deal);
        }
    }
}

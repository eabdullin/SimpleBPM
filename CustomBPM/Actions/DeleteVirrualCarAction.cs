using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("DeleteVirtualCarAction")]
    public class DeleteVirrualCarAction : IAction
    {
        private readonly IDossiersRepository _dossiersRepository;
        private readonly IDealsRepository _dealsRepository;
        private readonly ICarsService _carsService;

        public DeleteVirrualCarAction(IDossiersRepository dossiersRepository, IDealsRepository dealsRepository, ICarsService carsService)
        {
            _dossiersRepository = dossiersRepository;
            _dealsRepository = dealsRepository;
            _carsService = carsService;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            var deal = _dealsRepository.Find(dealId) as SaleDeal;
            if(deal == null) return;
            var virtualCars = deal.Dossier.VirtualCars.ToList();
            foreach (var virtualCar in virtualCars)
            {
                _carsService.DeleteVirtualCar(virtualCar.Id);
            }
        }
    }
}
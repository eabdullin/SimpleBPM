using System;
using System.Collections.Generic;
using System.Globalization;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("DeleteReserveAction")]
    class DeleteReserveAction : IAction
    {
        private ILog _logger = LogManager.GetLogger("BPM");
        private readonly IDealsRepository _dealsRepository;
         private readonly ILogItemsRepository _logItemsRepository;
         private readonly IReservesService _reservesService;
        public DeleteReserveAction(IDealsRepository dealsRepository,  IReservesService reservesService, ILogItemsRepository logItemsRepository)
        {
            _dealsRepository = dealsRepository;
            _reservesService = reservesService;
            _logItemsRepository = logItemsRepository;
        }

         public void Execute(IDictionary<string, string> parameters)
        {
            var dealString = parameters[ProcessConstants.DealId];
            if (dealString == null)
                throw new ArgumentNullException(ProcessConstants.DealId);
            long dealId = long.Parse(dealString);
            SaleDeal deal = _dealsRepository.Find(dealId) as  SaleDeal;
            if(deal == null)
                throw new Exception("Не найден резерв");
            CarReserve reserve = deal.Reserve;
             if (reserve != null)
             {
                 long userId = parameters.ContainsKey(ProcessConstants.UserId) ? long.Parse(parameters[ProcessConstants.UserId]) : deal.Dossier.ManagerId;
                 try
                 {
                     
                     var car = reserve.Car;
                     string text = string.Format("Автомобиль {0} {1} года выпуска по цене {2} перешел в статус \"Cнято с резерва\"", car.Name, car.Date.Year, car.Price.ToString("C0", new CultureInfo("ru-RU")));
                     LogItem logItem = DossierLogItem.New(deal.DossierId, text);
                     _logItemsRepository.Create(logItem);
                 }
                 catch (Exception e)
                 {
                     _logger.Error(e);
                 }
                 _reservesService.DeleteReserve(reserve.Id, userId);

             }
        }
    }
}

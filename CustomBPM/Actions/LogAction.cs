using System;
using System.Collections.Generic;
using System.Globalization;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    abstract class LogAction : IAction
    {
        private ILog _logger = LogManager.GetLogger("BPM");
        private readonly IDealsRepository _dealsRepository;
        private readonly ILogItemsRepository _logItemsRepository;

        protected LogAction(IDealsRepository dealsRepository, ILogItemsRepository logItemsRepository)
        {
            _dealsRepository = dealsRepository;
            _logItemsRepository = logItemsRepository;
        }

        public void Execute(IDictionary<string, string> parameters)
        {
            try
            {
                var dealString = parameters[ProcessConstants.DealId];
                if (dealString == null)
                    throw new ArgumentNullException(ProcessConstants.DealId);
                long dealId = long.Parse(dealString);
                SaleDeal deal = _dealsRepository.Find(dealId) as SaleDeal;
                if(deal == null)
                    throw new Exception("Не н");
                LogItem logItem = DossierLogItem.New(deal.DossierId, LogText(deal));
                _logItemsRepository.Create(logItem);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

        }

        protected abstract string LogText(SaleDeal d);
    }
    [Code("RequestLogAction")]
    class RequestLogAction :LogAction
    {
        public RequestLogAction(IDealsRepository dealsRepository, ILogItemsRepository logItemsRepository) : base(dealsRepository, logItemsRepository)
        {
        }

        protected override string LogText(SaleDeal d)
        {
            var description = d.Dossier.Requests.LastOrDefault().Return(r => r.Description);
                var text = string.Empty;
                text = string.IsNullOrEmpty(description)
                    ? string.Format("Обращение [{0}] отправлено на шаг \"Обращение\"", d.Dossier.BusinessType.GetDescription())
                    : string.Format("Обращение [{0}, {1}] отправлено на шаг \"Обращение\"", d.Dossier.BusinessType.GetDescription(), description);
            return text;
        }
    }
        [Code("InJobLogAction")]
    class InJobLogAction : LogAction
    {
        public InJobLogAction(IDealsRepository dealsRepository, ILogItemsRepository logItemsRepository)
            : base(dealsRepository, logItemsRepository)
        {
        }

        protected override string LogText(SaleDeal d)
        {
            var description = d.Dossier.Requests.LastOrDefault().Return(r => r.Description);
            var text = string.Empty;
            text = string.IsNullOrEmpty(description)
                ? string.Format("Обращение [{0}] отправлено на шаг \"В работе\"", d.Dossier.BusinessType.GetDescription())
                : string.Format("Обращение [{0}, {1}] отправлено на шаг \"В работе\"", d.Dossier.BusinessType.GetDescription(), description);
            return text;
        }
    }

    [Code("ReserveLogAction")]
    class ReserveLogAction : LogAction
    {
        public ReserveLogAction(IDealsRepository dealsRepository, ILogItemsRepository logItemsRepository)
            : base(dealsRepository, logItemsRepository)
        {
        }

        protected override string LogText(SaleDeal d)
        {
            var car = d.Reserve.Car;
            return string.Format("Автомобиль {0} {1} года выпуска по цене {2} перешел в статус \"Зарезервировано\"", car.Name, car.Date.Year, car.Price.ToString("C0", new CultureInfo("ru-RU")));

        }
    }

    [Code("ToIssueLogAction")]
    class ToIssueLogAction : LogAction
    {
        public ToIssueLogAction(IDealsRepository dealsRepository, ILogItemsRepository logItemsRepository)
            : base(dealsRepository, logItemsRepository)
        {
        }

        protected override string LogText(SaleDeal d)
        {
            var car = d.Reserve.Car;
            return string.Format("Автомобиль {0} {1} года выпуска по цене {2}  перешел в статус \"На выдаче\"", car.Name, car.Date.Year, car.Price.ToString("C0", new CultureInfo("ru-RU")));

        }
    }

    [Code("SuccessLogAction")]
    class SuccessLogAction : LogAction
    {
        public SuccessLogAction(IDealsRepository dealsRepository, ILogItemsRepository logItemsRepository)
            : base(dealsRepository, logItemsRepository)
        {
        }

        protected override string LogText(SaleDeal d)
        {
            var car = d.Reserve.Car;
            return string.Format("Автомобиль {0} {1} года выпуска по цене {2} перешел в статус \"Продано\"", car.Name, car.Date.Year, car.Price.ToString("C0", new CultureInfo("ru-RU")));

        }
    } 
}

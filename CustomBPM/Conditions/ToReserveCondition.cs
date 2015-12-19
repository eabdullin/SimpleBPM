using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ToReserveWithCarCheckCondition")]
    class ToReserveWithCarCheckCondition : ICondition
    {
        private readonly IDossiersRepository _dossiersRepository;
        private readonly IDealsRepository _dealsRepository;

        public ToReserveWithCarCheckCondition(IDossiersRepository dossiersRepository, IDealsRepository dealsRepository)
        {
            _dossiersRepository = dossiersRepository;
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            //var dossierString = parameters["DossierId"];
            //if (dossierString == null)
            //    throw new ArgumentNullException("DossierId");
            //long dossierId = long.Parse(dossierString);
            //var dossier = _dossiersRepository.Find(dossierId);

            var dealString = parameters[ProcessConstants.DealId];
            if (dealString == null)
                throw new ArgumentNullException(ProcessConstants.DealId);
            long dealId = long.Parse(dealString);
            SaleDeal deal = _dealsRepository.Find(dealId) as SaleDeal;
            CarReserve reserve = deal.Return(r => r.Reserve);
            if (reserve == null)
            {
                reasons = "Не создан резерв";
                return false;
            }
            reasons = null;
            return true;
        }
    }
    [Code("ToReserveWithOneCarCheckCondition")]
    class ToReserveWithOneCarCheckCondition : ICondition
    {
        private readonly IDossiersRepository _dossiersRepository;
        private readonly IDealsRepository _dealsRepository;

        public ToReserveWithOneCarCheckCondition(IDossiersRepository dossiersRepository, IDealsRepository dealsRepository)
        {
            _dossiersRepository = dossiersRepository;
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            //var dossierString = parameters["DossierId"];
            //if (dossierString == null)
            //    throw new ArgumentNullException("DossierId");
            //long dossierId = long.Parse(dossierString);
            //var dossier = _dossiersRepository.Find(dossierId);

            var dealString = parameters[ProcessConstants.DealId];
            if (dealString == null)
                throw new ArgumentNullException(ProcessConstants.DealId);
            long dealId = long.Parse(dealString);
            SaleDeal deal = _dealsRepository.Find(dealId) as SaleDeal;
            if(deal == null)
                throw new Exception("Не найдена сделка");
            var reserve = deal.Return(r => r.Reserve);
            if (reserve == null && deal.Dossier.CarsSelection.Count(x =>x.Status != CarStatus.Reserved) == 1)
            {
                reasons = "Не создан резерв";
                return false;
            }
            reasons = null;
            return true;
        }
    }

    [Code("ToReserveWithClientCheckCondition")]
    class ToReserveWithClientCheckCondition : ICondition
    {
        private readonly IDossiersRepository _dossiersRepository;

        public ToReserveWithClientCheckCondition(IDossiersRepository dossiersRepository)
        {
            _dossiersRepository = dossiersRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            var dossierString = parameters["DossierId"];
            if (dossierString == null)
                throw new ArgumentNullException("DossierId");
            long dossierId = long.Parse(dossierString);
            var dossier = _dossiersRepository.Find(dossierId);
            if (!dossier.Client.BirthDay.HasValue)
            {
                reasons = "Не указана дата рожения";
                return false;
            }
            if (!dossier.Client.IsPassportFilled)
            {
                reasons = "Не заполнены паспортные данные";
                return false;
            }

            if (!dossier.Client.IsAddressFilled)
            {
                reasons = "Не заполнен адрес";
                return false;
            }

            reasons = null;
            return true;
        }
    }
}
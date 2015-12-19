using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    abstract class ToArchiveCondition : ICondition
    {
        private readonly IDossiersRepository _dossiersRepository;
        private IDealsRepository _dealsRepository;

        internal ToArchiveCondition(IDossiersRepository dossiersRepository, IDealsRepository dealsRepository)
        {
            _dossiersRepository = dossiersRepository;
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            //var dossierString = parameters[ProcessConstants.DossierId];
            //if (dossierString == null)
            //    throw new ArgumentNullException(ProcessConstants.DossierId);
            //long dossierId = long.Parse(dossierString);
            //var dossier = _dossiersRepository.Find(dossierId);

            var dealString = parameters[ProcessConstants.DealId];
            if (dealString == null)
                throw new ArgumentNullException(ProcessConstants.DealId);
            long dealId = long.Parse(dealString);
            Deal deal = _dealsRepository.Find(dealId);
            string reason = deal.Return(x => x.Result as FailDealResult).Return(x => x.ReasonText);
            if (reason == null)
            {
                reasons = "Не указана причина переноса в архив";
                return false;
            }
            reasons = null;
            return true;

        }
    }
    [Code("ToArchiveFromRequestCondition")]
    class ToArchiveFromRequestCondition : ToArchiveCondition
    {
        public ToArchiveFromRequestCondition(IDossiersRepository dossiersRepository, IDealsRepository dealsRepository)
            : base(dossiersRepository, dealsRepository)
        {
        }
    }

    [Code("ToArchiveFromInjobCondition")]
    class ToArchiveFromInjobCondition : ToArchiveCondition
    {
        public ToArchiveFromInjobCondition(IDossiersRepository dossiersRepository, IDealsRepository dealsRepository)
            : base(dossiersRepository, dealsRepository)
        {
        }
    }

    [Code("ToArchiveFromReserveCondition")]
    class ToArchiveFromReserveCondition : ToArchiveCondition
    {
        public ToArchiveFromReserveCondition(IDossiersRepository dossiersRepository, IDealsRepository dealsRepository)
            : base(dossiersRepository, dealsRepository)
        {
        }
    }
}
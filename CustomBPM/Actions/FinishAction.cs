using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Actions
{
    [Code("finishAction")]
    public class FinishAction : IAction
    {
        private readonly IDossiersRepository _dossiersRepository;
        private readonly IDealsRepository _dealsRepository;

        public FinishAction(IDossiersRepository dossiersRepository, IDealsRepository dealsRepository)
        {
            _dossiersRepository = dossiersRepository;
            _dealsRepository = dealsRepository;
        }

        public void Execute(IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            //string dossierString = parameters[ProcessConstants.DealId];
            //if (dossierString == null)
            //    throw new ArgumentNullException("DossierId");
            //long dossierId = long.Parse(dossierString);
            //Dossier dossier = _dossiersRepository.Find(dossierId);
            var dealString = parameters[ProcessConstants.DealId];
            if (dealString == null)
                throw new ArgumentNullException(ProcessConstants.DealId);
            long dealId = long.Parse(dealString);
            var deal = _dealsRepository.Find(dealId);
            if (deal.Dossier.Deals.All(x => x.Result != null))
            {
                deal.Dossier.IsClosed = true;
                deal.Dossier.Client.CurrentDossierId = null;
            }

            _dossiersRepository.Update(deal.Dossier);
        }
    }
}

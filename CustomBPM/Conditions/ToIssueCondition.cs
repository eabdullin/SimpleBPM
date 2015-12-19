using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ToIssueWithClientCheckCondition")]
    class ToIssueWithClientCheckCondition : ICondition
    {
        private readonly IDossiersRepository _dossiersRepository;

        public ToIssueWithClientCheckCondition(IDossiersRepository dossiersRepository)
        {
            _dossiersRepository = dossiersRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            var dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            var dossierId = parameters.GetParameter<long>(ProcessConstants.DossierId);
            var dossier = _dossiersRepository.Find(dossierId);

            var deal = dossier.Deals.FirstOrDefault(_ => _.Id == dealId) as SaleDeal;
            if (deal == null)
                throw new Exception("Сделка не найдена или имеет неподдерживаемый тип");

            if (!dossier.Client.IsMainInfoFilled)
            {
                reasons = "Не указаны основные данные клиента";
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

            if (!deal.IsChecklistCompleted)
            {
                reasons = "Не заполнен чеклист";
                return false;
            }

            reasons = null;
            return true;
        }
    }
}
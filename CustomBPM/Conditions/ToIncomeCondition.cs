using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ToIncomeCondition")]
    class ToIncomeCondition : ICondition
    {
        private readonly IDealsRepository _dealsRepository;
        public ToIncomeCondition(IDealsRepository dealsRepository)
        {
            _dealsRepository = dealsRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            long dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
            BuyDeal deal = _dealsRepository.Find(dealId) as BuyDeal;
            if (deal == null)
                throw new NotSupportedException("Неподдерживаемый тип сделки");

            if (deal.Review.Result != ReviewResult.Approved)
            {
                reasons = "Необходимо согласовать оценку";
                return false;
            }

            reasons = null;
            return true;
        }
    }

    [Code("ToIncomeWithClientCheckCondition")]
    class ToIncomeWithClientCheckCondition : ICondition
    {
        private readonly IDossiersRepository _dossiersRepository;

        public ToIncomeWithClientCheckCondition(IDossiersRepository dossiersRepository)
        {
            _dossiersRepository = dossiersRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            long dossierId = parameters.GetParameter<long>(ProcessConstants.DossierId);
            var dossier = _dossiersRepository.Find(dossierId);
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

            reasons = null;
            return true;
        }
    }
}

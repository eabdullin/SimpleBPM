using System;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("ToInJobCondition")]
    class ToInJobCondition : ICondition
    {
        private IDossiersRepository _dossiersRepository;

        public ToInJobCondition(IDossiersRepository dossiersRepository)
        {
            _dossiersRepository = dossiersRepository;
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            string dossierString = null;
            parameters.TryGetValue("DossierId", out dossierString);
            if (dossierString == null)
                throw new ArgumentNullException("DossierId");
            long dossierId = long.Parse(dossierString);
            var dossier = _dossiersRepository.Find(dossierId);
            var isFilled = true;

            if (string.IsNullOrEmpty(dossier.Client.Firstname) && string.IsNullOrEmpty(dossier.Client.Name))
            {
                reasons = "Не заполнено имя клиента";
                return false;
            }

            if (!dossier.Client.PhoneNumbers.Any())
            {
                reasons = "Не заполнен номер телефона";
                return false;
            }
            if (!dossier.Agreements.Any())
            {
                reasons = "Нет ни одной договоренности";
                return false;
            }
            reasons = null;
            return true;
        }
    }
}

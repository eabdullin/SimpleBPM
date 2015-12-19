namespace CustomBPM.Actions
{


    //[Code("DeleteReviewAgreementsAction")]
    //class DeleteReviewAgreementsAction : IAction
    //{
    //    private readonly IDealsRepository _dealsRepository;

    //    public DeleteReviewAgreementsAction(IDealsRepository dealsRepository)
    //    {
    //        _dealsRepository = dealsRepository;
    //    }

    //    public void Execute(IDictionary<string, string> parameters)
    //    {
    //        var dealId = parameters.GetParameter<long>(ProcessConstants.DealId);
    //        var deal = _dealsRepository.Find(dealId) as BuyDeal;
    //        if (deal == null)
    //            throw new Exception("Сделка не найдена");
    //        deal.Review.
    //        deal.IsChecklistCompleted = false;
    //        deal.IsClientDataChecked = false;
    //        _dealsRepository.Update(deal);
    //    }
    //}
}

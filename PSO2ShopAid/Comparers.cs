using System.Collections.Generic;

namespace PSO2ShopAid
{
    public class EncounterComparer : IComparer<Encounter>
    {
        public int Compare(Encounter x, Encounter y)
        {
            return y.date.CompareTo(x.date);
        }
    }

    public class InvestmentByPurchaseComparer : IComparer<Investment>
    {
        public int Compare(Investment x, Investment y)
        {
            return y.PurchaseDate.CompareTo(x.PurchaseDate);
        }
    }
}

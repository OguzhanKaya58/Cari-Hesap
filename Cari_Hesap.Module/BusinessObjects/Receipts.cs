using Cari_Hesap.Module.Enum;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Code")]
    public class Receipts : XPObject
    {
        public Receipts(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        ReceiptMovementType type;
        StockMovementType movementType;
        string definition;
        double overallTotal;
        double taxTotal;
        double discountTotal;
        double subTotal;
        Account accountID;
        DateTime date;
        string code;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }
        [XafDisplayName("Cari Hesap")]
        [Association("Account-PurchaseSaleTransaction")]
        [ImmediatePostData]
        public Account AccountID
        {
            get => accountID;
            set => SetPropertyValue(nameof(AccountID), ref accountID, value);
        }

        public double SubTotal
        {
            get
            {
                if (!IsLoading && !IsSaving)
                {
                    CalculateSubTotal(false);

                }
                return subTotal;
            }
            set => SetPropertyValue(nameof(SubTotal), ref subTotal, value);
        }

        public double DiscountTotal
        {
            get
            {
                if (!IsLoading && !IsSaving)
                {
                    CalculateDiscountTotal(false);

                }
                return discountTotal;
            }
            set => SetPropertyValue(nameof(DiscountTotal), ref discountTotal, value);
        }

        public double TaxTotal
        {
            get
            {
                if (!IsLoading && !IsSaving)
                {
                    CalculateTaxTotal(false);

                }
                return taxTotal;
            }
            set => SetPropertyValue(nameof(TaxTotal), ref taxTotal, value);
        }

        public double OverallTotal
        {
            get
            {
                if (!IsLoading && !IsSaving)
                {
                    CalculateOverallTotal(false);

                }
                return overallTotal;
            }
            set => SetPropertyValue(nameof(OverallTotal), ref overallTotal, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Definition
        {
            get => definition;
            set => SetPropertyValue(nameof(Definition), ref definition, value);
        }

        [Association("Receipts-Detail"), DevExpress.ExpressApp.DC.Aggregated]
        [ImmediatePostData]
        public XPCollection<StockMovements> Detail
        {
            get
            {
                return GetCollection<StockMovements>(nameof(Detail));
            }
        }

        [VisibleInDetailView(false), VisibleInListView(false)]
        public StockMovementType MovementType
        {
            get => movementType;
            set => SetPropertyValue(nameof(MovementType), ref movementType, value);
        }

        [VisibleInDetailView(false)]
        public ReceiptMovementType Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }
        public void CalculateSubTotal(bool disposing)
        {
            double? oldSubTotal = subTotal;
            double temp = 0;
            foreach (StockMovements item in Detail)
            {
                temp += item.Total;
            }
            subTotal = temp;
            if (disposing)
            {
                OnChanged(nameof(SubTotal), oldSubTotal, subTotal);
            }
        }
        public void CalculateDiscountTotal(bool disposing)
        {
            double? oldDiscourtTotal = discountTotal;
            double temp = 0;
            foreach (StockMovements item in Detail)
            {
                temp = item.DiscountAmount;
            }
            discountTotal = temp;
            if (disposing)
            {
                OnChanged(nameof(DiscountTotal), oldDiscourtTotal, discountTotal);
            }
        }
        public void CalculateTaxTotal(bool disposing)
        {
            double? oldTaxTotal = taxTotal;
            double temp = 0;
            foreach (StockMovements item in Detail)
            {
                temp = item.TaxAmount;
            }
            taxTotal = temp;
            if (disposing)
            {
                OnChanged(nameof(TaxTotal), oldTaxTotal, taxTotal);
            }
        }
        public void CalculateOverallTotal(bool disposing)
        {
            double? oldOveralTotal = overallTotal;
            double temp = 0;
            foreach (StockMovements item in Detail)
            {
                temp = item.OverallTotal;
            }
            overallTotal = temp;
            if (disposing)
            {
                OnChanged(nameof(OverallTotal), oldOveralTotal, overallTotal);
            }
        }

    }
}
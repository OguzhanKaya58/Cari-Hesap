using Cari_Hesap.Module.Enum;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class StockMovements : XPObject
    {
        public StockMovements(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        string definition;
        Receipts receiptID;
        double overallTotal;
        double taxAmount;
        double taxRate;
        double netTotal;
        double discountAmount;
        double discountRate;
        double total;
        double unitPrice;
        Unit unitID;
        double amount;
        Stock stockID;
        StockMovementType stockType;
        DateTime date;

        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        public StockMovementType StockType
        {
            get => stockType;
            set => SetPropertyValue(nameof(StockType), ref stockType, value);
        }

        [XafDisplayName("Ürün")]
        [Association("Stock-StockMovements")]
        [ImmediatePostData]
        public Stock StockID
        {
            get => stockID;
            set
            {
                if (SetPropertyValue(nameof(StockID), ref stockID, value))
                {
                    UnitID = StockID.UnitID;
                    if (stockType == StockMovementType.Input)
                    {
                        UnitPrice = StockID.PurchasePrice;
                    }
                    else if (stockType == StockMovementType.Output)
                    {
                        UnitPrice = StockID.SalePrice;
                    }
                    TaxRate = StockID.Tax;
                }
            }
        }

        [ImmediatePostData]
        public double Amount
        {
            get => amount;
            set
            {
                if (SetPropertyValue(nameof(Amount), ref amount, value))
                {
                     /* if (StockID.Remaining >= 0 && IsSaving)
                      {
                          if (Amount >= StockID.Remaining)
                          {
                              Amount = StockID.Remaining;
                              CalculataTotal();
                          }
                      }
                      else
                      {
                         CalculataTotal();
                      }*/
                  CalculataTotal();
                }
            }
        }

        [ImmediatePostData]
        [XafDisplayName("Birim")]
        public Unit UnitID
        {
            get => unitID;
            set => SetPropertyValue(nameof(UnitID), ref unitID, value);
        }

        public double UnitPrice
        {
            get => unitPrice;
            set
            {
                if (SetPropertyValue(nameof(UnitPrice), ref unitPrice, value))
                {
                    CalculataTotal();
                }
            }
        }

        public double Total
        {
            get => total;
            set => SetPropertyValue(nameof(Total), ref total, value);
        }

        [ImmediatePostData]
        public double DiscountRate
        {
            get => discountRate;
            set
            {
                if (SetPropertyValue(nameof(DiscountRate), ref discountRate, value))
                {
                    CalculataTotal();
                }
            }
        }

        public double DiscountAmount
        {
            get => discountAmount;
            set => SetPropertyValue(nameof(DiscountAmount), ref discountAmount, value);
        }

        public double NetTotal
        {
            get => netTotal;
            set => SetPropertyValue(nameof(NetTotal), ref netTotal, value);
        }

        [ImmediatePostData]
        public double TaxRate
        {
            get => taxRate;
            set
            {
                if (SetPropertyValue(nameof(TaxRate), ref taxRate, value))
                {
                    CalculataTotal();
                }
            }
        }

        public double TaxAmount
        {
            get => taxAmount;
            set => SetPropertyValue(nameof(TaxAmount), ref taxAmount, value);
        }

        public double OverallTotal
        {
            get => overallTotal;
            set => SetPropertyValue(nameof(OverallTotal), ref overallTotal, value);
        }

        [XafDisplayName("Fiş")]
        [Association("Receipts-Detail")]
        public Receipts ReceiptID
        {
            get => receiptID;
            set
            {
                Receipts oldReceipt = receiptID;
                bool didItChange = SetPropertyValue(nameof(ReceiptID), ref receiptID, value);
                if (!IsLoading && !IsSaving && oldReceipt != receiptID && didItChange)
                {
                    oldReceipt = oldReceipt ?? receiptID;
                    oldReceipt.CalculateSubTotal(true);
                    oldReceipt.CalculateOverallTotal(true);
                    oldReceipt.CalculateDiscountTotal(true);
                    oldReceipt.CalculateTaxTotal(true);
                    stockType = ReceiptID.MovementType;
                    Date = ReceiptID.Date;

                    if (ReceiptID.AccountID.Discount)
                    {
                        DiscountRate = ReceiptID.AccountID.DiscountRate;
                    }
                }
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Definition
        {
            get => definition;
            set => SetPropertyValue(nameof(Definition), ref definition, value);
        }

        void CalculataTotal()
        {
            Total = Amount * UnitPrice;
            DiscountAmount = (Total / 100) * DiscountRate;
            NetTotal = Total - DiscountAmount;
            TaxAmount = (NetTotal / 100) * taxRate;
            OverallTotal = NetTotal + TaxAmount;
            if (ReceiptID != null)
            {
                ReceiptID.CalculateSubTotal(true);
                ReceiptID.CalculateOverallTotal(true);
                ReceiptID.CalculateDiscountTotal(true);
                ReceiptID.CalculateTaxTotal(true);
            }

        }
    }
}
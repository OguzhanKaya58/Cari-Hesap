using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class PurchaseInvoice : Receipts
    {
        public PurchaseInvoice(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Date = DateTime.Now;
            Type = Enum.ReceiptMovementType.Purchase;
            MovementType = Enum.StockMovementType.Input;
        }
        public override string ToString()
        {
            return Definition;
        }
        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork && (Session.DataLayer != null) && Session.IsNewObject(this) && string.IsNullOrEmpty(Code)))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "PurchaseInvoicePrefix");
                Code = string.Format("AF{0:D8}", value);
            }
            Definition = $"{Code} Nolu - {Date} Tarihli {AccountID} Hesaba  {OverallTotal} Tutarındaki Alış Faturası";
            base.OnSaving();
        }
    }
}
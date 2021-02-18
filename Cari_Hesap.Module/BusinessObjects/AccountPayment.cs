using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class AccountPayment : CaseTransactions
    {
        public AccountPayment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CaseMovementType = Enum.CaseMovementType.Payment;
            Date = DateTime.Now;
        }
        public double AccountBalance
        {
            get
            {
                if (AccountID != null)
                {
                    return AccountID.Balance;
                }
                else
                {
                    return 0;
                }
            }
        }

        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "AccountPaymentServerPrefix");
                Code = String.Format("CÖ{0:D7}", value);
            }
            Definition = $"{Code} Nolu - {Date} Tarihli {CaseID.Definition} Hesabından {AccountID.Definition} Hesabına {Amount} Tutarında Ödeme Yapılmıştır.";
            base.OnSaving();
        }
    }
}
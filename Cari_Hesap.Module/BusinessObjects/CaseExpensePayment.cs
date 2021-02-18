using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class CaseExpensePayment : CaseTransactions
    { 
        public CaseExpensePayment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CaseMovementType = Enum.CaseMovementType.Payment;
            Date = DateTime.Now;
        }

        Expenses expensesID;

        [Association("Expenses-ExpenseDetail")]
        [XafDisplayName("Masraf Türü")]
        public Expenses ExpensesID
        {
            get => expensesID;
            set => SetPropertyValue(nameof(ExpensesID), ref expensesID, value);
        }
        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "ExpensePaymentServerPrefix");
                Code = String.Format("MÖ{0:D7}", value);
            }
            Definition = $"{Code} Nolu - {Date} Tarihli {AccountID.Definition} Hesabından {ExpensesID.Definition} Hesabına {Amount} Tutarında Tahsilat Yapılmıştır.";
            base.OnSaving();
        }
    }
}
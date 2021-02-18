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
    public class BankCollection : BankTransactions
    {
        public BankCollection(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Date = DateTime.Now;
            CaseMovementType = Enum.CaseMovementType.BankCollection;
        }
        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "BankCollectionServerPrefix");
                Code = String.Format("BT{0:D7}", value);
            }
            Definition = $"{Code} Nolu - {Date} Tarihli {AccountID.Definition} Hesabından {BankID.Definition} Bankadan {AccountsID.Account} Nolu Hesaba {Amount} Tutarında Tahsilat Yapılmıştır.";
            base.OnSaving();
        }
    }
}
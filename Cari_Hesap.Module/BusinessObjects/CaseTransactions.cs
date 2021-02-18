using Cari_Hesap.Module.Enum;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class CaseTransactions : BaseObject
    {
        public CaseTransactions(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        string definition;
        DateTime date;
        double amount;
        CaseMovementType caseMovementType;
        Account accountID;
        string code;
        Case caseID;


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [XafDisplayName("Kasa Hesabı")]
        [Association("Case-CaseMovement")]
        public Case CaseID
        {
            get => caseID;
            set => SetPropertyValue(nameof(CaseID), ref caseID, value);
        }

        [XafDisplayName("Cari Hesap")]
        [Association("Account-MoneyMovement")]
        [ImmediatePostData]
        public Account AccountID
        {
            get => accountID;
            set => SetPropertyValue(nameof(AccountID), ref accountID, value);
        }

        public CaseMovementType CaseMovementType
        {
            get => caseMovementType;
            set => SetPropertyValue(nameof(CaseMovementType), ref caseMovementType, value);
        }

        public double Amount
        {
            get => amount;
            set => SetPropertyValue(nameof(Amount), ref amount, value);
        }
        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Definition
        {
            get => definition;
            set => SetPropertyValue(nameof(Definition), ref definition, value);
        }

        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "CaseMovementServerPrefix");
                Code = String.Format("{0:D7}", value);
            }
            base.OnSaving();
        }
    }
}
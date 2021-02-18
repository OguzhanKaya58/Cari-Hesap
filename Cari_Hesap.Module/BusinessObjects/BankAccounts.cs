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
    [DefaultProperty("Account")]
    public class BankAccounts : XPObject
    { 
        public BankAccounts(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        string explanation;
        BankBranches bankBranchID;
        Banks bankID;
        string ınternationalBankAccountNumber;
        string account;
        string code;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Account
        {
            get => account;
            set => SetPropertyValue(nameof(Account), ref account, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InternationalBankAccountNumber
        {
            get => ınternationalBankAccountNumber;
            set => SetPropertyValue(nameof(InternationalBankAccountNumber), ref ınternationalBankAccountNumber, value);
        }

        [XafDisplayName("Banka")]
        [Association("Banks-BankAccounts")]
        [ImmediatePostData]
        public Banks BankID
        {
            get => bankID;
            set => SetPropertyValue(nameof(BankID), ref bankID, value);
        }

        [XafDisplayName("Şube")]
        [Association("BankBranches-BankAccounts")]
        public BankBranches BankBranchID
        {
            get => bankBranchID;
            set => SetPropertyValue(nameof(BankBranchID), ref bankBranchID, value);
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Explanation
        {
            get => explanation;
            set => SetPropertyValue(nameof(Explanation), ref explanation, value);
        }

        [Association("BankAccounts-BankTransactions")]
        public XPCollection<BankTransactions> BankTransactions
        {
            get
            {
                return GetCollection<BankTransactions>(nameof(BankTransactions));
            }
        }

        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "BankAccountServerPrefix");
                Code = String.Format("BH{0:D7}", value);
            }
            base.OnSaving();
        }
    }
}
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
    public class BankTransactions : CaseTransactions
    {
        public BankTransactions(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        BankAccounts accountsID;
        Banks bankID;

        [XafDisplayName("Banka")]
        [ImmediatePostData]
        public Banks BankID
        {
            get => bankID;
            set => SetPropertyValue(nameof(BankID), ref bankID, value);
        }
        
        [XafDisplayName("Hesap No")]
        [Association("BankAccounts-BankTransactions")]
        public BankAccounts AccountsID
        {
            get => accountsID;
            set => SetPropertyValue(nameof(AccountsID), ref accountsID, value);
        }



    }
}
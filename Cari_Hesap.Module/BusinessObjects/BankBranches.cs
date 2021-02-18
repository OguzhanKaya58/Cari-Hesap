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
    [DefaultProperty("Definition")]
    public class BankBranches : XPObject
    { 
        public BankBranches(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        string explanation;
        Banks bankID;
        string definition;
        string code;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Definition
        {
            get => definition;
            set => SetPropertyValue(nameof(Definition), ref definition, value);
        }

        [XafDisplayName("Banka")]
        [Association("Banks-BankBranches")]
        public Banks BankID
        {
            get => bankID;
            set => SetPropertyValue(nameof(BankID), ref bankID, value);
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Explanation
        {
            get => explanation;
            set => SetPropertyValue(nameof(Explanation), ref explanation, value);
        }

        [Association("BankBranches-BankAccounts")]
        public XPCollection<BankAccounts> BankAccounts
        {
            get
            {
                return GetCollection<BankAccounts>(nameof(BankAccounts));
            }
        }
        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "BankBranchServerPrefix");
                Code = String.Format("BŞ{0:D7}", value);
            }
            base.OnSaving();
        }
    }
}
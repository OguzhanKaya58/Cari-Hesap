using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDefaultProperty("Definition")]
    public class Expenses : XPObject
    {
        public Expenses(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        ExpenseGroup expenseGroupID;
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

        [Association("ExpenseGroup-Expenses")]
        [XafDisplayName("Masraf Grubu")]
        public ExpenseGroup ExpenseGroupID
        {
            get => expenseGroupID;
            set => SetPropertyValue(nameof(ExpenseGroupID), ref expenseGroupID, value);
        }
        [Association("Expenses-ExpenseDetail")]
        public XPCollection<CaseExpensePayment> ExpenseDetail
        {
            get
            {
                return GetCollection<CaseExpensePayment>(nameof(ExpenseDetail));
            }
        }
        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "ExpensesServerPrefix");
                Code = String.Format("MS{0:D7}", value);
            }
            base.OnSaving();
        }

    }
}
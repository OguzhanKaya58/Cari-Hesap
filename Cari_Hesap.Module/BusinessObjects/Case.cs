using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DisplayProperty("Definition")]
    //[ListViewFilter("Tüm Liste", "")]
    //[ListViewFilter("Aktif Kasa", "[Status == True]", true)]
    //[ListViewFilter("Pasif Kasa", "[Status == False]")]
    public class Case : XPObject
    {
        public Case(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            Status = true;
            base.AfterConstruction();
        }


        bool status;
        string explanation;
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

        [Size(SizeAttribute.Unlimited)]
        public string Explanation
        {
            get => explanation;
            set => SetPropertyValue(nameof(Explanation), ref explanation, value);
        }

        public bool Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Association("Case-CaseMovement")]
        public XPCollection<CaseTransactions> CaseMovement
        {
            get
            {
                return GetCollection<CaseTransactions>(nameof(CaseMovement));
            }
        }


        public double Collection => CaseMovement.Where(x => x.CaseMovementType == Enum.CaseMovementType.Collection).Sum(x => x.Amount);

        public double Payment => CaseMovement.Where(x => x.CaseMovementType == Enum.CaseMovementType.Payment).Sum(x => x.Amount);

        public double Balance => Collection - Payment;

        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "CasePrefix");
                Code = String.Format("{0:D7}", value);
            }
            base.OnSaving();
        }
        public override string ToString()
        {
            return Explanation;
        }
    }
}
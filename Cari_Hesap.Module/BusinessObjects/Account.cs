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
    //[ListViewFilter ("Tüm Liste", "")]
    //[ListViewFilter("Aktif Cari", "[Status == True]", true)]
    //[ListViewFilter("Pasif Cari", "[Status == False]")]
    public class Account : BaseObject
    {
        public Account(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            Status = true;
            base.AfterConstruction();
        }


        double discountRate;
        bool discount;
        bool status;
        string email;
        string phone;
        string country;
        string city;
        string address;
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


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string City
        {
            get => city;
            set => SetPropertyValue(nameof(City), ref city, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Phone
        {
            get => phone;
            set => SetPropertyValue(nameof(Phone), ref phone, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }

        public bool Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }
        [VisibleInDetailView(false)]
        public double Receivable
        {
            get
            {
                double temp = 0;
                temp += PurchaseSaleTransaction.Where(x => x.Type == Enum.ReceiptMovementType.Purchase).Sum(x => x.OverallTotal);
                temp += MoneyMovement.Where(x => x.CaseMovementType == Enum.CaseMovementType.Collection).Sum(x => x.Amount);
                return temp;
            }
        }

        [VisibleInDetailView(false)]
        public double Debt
        {
            get
            {
                double temp = 0;
                temp += PurchaseSaleTransaction.Where(x => x.Type == Enum.ReceiptMovementType.Sale).Sum(x => x.OverallTotal);
                temp += MoneyMovement.Where(x => x.CaseMovementType == Enum.CaseMovementType.Payment).Sum(x => x.Amount);

                return temp;
            }
        }

        [VisibleInDetailView(false)]
        public double Balance => Debt - Receivable;

        public bool Discount
        {
            get => discount;
            set => SetPropertyValue(nameof(Discount), ref discount, value);
        }

        public double DiscountRate
        {
            get => discountRate;
            set => SetPropertyValue(nameof(DiscountRate), ref discountRate, value);
        }

        [Association("Account-PurchaseSaleTransaction")]
        public XPCollection<Receipts> PurchaseSaleTransaction
        {
            get
            {
                return GetCollection<Receipts>(nameof(PurchaseSaleTransaction));
            }
        }

        [Association("Account-MoneyMovement")]
        public XPCollection<CaseTransactions> MoneyMovement
        {
            get
            {
                return GetCollection<CaseTransactions>(nameof(MoneyMovement));
            }
        }

        public override string ToString()
        {
            return Definition;
        }
        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "AccountServerPrefix");
                Code = String.Format("{0:D7}", value);
            }
            base.OnSaving();
        }

    }
}
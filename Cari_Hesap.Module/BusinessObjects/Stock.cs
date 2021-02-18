using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Definition")]
    //[ListViewFilter("Tüm Liste","")]
    //[ListViewFilter("Aktif Stok","[Status == True]",true)]
    //[ListViewFilter("Pasif Stok","[Status == False]")]
    public class Stock : XPObject
    {
        public Stock(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Unit unit = Session.FindObject<Unit>(new BinaryOperator(nameof(unit.Default), true));
            if (unit != null) unitID = unit;
            Status = true;
        }

        Unit unitID;
        bool status;
        double salePrice;
        double purchasePrice;
        double tax;
        string definition;
        string code;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }


        [Size(200)]
        public string Definition
        {
            get => definition;
            set => SetPropertyValue(nameof(Definition), ref definition, value);
        }

        [XafDisplayName("Birim")]
        public Unit UnitID
        {
            get => unitID;
            set => SetPropertyValue(nameof(UnitID), ref unitID, value);
        }

        public double Tax
        {
            get => tax;
            set => SetPropertyValue(nameof(Tax), ref tax, value);
        }

        public double PurchasePrice
        {
            get => purchasePrice;
            set => SetPropertyValue(nameof(PurchasePrice), ref purchasePrice, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public double SalePrice
        {
            get => salePrice;
            set => SetPropertyValue(nameof(SalePrice), ref salePrice, value);
        }

        public bool Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Association("Stock-StockMovements")]
        public XPCollection<StockMovements> StockMovements
        {
            get
            {
                return GetCollection<StockMovements>(nameof(StockMovements));
            }
        }

        [VisibleInDetailView(false)]
        public double Entering => StockMovements.Where(x => x.StockType == Enum.StockMovementType.Input).Sum(x => x.Amount);

        [VisibleInDetailView(false)]
        public double Out => StockMovements.Where(x => x.StockType == Enum.StockMovementType.Output).Sum(x => x.Amount);

        [VisibleInDetailView(false)]
        public double Remaining => Entering - Out;


        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "StockServerPrefix");
                Code = String.Format("{0:D7}", value);
            }
            base.OnSaving();
        }
    }
}
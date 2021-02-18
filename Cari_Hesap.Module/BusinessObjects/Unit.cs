using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace Cari_Hesap.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Definition")]
    //[ListViewFilter("Tüm Liste", "")]
    //[ListViewFilter("Aktif Birim", "[Status == True]", true)]
    //[ListViewFilter("Pasif Birim", "[Status == False]")]
    public class Unit : XPObject
    {
        public Unit(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            status = true;
        }

        bool @default;
        bool status;
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

        public bool Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [RuleRequiredField]
        public bool Default
        {
            get => @default;
            set => SetPropertyValue(nameof(Default), ref @default, value);
        }
        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.IsNewObject(this)) && String.IsNullOrEmpty(Code))
            {
                int value = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "UnitServerPrefix");
                Code = String.Format("{0:D7}", value);
            }
            if (@Default)
            {
                Unit unit = Session.FindObject<Unit>(new BinaryOperator(nameof(unit.Default), true));
                if (unit != null)
                {
                    unit.Default = false;
                    unit.Save();
                }
            }
            base.OnSaving();
        }
    }
}
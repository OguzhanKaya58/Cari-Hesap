using Cari_Hesap.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;

namespace Cari_Hesap.Module.Controllers
{
    public class CustomizeNewActionItemsListController : ObjectViewController<ObjectView,Receipts>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            NewObjectViewController controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
                controller.CollectCreatableItemTypes += Controller_CollectCreatableItemTypes;
                controller.CollectDescendantTypes += Controller_CollectDescendantTypes;
                if (controller.Active)
                {
                    controller.UpdateNewObjectAction();
                }
            }
            // Perform various tasks depending on the target View.
        }
        void CustomizeList(ICollection<Type> types)
        {
            List<Type> _list = new List<Type>();
            foreach (Type item in types)
            {
                if (item == typeof(Receipts)) _list.Add(item);
            }
            foreach (Type item in _list)
            {
                types.Remove(item);
            }
        }
        protected void Controller_CollectDescendantTypes(object sender, CollectTypesEventArgs e)
        {
            CustomizeList(e.Types);
        }
        protected void Controller_CollectCreatableItemTypes(object sender, CollectTypesEventArgs e)
        {
            CustomizeList(e.Types);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            NewObjectViewController controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
                controller.CollectCreatableItemTypes += Controller_CollectCreatableItemTypes;
                controller.CollectDescendantTypes += Controller_CollectDescendantTypes;
            }
        }
    }
}

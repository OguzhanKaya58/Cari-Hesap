using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cari_Hesap.Module.Enum
{
    public enum ReceiptMovementType
    {
        [XafDisplayName("Alış Faturası")]
        Purchase,
        [XafDisplayName("Satış Faturası")]
        Sale
    }
}

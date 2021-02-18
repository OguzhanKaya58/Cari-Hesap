using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cari_Hesap.Module.Enum
{
    public enum CaseMovementType
    {
        [XafDisplayName("Kasa Ödeme")]
        Payment,
        [XafDisplayName("Kasa Tahsilat")]
        Collection,
        [XafDisplayName("Banka Ödeme")]
        BankPayment,
        [XafDisplayName("Banka Tahsilat")]
        BankCollection,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackLotus.Cards;
using EixoX;

namespace WindowsFormsApplication1
{
    public interface CardReadViewee : Viewee
    {
        void OnCardInformationRead(CardInformation card);
    }
}

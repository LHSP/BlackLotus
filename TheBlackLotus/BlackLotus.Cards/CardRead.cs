using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EixoX;

namespace BlackLotus.Cards
{
    public static class CardRead
    {
        public static List<Card> Read(Viewee viewee)
        {
            try
            {
                return Card.Select().ToList();
            }
            catch (Exception ex)
            {
                viewee.OnException(ex);
                return null;
            }
        }
    }
}

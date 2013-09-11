using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BlackLotus.Cards;
using EixoX;
using Helpers;

namespace WindowsFormsApplication1
{
    public class CardGather
    {
        private string _startOfCardTable = "class=\"cardDetails\"";
        private CardReadViewee _viewee;

        public CardGather(CardReadViewee viewee)
        {
            this._viewee = viewee;
        }

        public bool ReadCard(int cardNumber)
        {
            try
            {
                Card card = new Card();
                string page = "";
                using (WebClient client = new WebClient())
                {
                    page = client.DownloadString("http://gatherer.wizards.com/pages/Card/Details.aspx?multiverseid=" + cardNumber);
                }

                if (!page.Contains(this._startOfCardTable))
                    return false;

                while(page.CountOcurrences(this._startOfCardTable) > 0)
                {
                    page = page.Substring(page.RightIndexOf(this._startOfCardTable));
                    string cardTable = page.Substring(0, page.IndexOf("</table>"));
                    if (cardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_cardImage"))
                    {
                        string image = cardTable.Substring(cardTable.RightIndexOf("multiverseid="));
                        card.Art = image.Substring(0, image.IndexOf("&amp;"));
                    }
                    _viewee.OnCardRead(card);
                }
                return true;

            }
            catch (Exception ex)
            {
                _viewee.OnException(ex);
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackLotus.Cards;
using EixoX;

namespace TheBlackLotus.Controllers
{
    public class CardsController : Controller, Viewee
    {
        //
        // GET: /Card/

        public ActionResult Index()
        {
            return View(CardRead.Read(this));
        }


        void Viewee.OnException(Exception ex)
        {
            // DisplayErrorMessage("Ops, algo de errado ocorreu. =P");
        }
    }
}

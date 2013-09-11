using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace BlackLotus.Cards
{
    [DatabaseTable]
    public class CardAbilities : BlackLotusDbModel<CardAbilities>
    {
        [DatabaseColumn]
        public int CardId { get; set; }

        [DatabaseColumn]
        public string AbilitiesText { get; set; }
    }
}
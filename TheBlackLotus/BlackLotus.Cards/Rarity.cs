using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace BlackLotus.Cards
{
    [DatabaseTable]
    public class Rarity : BlackLotusDbModel<Rarity>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int RarityId { get; set; }

        [DatabaseColumn]
        public string RarityName { get; set; }
    }
}
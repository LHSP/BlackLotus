using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace BlackLotus.Cards
{
    [DatabaseTable]
    public class Ability : BlackLotusDbModel<Ability>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int AbilityId { get; set; }

        [DatabaseColumn]
        public string AbilityText { get; set; }
    }
}
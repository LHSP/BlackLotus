using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace TheBlackLotus
{
    [DatabaseTable]
    public class Ability : BlackLotusDbModel<Ability>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int AbilityId { get; set; }

        [DatabaseColumn]
        public string RuleText { get; set; }
    }
}
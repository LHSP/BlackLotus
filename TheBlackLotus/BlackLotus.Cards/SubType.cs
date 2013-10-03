using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace BlackLotus.Cards
{
    [DatabaseTable]
    public class SubType : BlackLotusDbModel<SubType>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int SubTypeId { get; set; }

        [DatabaseColumn(DatabaseColumnKind.Unique)]
        public string SubTypeName { get; set; }
    }
}
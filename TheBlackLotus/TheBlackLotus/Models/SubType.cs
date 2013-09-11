using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace TheBlackLotus
{
    [DatabaseTable]
    public class SubType : BlackLotusDbModel<SubType>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int SubTypeId { get; set; }

        [DatabaseColumn]
        public string SubTypeName { get; set; }
    }
}
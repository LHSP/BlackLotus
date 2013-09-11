using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace BlackLotus.Cards
{
    [DatabaseTable]
    public class Type : BlackLotusDbModel<Type>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int TypeId { get; set; }

        [DatabaseColumn]
        public string TypeName { get; set; }
    }
}
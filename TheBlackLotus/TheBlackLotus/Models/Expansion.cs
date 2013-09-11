using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace TheBlackLotus
{
    [DatabaseTable]
    public class Expansion : BlackLotusDbModel<Expansion>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int ExpansionId { get; set; }

        [DatabaseColumn]
        public string Name { get; set; }

        [DatabaseColumn]
        public string Symbol { get; set; }

        [DatabaseColumn]
        public int Size { get; set; }
    }
}
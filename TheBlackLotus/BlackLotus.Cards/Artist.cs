using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace BlackLotus.Cards
{
    [DatabaseTable]
    public class Artist : BlackLotusDbModel<Artist>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int ArtistId { get; set; }

        [DatabaseColumn(DatabaseColumnKind.Unique)]
        public string Name { get; set; }
    }
}
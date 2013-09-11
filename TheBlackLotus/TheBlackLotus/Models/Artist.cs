using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EixoX.Data;

namespace TheBlackLotus
{
    [DatabaseTable]
    public class Artist : BlackLotusDbModel<Artist>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int ArtistId { get; set; }

        [DatabaseColumn]
        public string Name { get; set; }
    }
}
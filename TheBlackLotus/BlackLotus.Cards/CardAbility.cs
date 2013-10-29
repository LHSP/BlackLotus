using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EixoX.Data;

namespace BlackLotus.Cards
{
    [DatabaseTable]
    public class CardAbility : BlackLotusDbModel<CardAbility>
    {
        [DatabaseColumn(DatabaseColumnKind.PrimaryKey)]
        public int CardId { get; set; }

        [DatabaseColumn(DatabaseColumnKind.PrimaryKey)]
        public int AbilityId { get; set; }
    }
}

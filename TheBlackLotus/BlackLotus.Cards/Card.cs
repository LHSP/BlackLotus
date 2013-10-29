using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EixoX.Data;

namespace BlackLotus.Cards
{
    [DatabaseTable]
    public class Card : BlackLotusDbModel<Card>
    {
        [DatabaseColumn(DatabaseColumnKind.Identity)]
        public int CardId { get; set; }
        [DatabaseColumn]
        public string Name { get; set; }
        [DatabaseColumn(true)]
        public string ManaCost { get; set; }
        [DatabaseColumn(true)]
        public string ConvertedManaCost { get; set; }
        [DatabaseColumn]
        public int TypeId { get; set; }
        [DatabaseColumn]
        public int SubTypeId { get; set; }
        [DatabaseColumn]
        public int ExpansionId { get; set; }
        [DatabaseColumn]
        public string Art { get; set; }
        [DatabaseColumn]
        public string FlavorText { get; set; }
        [DatabaseColumn]
        public string ArtistId { get; set; }
        [DatabaseColumn]
        public int CollectorsNumber { get; set; }
        [DatabaseColumn]
        public string Power { get; set; }
        [DatabaseColumn]
        public string Toughness { get; set; }
        [DatabaseColumn(true)]
        public string RuleText { get; set; }
        [DatabaseColumn]
        public int RarityId { get; set; }

        private Type _type;
        public Type Type
        {
            get
            {
                if (_type == null)
                    _type = Type.WithIdentity(this.TypeId);
                return _type;
            }
        }

        private SubType _subType;
        public SubType SubType
        {
            get
            {
                if (this.SubTypeId == null)
                    return null;
                if (_subType == null)
                    _subType = SubType.WithIdentity(this.SubTypeId);
                return _subType;
            }
        }

        private CardAbilities _abilities;
        public CardAbilities Abilities
        {
            get
            {
                if (_abilities == null)
                    _abilities = CardAbilities.WithMember("CardId", this.CardId);
                return _abilities;
            }
        }

        private Expansion _expansion;
        public Expansion Expansion
        {
            get
            {
                if (_expansion == null)
                    _expansion = Expansion.WithIdentity(this.ExpansionId);
                return _expansion;
            }
        }

        private Artist _artist;
        public Artist Artist
        {
            get
            {
                if (_artist == null)
                    _artist = Artist.WithIdentity(this.ArtistId);
                return _artist;
            }
        }

        private Rarity _rarity;
        public Rarity Rarity
        {
            get
            {
                if (_rarity == null)
                    _rarity = Rarity.WithIdentity(this.RarityId);
                return _rarity;
            }
        }
    }
}
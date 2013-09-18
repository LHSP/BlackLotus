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
        [DatabaseColumn]
        public int CardId { get; set; }
        [DatabaseColumn]
        public string Name { get; set; }
        [DatabaseColumn]
        public string ManaCost { get; set; }
        [DatabaseColumn]
        public int ConvertedManaCost { get; set; }
        [DatabaseColumn]
        public int TypeId { get; set; }
        [DatabaseColumn]
        public int SubTypeId { get; set; }
        [DatabaseColumn]
        public int ExpansionId { get; set; }
        //[DatabaseColumn]
        //public string Art { get; set; }
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
        [DatabaseColumn]
        public string RuleText { get; set; }
        [DatabaseColumn]
        public int RarityId { get; set; }

        private string _art;
        public string Art
        {
            get
            {
                return this._art;
            }
            set
            {
                this._art = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card".Replace("{0}", value);
            }
        }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Abilities { get; set; }
        public string Expansion { get; set; }
        private string _expansionSymbol;
        public string ExpansionSymbol
        {
            get
            {
                return this._expansionSymbol;
            }
        }
        public void SetExpansionSymbol(string set, string rarity)
        {
            this._expansionSymbol = "http://gatherer.wizards.com/Handlers/Image.ashx?type=symbol&set={0}&size=large&rarity={1}".Replace("{0}", set).Replace("{1}", rarity.First().ToString());
        }
        public string Artist { get; set; }
        public string Rarity { get; set; }

        //private Type _type;
        //public Type Type
        //{
        //    get
        //    {
        //        if (_type == null)
        //            _type = Type.WithIdentity(this.TypeId);
        //        return _type;
        //    }
        //}

        //private SubType _subType;
        //public SubType SubType
        //{
        //    get
        //    {
        //        if (this.SubTypeId == null)
        //            return null;
        //        if(_subType == null)
        //            _subType = SubType.WithIdentity(this.SubTypeId);
        //        return _subType;
        //    }
        //}

        //private CardAbilities _abilities;
        //public CardAbilities Abilities
        //{
        //    get
        //    {
        //        if (_abilities == null)
        //            _abilities = CardAbilities.WithMember("CardId", this.CardId);
        //        return _abilities;
        //    }
        //}

        //private Expansion _expansion;
        //public Expansion Expansion
        //{
        //    get
        //    {
        //        if (_expansion == null)
        //            _expansion = Expansion.WithIdentity(this.ExpansionId);
        //        return _expansion;
        //    }
        //}

        //private Artist _artist;
        //public Artist Artist
        //{
        //    get
        //    {
        //        if(_artist == null)
        //            _artist = Artist.WithIdentity(this.ArtistId);
        //        return _artist;
        //    }
        //}

        //private Rarity _rarity;
        //public Rarity Rarity
        //{
        //    get
        //    {
        //        if (_rarity == null)
        //            _rarity = Rarity.WithIdentity(this.RarityId);
        //        return _rarity;
        //    }
        //}


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Image: ");
            sb.Append(this._art);

            sb.Append("\r\nName: ");
            sb.Append(this.Name);

            sb.Append("\r\nCost: ");
            sb.Append(this.ManaCost);

            sb.Append("\r\nType: ");
            sb.Append(this.Type);

            sb.Append("\r\nSubType: ");
            sb.Append(this.SubType);

            if (!string.IsNullOrEmpty(this.Power))
            {
                sb.Append("\r\nPower: ");
                sb.Append(this.Power);
            }

            if (!string.IsNullOrEmpty(this.Toughness))
            {
                sb.Append("\r\nToughness: ");
                sb.Append(this.Toughness);
            }

            sb.Append("\r\nAbilities: ");
            sb.Append(this.Abilities);

            sb.Append("\r\nRulesText: ");
            sb.Append(this.RuleText);

            sb.Append("\r\nFlavorText: ");
            sb.Append(this.FlavorText);

            sb.Append("\r\nSymbol: ");
            sb.Append(this._expansionSymbol);

            sb.Append("\r\nSet: ");
            sb.Append(this.Expansion);

            sb.Append("\r\nRarity: ");
            sb.Append(this.Rarity);

            return sb.ToString();
        }
    }
}
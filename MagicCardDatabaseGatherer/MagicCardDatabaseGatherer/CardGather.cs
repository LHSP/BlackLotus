using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlackLotus.Cards;
using EixoX;
using Helpers;

namespace WindowsFormsApplication1
{
    public class CardGather
    {
        private string _startOfCardTable = "class=\"cardDetails\"";
        private CardReadViewee _viewee;

        public CardGather(CardReadViewee viewee)
        {
            this._viewee = viewee;
        }

        public bool ReadCard(int cardNumber)
        {
            try
            {
                CardInformation card = new CardInformation();
                string page = "";
                //using (WebClient client = new WebClient())
                //{
                //    page = client.DownloadString("http://gatherer.wizards.com/pages/Card/Details.aspx?multiverseid=" + cardNumber);
                //}

                page = new HttpDownloader("http://gatherer.wizards.com/pages/Card/Details.aspx?multiverseid=" + cardNumber, "", "").GetPage();

                if (!page.Contains(this._startOfCardTable))
                    return false;

                while (page.CountOcurrences(this._startOfCardTable) > 0)
                {
                    page = page.Substring(page.RightIndexOf(this._startOfCardTable));
                    string mainCardTable = page.Substring(0, page.IndexOf("</table>"));

                    //  Gets Card Image
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_cardImage"))
                    {
                        string image = mainCardTable.Substring(mainCardTable.RightIndexOf("multiverseid="));
                        card.Art = image.Substring(0, image.IndexOf("&amp;"));
                    }

                    //  Gets Card Name
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_nameRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_nameRow"));
                        string name = cardTable.Substring(cardTable.RightIndexOf("value\">"));
                        card.Name = name.Substring(0, name.IndexOf("</div>")).Trim();
                    }

                    //  Gets Card Mana Cost
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_manaRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_manaRow"));
                        string manaCost = cardTable.Substring(cardTable.RightIndexOf("value\">"));
                        manaCost = manaCost.Substring(0, manaCost.RightIndexOf("div>"));
                        while (manaCost.CountOcurrences("name=") > 0)
                        {
                            manaCost = manaCost.Substring(manaCost.RightIndexOf("name="));
                            card.ManaCost += manaCost.First();
                        }
                        card.ManaCost.Trim();
                    }

                    //  Gets Card Converted Mana Cost
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_cmcRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_cmcRow"));
                        string cmc = cardTable.Substring(cardTable.RightIndexOf("value\">"));
                        cmc = cmc.Substring(0, cmc.IndexOf("<")).Trim();
                        card.ConvertedManaCost = Int32.Parse(String.IsNullOrEmpty(cmc) ? "0" : cmc);
                    }

                    //  Gets Card Types
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_typeRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_typeRow"));
                        cardTable = cardTable.Substring(cardTable.RightIndexOf("value\">"));
                        string[] types = cardTable.Substring(0, cardTable.IndexOf("<")).Split(new string[] { "â€”" }, StringSplitOptions.RemoveEmptyEntries);
                        card.Type = types[0].Trim();
                        if (types.Length > 1 && !String.IsNullOrEmpty(types[1].Trim()))
                        {
                            card.SubType = types[1].Trim();
                        }
                    }

                    //  Gets Card Abilities
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_textRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_textRow"));
                        cardTable = cardTable.Substring(cardTable.RightIndexOf("value\">"));

                        string ruleTexts = cardTable;
                        if (ruleTexts.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_FlavorText"))
                            ruleTexts = ruleTexts.Substring(0, cardTable.IndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_FlavorText"));

                        int ruleTextCount = ruleTexts.CountOcurrences("cardtextbox");

                        for (int i = 0; i < ruleTextCount; i++)
                        {
                            ruleTexts = ruleTexts.Substring(ruleTexts.RightIndexOf("cardtextbox\">"));
                            string ruleText = ruleTexts.Substring(0, ruleTexts.IndexOf("</div>"));
                            ruleText = Regex.Replace(ruleText, @"<img([^\]]+?)name=([^\]]+?)&amp;.*?>", @"$2");

                            if (IsAbility(ruleText))
                            {
                                string[] abilities = ruleText.Split(',');
                                for (int j = 0; j < abilities.Length; j++)
                                {
                                    card.Abilities += abilities[j].Trim() + '|';
                                }
                            }
                            else
                            {
                                while (ruleText.CountOcurrences("<img") > 0)
                                {
                                    ruleText = ruleText.Substring(ruleText.RightIndexOf("name="));
                                    card.RuleText += ruleText.Substring(0, ruleText.IndexOf('&'));
                                    ruleText = ruleText.Substring(ruleText.RightIndexOf(">"));
                                }
                                card.RuleText += ruleText;
                            }
                            ruleTexts.Substring(ruleTexts.RightIndexOf("</div>"));
                        }
                    }

                    //  Gets Card Flavor Text
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_FlavorText"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_FlavorText"));
                        string flavorTexts = cardTable;
                        string flavorText = "";
                        while (flavorTexts.CountOcurrences("cardtextbox") > 0)
                        {
                            flavorTexts = flavorTexts.Substring(flavorTexts.RightIndexOf("cardtextbox") + 2);
                            flavorText += flavorTexts.Substring(0, flavorTexts.IndexOf("</div>")) + "\n";
                        }
                        card.FlavorText = flavorText;
                    }

                    //  Get Card Power/Toughness
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_ptRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_ptRow"));
                        cardTable = cardTable.Substring(cardTable.RightIndexOf("value\">"));

                        card.Power = cardTable.Substring(0, cardTable.IndexOf('/')).Trim();
                        string toughness = cardTable.Substring(cardTable.RightIndexOf('/'));
                        card.Toughness = toughness.Substring(0, toughness.IndexOf("</div")).Trim();
                    }

                    //  Get Card Set, Rarity and Symbol
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_setRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_setRow"));
                        cardTable = cardTable.Substring(cardTable.RightIndexOf("img title=\""));

                        card.Expansion = cardTable.Substring(0, cardTable.IndexOf('(')).Trim();
                        cardTable = cardTable.Substring(cardTable.RightIndexOf('('));
                        card.Rarity = cardTable.Substring(0, cardTable.IndexOf(')')).Trim();
                        cardTable = cardTable.Substring(cardTable.RightIndexOf("set="));
                        string exp = cardTable.Substring(0, cardTable.IndexOf("&amp;size"));
                        cardTable = cardTable.Substring(cardTable.RightIndexOf("rarity="));
                        string rty = cardTable.Substring(0, cardTable.IndexOf("\" alt"));
                        card.SetExpansionSymbol(exp, rty);
                    }

                    //  Get Card number
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_numberRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_numberRow"));
                        cardTable = cardTable.Substring(cardTable.RightIndexOf("value\">"));
                        card.CollectorsNumber = Int32.Parse(cardTable.Substring(0, cardTable.IndexOf("</div>")).Trim());
                    }

                    //  Get Card Artist
                    if (mainCardTable.Contains("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_artistRow"))
                    {
                        string cardTable = mainCardTable.Substring(mainCardTable.RightIndexOf("ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_artistRow"));
                        cardTable = cardTable.Substring(cardTable.RightIndexOf("artist=[%22"));
                        card.Artist = cardTable.Substring(0, cardTable.IndexOf("%22]"));
                    }

                    _viewee.OnCardInformationRead(card);
                }
                return true;

            }
            catch (Exception ex)
            {
                _viewee.OnException(ex);
                return false;
            }
        }

        public bool IsAbility(string text)
        {
            string maybeAbility = text;
            int commaIndex = maybeAbility.IndexOf(',');
            int parenthesisIndex = maybeAbility.IndexOf('(');
            if (commaIndex < 1)
                commaIndex = Int32.MaxValue;
            if (parenthesisIndex < 1)
                parenthesisIndex = Int32.MaxValue;

            if (commaIndex <= maybeAbility.Length || parenthesisIndex <= maybeAbility.Length)
            {
                maybeAbility = maybeAbility.Substring(0, Math.Min(commaIndex, parenthesisIndex));
                maybeAbility = Regex.Replace(maybeAbility, "<.*>", "");
            }

            return keywords.Contains(maybeAbility.Trim());
        }

        private string[] keywords = new string[] {
            "Absorb",
            "Affinity",
            "Amplify",
            "Annihilator",
            "Attach",
            "Aura swap",
            "Banding",
            "Bands with other",
            "Battalion",
            "Battle cry",
            "Bloodrush",
            "Bloodthirst",
            "Bury",
            "Bushido",
            "Buyback",
            "Cascade",
            "Champion",
            "Changeling",
            "Channel",
            "Chroma",
            "Cipher",
            "Clash",
            "Conspire",
            "Convoke",
            "Counter",
            "Cumulative upkeep",
            "Cycling",
            "Deathtouch",
            "Defender",
            "Delve",
            "Detain",
            "Devour",
            "Domain",
            "Double strike",
            "Dredge",
            "Echo",
            "Enchant",
            "Entwine",
            "Epic",
            "Equip",
            "Evoke",
            "Evolve",
            "Exalted",
            "Exile",
            "Extort",
            "Fading",
            "Fateful hour",
            "Fateseal",
            "Fear",
            "Fight",
            "First strike",
            "Flanking",
            "Flash",
            "Flashback",
            "Flip",
            "Flying",
            "Forecast",
            "Fortify",
            "Frenzy",
            "Graft",
            "Grandeur",
            "Gravestorm",
            "Haste",
            "Haunt",
            "Hellbent",
            "Hexproof",
            "Hideaway",
            "Horsemanship",
            "Imprint",
            "Indestructible",
            "Infect",
            "Intimidate",
            "Join forces",
            "Kicker",
            "Kinship",
            "Landfall",
            "Landhome",
            "Landwalk",
            "Level up",
            "Lifelink",
            "Living weapon",
            "Madness",
            "Metalcraft",
            "Miracle",
            "Modular",
            "Morbid",
            "Morph",
            "Multikicker",
            "Ninjutsu",
            "Offering",
            "Persist",
            "Phasing",
            "Poisonous",
            "Populate",
            "Proliferate",
            "Protection",
            "Provoke",
            "Prowl",
            "Radiance",
            "Rampage",
            "Reach",
            "Rebound",
            "Recover",
            "Regenerate",
            "Reinforce",
            "Replicate",
            "Retrace",
            "Ripple",
            "Sacrifice",
            "Scavenge",
            "Scry",
            "Shadow",
            "Shroud",
            "Soulbond",
            "Soulshift",
            "Splice",
            "Split second",
            "Storm",
            "Substance",
            "Sunburst",
            "Suspend",
            "Sweep",
            "Tap/Untap",
            "Threshold",
            "Totem armor",
            "Trample",
            "Transfigure",
            "Transform",
            "Transmute",
            "Typecycling",
            "Undying",
            "Unearth",
            "Unleash",
            "Vanishing",
            "Vigilance",
            "Wither"
        };
    }
}

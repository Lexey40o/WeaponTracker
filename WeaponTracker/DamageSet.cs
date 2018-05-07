using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WeaponTracker
{
    public class DamageSet
    {
        public enum AcriStone { None = 0, Green = 1, Yellow = 2, Orange = 3, Red = 4, Blue = 5, Purple = 6, Black = 7, White = 8 };
        public enum Dice { d4 = 4, d6 = 6, d8 = 8, d10 = 10, d12 = 12 };

        public List<Dice> DamageDice;
        public int DamageBonus;

        public DamageSet() { }
        public DamageSet(List<Dice> dices, int bonus)
        {
            DamageDice = dices;
            DamageBonus = bonus;
        }

        public bool isEmpty()
        {
            return DamageDice is null;
        }

        public string[] listOfDice()
        {
            return DamageDice.Select(i => i.ToString()).ToArray();
        }

        public DamageSet ModifyDamage(AcriStone stone)
        {
            int minDiceValue = (int)Enum.GetValues(typeof(Dice)).Cast<Dice>().First();
            int maxDiceValue = (int)Enum.GetValues(typeof(Dice)).Cast<Dice>().Last();

            var newDamageSet = new DamageSet();
            var newDiceList = new List<Dice>(DamageDice);
            int lowestDamageDice = (int)newDiceList.Min();
            int stoneValue = (int)stone;
            int extraDamage = stoneValue % 2 == 0 ? 0 : 1;

            if((maxDiceValue + minDiceValue) - lowestDamageDice <= stoneValue)
            {
                newDiceList[newDiceList.IndexOf(newDiceList.Min())] = Dice.d12;
                newDiceList.Insert(newDiceList.Count, (Dice)((stoneValue - extraDamage) - (maxDiceValue - lowestDamageDice)));
            }
            else if (maxDiceValue - lowestDamageDice < stoneValue && stoneValue - (maxDiceValue - lowestDamageDice) < minDiceValue)
            {
                newDiceList[newDiceList.IndexOf(newDiceList.Min())] = Dice.d12;
                extraDamage = stoneValue - (maxDiceValue - lowestDamageDice);
            }
            else
            {
                newDiceList[newDiceList.IndexOf(newDiceList.Min())] = (Dice)(lowestDamageDice + stoneValue - extraDamage);
            }
            
            newDamageSet.DamageDice = newDiceList;
            newDamageSet.DamageBonus = DamageBonus + extraDamage;

            return newDamageSet;
        }

        public static AcriStone DecodeAcriSymbol(string symbol)
        {
            switch (symbol.ToUpper())
            {
                case "VIRHREI":
                    return AcriStone.Green;
                case "KELAVOTAINEN":
                    return AcriStone.Yellow;
                case "AURANOSSI":
                    return AcriStone.Orange;
                case "PURUBRAINEN":
                    return AcriStone.Red;
                case "CAERINILEUM":
                    return AcriStone.Blue;
                case "VIOPURETTI":
                    return AcriStone.Purple;
                case "NISTAGREOS":
                    return AcriStone.Black;
                case "VALKOIBUNEN":
                    return AcriStone.White;
                default:
                    return AcriStone.None;
            }
        }

        public static string EncodeAcriSymbol(AcriStone stone)
        {
            switch (stone)
            {
                case AcriStone.Green:
                    return "VIRHREI";
                case AcriStone.Yellow:
                    return "KELAVOTAINEN";
                case AcriStone.Orange:
                    return "AURANOSSI";
                case AcriStone.Red:
                    return "PURUBRAINEN";
                case AcriStone.Blue:
                    return "CAERINILEUM";
                case AcriStone.Purple:
                    return "VIOPURETTI";
                case AcriStone.Black:
                    return "NISTAGREOS";
                case AcriStone.White:
                    return "VALKOIBUNEN";
                default:
                    return "";
            }
        }
    }
}
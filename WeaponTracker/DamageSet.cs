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
                newDiceList.Insert(newDiceList.Count, (Dice)((stoneValue - extraDamage) - (minDiceValue - (maxDiceValue - lowestDamageDice))));
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
    }
}
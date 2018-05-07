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
    class Weapon
    {
        public DamageSet BaseDamage, ModifiedDamage = new DamageSet();
        public DamageSet.AcriStone UpgradeStone;
        public string WeaponName;

        public Weapon(DamageSet damage, string name)
        {
            BaseDamage = damage;
            UpgradeStone = DamageSet.AcriStone.None;
            WeaponName = name;
        }

        public string GetWeaponStats()
        {
            string baseDamageString = string.Join(" ", BaseDamage.listOfDice());
            string modifiedDamageString = !ModifiedDamage.isEmpty() ? string.Join(" ", ModifiedDamage.listOfDice()) : "";

            return $"Base: {baseDamageString} + {BaseDamage.DamageBonus}" + (!ModifiedDamage.isEmpty() ? $"\n Modified: {modifiedDamageString} + {ModifiedDamage.DamageBonus}" : "") + $"\n Stone: {UpgradeStone}";
        }

        public void ChangeAcriStones(DamageSet.AcriStone stone)
        {
            UpgradeStone = stone;
            ModifiedDamage = BaseDamage.ModifyDamage(stone);
        }
    }
}
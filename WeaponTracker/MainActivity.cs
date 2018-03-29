using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;

namespace WeaponTracker
{
    [Activity(Label = "WeaponTracker", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private List<Weapon> weapons = new List<Weapon>();
        private ListView weaponList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));

            weapons[0].ChangeAcriStones(DamageSet.AcriStone.None);
            weapons[1].ChangeAcriStones(DamageSet.AcriStone.Green);
            weapons[2].ChangeAcriStones(DamageSet.AcriStone.Yellow);
            weapons[3].ChangeAcriStones(DamageSet.AcriStone.Orange);
            weapons[4].ChangeAcriStones(DamageSet.AcriStone.Red);
            weapons[5].ChangeAcriStones(DamageSet.AcriStone.Blue);
            weapons[6].ChangeAcriStones(DamageSet.AcriStone.Purple);
            weapons[7].ChangeAcriStones(DamageSet.AcriStone.Black);
            weapons[8].ChangeAcriStones(DamageSet.AcriStone.White);

            weaponList = FindViewById<ListView>(Resource.Id.WeaponListView);
            var viewAdapter = new WeaponListViewAdapter(this, weapons);
            weaponList.Adapter = viewAdapter;

            HandleEvents();
        }

        private void HandleEvents()
        {
            weaponList.ItemLongClick += WeaponList_ItemLongClick;
        }

        private void WeaponList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var intent = new Intent(this, typeof(CreateWeaponActivity));
            intent.PutExtra("WeaponPosition", e.Position);

            StartActivityForResult(intent, 100);
        }
    }
}


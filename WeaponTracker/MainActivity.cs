using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using Newtonsoft.Json;

namespace WeaponTracker
{
    [Activity(Label = "WeaponTracker", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private List<Weapon> Weapons = new List<Weapon>();
        private ListView WeaponListView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));
            Weapons.Add(new Weapon(new DamageSet(new List<DamageSet.Dice> { DamageSet.Dice.d10 }, 2), "Halberd"));

            Weapons[0].ChangeAcriStones(DamageSet.AcriStone.None);
            Weapons[1].ChangeAcriStones(DamageSet.AcriStone.Green);
            Weapons[2].ChangeAcriStones(DamageSet.AcriStone.Yellow);
            Weapons[3].ChangeAcriStones(DamageSet.AcriStone.Orange);
            Weapons[4].ChangeAcriStones(DamageSet.AcriStone.Red);
            Weapons[5].ChangeAcriStones(DamageSet.AcriStone.Blue);
            Weapons[6].ChangeAcriStones(DamageSet.AcriStone.Purple);
            Weapons[7].ChangeAcriStones(DamageSet.AcriStone.Black);
            Weapons[8].ChangeAcriStones(DamageSet.AcriStone.White);

            WeaponListView = FindViewById<ListView>(Resource.Id.WeaponListView);
            var viewAdapter = new WeaponListViewAdapter(this, Weapons);
            WeaponListView.Adapter = viewAdapter;

            HandleEvents();
        }

        private void HandleEvents()
        {
            WeaponListView.ItemLongClick += WeaponList_ItemLongClick;
        }

        private void WeaponList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var intent = new Intent(this, typeof(CreateWeaponActivity));
            intent.PutExtra("WeaponIndex", e.Position);
            intent.PutExtra("WeaponName", Weapons[e.Position].WeaponName);
            intent.PutExtra("BaseDamage", JsonConvert.SerializeObject(Weapons[e.Position].BaseDamage));
            intent.PutExtra("Stone", JsonConvert.SerializeObject(Weapons[e.Position].UpgradeStone));

            StartActivityForResult(intent, 100);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == 100 && resultCode == Result.Ok)
            {
                var changedWeaponIndex = data.Extras.GetInt("SelectedWeaponIndex");
                var changedWeaponName = data.Extras.GetString("ResultWeaponName");
                var changedWeaponDamageSet = new DamageSet(JsonConvert.DeserializeObject<List<DamageSet.Dice>>(data.Extras.GetString("ResultDamageDice")), data.Extras.GetInt("ResultDamageModifier"));
                var changedAcriStone = JsonConvert.DeserializeObject<DamageSet.AcriStone>(data.Extras.GetString("ResultAcriStone"));

                Weapons[changedWeaponIndex] = new Weapon(changedWeaponDamageSet, changedWeaponName);
                Weapons[changedWeaponIndex].ChangeAcriStones(changedAcriStone);
            }
        }
    }
}


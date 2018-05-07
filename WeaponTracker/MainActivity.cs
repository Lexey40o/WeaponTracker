using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using Newtonsoft.Json;

namespace WeaponTracker
{
    [Activity(Label = "Weapon List", Theme = "@android:style/Theme.Material")]
    public class MainActivity : Activity
    {
        private List<Weapon> Weapons = new List<Weapon>();
        private ListView WeaponListView;
        private WeaponListViewAdapter ViewAdapter;
        private Button NewWeaponButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            // Restore state or load from preferences.
            if (savedInstanceState != null)
            {
                var deserializedWeapons = JsonConvert.DeserializeObject<List<Weapon>>(savedInstanceState.GetString("Weapons"));
                Weapons = deserializedWeapons.Count > 0 ? deserializedWeapons : Weapons;
            }
            else if(GetPreferences(FileCreationMode.Private).Contains("WeaponList"))
            {
                Weapons = JsonConvert.DeserializeObject<List<Weapon>>(GetPreferences(FileCreationMode.Private).GetString("WeaponList", JsonConvert.SerializeObject(new List<Weapon>())));
            }

            // Create a weaponlistview with an adapter.
            WeaponListView = FindViewById<ListView>(Resource.Id.WeaponListView);
            ViewAdapter = new WeaponListViewAdapter(this, Weapons);
            WeaponListView.Adapter = ViewAdapter;

            // Create the button to add a weapon.
            NewWeaponButton = FindViewById<Button>(Resource.Id.NewWeaponButton);

            HandleEvents();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutString("Weapons", JsonConvert.SerializeObject(Weapons));
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Save weaponlist to a preferences string.
            using (var editor = GetPreferences(FileCreationMode.Private).Edit())
            {
                editor.PutString("WeaponList", JsonConvert.SerializeObject(Weapons));
                editor.Commit();
            }
        }

        private void HandleEvents()
        {
            WeaponListView.ItemLongClick += WeaponList_ItemLongClick;
            NewWeaponButton.Click += NewWeaponButton_Click;
        }

        private void NewWeaponButton_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateWeaponActivity));
            intent.PutExtra("NewWeapon", true);

            StartActivityForResult(intent, 101);
        }

        private void WeaponList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var intent = new Intent(this, typeof(CreateWeaponActivity));
            intent.PutExtra("WeaponIndex", e.Position);
            intent.PutExtra("WeaponName", Weapons[e.Position].WeaponName);
            intent.PutExtra("BaseDamage", JsonConvert.SerializeObject(Weapons[e.Position].BaseDamage));
            intent.PutExtra("Stone", DamageSet.EncodeAcriSymbol(Weapons[e.Position].UpgradeStone));

            StartActivityForResult(intent, 100);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(resultCode == Result.Ok)
            {
                var changedWeaponIndex = data.Extras.GetInt("SelectedWeaponIndex");
                var changedWeaponName = data.Extras.GetString("ResultWeaponName");
                var changedWeaponDamageSet = new DamageSet(JsonConvert.DeserializeObject<List<DamageSet.Dice>>(data.Extras.GetString("ResultDamageDice")), data.Extras.GetInt("ResultDamageModifier"));
                var changedAcriStone = DamageSet.DecodeAcriSymbol(data.Extras.GetString("ResultAcriStone"));

                if (requestCode == 100)
                {
                    Weapons[changedWeaponIndex] = new Weapon(changedWeaponDamageSet, changedWeaponName);
                    Weapons[changedWeaponIndex].ChangeAcriStones(changedAcriStone);
                }
                else if(requestCode == 101)
                {
                    Weapons.Add(new Weapon(changedWeaponDamageSet, changedWeaponName));
                    Weapons[Weapons.Count - 1].ChangeAcriStones(changedAcriStone);
                }
            }

            else if(resultCode == Result.FirstUser)
            {
                Weapons.Remove(Weapons[data.Extras.GetInt("SelectedWeaponIndex")]);
            }

            ViewAdapter.NotifyDataSetChanged();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WeaponTracker
{
    [Activity(Label = "CreateWeaponActivity")]
    public class CreateWeaponActivity : Activity
    {
        public static readonly string[] BaseDamageSpinnerSetsLabels = { "1d4", "1d6", "2d6", "1d8", "1d10", "1d12" };
        public static readonly List<DamageSet.Dice>[] BaseDamageSpinnerSets = { new List<DamageSet.Dice> { DamageSet.Dice.d4 }, new List<DamageSet.Dice> { DamageSet.Dice.d6 }, new List<DamageSet.Dice> { DamageSet.Dice.d6, DamageSet.Dice.d6 }, new List<DamageSet.Dice> { DamageSet.Dice.d8 }, new List<DamageSet.Dice> { DamageSet.Dice.d10 }, new List<DamageSet.Dice> { DamageSet.Dice.d12 } };
        public static readonly DamageSet.AcriStone[] StoneSpinnerStones = { DamageSet.AcriStone.None, DamageSet.AcriStone.Green, DamageSet.AcriStone.Yellow, DamageSet.AcriStone.Orange, DamageSet.AcriStone.Red, DamageSet.AcriStone.Blue, DamageSet.AcriStone.Purple, DamageSet.AcriStone.Black, DamageSet.AcriStone.White };

        private TextView WeaponNameTextField;
        private Spinner BaseDamageSpinner, StoneSpinner;
        private NumberPicker DamageModifier;
        private Button ConfirmButton, CancelButton;

        private int SelectedWeaponIndex, ResultDamageModifier;
        private string ResultWeaponName;
        private List<DamageSet.Dice> ResultDamageDice;
        private DamageSet.AcriStone ResultAcriStone;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.CreateWeaponLayout);

            // Get user input items
            WeaponNameTextField = FindViewById<TextView>(Resource.Id.WeaponNameTextField);
            BaseDamageSpinner = FindViewById<Spinner>(Resource.Id.BaseDamageSpinner);
            DamageModifier = FindViewById<NumberPicker>(Resource.Id.DamageModifierNumberPicker);
            StoneSpinner = FindViewById<Spinner>(Resource.Id.StoneSpinner);
            ConfirmButton = FindViewById<Button>(Resource.Id.ConfirmButton);
            CancelButton = FindViewById<Button>(Resource.Id.CancelButton);

            // Assign events to user inputs
            WeaponNameTextField.TextChanged += WeaponNameTextField_TextChanged;
            BaseDamageSpinner.ItemSelected += BaseDamageSpinner_ItemSelected;
            DamageModifier.ValueChanged += DamageModifier_ValueChanged;
            StoneSpinner.ItemSelected += StoneSpinner_ItemSelected;
            ConfirmButton.Click += ConfirmButton_Click;
            CancelButton.Click += CancelButton_Click;

            // Create array adapters for each spinner
            ArrayAdapter<string> BaseDamageSpinnerArrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, BaseDamageSpinnerSetsLabels);
            ArrayAdapter<DamageSet.AcriStone> StoneSpinnerArrayAdapter = new ArrayAdapter<DamageSet.AcriStone>(this, Android.Resource.Layout.SimpleSpinnerItem, StoneSpinnerStones);
            BaseDamageSpinnerArrayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);
            StoneSpinnerArrayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);

            // Set the spinners to have the correct array adapters
            BaseDamageSpinner.Adapter = BaseDamageSpinnerArrayAdapter;
            StoneSpinner.Adapter = StoneSpinnerArrayAdapter;

            // Add numbers to the number picker
            DamageModifier.MinValue = 0;
            DamageModifier.MaxValue = 20;


            // Get selected weapon attributes
            if (!Intent.Extras.IsEmpty)
            {
                SelectedWeaponIndex = Intent.Extras.GetInt("WeaponIndex");
                ResultWeaponName = Intent.Extras.GetString("WeaponName");
                ResultDamageDice = (JsonConvert.DeserializeObject<DamageSet>(Intent.Extras.GetString("BaseDamage")).DamageDice);
                ResultDamageModifier = (JsonConvert.DeserializeObject<DamageSet>(Intent.Extras.GetString("BaseDamage")).DamageBonus);
                ResultAcriStone = JsonConvert.DeserializeObject<DamageSet.AcriStone>(Intent.Extras.GetString("Stone"));

                WeaponNameTextField.Text = ResultWeaponName;
                BaseDamageSpinner.SetSelection(Array.IndexOf(BaseDamageSpinnerSets.ToArray(), ResultDamageDice.ToArray()));
                DamageModifier.Value = ResultDamageModifier;
                StoneSpinner.SetSelection(Array.IndexOf(StoneSpinnerStones, ResultAcriStone));
            }
            else
            {
                WeaponNameTextField.Text = "Unnamed Weapon";
                BaseDamageSpinner.SetSelection(0);
                DamageModifier.Value = 0;
                StoneSpinner.SetSelection(0);
            }
        }

        private void WeaponNameTextField_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            ResultWeaponName = Convert.ToString(e.Text);
        }

        private void BaseDamageSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ResultDamageDice = BaseDamageSpinnerSets[e.Position];
        }

        private void DamageModifier_ValueChanged(object sender, NumberPicker.ValueChangeEventArgs e)
        {
            ResultDamageModifier = e.NewVal;
        }

        private void StoneSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ResultAcriStone = (DamageSet.AcriStone)e.Position;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("SelectedWeaponIndex", SelectedWeaponIndex);
            intent.PutExtra("ResultWeaponName", ResultWeaponName);
            intent.PutExtra("ResultDamageDice", JsonConvert.SerializeObject(ResultDamageDice));
            intent.PutExtra("ResultDamageModifier", ResultDamageModifier);
            intent.PutExtra("ResultAcriStone", JsonConvert.SerializeObject(ResultAcriStone));
            SetResult(Result.Ok, intent);
            Finish();
        }
    }
}
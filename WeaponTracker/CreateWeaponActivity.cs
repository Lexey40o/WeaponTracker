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
using Android.Views.InputMethods;
using Android.Support.V4.Content.Res;

namespace WeaponTracker
{
    [Activity(Label = "Weapon Editor", Theme = "@android:style/Theme.Material")]
    public class CreateWeaponActivity : Activity
    {
        public static readonly string[] BaseDamageSpinnerSetsLabels = { "1d4", "1d6", "2d6", "1d8", "1d10", "1d12" };
        public static readonly List<List<DamageSet.Dice>> BaseDamageSpinnerSets = new List<List<DamageSet.Dice>> { new List<DamageSet.Dice> { DamageSet.Dice.d4 }, new List<DamageSet.Dice> { DamageSet.Dice.d6 }, new List<DamageSet.Dice> { DamageSet.Dice.d6, DamageSet.Dice.d6 }, new List<DamageSet.Dice> { DamageSet.Dice.d8 }, new List<DamageSet.Dice> { DamageSet.Dice.d10 }, new List<DamageSet.Dice> { DamageSet.Dice.d12 } };
        public static readonly DamageSet.AcriStone[] StoneSpinnerStones = { DamageSet.AcriStone.None, DamageSet.AcriStone.Green, DamageSet.AcriStone.Yellow, DamageSet.AcriStone.Orange, DamageSet.AcriStone.Red, DamageSet.AcriStone.Blue, DamageSet.AcriStone.Purple, DamageSet.AcriStone.Black, DamageSet.AcriStone.White };

        private TextView WeaponNameTextField, StoneTextField;
        private Spinner BaseDamageSpinner;
        private NumberPicker DamageModifier;
        private Button ConfirmButton, CancelButton, DeleteButton;

        private int SelectedWeaponIndex, ResultDamageModifier;
        private string ResultWeaponName, ResultAcriStoneCode;
        private List<DamageSet.Dice> ResultDamageDice;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.CreateWeaponLayout);

            // Get user input items.
            WeaponNameTextField = FindViewById<TextView>(Resource.Id.WeaponNameTextField);
            BaseDamageSpinner = FindViewById<Spinner>(Resource.Id.BaseDamageSpinner);
            DamageModifier = FindViewById<NumberPicker>(Resource.Id.DamageModifierNumberPicker);
            StoneTextField = FindViewById<TextView>(Resource.Id.StoneTextField);
            ConfirmButton = FindViewById<Button>(Resource.Id.ConfirmButton);
            CancelButton = FindViewById<Button>(Resource.Id.CancelButton);
            DeleteButton = FindViewById<Button>(Resource.Id.DeleteButton);
            DeleteButton.Visibility = Intent.GetBooleanExtra("NewWeapon", false) ? ViewStates.Gone : ViewStates.Visible;

            // Assign events to user inputs.
            WeaponNameTextField.TextChanged += WeaponNameTextField_TextChanged;
            BaseDamageSpinner.ItemSelected += BaseDamageSpinner_ItemSelected;
            DamageModifier.ValueChanged += DamageModifier_ValueChanged;
            StoneTextField.TextChanged += StoneTextField_TextChanged;
            ConfirmButton.Click += ConfirmButton_Click;
            CancelButton.Click += CancelButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            // Clear focus on text fields when enter button is clicked.
            WeaponNameTextField.EditorAction += TextField_OnDone;
            StoneTextField.EditorAction += TextField_OnDone;

            // Create array adapters for each spinner.
            ArrayAdapter<string> BaseDamageSpinnerArrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, BaseDamageSpinnerSetsLabels);
            ArrayAdapter<DamageSet.AcriStone> StoneSpinnerArrayAdapter = new ArrayAdapter<DamageSet.AcriStone>(this, Android.Resource.Layout.SimpleSpinnerItem, StoneSpinnerStones);
            BaseDamageSpinnerArrayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);

            // Set the spinners to have the correct array adapters.
            BaseDamageSpinner.Adapter = BaseDamageSpinnerArrayAdapter;

            // Add numbers to the number picker.
            DamageModifier.MinValue = 0;
            DamageModifier.MaxValue = 20;

            // Set the stone text field's font.
            var barazhad = ResourcesCompat.GetFont(this, Resource.Font.barazhad);
            StoneTextField.Typeface = barazhad;


            // Get selected weapon attributes.
            if (Intent.Extras.GetString("WeaponName") != null)
            {
                SelectedWeaponIndex = Intent.Extras.GetInt("WeaponIndex");
                ResultWeaponName = Intent.Extras.GetString("WeaponName");
                ResultDamageDice = (JsonConvert.DeserializeObject<DamageSet>(Intent.Extras.GetString("BaseDamage")).DamageDice);
                ResultDamageModifier = (JsonConvert.DeserializeObject<DamageSet>(Intent.Extras.GetString("BaseDamage")).DamageBonus);
                ResultAcriStoneCode = Intent.Extras.GetString("Stone");

                WeaponNameTextField.Text = ResultWeaponName;
                var damageDiceIndex = BaseDamageSpinnerSets.FindIndex(x => JsonConvert.SerializeObject(x) == JsonConvert.SerializeObject(ResultDamageDice));
                BaseDamageSpinner.SetSelection(damageDiceIndex);
                DamageModifier.Value = ResultDamageModifier;
                StoneTextField.Text = ResultAcriStoneCode;
            }
            else
            {
                WeaponNameTextField.Text = "Unnamed Weapon";
                BaseDamageSpinner.SetSelection(0);
                DamageModifier.Value = 0;
                StoneTextField.Text = "";
            }
        }

        private void TextField_OnDone(object sender, TextView.EditorActionEventArgs e)
        {
            var textView = (TextView)sender;
            var imm = (InputMethodManager)GetSystemService(InputMethodService);

            textView.ClearFocus();
            imm.HideSoftInputFromWindow(textView.WindowToken, 0);
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

        private void StoneTextField_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            ResultAcriStoneCode = Convert.ToString(e.Text);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled, new Intent(this, typeof(MainActivity)));
            Finish();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("SelectedWeaponIndex", SelectedWeaponIndex);
            intent.PutExtra("ResultWeaponName", ResultWeaponName);
            intent.PutExtra("ResultDamageDice", JsonConvert.SerializeObject(ResultDamageDice));
            intent.PutExtra("ResultDamageModifier", ResultDamageModifier);
            intent.PutExtra("ResultAcriStone", ResultAcriStoneCode);
            SetResult(Result.Ok, intent);
            Finish();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("SelectedWeaponIndex", SelectedWeaponIndex);
            SetResult(Result.FirstUser, intent);
            Finish();
        }
    }
}
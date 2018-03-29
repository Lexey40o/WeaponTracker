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
    [Activity(Label = "CreateWeaponActivity")]
    public class CreateWeaponActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.CreateWeaponLayout);

            var selectedWeaponPosition = Intent.Extras.GetInt("WeaponPosition");

            var weaponPositionText = FindViewById<TextView>(Resource.Id.WeaponPositionTextView);

            weaponPositionText.Text = Convert.ToString(selectedWeaponPosition);
        }
    }
}
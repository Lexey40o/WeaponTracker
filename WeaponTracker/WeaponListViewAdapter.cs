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
    class WeaponListViewAdapter : BaseAdapter<Weapon>
    {
        private Context Context;
        private List<Weapon> Weapons;

        public WeaponListViewAdapter(Context context, List<Weapon> items)
        {
            Context = context;
            Weapons = items;
        }
        public override int Count
        {
            get { return Weapons.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Weapon this[int position]
        {
            get { return Weapons[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(Context).Inflate(Resource.Layout.listview_weaponrow, null, false);
            }

            TextView weaponName = row.FindViewById<TextView>(Resource.Id.WeaponNameTextView);
            TextView weaponStats = row.FindViewById<TextView>(Resource.Id.WeaponStatsTextView);

            weaponName.Text = Weapons[position].WeaponName;
            weaponStats.Text = Weapons[position].GetWeaponStats();

            //row.LongClick += Row_LongClick;

            return row;
        }

        //private void Row_LongClick(object sender, View.LongClickEventArgs e)
        //{
            
        //}
    }
}
using Android.Content;
using System;
using Swibbl.Services;
using Swibbl.Models;
using Xamarin.Forms;

namespace Swibbl.Droid.Services
{
    public class Payment : IPayment
    {
        [Obsolete]
        public void Pay(string am,string apk)
        {
            var intent = new Intent(Forms.Context,typeof(PaymentActivity)).PutExtra("Am", am).PutExtra("Apk",apk);
            Forms.Context.StartActivity(intent);
        }
    }
}
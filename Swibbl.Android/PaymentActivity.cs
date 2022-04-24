using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Swibbl.Models;
using Swibbl.Services;
using Newtonsoft.Json;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;

namespace Swibbl.Droid
{
    [Activity(Label = "PaymentActivity")]
    public class PaymentActivity : Activity
    {
        int pay_result = 404;
        string am, apk;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var data = Intent.GetExtra<Upi>("PaymentInfo");
            data.Am = am;
            data.Apk = apk;

            PayViaUpi();
        }
        private void PayViaUpi()
        {
            if (IsGooglePayInstalled())
            {
                var uri = new Android.Net.Uri.Builder()
                    .Scheme("upi")
                    .Authority("pay")
                    .AppendQueryParameter("pa", "swibblopc@oksbi")
                    .AppendQueryParameter("pn", "Swibbl (OPC) Private Limited")
                    .AppendQueryParameter("pm", "Paying ₹" + am + "to Swibbl")
                    .AppendQueryParameter("mc", "0000")
                    .AppendQueryParameter("tr", Guid.NewGuid().ToString())
                    .AppendQueryParameter("tn", "Pay to Swibbl")
                    .AppendQueryParameter("am", am)
                    .AppendQueryParameter("cu", "INR")
                    .Build();

                Intent = new Intent(Intent.ActionView);
                Intent = Intent.SetData(uri);
                Intent = Intent.SetPackage(apk);

                StartActivityForResult(Intent, pay_result);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if(requestCode == pay_result)
            {
                if(data != null)
                {
                    string response = Intent.GetStringExtra("response");
                    string[] resArray = response.Split("&");
                    string txnId = resArray[0];
                    string responseCode = resArray[0];
                    string status = resArray[0].ToString();
                    string txnRef = resArray[0];
                    MessagingCenter.Send(this, "Response", LayoutState.Success);
                }
                else
                {
                    MessagingCenter.Send(this, "Response", LayoutState.Error);
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        private bool IsGooglePayInstalled()
        {
            PackageManager pm = PackageManager;
            bool installed = false;
            try
            {
                pm.GetPackageInfo(apk, PackageInfoFlags.Activities);
                installed = true;
            }
            catch (Exception)
            {

            }
            return installed;
        }

    }
}
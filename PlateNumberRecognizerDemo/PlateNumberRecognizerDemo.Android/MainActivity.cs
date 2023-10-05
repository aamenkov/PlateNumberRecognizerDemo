using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using OpenALPR_Xamarin.Android_Library;
using Android.Widget;
using OpenALPR_Xamarin.Android_Library.Models;

namespace PlateNumberRecognizerDemo.Droid
{
    [Activity(Label = "PlateNumberRecognizerDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity :  global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public string AndroidDataDir;
        public string OpenALPRConfigFile;
        public string OpenALPRRuntimeFolder;
        public string TestImagePath;
        public OpenALPR OpenALPRInstance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


            AndroidDataDir = ApplicationInfo.DataDir;
            OpenALPRConfigFile = AndroidDataDir + "/runtime_data/openalpr.conf";
            OpenALPRRuntimeFolder = AndroidDataDir + "/runtime_data";
            TestImagePath = AndroidDataDir + "/runtime_data/image.jpg";

            OpenALPRInstance = new OpenALPR(this, AndroidDataDir, OpenALPRConfigFile, "eu");

            LoadApplication(new App());
            Test();
        }

        private void Test()
        {
            try
            {
                string output = "";
                //TextView licensePlateTV = FindViewById<TextView>(Resource.Id.licensePlateTextview);

                OpenALPR_Results results = OpenALPRInstance.Recognize(TestImagePath);

                if (results.DidErrorOccur == false)
                {
                    foreach (var rr in results.FoundLicensePlates)
                    {
                        output += "Best: " + rr.BestLicensePlate + "(" + rr.Confidence + "%)\n";
                        foreach (var rc in rr.OtherCandidates)
                        {
                            output += "- " + rc.LicensePlate + "(" + rc.Confidence + "%)\n";
                        }
                    }
                }
                else
                {
                    output = results.Error.Message + ", " + results.Error.Stacktrace;
                }

                //licensePlateTV.Text = output;
               // Toast.MakeText(this, output, ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                //Toast.MakeText(this, ex.Message + ", " + ex.StackTrace, ToastLength.Long).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json;

//https://developer.xamarin.com/recipes/android/web_services/consuming_services/call_a_rest_web_service/
namespace ApiCaller
{
    [Activity(Label = "ApiCaller", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _button1;
        private TextView _textView1;
        private int _numberOfClicks;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            initFields();
        }

        private void initFields()
        {
            _button1 = FindViewById<Button>(Resource.Id.button1);
            _button1.Click += async (sender, e) => {

                string url = "http://192.168.42.76/api/Reservation/SayHello";
                JsonValue json;
                try
                {
                    json = await FetchWeatherAsync(url);
                    if (json != null)
                    {
                        string data = ParseInputData(json);
                        showData(data);
                    }
                }
                catch (Exception ex)
                {
                    WebException exception=new WebException();
                    
                    showData(ex.Message);
                }
            };
            _textView1 = FindViewById<TextView>(Resource.Id.textView1);
        }

        private void showData(string data)
        {
            _textView1.Text = data;
        }

        private string ParseInputData(JsonValue json)
        {
           return JsonConvert.DeserializeObject<string>(json.ToString());
        }

        private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            //request.Headers.Add(HttpRequestHeader.Cookie,"MyToken");

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader=new StreamReader(stream);
                    string res = reader.ReadToEnd();
                    
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                        Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                        // Return the JSON document:
                        return jsonDoc;
                }
            }
        }
    }
}


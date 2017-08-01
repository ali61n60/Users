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
        private Button _buttonGetToken;
        private TextView _textView1;
        private int _numberOfClicks;
        private Token _myToken;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            initFields();
        }

        private void initFields()
        {
            _buttonGetToken = FindViewById<Button>(Resource.Id.buttonGetToken);
            _buttonGetToken.Click += async (sender, args) =>
            {
                string url = "http://192.168.42.76/api/AccountApi/token";
                
                JsonValue json;
                try
                {
                    HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(new Uri(url));
                    request.ContentType = "application/json";
                    request.Method = "POST";

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string jsonData = "{\"Email\":\"bob@example.com\"," +
                                          "\"Password\":\"secret123\"}";

                        streamWriter.Write(jsonData);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }




                    // Send the request to the server and wait for the response:
                    using (WebResponse response = await request.GetResponseAsync())
                    {
                        // Get a stream representation of the HTTP web response:
                        using (Stream stream = response.GetResponseStream())
                        {
                            // Use this stream to build a JSON document object:
                            JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                            Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                             _myToken = JsonConvert.DeserializeObject<Token>(jsonDoc.ToString());
                            showData(_myToken.token);
                        }
                    }
                }
                catch (Exception ex)
                {
                    showData(ex.Message);
                }
            };



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
            //string n1 = ".AspNetCore.Identity.Application";
            //string n2 = ".AspNetCore.Antiforgery.8tWQedTd03A";
            //string n1Val = "CfDJ8A5SbqnuW9VIhrFstqlNZDXmY3fctMXxOGP-66GEJYFHJ5mtZ5lk7LFi0Swe-vQ5VcTlxgodlDJZlCABVQaZ_leEK1WiT6yBcmrswD_Ah4zQcn-ki0Wj8i0kKTB4idweYXjTBtm4uGJN9N4r3Kdi7yxA_l5MDzHkZiT1S3AYq3p5_nrFqu1t5j4bb2tSOFfyOB08jCBwQaWUCTV-O5J6T59tZMyXyKpZ6KX85akZJXtSeOttz9m3EMIqWPraqmj8qqKDlkIYLDD6kp1uWlTrza5qZYFrRZPwLuSijeDu0FRt9xyTFu4SRXHXOv1ZNC5C1Nk2Z70wNksr1RWlt1C7obNp_lFY5GCE73noKXE8ql4-uCgMIqJdGTW23iAGAhy2qrrQLtxXHmVvZK0LtNOGtuDtaNwK5_eGl19g8b7gbFY8-G3nP71I2euOMn1a8lxpEaqhv8m3CAIPKVkfVvwaGPKJUFHk9JHpUbUi3c36iGcmcGOtAL6uVhVB07jNs67cl1k1EcLVUd6yOa8UAhOeBiH6t6fvb2eZ0RzZ3zNqYlyUSFCYaHyk6Dx85GkFSYti2bM9-Ampq8WZ86mKCvu5F7g";
            //string n2val = "CfDJ8A5SbqnuW9VIhrFstqlNZDXFeSA8eiyUIaTi_mAy3tZxlyNk8He4EKCuvgNlURYxLVu643-Z2fJxID03lWjqliL7wfHOePVZBUZdsg0IUqSwAx2fgsFZRq9i7vwYlOUq2VlZH4l7uxYEoabQLXJrmn0";

            //request.Headers.Add(HttpRequestHeader.Cookie, String.Format("{0}={1};{2}={3}",n1,n1Val,n2,n2val));
            request.Headers.Add(HttpRequestHeader.Authorization, "bearer "+_myToken.token);
            
            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
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


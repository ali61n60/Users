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
using Model;
using Newtonsoft.Json;

//https://developer.xamarin.com/recipes/android/web_services/consuming_services/call_a_rest_web_service/
namespace ApiCaller
{
    [Activity(Label = "ApiCaller", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _button1;
        private Button _buttonGetToken;
        private Button _buttonHelloParam;
        private TextView _textView1;
        private int _numberOfClicks;
        private Token _myToken;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
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
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
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
            _button1.Click += async (sender, e) =>
            {

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

            _buttonHelloParam = FindViewById<Button>(Resource.Id.buttonHelloParam);
            _buttonHelloParam.Click += async (sender, args) =>
             {
                 string url = "http://192.168.42.76/api/Reservation/SayHelloWithParameter";
                 HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                 request.ContentType = "application/json";
                 request.Method = "POST";
                 request.Headers.Add(HttpRequestHeader.Authorization, "bearer " + _myToken.token);
                 try
                 {
                     using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                     {
                         MethodParam myMethodParam=new MethodParam(){Name = "Ali Nejati", Phone = "0912"};
                         //string jsonData = "{\"Name\":\"Ali Nejati\"," +
                           //               "\"Phone\":\"09122012908\"}";
                         string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(myMethodParam);

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
                             string result = JsonConvert.DeserializeObject<string>(jsonDoc.ToString());
                             showData(result);
                         }
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
            request.Headers.Add(HttpRequestHeader.Authorization, "bearer " + _myToken.token);

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


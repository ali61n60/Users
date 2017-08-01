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

namespace ApiCaller
{
    public class Token
    {
        public string token { get; set; }
        public string expiration { get; set; }
    }
}
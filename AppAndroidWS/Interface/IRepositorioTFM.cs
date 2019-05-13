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

using une_etp.TapsFM;
using une_etp.REST;


namespace une_etp.Interface
{
    interface IRepositorioTFM
    {
        void GetAllTaps();
        bool CreateDataBase();
    }
}
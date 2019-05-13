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

namespace une_etp.REST
{
    public class InsertarTap
    {

        public int IdTap { get; set; }

        public string Tipo { get; set; }

        public string SubTipoAC { get; set; }

        public string SubtipoNEW { get; set; }

        public int CodFAC { get; set; }

        public int CodFNEW { get; set; }

        public string MACT { get; set; }

        public string MNEW { get; set; }

        public string usuario { get; set; }

    }
}
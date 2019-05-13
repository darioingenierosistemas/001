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

    public class TipoActual
    {
        public string SUBTYPE { get; set; }
    }

    public class CodFabricanteActual
    {
        public int COD_FABRICANTE { get; set; }
        public string MOD_ELEMENTO { get; set; }
    }

    public class FabricanteActual
    {
        public string DESCRIPCION { get; set; }
    }

  
}
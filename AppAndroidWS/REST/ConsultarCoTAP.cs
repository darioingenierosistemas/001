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

    public class ConsultarCoTAP
    {
        private string idTap;
        private int bOCA;
        private string eSTADO;
        private string uSUARIO;
        private string dIRECCION;
        private string pLAN;



        public int BOCA { get => bOCA; set => bOCA = value; }
        public string ESTADO { get => eSTADO; set => eSTADO = value; }
        public string USUARIO { get => uSUARIO; set => uSUARIO = value; }
        public string DIRECCION { get => dIRECCION; set => dIRECCION = value; }
        public string PLAN { get => pLAN; set => pLAN = value; }
        public string IdTap { get => idTap; set => idTap = value; }
    }
}
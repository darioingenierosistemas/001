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
    public class ConsultarNapTecnico
    {
        private int iD;
        private string dIRECCION;
        private int nAPS;
        private string rESERVA;
        private int aRPON;
        private string mODELO_DIV;

        public int ID { get => iD; set => iD = value; }
        public string DIRECCION { get => dIRECCION; set => dIRECCION = value; }
        public int NAPS { get => nAPS; set => nAPS = value; }
        public string RESERVA { get => rESERVA; set => rESERVA = value; }
        public int ARPON { get => aRPON; set => aRPON = value; }
        public string MODELO_DIV { get => mODELO_DIV; set => mODELO_DIV = value; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using une_etp.REST;
using SQLite;
using Android.Util;
using une_etp.Activities;

namespace une_etp.FragmentsActivities
{
    [Activity(Label = "Consultar Tap")]
    [MetaData("android.support.PARENT_ACTIVITY",
        Value = "une_etp.Activities.MapActivity")]
    public class ConsultarTap : AppCompatActivity
    {
        private string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private ProgressDialog mProgress;
        private ArrayAdapter<string> adapter;
        private ListView ListTapInfo;
        private List<string> lista;
        private List<ConsultarCoTAP> NewCoTAP;
        public  string IDCOTAP;
        private List<TapsInfoGet> NewTap;
        private Button btnActualizarTapConsulta;
        public static string IDTAP;
        private static int H;
        private static double W;
        private string tapByMap = string.Empty;
        EditText Sig;
        private double x;
        private double y;
        private int reference;
        private string tipo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
     
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Fragment_ConsultarTap);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            Sig = FindViewById<EditText>(Resource.Id.edtTapSIG);
            Button btnConsultarTap = FindViewById<Button>(Resource.Id.btnConsultarTap);
            btnActualizarTapConsulta = FindViewById<Button>(Resource.Id.btnActualizarTapConsulta);
            Button btnConsultarComercial = FindViewById<Button>(Resource.Id.btnConsultarComercial);

            btnConsultarComercial.Click += BtnConsultarComercial_Click;
            btnConsultarTap.Click += BtnConsultarTap_Click;

            btnActualizarTapConsulta.Click += BtnActualizarTapConsulta_Click;
            tapByMap = Intent.GetStringExtra("codigoTag");
            x = Intent.GetDoubleExtra("x",0);
            y = Intent.GetDoubleExtra("y",0);
            tipo = Intent.GetStringExtra("tipo");
            reference = Intent.GetIntExtra("reference", 0);
            if ( null != tapByMap && !tapByMap.Equals(string.Empty))
            {
                Sig.Text = tapByMap;
            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            var intent = new Intent(this, typeof(MapActivity));
            intent.PutExtra("x",x);
            intent.PutExtra("y",y);
            intent.PutExtra("reference", reference);
            StartActivity(intent);
            return true;
        }


        private async void BtnConsultarComercial_Click(object sender, EventArgs e)
        {
            mProgress = new ProgressDialog(this);
            mProgress.SetCancelable(true);
            mProgress.SetMessage("Consultando.....");
            mProgress.SetProgressStyle(ProgressDialogStyle.Spinner);
            mProgress.Progress = 0;
            mProgress.Max = 100;

            mProgress.Show();

            NewCoTAP = new List<ConsultarCoTAP>();
            ListTapInfo = FindViewById<ListView>(Resource.Id.ListTapInfo);

            string url = "http://201.236.222.217:5154/API/etp_app/taps/OcupacionTap/?IdTap=" + tapByMap;

            var ListaCopTAP = await url.GetRequest<List<ConsultarCoTAP>>();
            NewCoTAP = ListaCopTAP;

            lista = new List<string>();

            if (NewCoTAP != null)
            {

                foreach (var data in NewCoTAP)
                {
                    if (!string.IsNullOrEmpty(data.BOCA.ToString()))
                    {
                        lista.Add("  Boca: " + data.BOCA.ToString());
                    }
                    else
                    {
                        lista.Add("  Boca: NO TIENE");
                    }

                    if (!string.IsNullOrEmpty(data.ESTADO))
                    {
                        lista.Add("  Estado: " + data.ESTADO.ToString());
                    }
                    else
                    {
                        lista.Add("  Estado: NO TIENE");
                    }

                    if (!string.IsNullOrEmpty(data.USUARIO))
                    { 
                        lista.Add("  Usuario: " + data.USUARIO.ToString());
                    }
                    else
                    {
                        lista.Add("  Usuario: NO TIENE");
                    }

                    if (!string.IsNullOrEmpty(data.DIRECCION))
                    {
                        lista.Add("  Direccion: " + data.DIRECCION.ToString());
                    }
                    else
                    {
                        lista.Add("  Direccion: NO TIENE");
                    }

                    if (!string.IsNullOrEmpty(data.PLAN))
                    {
                        lista.Add("  Plan: " + data.PLAN.ToString());
                    }
                    else
                    {
                        lista.Add("  Plan: NO TIENE");
                    }
                    lista.Add("");

                }

                adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.TestListItem, lista);
                ListTapInfo.SetMinimumHeight(H);
                ListTapInfo.Adapter = adapter;
                ListTapInfo.SetMinimumHeight(3);
                ListTapInfo.Visibility = 0;
                btnActualizarTapConsulta.Visibility = 0;

                mProgress.Dismiss();

            }
            else
            {
                mProgress.Dismiss();
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                alertDiag.SetTitle("ERROR");
                alertDiag.SetMessage("NO HAY INFORMACION COMERCIAL");
                alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
                {
                    //  StartActivity(typeof(ConsultarTap));
                    //  SetContentView(Resource.Layout.Fragment_ConsultarTap);
                });

                Dialog diag = alertDiag.Create();
                diag.Show();

            }

        }


        private void BtnActualizarTapConsulta_Click(object sender, EventArgs e)
        {
            mProgress = new ProgressDialog(this);
            mProgress.SetCancelable(true);
            mProgress.SetMessage("Consultando.....");
            mProgress.SetProgressStyle(ProgressDialogStyle.Spinner);
            mProgress.Progress = 0;
            mProgress.Max = 100;

            mProgress.Show();
            VerificarSyncTipos();
            VerificarSyncFabricacion();
            VerificarSyncModelos();
            mProgress.Dismiss();

            if (VerificarSyncTipos() == false || VerificarSyncFabricacion() == false || VerificarSyncModelos()==false)
            {
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                alertDiag.SetTitle("Sincronizacion");
                alertDiag.SetMessage("Debe volver a la pantalla de INICIO DE SESION y SINCRONIZAR la Aplicacion para poder continuar");
                alertDiag.SetPositiveButton("OK", (senderAlert, args) => {
                  //  StartActivity(typeof(ConsultarTap));
                    //SetContentView(Resource.Layout.Fragment_ConsultarTap);
                });

                Dialog diag = alertDiag.Create();
                diag.Show();

            }
            else
            {
                var intent = new Intent(this, typeof(ActualizarTap));
                intent.PutExtra("x", x);
                intent.PutExtra("y", y);
                intent.PutExtra("reference", reference);
                StartActivity(intent);

            }
        }

        private void BtnConsultarTap_Click(object sender, EventArgs e)
        {
            
            EditText OSF = FindViewById<EditText>(Resource.Id.edtTapOSF);
            ListTapInfo = FindViewById<ListView>(Resource.Id.ListTapInfo);
           

            if (!string.IsNullOrEmpty(Sig.Text.ToString()))
           {
                ListTapInfo.Visibility = 0;
                btnActualizarTapConsulta.Enabled = true;
                GetTapInfoRESTSig();
       
           }
           else if (!string.IsNullOrEmpty(OSF.Text.ToString()))
           {
                ListTapInfo.Visibility = 0;
                btnActualizarTapConsulta.Enabled = true;
                GetTapInfoRESTOSF();
    
            }
           else if (string.IsNullOrEmpty(OSF.Text.ToString()) && string.IsNullOrEmpty(OSF.Text.ToString()))
           {
                
                Toast.MakeText(this, "Solo debe llenar uno de los campos ", ToastLength.Long).Show();
           }
           else
           {
             
                Toast.MakeText(this, "Debe ingresar un IDENTIFICADOR SIG o un CODIGO OSF", ToastLength.Long).Show();
           }
     
        }

        private async void GetTapInfoRESTSig()
        {
            try
            { 
            mProgress = new ProgressDialog(this);
            mProgress.SetCancelable(true);
            mProgress.SetMessage("Consultando.....");
            mProgress.SetProgressStyle(ProgressDialogStyle.Spinner);
            mProgress.Progress = 0;
            mProgress.Max = 100;

            mProgress.Show();

            NewTap = new List<TapsInfoGet>();
            btnActualizarTapConsulta = FindViewById<Button>(Resource.Id.btnActualizarTapConsulta);
            ListTapInfo = FindViewById<ListView>(Resource.Id.ListTapInfo);
            EditText Sig = FindViewById<EditText>(Resource.Id.edtTapSIG);
            string url = "http://201.236.222.217:5154/API/etp_app/taps/BuscarSIG/";
            string Sig2 = Sig.Text.ToString().Trim();
            string URLSIG = url + "?CodigoSIG=" + Sig2;
            var GetTapInfo = await URLSIG.GetRequest<List<TapsInfoGet>>();
            NewTap = GetTapInfo;

            if (NewTap != null)
            {
                foreach (var data in NewTap)
                {

                    lista = new List<string>();
                    IDTAP = data.TAP.ToString();
                    lista.Add("  Identificador SIG: " + data.TAP.ToString());
                    lista.Add("  Codigo OSF: " + data.OSF.ToString());

                        if (data.DIRECCION != null)
                        {
                            lista.Add("  Dirección: " + data.DIRECCION.ToString());
                        }
                        else
                        {
                            lista.Add("  Dirección: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(data.ATENUACION.ToString()))
                        {
                            lista.Add("  Atenuacion: " + data.ATENUACION.ToString());
                        }
                        else
                        {
                            lista.Add("  Atenuacion: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(data.NUMERO_BOCAS))
                        {
                            lista.Add("  Numero de Bocas: " + data.NUMERO_BOCAS.ToString());
                        }
                        else
                        {
                            lista.Add("  Numero de Bocas: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(data.ESTADO_OPERATIVO.ToString()))
                        {
                            lista.Add("  Estado Operativo: " + data.ESTADO_OPERATIVO.ToString());
                        }
                        else
                        {
                            lista.Add("  Estado Operativo: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(data.MODELO))
                        {
                            lista.Add("  Modelo: " + data.MODELO.ToString());
                        }
                        else
                        {
                            lista.Add("  Modelo: NO TIENE");
                        }
                    //if (data.FORWARD_ALTO == 0)
                    //{
                    //    lista.Add("Nivel de Salida ALTO: NO TIENE");
                    //}
                    //else
                    //{
                    lista.Add("  FORWARD ALTO: " + data.FORWARD_ALTO.ToString());
                    //}

                    //if (data.FORWARD_BAJO == 0)
                    //    {
                    //        lista.Add("Nivel de Salida BAJO: NO TIENE");
                    //    }
                    //    else
                    //    {
                    lista.Add("  FORWARD BAJO: " + data.FORWARD_BAJO.ToString());
                    //}

                    lista.Add("  REVERSE ALTO: " + data.REVERSE_ALTO);

                    lista.Add("  REVERSE BAJO: " + data.REVERSE_BAJO);

                    lista.Add("  FORWARD ALTO BOCA: " + data.FORWARD_ALTO_BOCA);

                    lista.Add("  FORWARD BAJO BOCA: " + data.FORWARD_BAJO_BOCA);

                    lista.Add("  REVERSE ALTO BOCA: " + data.REVERSE_ALTO_BOCA);

                    lista.Add("  REVERSE BAJO BOCA: " + data.REVERSE_BAJO_BOCA);

                    if (data.TIPO_PLUG_IN == null)
                    {
                        lista.Add("  Tipo de Plug IN: NO TIENE");
                    }
                    else
                    {
                        lista.Add("  Tipo de Plug IN: " + data.TIPO_PLUG_IN.ToString());
                    }

                    if (data.MODLEO_PLUG_IN == null)
                    {
                        lista.Add("  Modelo de Plug IN: NO TIENE");
                    }
                    else
                    {
                        lista.Add("  Modelo de Plug IN: " + data.MODLEO_PLUG_IN.ToString());
                    }
                }

                adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.TestListItem, lista);
                ListTapInfo.SetMinimumHeight(H);
                ListTapInfo.Adapter = adapter;
                ListTapInfo.SetMinimumHeight(3);
                btnActualizarTapConsulta.Visibility = 0;
                btnActualizarTapConsulta.Enabled = true;

                mProgress.Dismiss();
            }
            else
            {
                mProgress.Dismiss();
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                alertDiag.SetTitle("ERROR");
                alertDiag.SetMessage("NO TIENE INFORMACION TECNICA");
                alertDiag.SetPositiveButton("OK", (senderAlert, args) => {
                  //  StartActivity(typeof(ConsultarTap));
                  //  SetContentView(Resource.Layout.Fragment_ConsultarTap);
                });
      
                Dialog diag = alertDiag.Create();
                diag.Show();

            }
            }
            catch(Exception EX)
            {
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                alertDiag.SetTitle("ERROR");
                alertDiag.SetMessage("ERROR");
                alertDiag.SetPositiveButton("OK", (senderAlert, args) => {
                    //    StartActivity(typeof(ConsultarTap));
                    //  SetContentView(Resource.Layout.Fragment_ConsultarTap);
                });

                Dialog diag = alertDiag.Create();
                diag.Show();
            }
        }

        private async void GetTapInfoRESTOSF()
        {
            try
            {
                mProgress = new ProgressDialog(this);
                mProgress.SetCancelable(true);
                mProgress.SetMessage("Consultando.....");
                mProgress.SetProgressStyle(ProgressDialogStyle.Spinner);
                mProgress.Progress = 0;
                mProgress.Max = 100;

                mProgress.Show();
                btnActualizarTapConsulta = FindViewById<Button>(Resource.Id.btnActualizarTapConsulta);
                ListTapInfo = FindViewById<ListView>(Resource.Id.ListTapInfo);
                EditText OSF = FindViewById<EditText>(Resource.Id.edtTapOSF);
                string url = "http://201.236.222.217:5154/API/etp_app/taps/BuscarOSF/";
                string OSF2 = OSF.Text.ToString().ToUpper().Trim();
                string URLSIG = url + "?CodigoOSF=" + OSF2;
                var GetTapInfo = await URLSIG.GetRequest<List<TapsInfoGet>>();
                NewTap = GetTapInfo;

                if (NewTap != null)
                {
                    foreach (var data in NewTap)
                    {

                        lista = new List<string>();
                        IDTAP = data.TAP.ToString();
                        lista.Add("  Identificador SIG: " + data.TAP.ToString());
                        lista.Add("  Codigo OSF: " + data.OSF.ToString());

                        if (data.DIRECCION != null)
                        {
                            lista.Add("  Dirección: " + data.DIRECCION.ToString());
                        }
                        else
                        {
                            lista.Add("  Dirección: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(data.ATENUACION.ToString()))
                        {
                            lista.Add("  Atenuacion: " + data.ATENUACION.ToString());
                        }
                        else
                        {
                            lista.Add("  Atenuacion: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(data.NUMERO_BOCAS))
                        {
                            lista.Add("  Numero de Bocas: " + data.NUMERO_BOCAS.ToString());
                        }
                        else
                        {
                            lista.Add("  Numero de Bocas: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(data.ESTADO_OPERATIVO.ToString()))
                        {
                            lista.Add("  Estado Operativo: " + data.ESTADO_OPERATIVO.ToString());
                        }
                        else
                        {
                            lista.Add("  Estado Operativo: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(data.MODELO))
                        {
                            lista.Add("  Modelo: " + data.MODELO.ToString());
                        }
                        else
                        {
                            lista.Add("  Modelo: NO TIENE");
                        }

                        //if (data.FORWARD_ALTO == 0)
                        //{
                        //    lista.Add("Nivel de Salida ALTO: NO TIENE");
                        //}
                        //else
                        //{
                        lista.Add("  ENT FORWARD ALTO: " + data.FORWARD_ALTO.ToString());
                        //}

                        //if (data.FORWARD_BAJO == 0)
                        //    {
                        //        lista.Add("Nivel de Salida BAJO: NO TIENE");
                        //    }
                        //    else
                        //    {
                        lista.Add("  ENT FORWARD BAJO: " + data.FORWARD_BAJO.ToString());
                        //}

                        lista.Add("  REVERSE ALTO: " + data.REVERSE_ALTO);

                        lista.Add("  REVERSE BAJO: " + data.REVERSE_BAJO);

                        lista.Add("  FORWARD ALTO BOCA: " + data.FORWARD_ALTO_BOCA);

                        lista.Add("  FORWARD BAJO BOCA: " + data.FORWARD_BAJO_BOCA);

                        lista.Add("  REVERSE ALTO BOCA: " + data.REVERSE_ALTO_BOCA);

                        lista.Add("  REVERSE BAJO BOCA: " + data.REVERSE_BAJO_BOCA);

                        if (data.TIPO_PLUG_IN == null)
                        {
                            lista.Add("  Tipo de Plug IN: NO TIENE");
                        }
                        else
                        {
                            lista.Add("  Tipo de Plug IN: " + data.TIPO_PLUG_IN.ToString());
                        }

                        if (data.MODLEO_PLUG_IN == null)
                        {
                            lista.Add("  Modelo de Plug IN: NO TIENE");
                        }
                        else
                        {
                            lista.Add("  Modelo de Plug IN: " + data.MODLEO_PLUG_IN.ToString());
                        }
                    }

                    adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.TestListItem, lista);
                    ListTapInfo.Adapter = adapter;
                    btnActualizarTapConsulta.Visibility = 0;
                    btnActualizarTapConsulta.Enabled = true;

                    mProgress.Dismiss();
                }
                else
                {
                    mProgress.Dismiss();
                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetTitle("ERROR");
                    alertDiag.SetMessage("NO TIENE INFORMACION TECNICA");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        //    StartActivity(typeof(ConsultarTap));
                        //  SetContentView(Resource.Layout.Fragment_ConsultarTap);
                    });

                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
            }
            catch (Exception EX)
            {
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                alertDiag.SetTitle("ERROR");
                alertDiag.SetMessage("ERROR");
                alertDiag.SetPositiveButton("OK", (senderAlert, args) => {
                    //    StartActivity(typeof(ConsultarTap));
                    //  SetContentView(Resource.Layout.Fragment_ConsultarTap);
                });

                Dialog diag = alertDiag.Create();
                diag.Show();
            }
        }

        private bool VerificarSyncTipos()
        {

            try
            {

                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_TIPOS.db3")))
                {
                    Int32 exist = conn.ExecuteScalar<Int32>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'ETP_TAPS_TIPOS'");
                    if (exist == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (SQLiteException ex)
            {
                Log.Info("SqliteEx", ex.Message);
                return false;
            }
        }

        private bool VerificarSyncFabricacion()
        {

            try
            {

                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_FABRICANTES.db3")))
                {
                    Int32 exist = conn.ExecuteScalar<Int32>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'ETP_TAPS_FABRICANTES'");
                    if (exist == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                
            }
            catch (SQLiteException ex)
            {
                Log.Info("SqliteEx", ex.Message);
                return false;
            }
        }

        private bool VerificarSyncModelos()
        {

            try
            {

                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_MODELOS.db3")))
                {
                    Int32 exist = conn.ExecuteScalar<Int32>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'ETP_TAPS_MODELOS'");
                    if (exist == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
 
            }
            catch (SQLiteException ex)
            {
                Log.Info("SqliteEx", ex.Message);
                return false;
            }
        }


        public override void OnBackPressed()
        {


            var intent = new Intent(this, typeof(MapActivity));
            intent.PutExtra("x", x);
            intent.PutExtra("y", y);
            intent.PutExtra("reference", reference);      
            StartActivity(intent);

       /*     ListTapInfo = FindViewById<ListView>(Resource.Id.ListTapInfo);
            if(ListTapInfo.Visibility!=0)
            { 
            Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
            alertDiag.SetTitle("CERRAR SESION");
            alertDiag.SetMessage("¿Desea Cerrar La Sesion?");
            alertDiag.SetNegativeButton("SI", (senderAlert, args) => {
                StartActivity(typeof(MainActivity));
                SetContentView(Resource.Layout.activity_main);
            });
            alertDiag.SetPositiveButton("CANCELAR", (senderAlert, args) => {
               
            }); 
            Dialog diag = alertDiag.Create();
             diag.Show();
            }
            else
            {
                StartActivity(typeof(ConsultarTap));
                SetContentView(Resource.Layout.Fragment_ConsultarTap);
            }
            */
        }
    
    }
   
}


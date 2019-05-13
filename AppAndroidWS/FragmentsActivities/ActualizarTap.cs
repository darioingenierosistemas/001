using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using une_etp.REST;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SQLite;
using Android.Util;
using une_etp.TapsFM;


namespace une_etp.FragmentsActivities
{
    [Activity(Label = "Login")]
    public class ActualizarTap : Activity
    {
        private string CodF;
        private ProgressDialog mProgress;
        private ArrayAdapter<string> adapter;
        private InsertarTap insertarTaps;
        private List<string> ListaTipo;
        private List<string> ListaFabricante;
        private List<string> ListaModelo;
        private Button btnActualizarTap;
        private static EditText edtTipo;
        private static EditText edtFabricante;
        private EditText edtModelo;
        private Spinner TipoNuevo;
        private Spinner FabNuevo;
        private Spinner ModNuevo;
        

        private List<TipoActual> TapTipoActual;
        private List<CodFabricanteActual> FabModActual;

        private string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private int IdTapConsulta = Convert.ToInt32(ConsultarTap.IDTAP.ToString());
        private string LogUsuario = MainActivity.Ulog.ToString();
        private static int ItemPositionTipo;
        private static int ItemPositionFab;
        private static int ItemPositionMod;

        private static string Tipo1;
        private static string Fabricacion1;
        private static int CodFabricacion1;
        private static string Modelo1;
        private static string Tipo2;
        private static string Fabricacion2;
        private static string Modelo2;
        private static int CodFabNew;
        private double x;
        private double y;
        private int reference;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Fragment_Actualizar);

            btnActualizarTap = FindViewById<Button>(Resource.Id.btnActualizarTap);

            ItemPositionTipo = 0;
            ItemPositionFab = 0;
            ItemPositionMod = 0;
            CodFabNew = 0;
            x = Intent.GetDoubleExtra("x", 0);
            y = Intent.GetDoubleExtra("y", 0);
            reference = Intent.GetIntExtra("reference", 0);
            TipoNuevo = FindViewById<Spinner>(Resource.Id.spiTipo);
            FabNuevo = FindViewById<Spinner>(Resource.Id.spiFab);
            ModNuevo = FindViewById<Spinner>(Resource.Id.spiModelo);

            mProgress = new ProgressDialog(this);
            mProgress.SetCancelable(true);
            mProgress.SetMessage("Cargando Informacion.....");
            mProgress.SetProgressStyle(ProgressDialogStyle.Spinner);
            mProgress.Progress = 0;
            mProgress.Max = 100;

            mProgress.Show();
            ObtenerDataActual(IdTapConsulta);
            PoblarSpiners();
            mProgress.Dismiss();

            TipoNuevo.SetSelection(0, false);
            TipoNuevo.ItemSelected += TipoNuevo_ItemSelected;

            FabNuevo.SetSelection(0, false);
            FabNuevo.ItemSelected += FabNuevo_ItemSelected;

            ModNuevo.SetSelection(0, false);
            ModNuevo.ItemSelected += ModNuevo_ItemSelected;


            btnActualizarTap.Click += BtnActualizarTap_Click;

        }

    
        private void TipoNuevo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {


                ItemPositionTipo = TipoNuevo.SelectedItemPosition;
                Tipo2 = TipoNuevo.GetItemAtPosition(ItemPositionTipo).ToString();

                ModNuevo = FindViewById<Spinner>(Resource.Id.spiModelo);
                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_MODELOS.db3")))
                {
                    if(CodFabNew == 0 || TipoNuevo.SelectedItemPosition == 0)
                    { 
                    var result = conn.Query<ETP_TAPS_MODELOS>("SELECT DISTINCT MOD_ELEMENTO FROM ETP_TAPS_MODELOS WHERE SUBTYPE = '" + Tipo2 + "' AND ID_FABRICANTE =" + CodFabricacion1 + " ORDER BY MOD_ELEMENTO");

                    ListaModelo = new List<string>();
                    ListaModelo.Add("SELECCIONE MODELO NUEVO");
                    foreach (var tipo in result)
                    {
                        ListaModelo.Add(tipo.MOD_ELEMENTO.ToString());
                    }

                    adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ListaModelo);
                    adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    ModNuevo.Adapter = adapter;
                    }
                    else
                    {
                        var result = conn.Query<ETP_TAPS_MODELOS>("SELECT DISTINCT MOD_ELEMENTO FROM ETP_TAPS_MODELOS WHERE SUBTYPE = '" + Tipo2 + "' AND ID_FABRICANTE =" + CodFabNew + " ORDER BY MOD_ELEMENTO");

                        ListaModelo = new List<string>();
                        ListaModelo.Add("SELECCIONE MODELO NUEVO");
                        foreach (var tipo in result)
                        {
                            ListaModelo.Add(tipo.MOD_ELEMENTO.ToString());
                        }

                        adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ListaModelo);
                        adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                        ModNuevo.Adapter = adapter;

                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SqliteEx", ex.Message);
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

        }

        private void FabNuevo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                ItemPositionFab = FabNuevo.SelectedItemPosition;
                Fabricacion2 = FabNuevo.GetItemAtPosition(ItemPositionFab).ToString();


                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_FABRICANTES.db3")))
                {

                    var result = conn.Query<ETP_TAPS_FABRICANTES>("SELECT ID_FABRICANTE FROM ETP_TAPS_FABRICANTES WHERE DESCRIPCION='" + Fabricacion2 + "'");
                    foreach (var fab in result)
                    {
                        CodFabNew = Convert.ToUInt16(fab.ID_FABRICANTE.ToString());
                    }

                }

                ModNuevo = FindViewById<Spinner>(Resource.Id.spiModelo);
                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_MODELOS.db3")))
                {
                    if (TipoNuevo.SelectedItemPosition == 0)
                    {
                        var result = conn.Query<ETP_TAPS_MODELOS>("SELECT DISTINCT MOD_ELEMENTO FROM ETP_TAPS_MODELOS WHERE SUBTYPE ='" + Tipo1 + "'AND ID_FABRICANTE =" + CodFabNew + " ORDER BY MOD_ELEMENTO");

                        ListaModelo = new List<string>();
                        ListaModelo.Add("SELECCIONE MODELO NUEVO");
                        foreach (var modelo in result)
                        {
                            ListaModelo.Add(modelo.MOD_ELEMENTO.ToString());
                        }

                        adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ListaModelo);
                        adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                        ModNuevo.Adapter = adapter;
                    }
                    else
                    {
                        var result = conn.Query<ETP_TAPS_MODELOS>("SELECT DISTINCT MOD_ELEMENTO FROM ETP_TAPS_MODELOS WHERE SUBTYPE ='" + Tipo2 + "'AND ID_FABRICANTE =" + CodFabNew + " ORDER BY MOD_ELEMENTO");

                        ListaModelo = new List<string>();
                        ListaModelo.Add("SELECCIONE MODELO NUEVO");
                        foreach (var modelo in result)
                        {
                            ListaModelo.Add(modelo.MOD_ELEMENTO.ToString());
                        }

                        adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ListaModelo);
                        adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                        ModNuevo.Adapter = adapter;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SqliteEx", ex.Message);
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

        }

        private void ModNuevo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            { 
                ItemPositionMod = ModNuevo.SelectedItemPosition;
                Modelo2 = ModNuevo.GetItemAtPosition(ItemPositionMod).ToString();
            }
            catch (SQLiteException ex)
            {
                Log.Info("SqliteEx", ex.Message);
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

        }


        private async void ObtenerDataActual(int IdTap)
        {
            try
            {

                edtTipo = FindViewById<EditText>(Resource.Id.edtTipo);
                edtFabricante = FindViewById<EditText>(Resource.Id.edtFabricante);
                edtModelo = FindViewById<EditText>(Resource.Id.edtModelo);

                string url = "http://201.236.222.217:5154/API/etp_app/taps/ACTUALTipoTap/?CodigoSigTipo=" + IdTap;
                string url2 = "http://201.236.222.217:5154/API/etp_app/taps/ACTUALCodigoFab/?CodigoSigFab=" + IdTap;

                var tipo = await url.GetRequest<List<TipoActual>>();
                if(tipo == null)
                {
                    AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                    alertDiag.SetTitle("EROR");
                    alertDiag.SetMessage("NO SE PUEDE ACTUALIZAR ESTE ELEMENTO, CONSULTE CON EL ENCARGADO");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) => {
                        var intent = new Intent(this, typeof(ConsultarTap));
                        intent.PutExtra("x", x);
                        intent.PutExtra("y", y);
                        intent.PutExtra("reference", reference);
                        StartActivity(intent);
                    });
                   
                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
                else
                { 
                var CodFabricante = await url2.GetRequest<List<CodFabricanteActual>>();

                TapTipoActual = tipo;
                FabModActual = CodFabricante;

                foreach (var data in TapTipoActual)
                {
                    edtTipo.Text = data.SUBTYPE.ToString();
                    Tipo1 = data.SUBTYPE.ToString();    
                }

                foreach (var data2 in FabModActual)
                {
                    edtModelo.Text = data2.MOD_ELEMENTO.ToString();
                    CodF = data2.COD_FABRICANTE.ToString();
                    CodFabricacion1 = Convert.ToUInt16(data2.COD_FABRICANTE.ToString());
                    Modelo1 = data2.MOD_ELEMENTO.ToString();
                }

                string url3 = "http://201.236.222.217:5154/API/etp_app/taps/ACTUALNombreFab/?FabCod=" + CodF;
                var NombreFab = await url3.GetRequest<List<FabricanteActual>>();

                foreach (var data3 in NombreFab)
                {
                    edtFabricante.Text = data3.DESCRIPCION.ToString();
                    Fabricacion1 = data3.DESCRIPCION.ToString();
                }


                }
            }
            catch
            {
                Toast.MakeText(this, "Error Inesperado, consulte con el ADMINISTRADOR si este persiste", ToastLength.Long).Show();
            }
        }

        private void PoblarSpiners()
        {
            edtTipo = FindViewById<EditText>(Resource.Id.edtTipo);
            edtFabricante = FindViewById<EditText>(Resource.Id.edtFabricante);
            edtModelo = FindViewById<EditText>(Resource.Id.edtModelo);

            TipoNuevo = FindViewById<Spinner>(Resource.Id.spiTipo);
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_TIPOS.db3")))
            {
                var result = conn.Query<ETP_TAPS_TIPOS>("SELECT FEATURE_SUBTYPE FROM ETP_TAPS_TIPOS"); ;

                ListaTipo = new List<string>();
                ListaTipo.Add("SELECCIONE TIPO NUEVO");
                foreach (var tipo in result)
                {
                    ListaTipo.Add(tipo.FEATURE_SUBTYPE.ToString());
                }

                adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ListaTipo);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                TipoNuevo.Adapter = adapter;


            }

            FabNuevo = FindViewById<Spinner>(Resource.Id.spiFab);
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_FABRICANTES.db3")))
            {
                var result = conn.Query<ETP_TAPS_FABRICANTES>("SELECT DESCRIPCION FROM ETP_TAPS_FABRICANTES"); ;

                ListaFabricante = new List<string>();
                ListaFabricante.Add("SELECCIONE FABRICANTE NUEVO");
                foreach (var fab in result)
                {
                    ListaFabricante.Add(fab.DESCRIPCION.ToString());
                }

                adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ListaFabricante);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                FabNuevo.Adapter = adapter;
            }

            ModNuevo = FindViewById<Spinner>(Resource.Id.spiModelo);
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_MODELOS.db3")))
            {
                var result = conn.Query<ETP_TAPS_MODELOS>("SELECT MOD_ELEMENTO FROM ETP_TAPS_MODELOS");

                ListaModelo = new List<string>();
                ListaModelo.Add("SELECCIONE MODELO NUEVO");
                foreach (var modelo in result)
                {
                    ListaModelo.Add(modelo.MOD_ELEMENTO.ToString());
                }

                adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ListaModelo);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                ModNuevo.Adapter = adapter;
            }

        }
      

        private void BtnActualizarTap_Click(object sender, System.EventArgs e)
        {
            DataToSend();
        }

        private void DataToSend()
        {
            mProgress = new ProgressDialog(this);
            mProgress.SetCancelable(true);
            mProgress.SetMessage("Enviando Informacion.....");
            mProgress.SetProgressStyle(ProgressDialogStyle.Spinner);
            mProgress.Progress = 0;
            mProgress.Max = 100;
            mProgress.Show();
            insertarTaps = new InsertarTap();
          
                insertarTaps.IdTap = IdTapConsulta;
                insertarTaps.Tipo = "TAP";

                if (ItemPositionTipo == 0)
                {
                insertarTaps.SubTipoAC = Tipo1;
                insertarTaps.SubtipoNEW = Tipo1;
                }
                else
                {
                insertarTaps.SubTipoAC = Tipo1;
                insertarTaps.SubtipoNEW = Tipo2;
                }

                if (ItemPositionFab == 0)
                {
                insertarTaps.CodFAC = CodFabricacion1;
                insertarTaps.CodFNEW = CodFabricacion1;
                }
                else
                {
                insertarTaps.CodFAC = CodFabricacion1;
                insertarTaps.CodFNEW = CodFabNew;
                }

                if (ItemPositionMod == 0)
                {
                insertarTaps.MACT = Modelo1;
                insertarTaps.MNEW = Modelo1;
                }
                else
                {
                insertarTaps.MACT = Modelo1;
                insertarTaps.MNEW = Modelo2;
                }

                insertarTaps.usuario = LogUsuario;

                var Insertado = Insertar_TapNewInfo(insertarTaps);
                 
                if (Insertado != null)
                {

                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                alertDiag.SetTitle("INVIO DE DATOS");
                alertDiag.SetMessage("DATOS ENVIADOS CON EXITO");
                alertDiag.SetPositiveButton("OK", (senderAlert, args) => {
                    var intent = new Intent(this, typeof(ConsultarTap));
                    intent.PutExtra("x", x);
                    intent.PutExtra("y", y);
                    intent.PutExtra("reference", reference);
                    StartActivity(intent);
                    Toast.MakeText(this, "OK", ToastLength.Long).Show();
                });

                Dialog diag = alertDiag.Create();
                diag.Show();
                }
                else
                {
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                alertDiag.SetTitle("INVIO DE DATOS");
                alertDiag.SetMessage("FALLO AL ENVIAR DATOS");
                alertDiag.SetPositiveButton("OK", (senderAlert, args) => {
                    Toast.MakeText(this, "ERROR", ToastLength.Long).Show();
                });

                Dialog diag = alertDiag.Create();
                diag.Show();
                }
            mProgress.Dismiss();
        }

        private InsertarTap Insertar_TapNewInfo(InsertarTap NewInfo)
        {
            try
            {
            string url = "http://201.236.222.217:5154/API/etp_app/taps/InsertarInfo/";
            int idTap = Convert.ToInt32(NewInfo.IdTap.ToString());
            string Tap = NewInfo.Tipo.ToString();
            string SubTipoAC = NewInfo.SubTipoAC.ToString();
            string SubtipoNEW = NewInfo.SubtipoNEW.ToString();
            int CodFAC = Convert.ToInt16(NewInfo.CodFAC.ToString());
            int CodFNEW = Convert.ToInt16(NewInfo.CodFNEW.ToString());
            string MACT = NewInfo.MACT.ToString();
            string MNEW = NewInfo.MNEW.ToString();
            string usuario = NewInfo.usuario.ToString();
            string urlinsert = (url + "?IdTap=" + idTap + "&Tipo=" + Tap + "&SubTipoAC=" + SubTipoAC + "&SubtipoNEW=" + SubtipoNEW + "&CodFAC=" + CodFAC + "&CodFNEW=" + CodFNEW + "&MACT=" + MACT + "&MNEW=" + MNEW + "&usuario=" + usuario);
            var result =  urlinsert.GetRequest<InsertarTap>();
                if (result == null)
                {
                    AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                    alertDiag.SetTitle("ERROR");
                    alertDiag.SetMessage("Error al Enviar Informacion");
                    alertDiag.SetNegativeButton("OK", (senderAlert, args) => {
                        //StartActivity(typeof(ActualizarTap));
                        //SetContentView(Resource.Layout.Fragment_Actualizar);
                    });
                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }

            }
            catch (Exception ex)
            {
                //ex.Message;
                return null;
            }
            return NewInfo;

        }

        public override void OnBackPressed()
        {

            var intent = new Intent(this, typeof(ConsultarTap));
            intent.PutExtra("x", x);
            intent.PutExtra("y", y);
            intent.PutExtra("reference", reference);
            StartActivity(intent);

        }

    }   

}
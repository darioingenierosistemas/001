using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using une_etp.REST;
using une_etp.Activities;

namespace une_etp.FragmentsActivities
{
    [Activity(Label = "Consultar Nap"
        //, MainLauncher = true
    )]
    [MetaData("android.support.PARENT_ACTIVITY",
        Value = "une_etp.Activities.MapActivity")]
    public class ConsultarNap : AppCompatActivity
    {
        private ProgressDialog mProgress;
        private ListView ListTapInfo;
        private List<ConsultarNapTecnico> NewTap;
        private List<string> lista;
        private ArrayAdapter<string> adapter;

        Button btnButtonComercial;
        EditText textView;
        ListView listView;
        private static int M;
        public List<ConsultarCoNAP> newCoTap;
        public List<string> listaNap;
        public string IDNAP;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Fragment_ConsultarNap);
            IDNAP = Intent.GetStringExtra("codigoTag");

            textView = FindViewById<EditText>(Resource.Id.etIdentificador);
            Button btnTecnica = FindViewById<Button>(Resource.Id.btnTecnica);
            Button BtnButtonConsultarComercial = FindViewById<Button>(Resource.Id.btnComercial);

            textView.Text = IDNAP;

            btnTecnica.Click += BtnTecnica_Click;
            BtnButtonConsultarComercial.Click += BtnButtonConsultarComercial_Click;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);
            return true;
        }

        private async void BtnTecnica_Click(object sender, EventArgs e)
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

                NewTap = new List<ConsultarNapTecnico>();
                EditText etIdentificador = FindViewById<EditText>(Resource.Id.etIdentificador);
                ListTapInfo = FindViewById<ListView>(Resource.Id.ListTapInfo);

                string url = "http://201.236.222.217:5154/API/etp_app/taps/BuscarNap/?IdNap=";
                string idNap = etIdentificador.Text.ToString();

                if (!string.IsNullOrEmpty(idNap))
                {
                    string urlNapTecnica = url + idNap;

                    var GetTapInfo = await urlNapTecnica.GetRequest<List<ConsultarNapTecnico>>();
                    NewTap = GetTapInfo;
                    lista = new List<string>();

                    if (NewTap != null)
                    {
                        foreach (var data in NewTap)
                        {
                           
                            lista.Add("  ID: " + data.ID.ToString());
                            if (!string.IsNullOrEmpty(data.DIRECCION))
                            {
                                lista.Add("  Dirección: " + data.DIRECCION.ToString());
                            }
                            else
                            {
                                lista.Add("  Dirección: NO TIENE");
                            }

                            if (!string.IsNullOrEmpty(data.NAPS.ToString()))
                            {
                                lista.Add("  Naps: " + data.NAPS.ToString());
                            }
                            else
                            {
                                lista.Add("  Naps: NO TIENE");
                            }
                          
                            if (!string.IsNullOrEmpty(data.RESERVA))
                            {
                                lista.Add("  Reserva: " + data.RESERVA.ToString());
                            }
                            else
                            {
                                lista.Add("  Reserva: NO TIENE");
                            }

                            if (!string.IsNullOrEmpty(data.ARPON.ToString()))
                            {
                                lista.Add("  Arpon: " + data.ARPON.ToString());
                            }
                            else
                            {
                                lista.Add("  Arpon: NO TIENE");
                            }

                            if (!string.IsNullOrEmpty(data.MODELO_DIV))
                            {
                                lista.Add("  Modelo_Div: " + data.MODELO_DIV.ToString());
                            }
                            else
                            {
                                lista.Add("  Modelo_Div: NO TIENE");
                            }
                            lista.Add(" ");
                        }

                        adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.TestListItem, lista);
                        ListTapInfo.Adapter = adapter;
                        ListTapInfo.SetMinimumHeight(3);
                        ListTapInfo.Visibility = 0;

                        mProgress.Dismiss();
                    }
                    else
                    {
                        mProgress.Dismiss();
                        Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                        alertDiag.SetTitle("ERROR");
                        alertDiag.SetMessage("NO TIENE INFORMACION TECNICA");

                        Dialog diag = alertDiag.Create();
                        diag.Show();
                    }
                }
                else
                {
                    mProgress.Dismiss();
                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetTitle("ERROR");
                    alertDiag.SetMessage("El campo no puede estar vació");

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

        private void BtnButtonConsultarComercial_Click(object sender, EventArgs e)
        {
            listView = FindViewById<ListView>(Resource.Id.ListTapInfo);

            if (!string.IsNullOrEmpty(textView.Text.ToString()))
            {
                listView.Visibility = 0;
                //btnButtonComercial.Enabled = true;
                GetTapInfoRESTCoNap();
            }
            else if (string.IsNullOrEmpty(textView.Text.ToString()))
            {
                Toast.MakeText(this, "Por favor llene los campos", ToastLength.Long).Show();
            }

        }

        private async void GetTapInfoRESTCoNap()
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

                newCoTap = new List<ConsultarCoNAP>();
                textView = FindViewById<EditText>(Resource.Id.etIdentificador);
                listView = FindViewById<ListView>(Resource.Id.ListTapInfo);

                string url = "http://201.236.222.217:5154/API/etp_app/taps/OcupacionNap/?IdNap=";

                string ComercialNap = textView.Text.ToString().Trim();
                string URLNAP = url + ComercialNap;
                var GetTapInfoNap = await URLNAP.GetRequest<List<ConsultarCoNAP>>();
                newCoTap = GetTapInfoNap;
                listaNap = new List<string>();
                if (newCoTap != null)
                {
                    foreach (var dataNap in newCoTap)
                    {

                        //	IDNAP = dataNap.IdTap.ToString();
                        //	listaNap.Add("Identificador NAP: " + dataNap.IdTap.ToString());
                        if (!string.IsNullOrEmpty(dataNap.PUERTO.ToString()))
                        { 
                            listaNap.Add("  Puerto: " + dataNap.PUERTO.ToString());
                        }
                        else
                        {
                            listaNap.Add("  Puerto: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(dataNap.USUARIO))
                        {
                            listaNap.Add("  Usuario: " + dataNap.USUARIO.ToString());
                        }
                        else
                        {
                            listaNap.Add("  Usuario: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(dataNap.ESTADO))
                        { 
                            listaNap.Add("  Estado: " + dataNap.ESTADO.ToString());
                        }
                        else
                        {
                            listaNap.Add("  Estado: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(dataNap.DIRECCION))
                        { 
                            listaNap.Add("  Direccion: " + dataNap.DIRECCION.ToString());
                        }
                        else
                        {
                            listaNap.Add("  Direccion: NO TIENE");
                        }

                        if (!string.IsNullOrEmpty(dataNap.PLAN))
                        {
                            listaNap.Add("  Plan: " + dataNap.PLAN.ToString());
                        }
                        else
                        {
                            listaNap.Add("  Plan: NO TIENE");
                        }
                        listaNap.Add(" ");



                    }
                    adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.TestListItem, listaNap);
                    //	listView.SetMinimumHeight(M);
                    listView.Adapter = adapter;
                    listView.SetMinimumHeight(2);
                    //btnButtonComercial.Enabled = true;
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
    }
}
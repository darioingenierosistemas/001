using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System.Linq;
using Android.Runtime;
using Android.Widget;
using Android.Text;
using Android.Content;
using System.Collections.Generic;
using une_etp.TapsFM;
using AlertDialog = Android.App.AlertDialog;
using une_etp.REST;
using System;
using une_etp.Activities;

namespace une_etp.FragmentsActivities
{
    [Activity(Label = "Consulta y Actualizacion Coaxial",  Theme = "@style/AppTheme", Icon = "@drawable/GYG_APP", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ProgressDialog mProgress;
        public static string Ulog = "";
     

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.SetLicense("runtimelite,1000,rud2018024756,none,3M2PMD17JYEFP2ELJ08");
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            Button btnSincronizar = FindViewById<Button>(Resource.Id.btnSincronizar);
            CheckBox chkMostar = FindViewById<CheckBox>(Resource.Id.chkMostrar);
            btnSincronizar.Click += BtnSincronizar_Click1;
            btnLogin.Click += BtnLogin_Click;
            chkMostar.CheckedChange += ChkMostar_CheckedChange;

        }

        private void ChkMostar_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            CheckBox chkMostar = FindViewById<CheckBox>(Resource.Id.chkMostrar);
            EditText edtPass = FindViewById<EditText>(Resource.Id.edtPass);

            if (chkMostar.Checked == true)
            {
                edtPass.InputType = InputTypes.TextVariationVisiblePassword;
            }
            else if (chkMostar.Checked == false)
            {

                edtPass.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
            }
            edtPass.RequestFocus();
        }

        private void BtnSincronizar_Click1(object sender, System.EventArgs e)
        {
            RepositorioTFM implementar = new RepositorioTFM();

            implementar.CreateDataBase();
            implementar.GetAllTaps();

            AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
            alertDiag.SetTitle("Sincronizacion");
            alertDiag.SetMessage("Base de Datos Sincronizada");
            alertDiag.SetPositiveButton("OK", (senderAlert, args) => {
                Toast.MakeText(this, "OK", ToastLength.Short).Show();
            });
         
            Dialog diag = alertDiag.Create();
            diag.Show();

        }

        private async void BtnLogin_Click(object sender, System.EventArgs e)
        {
          
            EditText edtUser = FindViewById<EditText>(Resource.Id.edtUser);
            EditText edtPass = FindViewById<EditText>(Resource.Id.edtPass);
            if(!string.IsNullOrEmpty(edtUser.Text.ToString()) && !string.IsNullOrEmpty(edtPass.Text.ToString()))
            {
                VerificarUser();
            }
            else if(string.IsNullOrEmpty(edtUser.Text.ToString()) || string.IsNullOrEmpty(edtPass.Text.ToString()))
            {
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                alertDiag.SetTitle("ERROR");
                alertDiag.SetMessage("Los Campos de USER y PASSWORD no pueden estas vacios");
                alertDiag.SetNegativeButton("OK", (senderAlert, args) => {
                });
                Dialog diag = alertDiag.Create();
                diag.Show();
            }

        }

        private async void VerificarUser()
        {
            EditText edtUser = FindViewById<EditText>(Resource.Id.edtUser);
            EditText edtPass = FindViewById<EditText>(Resource.Id.edtPass);

            mProgress = new ProgressDialog(this);
            mProgress.SetCancelable(true);
            mProgress.SetMessage("Verificando Usuario");
            mProgress.SetProgressStyle(ProgressDialogStyle.Spinner);
            mProgress.Progress = 0;
            mProgress.Max = 100;

            mProgress.Show();
            try
            {
                string url = "http://201.236.222.217:5154/API/etp_app/taps/VerificarUsuario/?Usuario="+edtUser.Text.ToString()+"&Password="+edtPass.Text.ToString();
                var GetUser = await url.GetRequestUSER<UsuarioVerificar>();

                if (GetUser.ToString() == "SI")
                {
                    mProgress.Dismiss();
                    AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                    alertDiag.SetTitle("INICIO DE SESION");
                    alertDiag.SetMessage("USUARIO VERIFICADO");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        StartActivity(typeof(MapActivity));
                //        SetContentView(Resource.Layout.Fragment_ConsultarTap);
                        Ulog = edtUser.Text.ToString();
                    });
                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
                else
                {
                    mProgress.Dismiss();
                    AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                    alertDiag.SetTitle("ERROR!");
                    alertDiag.SetMessage("Usuario o Constraseña invalida");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        edtUser.Text = "";
                        edtPass.Text = "";
                    });
                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
            }
            catch (Exception ex)
            {
                mProgress.Dismiss();
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                alertDiag.SetTitle("ERROR");
                alertDiag.SetMessage(ex.Message);
                alertDiag.SetNegativeButton("OK", (senderAlert, args) =>
                {
                    //StartActivity(typeof(MainActivity));
                    //SetContentView(Resource.Layout.activity_main);
                });
                Dialog diag = alertDiag.Create();
                diag.Show();

            }

        }

        public override void OnBackPressed()
        {

            Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
            alertDiag.SetTitle("SALIR");
            alertDiag.SetMessage("¿Desea Cerrar La Aplicacion?");
            alertDiag.SetNegativeButton("SI", (senderAlert, args) => {
                FinishAffinity();
            });
            alertDiag.SetPositiveButton("NO", (senderAlert, args) => {

            });
            Dialog diag = alertDiag.Create();
            diag.Show();

        }

    }

}
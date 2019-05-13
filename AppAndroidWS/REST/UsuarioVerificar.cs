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
    public class UsuarioVerificar
    {

        private string m_Usuario;
        public string Usuario
        {
            get
            {
                return m_Usuario;
            }
            set
            {
                this.m_Usuario = value;
            }
        }

        private string m_Password;
        public string Password
        {
            get
            {
                return m_Password;
            }
            set
            {
                this.m_Password = value;
            }
        }

    }
}
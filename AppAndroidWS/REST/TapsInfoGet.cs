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
    public class TapsInfoGet
    {
        private int m_TAP;
        public int TAP
        {
            get
            {
                return m_TAP;
            }
            set
            {
                this.m_TAP = value;
            }
        }

        private string m_OSF;
        public string OSF
        {
            get
            {
                return m_OSF;
            }
            set
            {
                this.m_OSF = value;
            }
        }

        private string m_DIRECCION;
        public string DIRECCION
        {
            get
            {
                return m_DIRECCION;
            }
            set
            {
                this.m_DIRECCION = value;
            }
        }

        private int m_ATENUACION;
        public int ATENUACION
        {
            get
            {
                return m_ATENUACION;
            }
            set
            {
                this.m_ATENUACION = value;
            }
        }

        private string m_NUMERO_BOCAS;
        public string NUMERO_BOCAS
        {
            get
            {
                return m_NUMERO_BOCAS;
            }
            set
            {
                this.m_NUMERO_BOCAS = value;
            }
        }

        private int m_ESTADO_OPERATIVO;
        public int ESTADO_OPERATIVO
        {
            get
            {
                return m_ESTADO_OPERATIVO;
            }
            set
            {
                this.m_ESTADO_OPERATIVO = value;
            }
        }

        private string m_MODELO;
        public string MODELO
        {
            get
            {
                return m_MODELO;
            }
            set
            {
                this.m_MODELO = value;
            }
        }

        private double m_FORWARD_ALTO;
        public double FORWARD_ALTO
        {
            get
            {
                return m_FORWARD_ALTO;
            }
            set
            {
                this.m_FORWARD_ALTO = value;
            }
        }

        private double m_FORWARD_BAJO;
        public double FORWARD_BAJO
        {
            get
            {
                return m_FORWARD_BAJO;
            }
            set
            {
                this.m_FORWARD_BAJO = value;
            }
        }

        private double m_REVERSE_ALTO;
        public double REVERSE_ALTO
        {
            get
            {
                return m_REVERSE_ALTO;
            }
            set
            {
                this.m_REVERSE_ALTO = value;
            }
        }

        private double m_REVERSE_BAJO;
        public double REVERSE_BAJO
        {
            get
            {
                return m_REVERSE_BAJO;
            }
            set
            {
                this.m_REVERSE_BAJO = value;
            }
        }

        private double m_FORWARD_ALTO_BOCA;
        public double FORWARD_ALTO_BOCA
        {
            get
            {
                return m_FORWARD_ALTO_BOCA;
            }
            set
            {
                this.m_FORWARD_ALTO_BOCA = value;
            }
        }

        private double m_FORWARD_BAJO_BOCA;
        public double FORWARD_BAJO_BOCA
        {
            get
            {
                return m_FORWARD_BAJO_BOCA;
            }
            set
            {
                this.m_FORWARD_BAJO_BOCA = value;
            }
        }

        private double m_REVERSE_ALTO_BOCA;
        public double REVERSE_ALTO_BOCA
        {
            get
            {
                return m_REVERSE_ALTO_BOCA;
            }
            set
            {
                this.m_REVERSE_ALTO_BOCA = value;
            }
        }

        private double m_REVERSE_BAJO_BOCA;
        public double REVERSE_BAJO_BOCA
        {
            get
            {
                return m_REVERSE_BAJO_BOCA;
            }
            set
            {
                this.m_REVERSE_BAJO_BOCA = value;
            }
        }

        private string m_TIPO_PLUG_IN;
        public string TIPO_PLUG_IN
        {
            get
            {
                return m_TIPO_PLUG_IN;
            }
            set
            {
                this.m_TIPO_PLUG_IN = value;
            }
        }

        private string m_MODLEO_PLUG_IN;
        public string MODLEO_PLUG_IN
        {
            get
            {
                return m_MODLEO_PLUG_IN;
            }
            set
            {
                this.m_MODLEO_PLUG_IN = value;
            }
        }


    }

}

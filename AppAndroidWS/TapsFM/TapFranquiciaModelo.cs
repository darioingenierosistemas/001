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
using SQLite;

namespace une_etp.TapsFM
{
    [Table("ETP_TAPS_TIPOS")]
    public class ETP_TAPS_TIPOS
    {
        private string m_FEATURE_SUBTYPE;
        [MaxLength(6)]
        public string FEATURE_SUBTYPE
        {

            get
            {
                return m_FEATURE_SUBTYPE;
            }
            set
            {
                this.m_FEATURE_SUBTYPE = value;
            }
        }

        private string m_DESCR;
        [MaxLength(32)]
        public string DESCR
        {

            get
            {
                return m_DESCR;
            }
            set
            {
                this.m_DESCR = value;
            }

        }
    }

    [Table("ETP_TAPS_FABRICANTES")]
    public class ETP_TAPS_FABRICANTES
    {
        private int m_ID_FABRICANTE;
        [PrimaryKey, MaxLength(3)]
        public int ID_FABRICANTE
        {

            get
            {
                return m_ID_FABRICANTE;
            }
            set
            {
                this.m_ID_FABRICANTE = value;
            }

        }

        private string m_DESCRIPCION;
        [MaxLength(32)]
        public string DESCRIPCION
        {

            get
            {
                return m_DESCRIPCION;
            }
            set
            {
                this.m_DESCRIPCION = value;
            }

        }
    }

    [Table("ETP_TAPS_MODELOS")]
    public class ETP_TAPS_MODELOS
    {
        private int m_ID_FABRICANTE;
        [MaxLength(3)]
        public int ID_FABRICANTE
        {

            get
            {
                return m_ID_FABRICANTE;
            }
            set
            {
                this.m_ID_FABRICANTE = value;
            }

        }

        private string m_SUBTYPE;
        [MaxLength(6)]
        public string SUBTYPE
        {

            get
            {
                return m_SUBTYPE;
            }
            set
            {
                this.m_SUBTYPE = value;
            }


        }

        private string m_MOD_ELEMENTO;
        [MaxLength(32)]
        public string MOD_ELEMENTO
        {

            get
            {
                return m_MOD_ELEMENTO;
            }
            set
            {
                this.m_MOD_ELEMENTO = value;
            }


        }

    }


}
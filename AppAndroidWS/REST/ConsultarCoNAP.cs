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
	public class ConsultarCoNAP
	{
		
		public int IdTap
		{
			get;set;
		}
		public int PUERTO
		{
			get; set;
		}
		public string USUARIO
		{
			get; set;
		}
		public string ESTADO
		{
			get; set;
		}

		public string DIRECCION
		{
			get;set;
		}

		public string PLAN
		{
			get; set;
		}

	}
}
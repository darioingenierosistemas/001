using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;
using une_etp.REST;
using une_etp.Interface;

namespace une_etp.TapsFM
{
    public class RepositorioTFM: IRepositorioTFM
    {

        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public async void GetAllTaps()
        {
            try
            { 
            List<ETP_TAPS_TIPOS> tipos = new List<ETP_TAPS_TIPOS>();
            List<ETP_TAPS_FABRICANTES> fabricantes = new List<ETP_TAPS_FABRICANTES>();
            List < ETP_TAPS_MODELOS > modelos = new List<ETP_TAPS_MODELOS>();


            var ListaTipos = await "http://201.236.222.217:5154/API/etp_app/taps/MostrarTapTipo/".GetRequest<List<ETP_TAPS_TIPOS>>();
            tipos = ListaTipos.ToList();

            using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_TIPOS.db3")))
            {
                conn.InsertAll(tipos);    
            }



            var ListaFranbricantes = await "http://201.236.222.217:5154/API/etp_app/taps/MostrarFabricante/".GetRequest<List<ETP_TAPS_FABRICANTES>>();
            fabricantes = ListaFranbricantes.ToList();
            
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_FABRICANTES.db3")))
            {
                conn.InsertAll(fabricantes);
            }



            var ListaModelos = await "http://201.236.222.217:5154/API/etp_app/taps/MostrarModelo/".GetRequest<List<ETP_TAPS_MODELOS>>();
            modelos = ListaModelos;
         
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_MODELOS.db3")))
            {
                conn.InsertAll(modelos);
            }
             
            }

            catch (SQLiteException ex)
            {
                Log.Info("SqliteEx", ex.Message);
            }

        }

        public bool CreateDataBase()
        {
            
            try
            {
               
                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_TIPOS.db3")))
                {
                    Int32 exist = conn.ExecuteScalar<Int32>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'ETP_TAPS_TIPOS'");
                    if (exist == 1)
                    {
                        conn.DropTable<ETP_TAPS_TIPOS>();
                        conn.CreateTable<ETP_TAPS_TIPOS>();
                       
                    }
                    else if (exist == 0)
                    {
                        conn.CreateTable<ETP_TAPS_TIPOS>();
                    }

                }

                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_FABRICANTES.db3")))
                {
                    Int32 exist = conn.ExecuteScalar<Int32>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'ETP_TAPS_FABRICANTES'");
                    if (exist == 1)
                    {
                        conn.DropTable<ETP_TAPS_FABRICANTES>();
                        conn.CreateTable<ETP_TAPS_FABRICANTES>();
                    }
                    else if (exist == 0)
                    {
                        conn.CreateTable<ETP_TAPS_FABRICANTES>();
                    }


                }

                using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "ETP_TAPS_MODELOS.db3")))
                {            
                    Int32 exist = conn.ExecuteScalar<Int32>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'ETP_TAPS_MODELOS'");
                    if (exist == 1)
                    {
                        conn.DropTable<ETP_TAPS_MODELOS>();
                        conn.CreateTable<ETP_TAPS_MODELOS>();
                    }
                    else if (exist == 0)
                    {
                        conn.CreateTable<ETP_TAPS_MODELOS>();
                    }

                }

                return true;
            }
            catch (SQLiteException ex)
            {
                Log.Info("SqliteEx", ex.Message);
                return false;
            }

        }


    }



}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using une_etp.FragmentsActivities;
using une_etp.REST;


namespace une_etp.Activities
{
    [Activity(Label = "Consulta y Actualizacion Coaxial", Theme = "@style/AppTheme", Icon = "@drawable/GYG_APP")]
    [MetaData("android.support.PARENT_ACTIVITY",
        Value = "une_etp.FragmentsActivities.MainActivity")]
    public class MapActivity : AppCompatActivity
    {

        private MapView mapView = new MapView();
        public Dictionary<string, Geometry> listGraphicsOverlay { get; set; }
        private FeatureLayer _featureLayer;
        private FeatureLayer featureLayerCableCoaxial;
        //private FeatureLayer featureLayerAcoplador;
        private FeatureLayer featureLayerMDU;
        private FeatureLayer featureLayerNAP;
        private double x;
        private double y;
        private int reference;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.SetLicense("runtimelite,1000,rud2018024756,none,3M2PMD17JYEFP2ELJ08");
            SetContentView(Resource.Layout.map);
            BuildMapAsync();
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            return true;
        }

        private async void BuildMapAsync()
        {
            try
            {
                mapView = FindViewById<MapView>(Resource.Id.mapa);
                mapView.Map = new Map(Basemap.CreateTopographic());
             //   mapView.LocationDisplay.ShowLocation = true;
                mapView.LocationDisplay.IsEnabled = true;
                mapView.GeoViewTapped += MapView_GeoViewTappedAsync;
                Uri wmsUrl = new Uri("http://arcgis.etp.com.co:8399/arcgis/rest/services/sigetp/coaxial/FeatureServer/0");
                var featureTable = new ServiceFeatureTable(wmsUrl);
                _featureLayer = new FeatureLayer(featureTable);
                await _featureLayer.LoadAsync();
                if (_featureLayer.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
                {
                    mapView.Map.OperationalLayers.Add(_featureLayer);
                }
                FillNap();
                FillCableCoaxialAsync();
                FillMDU();
                //FillAcoplador();
                await SetInitialPointAsync();


            }
            catch (Exception ex)
            {
                Log.Info("Error", ex.Message);
                GenerateAlertError("Error creando el mapa");
            }
          

        }



        private async void FillCableCoaxialAsync()
        {
            Uri cableCoaxial = new Uri("http://arcgis.etp.com.co:8399/arcgis/rest/services/sigetp/coaxial/FeatureServer/10");
            var featureTableCableCoaxial = new ServiceFeatureTable(cableCoaxial);
            featureLayerCableCoaxial = new FeatureLayer(featureTableCableCoaxial);
            await featureLayerCableCoaxial.LoadAsync();
            if (featureLayerCableCoaxial.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
            {
                mapView.Map.OperationalLayers.Add(featureLayerCableCoaxial);
            }
        }

        //private async void FillAcoplador()
        //{
        //    Uri Acoplador = new Uri("http://arcgis.etp.com.co:8399/arcgis/rest/services/sigetp/coaxial/FeatureServer/1");
        //    var featureTableAcoplador = new ServiceFeatureTable(Acoplador);
        //    featureLayerAcoplador = new FeatureLayer(featureTableAcoplador);
        //    await featureTableAcoplador.LoadAsync();
        //    if (featureLayerAcoplador.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
        //    {
        //        mapView.Map.OperationalLayers.Add(featureLayerAcoplador);
        //    }
        //}

        private async void FillMDU()
        {
            Uri MDU = new Uri("http://arcgis.etp.com.co:8399/arcgis/rest/services/sigetp/coaxial/FeatureServer/7");
            var featureTableMDU = new ServiceFeatureTable(MDU);
            featureLayerMDU = new FeatureLayer(featureTableMDU);
            await featureLayerMDU.LoadAsync();
            if (featureLayerMDU.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
            {
                mapView.Map.OperationalLayers.Add(featureLayerMDU);
            }
            await SetInitialPointAsync();
        }

        private async void FillNap()
        {
            Uri NAP = new Uri("http://arcgis.etp.com.co:8399/arcgis/rest/services/sigetp/coaxial/FeatureServer/3");
            var featureTableNAP = new ServiceFeatureTable(NAP);
            featureLayerNAP = new FeatureLayer(featureTableNAP);
            await featureLayerNAP.LoadAsync();
            if (featureLayerNAP.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
            {
                mapView.Map.OperationalLayers.Add(featureLayerNAP);
            }
           
        }

        private async Task SetInitialPointAsync()
        {
            x = Intent.GetDoubleExtra("x", 0);
            y = Intent.GetDoubleExtra("y", 0);
            reference = Intent.GetIntExtra("reference", 0);

            if (reference > 0)
            {
                await FillPointCenterAsync(x,y, SpatialReference.Create(reference));
            }
            else
            {
                if (null != mapView.LocationDisplay.Location && null != mapView.LocationDisplay.Location.Position)
                {
                   await  FillPointCenterAsync(mapView.LocationDisplay.Location.Position.X, mapView.LocationDisplay.Location.Position.Y, mapView.LocationDisplay.Location.Position.SpatialReference);
                }
            }
            mapView.LocationDisplay.IsEnabled = false;
        }

        private async Task FillPointCenterAsync(double x , double y , SpatialReference spatialReference)
        {
            MapPoint centralPoint = new MapPoint(x,y, spatialReference);
            mapView.LocationDisplay.InitialZoomScale = 1100;
            Viewpoint startingViewpoint = new Viewpoint(centralPoint, 1100);
            mapView.Map.InitialViewpoint = startingViewpoint;
            await mapView.SetViewpointCenterAsync(centralPoint, 1100);
        }

        private async void MapView_GeoViewTappedAsync(object sender, GeoViewInputEventArgs e)
        {
            try
            {
                double tolerance = 10;
                double mapTolerance = tolerance * mapView.UnitsPerPixel;
                MapPoint geometry = e.Location;
                if (mapView.IsWrapAroundEnabled) { geometry = GeometryEngine.NormalizeCentralMeridian(geometry) as MapPoint; }
                var selectionEnvelope = new Envelope(geometry.X - mapTolerance, geometry.Y - mapTolerance, geometry.X + mapTolerance,
                    geometry.Y + mapTolerance, mapView.Map.SpatialReference); 
                var queryParams = new QueryParameters();         
                queryParams.Geometry = selectionEnvelope;
                SelectTagByTypoAsync(_featureLayer, "TAP", queryParams);
                SelectTagByTypoAsync(featureLayerNAP, "NAP", queryParams);
                SelectTagByTypoAsync(featureLayerMDU, "MDU", queryParams);
                await featureLayerCableCoaxial.SelectFeaturesAsync(queryParams, SelectionMode.New);

            }
            catch (Exception ex)
            {
                Log.Info("Error", ex.Message);
                GenerateAlertError("Error buscando el tap");
            }
        }

        private async void SelectTagByTypoAsync(FeatureLayer featureLayer, string tipo, QueryParameters queryParams)
        {
            try
            {
                FeatureQueryResult result = await featureLayer.SelectFeaturesAsync(queryParams, SelectionMode.New);
                if (null != result)
                {
                    IEnumerator<Feature> enumerator = result.GetEnumerator();
                    if (null != enumerator)
                    {
                        while (enumerator.MoveNext())
                        {
                            Feature item = enumerator.Current;
                            MapPoint projectedLocation = (MapPoint)item.Geometry;
                            var tag = item.Attributes["TAG"];
                            var intent = new Intent(this, typeof(ConsultarTap));
                            intent.PutExtra("codigoTag", tag.ToString());
                            intent.PutExtra("x", projectedLocation.X);
                            intent.PutExtra("y", projectedLocation.Y);
                            intent.PutExtra("reference", projectedLocation.SpatialReference.Wkid);
                            intent.PutExtra("tipo", tipo);
                            StartActivity(intent);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Info("Error", ex.Message);
                GenerateAlertError("No tiene Tag");
            }
        }


        private void FindTagByPosition(Android.Graphics.PointF position)
        {
            try
            {
                if (null != listGraphicsOverlay && listGraphicsOverlay.Count > 0)
                {
                    foreach (var item in listGraphicsOverlay)
                    {
                        var pointFind = new MapPoint(position.X, position.Y, mapView.LocationDisplay.Location.Position.SpatialReference);
                        var simpleMarkerSymbolFind = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, Color.Red, 10);
                        var graphicFind = new Graphic(pointFind, simpleMarkerSymbolFind);
                        Geometry geometryData = GeometryEngine.Project(item.Value, mapView.LocationDisplay.Location.Position.SpatialReference);
                        Boolean result = GeometryEngine.Contains(geometryData, graphicFind.Geometry);
                        Boolean resultIntersect = GeometryEngine.Intersects(geometryData, graphicFind.Geometry);
                        if (result && resultIntersect)
                        {
                            var intent = new Intent(this, typeof(ConsultarTap));
                            intent.PutExtra("codigoTag", item.Key);
                            StartActivity(intent);
                            SetContentView(Resource.Layout.Fragment_ConsultarTap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("Error", ex.Message);
                throw new System.Exception(ex.Message);
            }
        }



        public void GenerateAlertError( string mensajeError)
        {
            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle("ERROR !!");
            alert.SetMessage(mensajeError);
            alert.SetPositiveButton("Aceptar", (senderAlert, args) => {

            });
            Dialog dialog = alert.Create();
            dialog.Show();

        }
    }
}
 
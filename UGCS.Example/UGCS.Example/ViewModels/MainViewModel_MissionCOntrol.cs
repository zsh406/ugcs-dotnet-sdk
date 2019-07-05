using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
#if NETFX_CORE
using Windows.UI.Xaml;
#else
using System.Windows.Threading;
#endif
using MapControl;
using UGCS.Example.Models;
using Services;
using System.Windows;
using UGCS.Example.Properties;
using UGCS.Sdk.Protocol.Encoding;
using System.Threading.Tasks;
using Services.Helpers;
using Services.SdkServices;
using System.Collections.Generic;
using System.Timers;
using UGCS.Example.Enums;
using Services.Commands;



namespace UGCS.Example.ViewModels
{
    public partial class MainViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public void CreateMission()
        {
            if (Mission != null)
            {
                MessageBox.Show("Mission already created");
                return;
            }
            Task.Factory.StartNew(() => 
            {
                Mission = _missionService.CreateNewMission("TestMission");
                if (Mission != null)
                {
                    Mission.Vehicles.Add(
                        new MissionVehicle
                        {
                            Vehicle = new Vehicle { Id = ClientVehicle.Vehicle.Id }
                        });
                }

            }).ContinueWith((result) =>
            {
                if (result.Exception != null)
                {
                    MessageBox.Show(result.Exception.Message);
                }
                else
                {
                    NotifyOfPropertyChange(() => MissionName);
                }
            });
        }

        public void CreateRoute()
        {
            openFileDialog1 = openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\Users\shuhang\Documents\GitHub\UAV_planning_photogrammetry\";

            if (Mission == null)
            {
                MessageBox.Show("Mission not created");
                return;
            }
            if (Route != null)
            {
                MessageBox.Show("Route already created");
                return;
            }
            Task.Factory.StartNew(() =>
            {
                string filename = "";
                //Route = _routeService.CreateNewRoute(Mission, ClientVehicle.Vehicle.Profile, "TestRoute");
                //Route = _routeService.AddWaypoint(Route, 0, 0.8163862613378483, 0.14565284453422503);
                //Route = _routeService.AddWaypoint(Route, 1, 0.8163939269001643, 0.14566059986372001);
                //Route = _routeService.AddWaypoint(Route, 2, 0.8164021444313243, 0.1456445439943193);
                //Route = _routeService.AddWaypoint(Route, 3, 0.8163927607338599, 0.14564369686869438);
                //if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    filename = openFileDialog1.FileName;
                //}
                // filename = @"C:\Users\shuhang\Documents\GitHub\UAV_planning_photogrammetry\results\0617-30h-ds2-20m-2-2xob\EO_export.txt";
                filename = @"D:\Datasets\Hessigheim_Pointcloud\DJI_flight\metashape_path\all_43.kml";

                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(filename);
                Route = _routeService.CreateNewRoute(Mission, ClientVehicle.Vehicle.Profile, fileNameWithoutExtension);
                // Route = _routeService.AddWaypointFromTxt(filename, Route);
                Route = _routeService.AddWaypointFromKml(filename, Route);

            }).ContinueWith((result) =>
            {
                if (result.Exception != null)
                {
                    MessageBox.Show(result.Exception.Message);
                }
                else
                {
                    MapViewModel.UpdateRouteView(Route);
                    NotifyOfPropertyChange(() => RouteName);
                }
            });
        }
        
    }
}

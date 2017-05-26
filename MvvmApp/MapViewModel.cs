using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmApp
{
    class MapViewModel : INotifyPropertyChanged
    {
        private Map map;
        public Map IncidentMap
        {
            get { return this.map; }
            set { this.map = value; }
        }

        public DelegateCommand ToggleLayerCommand { get; set; }
        public DelegateCommand ExtentChangedCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string extentString;
        public string CurrentExtentString
        {
            get
            {
                return this.extentString;
            }
            set
            {
                this.extentString = value;
                this.RaiseNotifyPropertyChanged(); 
            }
        }

        public MapViewModel()
        {
            // when the view model initializes, read the map from the App.xaml resources
            this.map = MvvmApp.App.Current.Resources["IncidentMap"] as Map;
            ToggleLayerCommand = new DelegateCommand(ToggleLayer, OkToExecute);
            ExtentChangedCommand = new DelegateCommand(MyMapViewExtentChanged);
        }
        private void ToggleLayer(object parameter)
        {
            var lyr = this.map.Layers[parameter.ToString()];
            lyr.IsVisible = !(lyr.IsVisible);

        }
        private bool OkToExecute(object parameter)
        {
            var lyr = this.map.Layers[parameter.ToString()] as FeatureLayer;
            return (lyr != null);
        }
        public void MyMapViewExtentChanged(object parameter)
        {
            var mv = parameter as MapView;
            var extent = mv.Extent;
            CurrentExtentString = string.Format("XMin={0:F2} YMin={1:F2} XMax={2:F2} YMax={3:F2}",
                                                    extent.XMin, extent.YMin, extent.XMax, extent.YMax);
        }
        private void RaiseNotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

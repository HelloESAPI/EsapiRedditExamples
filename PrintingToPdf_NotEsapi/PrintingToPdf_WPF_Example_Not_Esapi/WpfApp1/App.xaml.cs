using EsapiDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PrintingToPdf_WPF_Example_Not_Esapi.ViewModels;
using PrintingToPdf_WPF_Example_Not_Esapi.Views;

namespace PrintingToPdf_WPF_Example_Not_Esapi
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    // this method will represent the "esapi" Execute(context, window) method
    private void Application_Startup(object sender, StartupEventArgs e)
    {

      // EsapiDataLibrary is a dummy library I have for examples when not running in eclipse environment...

      // dummy structure set model that will contain structures you might see in a prostate plan that will have random properties, i.e., volume, colors, etc.
      StructureSet structureSet = EsapiDataLibrary.Data.Structures.GetSampleStructureSet("Prostate", EsapiDataLibrary.Data.Structures.GetStructureSet("Prostate"), new Random());


      // the main window will represent the "esapi" window
      MainWindow window = new MainWindow();

      window.FontFamily = new System.Windows.Media.FontFamily("Calibri");
      window.FontSize = 14;
      window.SizeToContent = SizeToContent.WidthAndHeight;
      window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

      window.Content = new MainView { DataContext = new MainViewModel(structureSet) };

      // don't need this in the esapi binary plugin -> eclipse takes care of that
      window.Show();
    }
  }
}

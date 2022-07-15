using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using PrintingToPdf.Views;
using PrintingToPdf.ViewModels;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
// [assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
  public class Script
  {
    public Script()
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Execute(ScriptContext context, System.Windows.Window window/*, ScriptEnvironment environment*/)
    {

      // binary plugin example....hasn't been tested but should be similar

      // TODO : Add here the code that is called when the script is launched from Eclipse.

      // get some structures
      List<Structure> structures = context.StructureSet.Structures.Where(x => x.HasSegment && !x.IsEmpty).ToList();

      // create an instance of the main user control / view
      MainView mainView = new MainView { DataContext = new MainViewModel(context.StructureSet.Id, structures) };

      // set some window settings and content
      window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      window.FontFamily = new System.Windows.Media.FontFamily("Calibri");
      window.FontSize = 14;
      window.Content = mainView;
    }
  }
}

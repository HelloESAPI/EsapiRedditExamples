using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

[assembly: AssemblyVersion("1.0.0.1")]

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
      window.Margin = new Thickness(50);
      window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      window.FontSize = 14;
      window.FontFamily = new System.Windows.Media.FontFamily("Garamond");
      window.SizeToContent = SizeToContent.WidthAndHeight;
      // TODO : Add here the code that is called when the script is launched from Eclipse.

      Structure body = context.StructureSet.Structures.FirstOrDefault(x => x.DicomType == "EXTERNAL");

      if (body == null || context.PlanSetup == null || context.PlanSetup.Beams == null || context.PlanSetup.Beams.Count() < 1)
      {
        window.Content = new TextBlock
        {
          TextAlignment = TextAlignment.Center,
          Text = "Please ensure there is...\n" +
          "\ta Body contour\n" +
          "\ta plan with atleast one beam"
        };
      }
      else // continue
      {
        MeshGeometry3D bodyMesh = body.MeshGeometry;
        Point3DCollection bodyPositions = bodyMesh.Positions;
        VVector isocenter = context.PlanSetup.Beams.FirstOrDefault().IsocenterPosition;

        // get matching Y coordinates (assuming head first suppine)
        // the matching z means it's the same slice as the iso
        // the matching x means (at least in my system) it's the same lateral slice (left/right)
        // so the points it returns should in theory be two....the matchin anterior Y and matching posterior Y
        IEnumerable<Point3D> matchingPositions = bodyPositions.Where(pos => pos.X == isocenter.x && pos.Z == isocenter.z);

        // figure out which point3d is the ant and which is the post

        // for the anterior point...
        // Calculate distance from iso to the anterior point...
        // something like this?
        //var distance = isocenter.y - matchingPositions.FirstOrDefault().Y;
        // Subtract from 100 to get SSD at anterior position of the body at isocenter

        // do the same for the posterior point

        // with some maths (trig maybe?), you could calculate the distance from the iso to the body position at each gantry angle to get the ssd at each control point...

      }

    }
  }
}

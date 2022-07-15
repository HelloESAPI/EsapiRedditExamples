using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
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
    public void Execute(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
    {
      // TODO : Add here the code that is called when the script is launched from Eclipse.

      // just getting the first ptv it finds but would want to do this with the ptv you're interested
      Structure ptvOfInterest = context.StructureSet.Structures.FirstOrDefault(x => x.DicomType == "PTV");

      // seems the author wants to find the middle of the ptv they're interested in sup to inf (along the z axis)...and place the beam iso there.

      MeshGeometry3D ptvMesh = ptvOfInterest.MeshGeometry;

      // I feel like this would give you the middle of the bounds
      double middleZ = (ptvMesh.Bounds.Z + ptvMesh.Bounds.SizeZ) / 2;
      // guessing this isn't giving the exact value they want since it's divided by two...

      // not sure why they need the slice thickness...but you can get it from the image resolution in the z axis...
      double zRes = context.StructureSet.Image.ZRes;

      // maybe they need the middle or close to middle index...
      // number of slices -> total bound size in z direction / zRes?
      int numSlices = Convert.ToInt32((ptvMesh.Bounds.SizeZ - ptvMesh.Bounds.Z) / zRes);
      int approxMiddleSlice = (int)Math.Floor((double)numSlices / 2);
      // the corresponding z value I think would be...
      double zVal = ptvMesh.Bounds.SizeZ - (approxMiddleSlice * zRes);

      // or from simpler attempt...? would be able to skip lines 35 through 46...if it works/is good enough
      double zValNotExactZres = (ptvMesh.Bounds.Z + ptvMesh.Bounds.SizeZ) / 2;


      // use whatever x and y values that you want...
      // may need to subtract from the dicom or user origin...depending on what's required when setting iso
      double xOfInterest = 0;
      double yOfInterest = 0;
      var beamIso = new VVector(xOfInterest, yOfInterest, zVal);

      // i assume the "con" is the context but don't see that method...guessing it's a custom class?
      //context.getmesmiddle?



    }


    // original method from the post but with some additional techniques and comments...
    //double GetZValueOfPtvMiddle(Structure ptv_izo, StructureSet ss)
    //{

    //  // can we just use the user origin? instead of creating a new vvector?
    //  VVector p_Userorigin = new VVector(ss.Image.UserOrigin.x, ss.Image.UserOrigin.y, ss.Image.UserOrigin.z);

    //  // same? maybe just for less typing?
    //  VVector p_Imageorigin = new VVector(ss.Image.Origin.x, ss.Image.Origin.y, ss.Image.Origin.z);


    //  // for those, would suggest simplifying it to something like...
    //  VVector uo = ss.Image.UserOrigin;
    //  VVector io = ss.Image.Origin;
    //  // ...instead of recreating the vvectors with the x, y, and z



    //  var ptvBounds = ptv_izo.MeshGeometry.Bounds;

    //  //double ptvSizeZ = ptvBounds.SizeZ;


    //  // not sure what's being done in these steps...maybe getting the slices?
    //  //int ptvmeshLow = con.GetMeshLow(ptv_izo, ss); // not sure this is necessary
    //  //int ptvmeshUp = con.GetMeshUp(ptv_izo, ss); // not sure this is necessary
    //  //int ptvmeshMiddle = con.GetMeshMiddle(ptv_izo, ss); // not sure this is necessary




    //  // can do this with image.zres
    //  //double slicethikness = ptvSizeZ / (Math.Abs(ptvmeshUp - ptvmeshLow) + 1);
    //  double slicethikness = ss.Image.ZRes;




    //  // guessing this is necessary when setting isocenters...would have to test this...
    //  //double distanceoforigins = p_Userorigin.z - p_Imageorigin.z;


    //  // perhaps we can skip the above? and just do it in the step below?
    //  // it's either looking for the z value to be relative to the image origin or the user origin....guessing it's the image origin...

    //  // so could calculate the z value, then subtract that from the image origin...


    //  // maybe they need the middle or close to middle index...
    //  // number of slices -> total bound size in z direction / zRes?
    //  int numSlices = Convert.ToInt32((ptvBounds.SizeZ - ptvBounds.Z) / slicethikness);
    //  // can maybe use floor or ceiling or even just round...or jsut divide and set as strongly typed int?
    //  //int approxMiddleSlice = (int)Math.Floor((double)numSlices / 2);
    //  int approxMiddleSlice = numSlices / 2;
    //  //// the corresponding z value I think would be...
    //  double zVal = ptvBounds.SizeZ - (approxMiddleSlice * slicethikness);
    //  // would need to be tested...i think it makes sense but could be wrong...


    //  // seems the x and y are not an issue...
    //  // use whatever x and y values that you want...
    //  // may need to subtract from the dicom or user origin...depending on what's required when setting iso
    //  //double xOfInterest = 0;
    //  //double yOfInterest = 0;
    //  // create the vvector to be used in adding the beams...or setting the iso
    //  //VVector beamIso = new VVector(xOfInterest, yOfInterest, zVal);


    //  // actually was only returning the z....
    //  return zVal;



    //  //double distancemiddleuserorigin = distanceoforigins - (ptvmeshMiddle * slicethikness);

    //  //return distancemiddleuserorigin;

    //}

    double GetZValueOfPtvMiddle(Structure ptv_izo, StructureSet ss)
    {
      // ptv bounds
      Rect3D ptvBounds = ptv_izo.MeshGeometry.Bounds;

      // can we do this more simply? Is it necessary to have an exact z/slice that matches the z resolution? Does eclipse do this as well?
      // if eclipse doesn't do it, or if it's not necessary, just return the z here
      // and can skip all the other stuffs...
      return (ptvBounds.Z + ptvBounds.SizeZ) / 2; 
      // this seems to be how i would try first...
      // this would also already be relative to the image origin...so maybe it actually is relative to the user origin instead? in which case you could just subtract from that...after testing of course :)



      //// can do these things maybe to get a z value that exactly matches the resolution...I'm not convinced it's necessary but perhaps it is or is useful in some other way...


      //// guessing it's the image origin that's needed as opposed to user origin
      //VVector imageOrigin = ss.Image.Origin;
      //// slice thickness (resolution in the z axis)
      //double slicethikness = ss.Image.ZRes;


      ////if need z that matches the resolution....
      //// number of slices -> total bound size in z direction / zRes?
      //int numSlicesInStructure = Convert.ToInt32((ptvBounds.SizeZ - ptvBounds.Z) / slicethikness);
      //// approximation of middle slice
      //int approxMiddleSlice = numSlicesInStructure / 2;
      //// convert slice back to z value 
      //return imageOrigin.z - (ptvBounds.SizeZ - (approxMiddleSlice * slicethikness));
      //// would need to be tested...i think it makes sense but could be wrong...
    }

  }
}

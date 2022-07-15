using EsapiDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingToPdf_WPF_Example_Not_Esapi.Models
{
  public class StructureListModel
  {
    public string StructureSetId { get; set; }
    public List<Structure> Structures { get; set; }

    public StructureListModel(StructureSet structureSet)
    {
      StructureSetId = structureSet.Id;
      Structures = structureSet.Structures;
    }
  }
}

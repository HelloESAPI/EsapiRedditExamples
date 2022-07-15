using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace PrintingToPdf.Models
{
  /// <summary>
  /// Model to hold the data used for the view when printing
  /// </summary>
  public class StructureListModel
  {
    public string StructureSetId { get; set; }
    public List<Structure> Structures { get; set; }
  }
}

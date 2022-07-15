using PrintingToPdf.Models;
using PrintingToPdf.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using VMS.TPS.Common.Model.API;

namespace PrintingToPdf.ViewModels
{
  public class MainViewModel : BindableBase
  {
    // private backing fields are needed when updating the view...if only updated at the start, prob not needed
    private List<Structure> _structures;
    private string _structureSetId;
    public MainViewModel(string structureSetId, List<Structure> structures)
    {
      StructureSetId = structureSetId;
      PrintCommand = new DelegateCommand(() => Print());
      Structures = structures;
    }

    private void Print()
    {
      FlowDocument fd = new FlowDocument { FontSize = 14, FontFamily = new System.Windows.Media.FontFamily("Calibri") };

      fd.Blocks.Add(new BlockUIContainer(new StructureDataView { DataContext = new StructureListModel { StructureSetId = StructureSetId, Structures = Structures } }));

      System.Windows.Controls.PrintDialog printer = new System.Windows.Controls.PrintDialog();
      //printer.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
      fd.PageHeight = 1056;
      fd.PageWidth = 816;
      fd.PagePadding = new System.Windows.Thickness(0);
      fd.ColumnGap = 0;
      fd.ColumnWidth = 816;
      IDocumentPaginatorSource source = fd;
      if (printer.ShowDialog() == true)
      {
        printer.PrintDocument(source.DocumentPaginator, $"StructureList");
      }
    }

    // the public property that's referenced/bound in the view
    public List<Structure> Structures
    {
      get { return _structures; }
      set { SetProperty(ref _structures, value); }
    }

    public string StructureSetId
    {
      get { return _structureSetId; }
      set { SetProperty(ref _structureSetId, value); }
    }
    public DelegateCommand PrintCommand { get; set; }
  }
}

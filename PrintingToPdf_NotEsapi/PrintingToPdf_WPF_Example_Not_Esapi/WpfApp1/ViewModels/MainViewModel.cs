using EsapiDataLibrary.Models;
using PrintingToPdf_WPF_Example_Not_Esapi.Models;
using PrintingToPdf_WPF_Example_Not_Esapi.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PrintingToPdf_WPF_Example_Not_Esapi.ViewModels
{
  // Prism used for Delegate Commands, Binding - can also use OnPropertyChanged
  // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-implement-property-change-notification?view=netframeworkdesktop-4.8
  // Prism makes following MVVM easier though for using button commands, etc. 
  public class MainViewModel : BindableBase
  {
    // for binding properties that need to be updated in the view, you need (or I think you need) a public property with a private backing field and the public property needs the get; set; properties. 

    // private fields
    private string _structureSetId;

    private List<Structure> _structures;





    // constructor
    public MainViewModel(StructureSet structureSet)
    {
      StructureSetId = structureSet.Id;
      Structures = structureSet.Structures;

      // set print command
      PrintCommand = new DelegateCommand(() => Print(structureSet));

    }

    private void Print(StructureSet structureSet)
    {
      // I believe this example is taken from other examples from Matt Schmidt (or someone in the esapi reddit community or possibly other varian/github examples....i don't remember)
      FlowDocument fd = new FlowDocument { FontFamily = new System.Windows.Media.FontFamily("Calibri"), FontSize = 14 };

      fd.Blocks.Add(new BlockUIContainer(
        new StructureDataView
        {
          DataContext = new StructureListModel(structureSet)
        }));

      PrintDialog printer = new PrintDialog();
      fd.PageHeight = 1056;
      fd.PageWidth = 816;
      fd.PagePadding = new Thickness(0);
      fd.ColumnGap = 0;
      fd.ColumnWidth = 816;
      IDocumentPaginatorSource source = fd;
      if (printer.ShowDialog() == true)
      {
        printer.PrintDocument(source.DocumentPaginator, $"{structureSet.Id}_StructureList");
      }
    }



    // public properties

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

    // delegate commands - from prism
    public DelegateCommand PrintCommand { get; set; }


    //// or propfull tab tab
    //private int _structureSet;

    //public int StructureSet
    //{
    //  get { return _structureSet; }
    //  set { SetProperty(ref _structureSet, value); }
    //}

  }
}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using InpadPlugins.RevitElementsHierarchy.Models;
using InpadPlugins.RevitElementsHierarchy.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InpadPlugins.RevitElementsHierarchy
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RevitCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            try
            {
                Transaction trans = new Transaction(doc);
                trans.Start("Lab");

                Reference pickedRef = null;

                FilteredElementCollector collector = new FilteredElementCollector(doc);

                var new1 = collector.WhereElementIsNotElementType().Where(x => x.Category != null).ToList();


                //new1[0].GetType();

                //var t1 = from i in new1 group i by i.GetType();
                var t1 = new1.Where(x => x.GetTypeId().IntegerValue != -1).GroupBy(x => x.GetTypeId());

                var t2 = from i in t1 group i by i.ElementAt(0).Category.Id;

                //var t3 = new1.FirstOrDefault().Id;
                //var t4 = Category.GetCategory(doc, new ElementId(-2000110));

                ObservableCollection<Inst> Insts = new ObservableCollection<Inst>();
                ObservableCollection<Inst> Types = new ObservableCollection<Inst>();
                ObservableCollection<Inst> Elem = new ObservableCollection<Inst>();
                Inst UnknownCat = new Inst { Name = "Unknown" };
                Inst currentCategory;
                Inst currentType;
                bool isUnknownCat;

                foreach (var cat in t2)
                {
                    try
                    {
                        currentCategory = new Inst { Name = Category.GetCategory(doc, cat.Key).Name };
                    }
                    catch (NullReferenceException ex)
                    {
                        isUnknownCat = true;
                        currentCategory = new Inst { Name = "Unknown" };
                    }
                    foreach (var type in cat)
                    {
                        currentType = new Inst { Name = doc.GetElement(type.Key).Name };
                        foreach (var inst in type)
                        {
                            Elem.Add(new Inst { Name = inst.Name, Parameters = GetParameters1(inst) });
                        }
                        currentType.InstItem = Elem;
                        Elem = new ObservableCollection<Inst>();
                        Types.Add(currentType);
                    }
                    currentCategory.InstItem = Types;
                    Types = new ObservableCollection<Inst>();
                    Insts.Add(currentCategory);
                }
                MainWindow mainWindow = new MainWindow();
                (mainWindow.DataContext as ViewModels.ViewModel).Insts=Insts;
                mainWindow.ShowDialog();
                //string walls = "";
                trans.Commit();
            }

            //If the user right-clicks or presses Esc, handle the exception
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                message = ex.Message+ex.StackTrace;
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        ObservableCollection<Models.Parameter> GetParameters1(Element element)
        {
            ObservableCollection<Models.Parameter> res = new ObservableCollection<Models.Parameter>();

            foreach (Autodesk.Revit.DB.Parameter p in element.Parameters)
            {
                string value;
                if (p.StorageType == StorageType.None) continue;
                if (p.StorageType == Autodesk.Revit.DB.StorageType.String)
                    value = element.get_Parameter(p.Definition).AsString();
                else
                    value = element.get_Parameter(p.Definition).AsValueString();

                res.Add(new Models.Parameter { Name = p.Definition.Name, Value = value });
            }
            ObservableCollection<Models.Parameter> result = new ObservableCollection<Models.Parameter>(res.OrderBy(x => x.Name));

            return result;
        }
    }
}

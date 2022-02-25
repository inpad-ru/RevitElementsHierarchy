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
                bool isUnknownCat=false;

                foreach (var cat in t2)
                {
                    try
                    {
                        currentCategory = new Inst { Name = Category.GetCategory(doc, cat.Key).Name };
                    }
                    catch (NullReferenceException ex)
                    {
                        isUnknownCat = true;
                        currentCategory = new Inst();
                    }
                    foreach (var type in cat)
                    {
                        currentType = new Inst { Name = doc.GetElement(type.Key).Name };
                        foreach (var inst in type)
                        {
                            Elem.Add(new Inst { Name = inst.Name, Element = inst, Parameters = GetParameters1(inst) });
                        }
                        currentType.InstItem = Elem;
                        Elem = new ObservableCollection<Inst>();
                        Types.Add(currentType);
                    }
                    if (isUnknownCat)
                    {
                        if(UnknownCat.InstItem == null) 
                            UnknownCat.InstItem = Types;
                        else
                            UnknownCat.InstItem = new ObservableCollection<Inst>(UnknownCat.InstItem.Union(Types));
                        isUnknownCat = false;
                    }
                    else
                    {
                        currentCategory.InstItem = Types;
                        Insts.Add(currentCategory);
                    }
                    Types = new ObservableCollection<Inst>();
                }
                Insts.Add(UnknownCat);
                MainWindow mainWindow = new MainWindow();
                (mainWindow.DataContext as ViewModels.ViewModel).Insts=Insts;
                mainWindow.ShowDialog();

                foreach (var cat in Insts)
                    foreach (var type in cat.InstItem)
                        foreach (var inst in type.InstItem)
                            foreach (var par in inst.Parameters)
                            {
                                if (par.OldValue == null || par.Value == null) continue;
                                if (!par.OldValue.Equals(par.Value))
                                    if (!SetParameter1(inst.Element, par))
                                    { 
                                        //throw new Exception(); 
                                    }
                            }

                //SetParameter1();
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

                res.Add(new Models.Parameter { Name = p.Definition.Name, RevParameter = p, Value = value , OldValue = value});
            }
            ObservableCollection<Models.Parameter> result = new ObservableCollection<Models.Parameter>(res.OrderBy(x => x.Name));

            return result;
        }

        bool SetParameter1(Element element, Models.Parameter parameter)
        {
            try
            {
                if (parameter.RevParameter.IsReadOnly)
                    return false;
                
                    switch (parameter.RevParameter.StorageType)
                    {
                        case StorageType.String:
                            return element.get_Parameter(parameter.RevParameter.Definition).Set(parameter.Value);
                        case StorageType.Double:
                            return element.get_Parameter(parameter.RevParameter.Definition).Set(Convert.ToDouble(parameter.Value));
                        case StorageType.Integer:
                            return element.get_Parameter(parameter.RevParameter.Definition).Set(Convert.ToInt32(parameter.Value));
                        default: return false;
                    }
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

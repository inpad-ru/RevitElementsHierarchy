// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RevitElementsHierarchy
 * ExternalApplication.cs
 * https://inpad.ru
 * © Inpad, 2022
 *
 * The external application. This is the entry point of the
 * 'RevitElementsHierarchy' (Revit add-in).
 */
#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using InpadPlugins.RevitElementsHierarchy.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using WPF = System.Windows;

#endregion

namespace InpadPlugins.RevitElementsHierarchy
{
    /// <summary>
    /// Revit external application.
    /// </summary>  
    public sealed partial class ExternalApplication
        : IExternalApplication
    {

        /// <summary>
        /// This method will be executed when Autodesk Revit 
        /// will be started.
        /// 
        /// WARNING
        /// Don't use the RevitDevTools.dll features directly 
        /// in this method. You are to call other methods which
        /// do it instead of.
        /// </summary>
        /// <param name="uic_app">A handle to the application 
        /// being started.</param>
        /// <returns>Indicates if the external application 
        /// completes its work successfully.</returns>
        Result IExternalApplication.OnStartup(
            UIControlledApplication uic_app)
        {

            ResourceManager res_mng = new ResourceManager(GetType());
            ResourceManager def_res_mng = new ResourceManager(typeof(Properties.Resources));

            Result result = Result.Succeeded;

            try
            {
                // TODO: put your code here.

            }
            catch (Exception ex)
            {

                TaskDialog.Show(def_res_mng.GetString("_Error"), ex.Message);

                result = Result.Failed;
            }
            finally
            {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }

            return result;
        }

        /// <summary>
        /// This method will be executed when Autodesk Revit 
        /// shuts down.</summary>
        /// <param name="uic_app">A handle to the application 
        /// being shut down.</param>
        /// <returns>Indicates if the external application 
        /// completes its work successfully.</returns>
        Result IExternalApplication.OnShutdown(
            UIControlledApplication uic_app)
        {

            ResourceManager res_mng = new ResourceManager(GetType());
            ResourceManager def_res_mng = new ResourceManager(typeof(Properties.Resources));

            Result result = Result.Succeeded;

            try
            {
                // TODO: put your code here.

            }
            catch (Exception ex)
            {

                TaskDialog.Show(def_res_mng.GetString("_Error"), ex.Message);

                result = Result.Failed;
            }
            finally
            {

                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }

            return result;
        }        
    }
}

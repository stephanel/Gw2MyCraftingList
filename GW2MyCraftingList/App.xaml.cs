using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace GW2ExplorerCraftTool
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        App(){ }

        public static string Name
        {
            get
            {
                Assembly assembly =Application.Current.MainWindow.GetType().Assembly;
                string productName=null;

                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    productName = ((AssemblyProductAttribute)customAttributes[0]).Product;
                }
                if (string.IsNullOrEmpty(productName))
                {
                    productName = assembly.GetName().Name;
                }

                AssemblyName an = assembly.GetName();
                string version = String.Format("{0}.{1}", an.Version.Major, an.Version.Minor);
                return String.Format("{0} - {1}", productName, version);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleException);

            // Set application startup culture
            SetCulture();           
        }

        public static void Restart()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public static void SetCulture()
        {
            bool cultureChanged = Config.CultureInfo.IetfLanguageTag != Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            if (!cultureChanged)
                return;

            Thread.CurrentThread.CurrentCulture = Config.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = Config.CultureInfo;
        }

        public static void HandleException(object sender, UnhandledExceptionEventArgs e)
        {
            string txt;
            if (e.IsTerminating)
            {
                txt = String.Format("Application terminating cause of unhandled exception: {0}", e.ExceptionObject);
            }
            else
            {
                txt = ((Exception)e.ExceptionObject).Message;
            }
            System.Windows.MessageBox.Show(txt);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Windows.Forms;

namespace HKeInvestWebApplication
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Thread mythread = new Thread(PeriodicTask);
            mythread.IsBackground = true;
            mythread.Start();
        }
        private void PeriodicTask()
        {
        do
            {
                // Place the method call for the periodic task here.
                HKeInvestData myHKeInvestData = new HKeInvestData();
                HKeInvestCode myHKeInvestCode = new HKeInvestCode();
                ExternalFunctions myExternalFunctions = new ExternalFunctions();

                myHKeInvestCode.updateAlert();
                myHKeInvestCode.checkAlert();
                myHKeInvestCode.RefreshStatus();
                //MessageBox.Show(new Form { TopMost = true }, myHKeInvestCode.RefreshStatus());

                Thread.Sleep(15000);
            } while (true);
        }
    }
}
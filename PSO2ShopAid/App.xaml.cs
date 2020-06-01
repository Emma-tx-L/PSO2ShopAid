using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PSO2ShopAid
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void Application_Exit(object sender, ExitEventArgs e)
        {
            DataManager.Save();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DataManager.Save();
            base.OnExit(e);
        }
    }
}

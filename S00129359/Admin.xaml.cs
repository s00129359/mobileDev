using Microsoft.WindowsAzure.MobileServices;
using S00129359.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace S00129359
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin : Page
    {
        //route
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();
        public Admin()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadRoutes();
            //lstRoutes.Items.Add("1. IT Sligo TO Rosses Point");
        }

        private async void loadRoutes()
        {
            List<Route> Routes = await routeTbl
                .ToListAsync();

            foreach (var route in Routes)
            {
                string routeData = route.Route_id + ". " + route.Departs + " TO " + route.Arrives;
                lstRoutes.Items.Add(routeData);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddRoute));
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            string Selcted = lstRoutes.SelectedItem.ToString();
            int indx = Selcted.LastIndexOf(".");
            string routeId = Selcted.Substring(0, indx);
            
            Frame.Navigate(typeof(EditRoute), routeId);
        }

        private void lstRoutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = true;
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Profile));
        }
    }
}

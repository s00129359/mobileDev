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
    public sealed partial class EditRoute : Page
    {
        public int routeId;
        string stringId;
        private IMobileServiceTable<Journey> journeyTbl = App.MobileService.GetTable<Journey>();
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();

        public EditRoute()
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
           // string sRouteId = e.Parameter as string;
           // routeId = Convert.ToInt32(sRouteId);
            Params prm = e.Parameter as Params;
            if (prm != null)
            {
                routeId = prm.routeId;
            }
            Route();
            getJourneys();
        }

        private async void Route()
        {
            List<Route> routes = await routeTbl
                .Where(r => r.Route_id == routeId)
                .ToListAsync();
            foreach (var route in routes)
            {
                tbRoute.Text = route.Departs + " To " + route.Arrives;
            }
        }

        private async void getJourneys()
        {
            List<Journey> journeys = await journeyTbl
                .Where(j => j.Route_id == routeId)
                .ToListAsync();

            foreach (var journey in journeys)
            {
                stringId = journey.id;
                string data = journey.Journey_id + ". " + "Departs " + journey.DepartureTime + " Arrives " + journey.ArrivalTime;
                lstJourneys.Items.Add(data);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            string param = routeId.ToString();
            Frame.Navigate(typeof(AddJourney), param);
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Admin));
        }

        private void lstJourneys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            string reportSelcted = lstJourneys.SelectedItem.ToString();
            int indx = reportSelcted.LastIndexOf(".");
            string journeyId = reportSelcted.Substring(0, indx);

            Params prm = new Params { routeId = routeId, JourneyId = Convert.ToInt32(journeyId) };

            Frame.Navigate(typeof(EditJourney), prm);
        }

        private async void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            //delte the route you have clicked int0
            Route deleteRoute = await routeTbl
                .LookupAsync(stringId);
                //.Where(r => r.Route_id == routeId);
                //.Where(r => r.id == "scf");

            ////http://stackoverflow.com/questions/32118966/how-to-delete-particular-record-in-azure-mobile-service

            await routeTbl.DeleteAsync(deleteRoute);

            Frame.Navigate(typeof(Admin));
        }
    }
}

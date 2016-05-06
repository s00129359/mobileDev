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
    public sealed partial class AddRoute : Page
    {
        private IMobileServiceTable<Journey> journeyTbl = App.MobileService.GetTable<Journey>();
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();

        public AddRoute()
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
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Admin));
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            AddToRoute();
            Frame.Navigate(typeof(Admin));
        }

        int nextId;

        private async void AddToRoute()
        {
            //get highest routeId in database
            //new id must be one higher

            //again i done this for an increment as I couldnt find another 
            //way to increment. so got the hoghtest current Id and added 1
            int highestId = 0;
            List<Route> route = await routeTbl
                .OrderBy(r => r.Route_id)
                .ToListAsync();
            foreach (var rte in route)
            {
                highestId = rte.Route_id;
            }

            nextId = 1 + highestId;
            //tbArrive.Text = nextId.ToString();
            int cost = Convert.ToInt32(tbCost.Text);
            var addNewRoute = new Route() { Route_id = nextId, Departs = tbDepart.ToString(), Arrives = tbArrive.Text, Cost = cost };

            await routeTbl.InsertAsync(addNewRoute);

            //add new journey
            bool monday = false;
            bool tuesday = false;
            bool wednesday = false;
            bool thursday = false;
            bool friday = false;

            if (cbxMonday.IsChecked == true)
            {
                monday = true;
            }
            if (cbxTuesday.IsChecked == true)
            {
                tuesday = true;
            }
            if (cbxWednesday.IsChecked == true)
            {
                wednesday = true;
            }
            if (cbxThursday.IsChecked == true)
            {
                thursday = true;
            }
            if (cbxFriday.IsChecked == true)
            {
                friday = true;
            }
            //add first journey for the route. therfore id will be 1
            var addNewJourney = new Journey() { Route_id = nextId, Journey_id = 1, DepartureTime = DateTime.Parse(tbDepartTime.Time.ToString()).ToString("HH:mm"), ArrivalTime = DateTime.Parse(tbArriveTime.Time.ToString()).ToString("HH:mm"), Monday = monday, Tuesday = tuesday, Wednesday = wednesday, Thursday = thursday, Friday = friday };

            await journeyTbl.InsertAsync(addNewJourney);
        }

    }
}

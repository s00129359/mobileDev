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
    public sealed partial class Journeys : Page
    {

        //Declare  passed vars
        public int routeId;
        public string day = "Monday";
        //could be empty if no reuturn
        public string returnDay = "Sunday";

        //single ticket by default
        public string ticketType = "Single";

        private IMobileServiceTable<Journey> journeyTbl = App.MobileService.GetTable<Journey>();
        //route
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();

        public Journeys()
        {
            this.InitializeComponent();


            ////while no azure 
            //tblJourney.Text = "IT Sligo to Rosses Point";
            //tblJourneyDay.Text = "On Wednesday";

            //tblJourney1.Text = "IT Sligo to Rosses Point";
            //tblJourneyDay1.Text = "On Thursday";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Params prm = e.Parameter as Params;
            if (prm != null)
            {
                routeId = prm.routeId;
                day = prm.date;
                returnDay = prm.returnDate;
                ticketType = prm.ticketType;

            }

            PopulateJourney();
            PopulateReturnJourney();
        }

        private void NavigationHelper_LoadState(object sender, NavigationEventArgs e)
        {
            Params prm = e.Parameter as Params;
            if (prm != null)
            {
                routeId = prm.routeId;
                day = prm.date;
                returnDay = prm.returnDate;
                ticketType = prm.ticketType;

            }

            PopulateJourney();
            PopulateReturnJourney();

        }

        private async void PopulateJourney()
        {
            List<Route> itm = await routeTbl
                 .Where(id => id.Route_id == routeId)
                 .ToListAsync();

            string depart = "", arrive = "";

            if (0 == 0)
            {
                foreach (var route in itm)
                {
                    depart = route.Departs;
                    arrive = route.Arrives;
                }
            }

            tblJourney.Text = depart + " TO " + arrive;

            //first journey
            tblJourneyDay.Text = "On a " + day;

            List<Journey> journ = await journeyTbl
                .Where(r => r.Route_id == routeId)
                .ToListAsync();

            string depTime, arrTime;
            int journeyId;
            foreach (var jour in journ)
            {
                journeyId = jour.Journey_id;
                arrTime = jour.ArrivalTime;
                depTime = jour.DepartureTime;
                string list = "Departs : " + depTime + ", Arrives : " + arrTime;

                lstDepart.Items.Add(list);
            }

        }

        private async void PopulateReturnJourney()
        {
            if (ticketType == "Single")
            {
                tblJourney1.Text = "No Return";
                lstDepart.ItemsSource = ("No Return Journey");
                lstDepart.IsEnabled = false;
            }
            else
            {
                List<Route> itm = await routeTbl
                     .Where(id => id.Route_id == routeId)
                     .ToListAsync();

                string depart = "", arrive = "";

                if (0 == 0)
                {
                    foreach (var route in itm)
                    {
                        depart = route.Departs;
                        arrive = route.Arrives;
                    }
                }

                //change the depart and return 
                //for return journey
                tblJourney1.Text = arrive + " TO " + depart;

                //return journey
                tblJourneyDay1.Text = "On a " + returnDay;

                List<Journey> journ = await journeyTbl
                    .Where(r => r.Route_id == routeId)
                    .ToListAsync();

                string depTime, arrTime;

                if (true)
                {
                    foreach (var jour in journ)
                    {
                        depTime = jour.ArrivalTime;
                        arrTime = jour.DepartureTime;
                        string list = "Departs : " + depTime + ", Arrives : " + arrTime;

                        lstReturn.Items.Add(list);
                    }
                }
            }

        }


        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

            Params prm = new Params { routeId = routeId, date = day, returnDate = returnDay, ticketType = ticketType };

            Frame.Navigate(typeof(Confirm), prm);
        }
    }
}

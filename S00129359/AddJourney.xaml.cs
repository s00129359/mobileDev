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
    public sealed partial class AddJourney : Page
    {
        private IMobileServiceTable<Journey> journeyTbl = App.MobileService.GetTable<Journey>();
        int routeId;
        public AddJourney()
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
            //retrieve an Id for the route this 
            //journey will belong to
            string param = e.Parameter as string;
            routeId = Convert.ToInt32(param);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditRoute));
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            AddNewJourney();
            Frame.Navigate(typeof(Admin));
        }

        public int jounreyId;

        private async void AddNewJourney()
        {
            //again as no autoincrement
            //get highest journey id 
            //add one for new journey Id
            List<Journey> journey = await journeyTbl
                .OrderBy(jid => jid.Journey_id)
                .ToListAsync();

            foreach (var journId in journey)
	        {
		        jounreyId = journId.Journey_id;
	        }

            int newJourneyId = jounreyId +1;

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
            
            //get from inputs and insert
            var addJounrey = new Journey() { Route_id = routeId, Journey_id = newJourneyId, DepartureTime = DateTime.Parse(tbDeaprt.Time.ToString()).ToString("HH:mm"), ArrivalTime = DateTime.Parse(tbArrive.Time.ToString()).ToString("HH:mm"), Monday = monday, Tuesday = tuesday, Wednesday = wednesday, Thursday = thursday, Friday = friday };

            await journeyTbl.InsertAsync(addJounrey);

        }
    }
}

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
    public sealed partial class EditJourney : Page
    {
        int journeyid;
        int routeId;
        private IMobileServiceTable<Journey> journeyTbl = App.MobileService.GetTable<Journey>();

        public EditJourney()
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
            Params prm = e.Parameter as Params;
            if (prm != null)
            {
                routeId = prm.routeId;
                journeyid = prm.JourneyId;
            }

            JourneyDetails();
        }

        private async void JourneyDetails()
        {
            List<Journey> journey = await journeyTbl
                .Where(jid => jid.Journey_id == journeyid)
                .ToListAsync();

            foreach (var journeyDetail in journey)
            {
                string dpt = journeyDetail.DepartureTime;
                string arv = journeyDetail.ArrivalTime;
                DateTime departTime = DateTime.Parse(dpt);
                DateTime arriveTime = DateTime.Parse(arv);
                
                tbArrive.Time = arriveTime.TimeOfDay;
                tbDeaprt.Time = departTime.TimeOfDay;

                //check the box for each day the service runs on
                if (journeyDetail.Monday == true)
                {
                    cbxMonday.IsChecked = true;
                }
                if (journeyDetail.Tuesday == true)
                {
                    cbxTuesday.IsChecked = true;
                }
                if (journeyDetail.Wednesday == true)
                {
                    cbxWednesday.IsChecked = true;
                }
                if (journeyDetail.Thursday == true)
                {
                    cbxThursday.IsChecked = true;
                }
                if (journeyDetail.Friday == true)
                {
                    cbxFriday.IsChecked = true;
                }
                if (journeyDetail.Saturday == true)
                {
                    cbxSaturday.IsChecked = true;
                }
                if (journeyDetail.Sunday == true)
                {
                    cbxSunday.IsChecked = true;
                }
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Params prm = new Params { routeId = routeId };

            Frame.Navigate(typeof(EditRoute), prm);
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            edit();
        }

        private async void edit()
        {

            List<Journey> ticket = await journeyTbl
                .Where(id => id.Journey_id == journeyid)
                .ToListAsync();
            var tickEdit = ticket.FirstOrDefault();
            if (tickEdit != null)
            {
                tickEdit.ArrivalTime = DateTime.Parse(tbArrive.Time.ToString()).ToString("HH:mm");
                tickEdit.DepartureTime = DateTime.Parse(tbDeaprt.Time.ToString()).ToString("HH:mm");
                

                if (cbxMonday.IsChecked == true)
                {
                    tickEdit.Monday = true;
                }
                if (cbxTuesday.IsChecked == true)
                {
                    tickEdit.Tuesday = true;
                }
                if (cbxWednesday.IsChecked == true)
                {
                    tickEdit.Wednesday = true;
                }
                if (cbxThursday.IsChecked == true)
                {
                    tickEdit.Thursday = true;
                }
                if (cbxFriday.IsChecked == true)
                {
                    tickEdit.Friday = true;
                }

                await journeyTbl.UpdateAsync(tickEdit);

                Frame.Navigate(typeof(Admin));

            }
        }
    }
}

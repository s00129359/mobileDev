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
    public sealed partial class Confirm : Page
    {

        //user
        private IMobileServiceTable<User> userTbl = App.MobileService.GetTable<User>();
        //route
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();
        //ticket
        private IMobileServiceTable<Ticket> ticketTbl = App.MobileService.GetTable<Ticket>();

        public int RouteId;
        public string TicketType;
        public string Depart;
        public string Arrive;
        public int Cost;
        public int credits;

        string sendParam;

        //user logged in
        //when authentication added
        //this var will be = to whoever is logged in
        public int UserLoggedIn = 1;

        public Confirm()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Params prm = e.Parameter as Params;
            if (prm != null)
            {
                RouteId = prm.routeId;
                TicketType = prm.ticketType;
            }

            GetUsersCredits();
            RouteDetails();
            FillPage();
        }


        private async void GetUsersCredits()
        {
            List<User> customer = await userTbl
                .Where(id => id.UserId == UserLoggedIn)
                .ToListAsync();


            foreach (var cred in customer)
            {
                credits = cred.Credits;

            }
            //write to text bloxk how many credits in user account
            credsRemaining.Text += credits;

            //if user has enough credits
            //user has less crediits than cost
            if (credits < Cost)
            {
                tBxEnoughCredits.Text = "Not enough credits";
                btnConfirm.IsEnabled = false;
            }
            else if (credits > Cost)
            {
                tBxEnoughCredits.Text = "You can purchase";
            }
        }

        private async void RouteDetails()
        {
            List<Route> itm = await routeTbl
                .Where(id => id.Route_id == RouteId)
                .ToListAsync();

            foreach (var item in itm)
            {
                Depart = item.Departs;
                Arrive = item.Arrives;
                Cost = item.Cost;
            }
            tBxDeparts.Text = Depart;
            tBxCost.Text = Cost.ToString();
            tBxArrive.Text = Arrive;

        }

        private void FillPage()
        {
            // credsRemaining += 
            if (TicketType == "Return")
            {
                tBxReturn.Text = "Yes";
            }
            else if (TicketType == "Single")
            {
                tBxReturn.Text = "No";
            }

            tBxDate.Text = DateTime.Now.Date.ToString("d");
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateUser();

            AddTicket();

            //on ticket page
           // Frame.Navigate(typeof(ViewTicket));
        }

        private async void AddTicket()
        {
            // I could not find a way to auto increment
            //so decided to get the highest ID in database
            //highest will be the last ticket addded
            //therefore add 1, gets a new id
            //ticket can then be inserted with this Id
            //this is the new ID and incremented next time a ticket is added
            //*not ideal but the best I could do in the time to get working without hard coding in data*
            int highestId = 0;
            List<Ticket> route = await ticketTbl
                .OrderBy(r => r.ticketId)
                .ToListAsync();
            foreach (var rte in route)
            {
                highestId = rte.ticketId;
            }

            int nextId = 1 + highestId;

             var ds = new Ticket() { ticketId=nextId, custId = UserLoggedIn, RouteId = RouteId, PurchaseDate = DateTime.Now.Date, TicketType = TicketType };

             await ticketTbl.InsertAsync(ds);

            //send param as string
            //easier than creating new classes
             sendParam = nextId.ToString();


        }

        private async void UpdateUser()
        {
            //calculates new credit amount
            var userCreds = await userTbl
                .Where(user => user.UserId == UserLoggedIn)
                .ToCollectionAsync();

            var use = userCreds.FirstOrDefault();

                use.Credits -= Cost;
                await userTbl.UpdateAsync(use);


            //navigate with send param
            Frame.Navigate(typeof(ViewTicket), sendParam);

        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}

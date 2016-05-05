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
using Microsoft.WindowsAzure.MobileServices;
using S00129359.Data;
using Windows.Graphics.Display;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace S00129359
{
    public sealed partial class MainPage : Page
    {
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();
        private IMobileServiceTable<User> userTbl = App.MobileService.GetTable<User>();
        private IMobileServiceTable<Journey> journeyTbl = App.MobileService.GetTable<Journey>();

        public MainPage()
        {

            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            //disable screen rotate
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            Routes();
            UpdteUser();

            //while azure gone
            //cbxDepart.Items.Add("1. IT Sligo To Rosses Point");
            //cbxDepart.Items.Add("2. Grange To IT Sligo");


            TicketType();

            PopulateDays();

            //only for prepoulation data to use and test
            // Add();

        }

        private async void Add()
        {
            // One Example of each

            //var ds = new User() { FirstName = "Cormac", SeondName = "Hallinan", Email = "s00129359@mail.itsligo.ie", Password = "12345" };
            //await userTbl.InsertAsync(ds);

            //var route = new Route() { Route_id = 1, Departs = "IT Sligo", Arrives = "Rosses Point", Cost = 4 };
            //await routeTbl.InsertAsync(route);

            //var journey = new Journey { Route_id = 1, DepartureTime = "09:00", ArrivalTime = "09:30", Monday = true, Tuesday = true, Wednesday = true, Thursday = true, Friday = true, Saturday = false, Sunday = false };
            //await journeyTbl.InsertAsync(journey);

        }

        private void cbxDepart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // tbName.Text = cbxDepart.SelectedValuePath.ToString();
        }

        private void cbxTicket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string TicketType = Convert.ToString(cbxTicket.SelectedValue);

            if (TicketType == "Return")
            {
                cbxReturnDate.IsEnabled = true;
                cbxTicket.PlaceholderText = "Return";
            }
            else if (TicketType == "Single")
            {
                cbxReturnDate.IsEnabled = false;
                cbxTicket.PlaceholderText = "Single";
            }

        }

        private async void AddRoutes()
        {

            //   //  = new Route();
            var ds = new Route() { Route_id = 5, Departs = "Rosses Point", Arrives = "IT Sligo", Cost = 2 };

            await routeTbl.InsertAsync(ds);

            //   // tbxBox.Text = "gt";
        }

        public async void Routes()
        {

            List<Route> itm = await routeTbl
                .ToListAsync();
            //decalre combobox items and value vars
            int id = 0;
            string depart;
            string arrive;

            foreach (var route in itm)
            {
                //get each row
                id = route.Route_id;
                depart = route.Departs;
                arrive = route.Arrives;

                //create comboboxitem
                ComboboxItem item = new ComboboxItem();
                item.Text = depart + " To " + arrive;
                item.Value = id;

                //bind item to combobox
                cbxDepart.Items.Add(item);
            }

            cbxDepart.SelectedIndex = 0;

        }

        public async void UpdteUser()
        {
       // wanted to update users credits so he has some be default

            var user = await userTbl
                .Where(e => e.Email == "S00129359@mail.itsligo.ie")
                .ToCollectionAsync();
            var use = user.FirstOrDefault();
            if (use != null)
            {
                use.UserId = 1;
                use.Credits = 100;
                await userTbl.UpdateAsync(use);

            }
        }


        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            int routeId;
            string day;
            string returnDay = "";

            //false == single ticket
            //single/false by default
            string ticketType;

            //need to send RouteID
            int route = cbxDepart.SelectedIndex;
            //must add +1 on to index of selected item because
            //index starts at 0 where id starts at 1
            //if first selected index = 0 + 1 = 1. therefore gets gets route with id 1 (first one)
            routeId = route + 1;

            //journey day
            day = cbxDepartDay.SelectedItem.ToString();

            //ticket type
            ticketType = Convert.ToString(cbxTicket.SelectedValue);

            if (ticketType == "Return")
            {
                //its a return ticket
                ticketType = "Return";
            }
            else if (ticketType == "Single")
            {
                //its a return ticket
                ticketType = "Single";
            }

            //Single/Return (type)
            //if Return - return day
            if (cbxTicket.SelectedItem.ToString() == "Return")
            {
                returnDay = cbxReturnDate.SelectedItem.ToString();
            }

            Params prm = new Params { routeId = routeId, date = day, returnDate = returnDay, ticketType = ticketType };
            Frame.Navigate(typeof(Journeys), prm);

            // Frame.Navigate(typeof(Journeys));



        }

        private void TicketType()
        {
            //Populate ticket type
            cbxTicket.Items.Add("Single");
            cbxTicket.Items.Add("Return");

            cbxTicket.SelectedIndex = 0;
        }

        private void PopulateDays()
        {
            var days = new List<Days>();
            days.Add(new Days() { day = "Monday" });
            days.Add(new Days() { day = "Tuesday" });
            days.Add(new Days() { day = "Wednesday" });
            days.Add(new Days() { day = "Thursday" });
            days.Add(new Days() { day = "Friday" });
            days.Add(new Days() { day = "Saturday" });
            days.Add(new Days() { day = "Sunday" });

            foreach (var varDay in days)
            {
                //Populate days combobox
                string comboDays = varDay.day;

                ComboboxItem item = new ComboboxItem();
                item.Text = comboDays;

                cbxDepartDay.Items.Add(item);
                cbxReturnDate.Items.Add(item);
            }
        }

        //class for days of the week
        //two comboboxes to be filled

        public class Days
        {
            public string day { get; set; }
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Admin));
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Profile));
        }
    }
    
}

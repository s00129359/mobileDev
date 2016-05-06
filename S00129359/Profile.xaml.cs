using Microsoft.WindowsAzure.MobileServices;
using S00129359.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Profile : Page
    {
        //user logged in
        //shoudld be saved in isolated storage when user logs in
        public int UserLoggedIn;
        //user
        private IMobileServiceTable<User> userTbl = App.MobileService.GetTable<User>();
        //ticket
        private IMobileServiceTable<Ticket> ticketTbl = App.MobileService.GetTable<Ticket>();
        //route
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();

        public Profile()
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
            getUserId();
        }

        private async void getUserId()
        {
            int cID = 1;
            // get id of customer stored in iso storage
            StorageFolder storeFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;

            //write to storage
            //this should be done on log in or 
            //register page and stored so it
            //keeps a user logged in on their phone
            StorageFile dataFile =
                await storeFolder.CreateFileAsync("CustomerId.txt", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(dataFile, cID.ToString());

            //read from storage
            StorageFile readFile =
                await storeFolder.GetFileAsync("CustomerId.txt");
            string custIdStr = await Windows.Storage.FileIO.ReadTextAsync(readFile);
            UserLoggedIn = Convert.ToInt32(custIdStr);

            getUserDetails();
            loadTickets();

        }

        private async void getUserDetails()
        {
            List<User> user = await userTbl
                    .Where(id => id.UserId == UserLoggedIn)
                    .ToListAsync();
            foreach (var usr in user)
            {
                tbxemail.Text = usr.Email;
                tbxName.Text = usr.FirstName;
                tbxSurName.Text = usr.SeondName;
                tbCreds.Text += usr.Credits.ToString();
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            List<User> user = await userTbl
                        .Where(id => id.UserId == UserLoggedIn)
                        .ToListAsync();
            var use = user.FirstOrDefault();
            if (use != null)
            {
                use.Email = tbxemail.Text;
                use.FirstName = tbxName.Text;
                use.SeondName = tbxSurName.Text;
                await userTbl.UpdateAsync(use);

            }
        }

        private async void loadTickets()
        {

            List<Ticket> tickets = await ticketTbl
                .Where(cid => cid.custId == UserLoggedIn)
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                int tickid = ticket.ticketId;
                List<Route> routes = await routeTbl
                    .Where(rid => rid.Route_id == ticket.RouteId)
                    .ToListAsync();
                foreach (var route in routes)
                {
                    string j = tickid.ToString() + ". " + " " + route.Departs + " " + route.Arrives;
                    lstRoutes.Items.Add(j);
                }

            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            string Selcted = lstRoutes.SelectedItem.ToString();
            int indx = Selcted.LastIndexOf(".");
            string sendid = Selcted.Substring(0, indx);
            
            Frame.Navigate(typeof(ViewTicket),sendid);


        }

    }
}

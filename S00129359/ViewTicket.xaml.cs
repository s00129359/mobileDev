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
    public sealed partial class ViewTicket : Page
    {
        public int ticketId;
        private IMobileServiceTable<Ticket> ticketTbl = App.MobileService.GetTable<Ticket>();
        private IMobileServiceTable<Route> routeTbl = App.MobileService.GetTable<Route>();

        public ViewTicket()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string param = e.Parameter as string;
            ticketId = Convert.ToInt32(param);

            populate();

        }

        string ticketType;
        string firstuse;
        string seconduse;
        int routeId;
        string type;

        private async void populate()
        {
            List<Ticket> ticket = await ticketTbl
                .Where(id => id.ticketId == ticketId)
                .ToListAsync();

            foreach (var tick in ticket)
            {
                routeId = tick.RouteId;

                type = tick.TicketType;
                if (type == "Single")
                {
                   ticketType = "No";
                }
                else if (type == "Return")
                {
                   ticketType = "Yes";
                }


                if (tick.FirstJourneyUsed == false)
                {
                    firstuse = "unused";
                }
                else if (tick.FirstJourneyUsed == true)
                {
                    firstuse = "used";
                }

                if (tick.SecondJourneyUsed == false)
                {
                    seconduse = "unused";
                }
                else if (tick.SecondJourneyUsed == true)
                {
                    seconduse = "Fully used";
                }
                
            }

            tBxreturn.Text = ticketType;
            if (firstuse == "unused")
            {
                tblStatus.Text = "Un-Used";
            }
            else if ((ticketType == "No") || (firstuse == "used"))
            {
                tblStatus.Text = "Used";
            }
            else if ((ticketType == "Yes")||(firstuse == "used"))
            {
                tblStatus.Text = "Return Journey Remains";
            }
            else if ((ticketType == "Yes")||(seconduse == "used"))
            {
                tblStatus.Text = "Used";
            }

            //Get Route Detials
            List<Route> route = await routeTbl
                .Where(rid => rid.Route_id == routeId)
                .ToListAsync();

            foreach (var rte in route)
            {
                tBxDeparts.Text = rte.Departs;
                tBxArrive.Text = rte.Arrives;
                tBxRouteNo.Text = rte.Route_id.ToString();
            }

        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            List<Ticket> ticket = await ticketTbl
                        .Where(id => id.ticketId == ticketId)
                        .ToListAsync();
            var tickEdit = ticket.FirstOrDefault();

               
            
            //single ticket
            if (type == "Single")
            {
                //ticket first used
                tickEdit.FirstJourneyUsed = true;
                await ticketTbl.UpdateAsync(tickEdit);
            }
            else if (type == "Return")
            {
                //if first not used
                if (firstuse == "unused")
                {
                    //edit ticket tbl
                    tickEdit.FirstJourneyUsed = true;
                    await ticketTbl.UpdateAsync(tickEdit);
                }
               //else if second not used                  
                else if (seconduse == "unused")
                {
                    //second use is true 
                    tickEdit.SecondJourneyUsed = true;
                    await ticketTbl.UpdateAsync(tickEdit);
                }
                else
                {
                    //ticket already used  
                    tblStatus.Text = "This has been used";
                }
            }

            Frame.Navigate(typeof(Admin));
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Profile));
        }
    }
}

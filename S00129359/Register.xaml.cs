using Microsoft.WindowsAzure.MobileServices;
using S00129359.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
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
    public sealed partial class BlankPage1 : Page
    {
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> userTbl = App.MobileService.GetTable<User>();

        string passwrd;

        public BlankPage1()
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

        private  void Submit_Click(object sender, RoutedEventArgs e)
        {

           passwrd = tbPassword.Password;

           passHash(ref passwrd);

           CheckEmail();
            
        }

        public string passHash(ref string pass)
        {
            //has to sha1
            HashAlgorithmProvider hashProvider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            IBuffer hash = hashProvider.HashData(CryptographicBuffer.ConvertStringToBinary(pass, BinaryStringEncoding.Utf8));
            pass = CryptographicBuffer.EncodeToBase64String(hash);
            //return hased string
            return pass;
        }

        private async void Insert()
        {
            var ds = new User() { FirstName = tbFirstName.Text, SeondName = tbSecondName.Text, Email = tbEmail.Text, Password = passwrd };
            await userTbl.InsertAsync(ds);
        }

        public async void CheckEmail()
        {
            List<User> itm = await userTbl
                .ToListAsync();

            bool emailExist = false;
            foreach (var item in itm)
            {
                if(item.Email == tbEmail.Text)
                {
                    emailExist = true;
                }
                else
                {
                    emailExist = false;
                }
            }

            if (emailExist == false)
            {
                Insert();
            }
            else
                tbError.Text = "Email Already Used";
            
        }
    }
}

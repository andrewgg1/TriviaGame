/*
 *	FILE				: LoginPage.xaml.cs
 *	PROJECT				: Relational Databases PROG2111 - Assignment 4
 *	PROGRAMMER			: Andrew Gordon
 *	FIRST VERSION		: Dec. 1, 2020
 *  LAST UPDATE         : Dec. 9, 2020
 *	DESCRIPTION			: Contains the code behind for the Login Page
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TriviaGame
{
    /* ---------------------------------------------------------------------------------
    CLASS NAME  :	LoginPage
    PURPOSE     :	The purpose of this class is to provide handling for the Login Page.

                    Includes methods:
                    - ServerConnect_Click
    --------------------------------------------------------------------------------- */
    public partial class LoginPage : Page
    {
        public static string Player { get; set; }
        public LoginPage()
        {
            InitializeComponent();
        }

        /*  -- Method Header Comment
        Name	:	ServerConnect_Click
        Purpose :	Handles the server connect button click. Makes sure a valid IP address
                    and a Player Name were input.
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	None
        Returns	:	None
        */
        private void ServerConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // attempt database connection
                if (DBAccess.ConnectToDatabase(IPInput.Text))
                {
                    // if the name was not blank
                    if (nameInput.Text != "")
                    {
                        Player = nameInput.Text;
                        NavigationService.Navigate(new GamePage());
                    }
                    // if name blank, display error
                    else
                    {
                        ErrorMessage.Content = "Player name blank";
                        ErrorMessage.Visibility = Visibility.Visible;
                    }
                }
                // if server connection connection fails, display error
                else
                {
                    ErrorMessage.Content = "Invalid server IP or server not available";
                    ErrorMessage.Visibility = Visibility.Visible;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

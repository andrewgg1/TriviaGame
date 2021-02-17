/*
 *	FILE				: GamePage.xaml.cs
 *	PROJECT				: Relational Databases PROG2111 - Assignment 4
 *	PROGRAMMER			: Andrew Gordon
 *	FIRST VERSION		: Dec. 1, 2020
 *  LAST UPDATE         : Dec. 9, 2020
 *	DESCRIPTION			: Contains the code behind for the MainWindow of the Trivia Game
 */

using System.ComponentModel;
using System.Windows;

namespace TriviaGame
{
    /* ---------------------------------------------------------------------------------
    CLASS NAME  :	MainWindow
    PURPOSE     :	The purpose of this class is to provide handling for the Trivia Game
                    Main Window.
    --------------------------------------------------------------------------------- */
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Main.Navigate(new LoginPage());
        }

        /*  -- Method Header Comment
        Name	:	OnClosing
        Purpose :	Overrides the OnClosing method to show a message box confirmation before closing.
        Inputs	:	CancelEventArgs e
        Outputs	:	None
        Returns	:	None
        */
        protected override void OnClosing(CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("\t   Are you sure?", "Quit the game", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}

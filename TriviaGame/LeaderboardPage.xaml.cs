/*
 *	FILE				: LeaderboardPage.xaml.cs
 *	PROJECT				: Relational Databases PROG2111 - Assignment 4
 *	PROGRAMMER			: Andrew Gordon
 *	FIRST VERSION		: Dec. 1, 2020
 *  LAST UPDATE         : Dec. 9, 2020
 *	DESCRIPTION			: Contains the code behind for the Leaderboard Page, the LeaderBoardObj class,
 *	                      and the CorrectAnswersObj class
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace TriviaGame
{
    /* ---------------------------------------------------------------------------------
    CLASS NAME  :	LeaderBoardObj
    PURPOSE     :	The purpose of this class is to contain all the relevant statistics
                    from the leaderboard
    --------------------------------------------------------------------------------- */
    public class LeaderBoardObj
    {
        public int PlayerRank { get; set; }
        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }
    }

    /* ---------------------------------------------------------------------------------
    CLASS NAME  :	CorrectAnswersObj
    PURPOSE     :	The purpose of this class is to contain all the relevant statistics
                    about the player's game.
    --------------------------------------------------------------------------------- */
    public class CorrectAnswersObj
    {
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string RightOrWrong { get; set; }
        public int TimeInS { get; set; }
        public int Points { get; set; }
    }

    /* ---------------------------------------------------------------------------------
    CLASS NAME  :	LeaderboardPage
    PURPOSE     :	The purpose of this class is to provide handling for the Leaderboard Page.

                    Includes methods:
                    - PlayAgainBtn_Click
    --------------------------------------------------------------------------------- */
    public partial class LeaderboardPage : Page
    {
        public LeaderboardPage()
        {
            InitializeComponent();
            // insert score and player name into the leaderboard
            DBAccess.InsertToLeaderboard(LoginPage.Player, GamePage.GetScore());
            // get the updated leaderboard
            LeaderboardGrid.DataContext = DBAccess.GetLeaderboard();
            // assign the leaderboard and the players game statistics to their relavant grids
            PromptBlock.Text = $"{LoginPage.Player}'s final score is {GamePage.GetScore()}";
            GameStatsGrid.DataContext = DBAccess.GameStatistics();
        }

        /*  -- Method Header Comment
        Name	:	PlayAgainBtn_Click
        Purpose :	Handles the Play Again button
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	None
        Returns	:	None
        */
        private void PlayAgainBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GamePage());
        }
    }
}

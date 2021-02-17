/*
 *	FILE				: GamePage.xaml.cs
 *	PROJECT				: Relational Databases PROG2111 - Assignment 4
 *	PROGRAMMER			: Andrew Gordon
 *	FIRST VERSION		: Dec. 1, 2020
 *  LAST UPDATE         : Dec. 9, 2020
 *	DESCRIPTION			: Contains the code behind for the Game Page
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace TriviaGame
{
    /* ---------------------------------------------------------------------------------
    CLASS NAME  :	GamePage
    PURPOSE     :	The purpose of this class is to provide handling for the Game Page.

                    Includes methods:
                    - StartQuestion
                    - CountDownTimer
                    - TimerUpdate
                    - ClearButtons
                    - GetSelectedAnswer
                    - AssignAnswerButtons
                    - ShuffleList
                    - SubmitAnswerBtn_Click
                    - UpdateScore
    --------------------------------------------------------------------------------- */
    public partial class GamePage : Page
    {
        public int QuestionNumber { get; set; }
        public static readonly int AllowedTime = 20;
        public static readonly int NumberOfQs = 10;
        public int ElapsedTime { get; set; }
        private DispatcherTimer Timer;
        private readonly Random rnd = new Random();

        public GamePage()
        {
            InitializeComponent();
            // initialize player name and question number
            PlayerLabel.Content += LoginPage.Player;
            QuestionNumber = 0;
            try
            {
                // try resetting the player answers table
                DBAccess.ResetPlayerAnswers();
                CurrentScore.Content = GetScore();
                StartQuestion();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection lost, please close the program and try again");
            }
        }

        /*  -- Method Header Comment
        Name	:	StartQuestion
        Purpose :	Initializes the page for the current question
        Inputs	:	None
        Outputs	:	MessageBox error message if an exception is caught
        Returns	:	None
        */
        private void StartQuestion()
        {
            // if the question count is under the amount expected
            if (QuestionNumber < NumberOfQs)
            {
                // reset countdown and elapsed time
                Countdown.Content = AllowedTime;
                ElapsedTime = 0;
                // reset error message
                ErrorMessage.Visibility = Visibility.Hidden;
                try
                {
                    // show question and question #
                    QuestionBlock.Text = $"Question #{++QuestionNumber}   {DBAccess.GetQuestion(QuestionNumber)}";
                    // get answers and assign them to the radio box buttons
                    List<string> answers = DBAccess.GetAnswers(QuestionNumber);
                    AssignAnswerButtons(answers);
                    // clear selected button and start timer
                    ClearButtons();
                    CountDownTimer();
                }
                catch (Exception)
                {
                    MessageBox.Show("Connection lost, please close the program and try again");
                }
            }
            // game is over, go to the leaderboard page
            else
            {
                NavigationService.Navigate(new LeaderboardPage());
            }
        }

        /*  -- Method Header Comment
        Name	:	CountDownTimer
        Purpose :	Initializes the countdown timer to an interval of 1 second
        Inputs	:	None
        Outputs	:	None
        Returns	:	None
        */
        private void CountDownTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += TimerUpdate;
            Timer.Start();
        }

        /*  -- Method Header Comment
        Name	:	TimerUpdate
        Purpose :	Updates the page with the new time, counting down. If the
                    Time reached 0 then the question will be over.
        Inputs	:	object sender, EventArgs e
        Outputs	:	None
        Returns	:	None
        */
        void TimerUpdate(object sender, EventArgs e)
        {
            // update countdown timer
            Countdown.Content = AllowedTime - ++ElapsedTime;
            // if elapsed time has gone past the allowed time, update score with a blank answer
            if (ElapsedTime >= AllowedTime)
            {
                UpdateScore("");
            }
        }

        /*  -- Method Header Comment
        Name	:	ClearButtons
        Purpose :	Resets all the answer radio boxes to unchecked
        Inputs	:	None
        Outputs	:	None
        Returns	:	None
        */
        private void ClearButtons()
        {
            AnswerA.IsChecked = false;
            AnswerB.IsChecked = false;
            AnswerC.IsChecked = false;
            AnswerD.IsChecked = false;
        }

        /*  -- Method Header Comment
        Name	:	AssignAnswerButtons
        Purpose :	Assigns the list of answers to the answer radio buttons
        Inputs	:	List<string> answers    the list of answers
        Outputs	:	None
        Returns	:	None
        */
        private void AssignAnswerButtons(List<string> answers)
        {
            // shuffle answers and display them in each radio button
            answers = ShuffleList(answers);
            AnswerA.Content = answers[0];
            AnswerB.Content = answers[1];
            AnswerC.Content = answers[2];
            AnswerD.Content = answers[3];
        }

        /*  -- Method Header Comment
        Name	:	GetSelectedAnswer
        Purpose :	Gets the currently selected answer
        Inputs	:	None
        Outputs	:	None
        Returns	:	string answer       the content of the selected answer
        */
        private string GetSelectedAnswer()
        {
            string answer = "";
            if (AnswerA.IsChecked == true)
            {
                answer = AnswerA.Content.ToString();
            }
            else if (AnswerB.IsChecked == true)
            {
                answer = AnswerB.Content.ToString();
            }
            else if (AnswerC.IsChecked == true)
            {
                answer = AnswerC.Content.ToString();
            }
            else if (AnswerD.IsChecked == true)
            {
                answer = AnswerD.Content.ToString();
            }

            return answer;
        }



        // This method has been slightly modifed from the example at https://stackoverflow.com/revisions/1262619/1
        /*  -- Method Header Comment
        Name	:	ShuffleList
        Purpose :	Shuffles a list of strings randomly
        Inputs	:	List<string> list   the pre-shuffled list
        Outputs	:	None
        Returns	:	List<string> list   the post-shuffled list
        */
        private List<string> ShuffleList(List<string> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rnd.Next(0, n);
                n--;
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        /*  -- Method Header Comment
        Name	:	SubmitAnswerBtn_Click
        Purpose :	Handles the Submit Answer button
        Inputs	:	object sender, RoutedEventArgs e
        Outputs	:	None
        Returns	:	None
        */
        private void SubmitAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            // get the selected answer
            string answer = GetSelectedAnswer();
            // if answer is empty, player took too long to answer
            if (answer == "")
            {
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                UpdateScore(answer);
            }
        }

        /*  -- Method Header Comment
        Name	:	UpdateScore
        Purpose :	Stops the timer, Updates score tracking, initiates the next questions
        Inputs	:	string answer       the player selected answer
        Outputs	:	MessageBox error message if an exception is caught
        Returns	:	None
        */
        private void UpdateScore(string answer)
        {
            // stop timer
            Timer.Stop();
            try
            {
                // if the answer is correct
                if (DBAccess.IsCorrectAnswer(QuestionNumber, answer))
                {
                    // inset the player chosen answer and the elapsed time
                    DBAccess.InsertPlayerAnswers(answer, ElapsedTime);
                }
                else
                {
                    // if incorrect answer, insert chosen answer and the max allowed time to indicate a score of 0
                    DBAccess.InsertPlayerAnswers(answer, AllowedTime);
                }
                // update the shown score
                CurrentScore.Content = GetScore();

                // start the next question
                StartQuestion();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection lost, please close the program and try again");
            }
        }

        /*  -- Method Header Comment
        Name	:	GetScore
        Purpose :	Gets the current score of the player
        Inputs	:	None
        Outputs	:	None
        Returns	:	int score       the current player score
        */
        public static int GetScore()
        {
            int timeToAnswer = DBAccess.GetTotalTimeToAnswer();
            int numOfQs = DBAccess.GetNumOfQuestionsAnswered();
            // calculate the score by multiplying the number of questions answered by the
            // allowed time subtract the total time to answer
            int score = (numOfQs * AllowedTime) - timeToAnswer;

            return score;
        }
    }
}

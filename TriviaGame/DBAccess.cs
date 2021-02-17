/*
 *	FILE				: DBAccess.cs.cs
 *	PROJECT				: Relational Databases PROG2111 - Assignment 4
 *	PROGRAMMER			: Andrew Gordon
 *	FIRST VERSION		: Dec. 1, 2020
 *  LAST UPDATE         : Dec. 9, 2020
 *	DESCRIPTION			: Contains the DBAccess class, used to handle all MySql
 *	                      database requests/commands
 */

using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;

namespace TriviaGame
{
    /* ---------------------------------------------------------------------------------
    CLASS NAME  :	DBAccess
    PURPOSE     :	The purpose of this class is to provide database handling functionality.

                    Includes methods:
                    - ConnectToDatabase
                    - GetQuestion
                    - GetAnswers
                    - IsCorrectAnswer
                    - GetCurrentScore
                    - ResetPlayerAnswers
                    - InsertPlayerAnswers
                    - InsertToLeaderboard
                    - GetLeaderboard
    --------------------------------------------------------------------------------- */
    class DBAccess
    {
        static MySqlConnection connection;

        /*  -- Method Header Comment
        Name	:	ConnectToDatabase
        Purpose :	Initializes connection to the database
        Inputs	:	string IPaddress    IPAddress of the MySql database to connect to
        Outputs	:	MessageBox error    message if an exception is caught
        Returns	:	bool    true    connection successful
                            false   connection unsuccessful
        */
        public static bool ConnectToDatabase(string IPaddress)
        {
            // attempt connection
            try
            {
                connection = new MySqlConnection($"server={IPaddress};database=Trivia_Game;userID=root;password=root");
                connection.Open();
            }
            // catch exceptions and show error
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
            return true;
        }

        /*  -- Method Header Comment
        Name	:	GetQuestion
        Purpose :	Gets the requested question from the database
        Inputs	:	int questionID    QuestionID of the needed question
        Outputs	:	None
        Returns	:	string question     the question needed
        */
        public static string GetQuestion(int questionID)
        {
            string question = "";
            MySqlCommand cmd = new MySqlCommand($"SELECT QuestionDesc from Questions Where QuestionID = {questionID};", connection);
            using (var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    question = (string) reader[0];
                }
                reader.Close();
            }
            return question;
        }

        /*  -- Method Header Comment
        Name	:	GetAnswers
        Purpose :	Gets the list of answers to the requested question
        Inputs	:	int questionID    QuestionID of the needed question answers
        Outputs	:	None
        Returns	:	List<string> answers     list of answers
        */
        public static List<string> GetAnswers(int questionID)
        {
            List<string> answers = new List<string>();
            MySqlCommand cmd = new MySqlCommand($"SELECT AnswerDesc FROM Answers WHERE QuestionID = {questionID};", connection);
            using (var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    answers.Add((string)reader[0]);
                }
                reader.Close();
            }
            return answers;
        }

        /*  -- Method Header Comment
        Name	:	IsCorrectAnswer
        Purpose :	Checks if the answer is correct for the question
        Inputs	:	int questionID      QuestionID of the question to be checked
                    string answer       answer to be checked
        Outputs	:	None
        Returns	:	bool isCorrect  true    the answer is correct
                                    false   the answer is incorrect
        */
        public static bool IsCorrectAnswer(int questionID, string answer)
        {
            bool isCorrect = false;
            MySqlCommand cmd = new MySqlCommand($"SELECT AnswerDesc FROM Answers INNER JOIN Questions ON Answers.QuestionID = Questions.QuestionID AND Answers.AnswerID = Questions.CorrectAnswerID WHERE Answers.Questionid = {questionID}; ", connection);
            using (var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    if(answer == (string) reader[0])
                    {
                        isCorrect = true;
                    }
                }
                reader.Close();
            }
            return isCorrect;
        }

        /*  -- Method Header Comment
        Name	:	GetTotalTimeToAnswer
        Purpose :	Gets the total time taken to answer all questions so far
        Inputs	:	None
        Outputs	:	None
        Returns	:	int timeToAnswer    total time to answer the questions so far
        */
        public static int GetTotalTimeToAnswer()
        {
            int timeToAnswer = 0;
            MySqlCommand cmd = new MySqlCommand("SELECT SUM(TimeToAnswer) FROM PlayerAnswers;", connection);
            using(var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    if(reader[0].GetType() != typeof(DBNull))
                    {
                        timeToAnswer = Convert.ToInt32(reader[0]);
                    }
                }
            }
            return timeToAnswer;
        }

        /*  -- Method Header Comment
        Name	:	GetNumOfQuestionsAnswered
        Purpose :	Gets the total time number of questions answered so far
        Inputs	:	None
        Outputs	:	None
        Returns	:	int numOfQs    total number of questions answered so far
        */
        public static int GetNumOfQuestionsAnswered()
        {
            int numOfQs = 0;
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM PlayerAnswers", connection);
            using(var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    numOfQs = Convert.ToInt32(reader[0]);
                }
            }
            return numOfQs;
        }

        /*  -- Method Header Comment
        Name	:	GetNumOfQuestionsAnswered
        Purpose :	Clears the PlayerAnswers table; used a the start of each game
        Inputs	:	None
        Outputs	:	None
        Returns	:	None
        */
        public static void ResetPlayerAnswers()
        {
            MySqlCommand cmd = new MySqlCommand("TRUNCATE TABLE PlayerAnswers", connection);
            cmd.ExecuteNonQuery();
        }

        /*  -- Method Header Comment
        Name	:	InsertPlayerAnswers
        Purpose :	Inserts the player's answer and time to answer to the PLayerAnswers table
        Inputs	:	string answerDesc   description of the answer chosen
                    int timeToAnswer    time taken to answer the question in seconds
        Outputs	:	None
        Returns	:	None
        */
        public static void InsertPlayerAnswers(string answerDesc, int timeToAnswer)
        {
            MySqlCommand cmd = new MySqlCommand($"INSERT INTO PlayerAnswers (AnswerDesc, TimeToAnswer) VALUES ('{answerDesc}', {timeToAnswer})", connection);
            cmd.ExecuteNonQuery();
        }

        /*  -- Method Header Comment
        Name	:	InsertToLeaderboard
        Purpose :	Inserts the player's name and final score into the leaderboard
        Inputs	:	string playerName   the player's name
                    int finalScore      the player's final score
        Outputs	:	None
        Returns	:	None
        */
        public static void InsertToLeaderboard(string playerName, int finalScore)
        {
            MySqlCommand cmd = new MySqlCommand($"INSERT INTO Leaderboard (PlayerName, PlayerScore) VALUES ('{playerName}', {finalScore})", connection);
            cmd.ExecuteNonQuery();
        }

        /*  -- Method Header Comment
        Name	:	GetLeaderboard
        Purpose :	Gets the entire leaderboard and inserts it into a list
        Inputs	:	None
        Outputs	:	None
        Returns	:	List<LeaderBoardObj> leaderboard    the full leaderboard, sorted by score descending
        */
        public static List<LeaderBoardObj> GetLeaderboard()
        {
            List<LeaderBoardObj> leaderboard = new List<LeaderBoardObj>();
            MySqlCommand cmd = new MySqlCommand("SELECT PlayerName, PlayerScore, RANK() OVER(ORDER BY PlayerScore DESC) PlayerRank FROM Leaderboard;", connection);
            using(var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    // save all values to LeaderBoardObj and add to the list
                    var player = new LeaderBoardObj
                    {
                        PlayerName = (string)reader[0],
                        PlayerScore = (int)reader[1],
                        PlayerRank = Convert.ToInt32(reader[2])
                    };
                    leaderboard.Add(player);
                }
            }
            return leaderboard;
        }

        /*  -- Method Header Comment
        Name	:	GetCorrectAnswers
        Purpose :	Gets the all the questions, correct answers, whethere the user was right or wrong,
                    time to answer(s), and points awarded and inserts them into a list.
        Inputs	:	None
        Outputs	:	None
        Returns	:	List<CorrectAnswersObj> correctAnswers    A list of all the player's game statistics
        */
        public static List<CorrectAnswersObj> GameStatistics()
        {
            List<CorrectAnswersObj> correctAnswers = new List<CorrectAnswersObj>();
            MySqlCommand cmd = new MySqlCommand("SELECT questions.QuestionDesc AS Questions, answers.AnswerDesc AS `Correct Answers`, IF(PlayerAnswers.TimeToAnswer < 20, 'Correct', 'Wrong') AS `Were you right?`, PlayerAnswers.TimeToAnswer AS `Time(s)`, (20 - PlayerAnswers.TimeToAnswer) AS Points from questions INNER JOIN answers ON questions.questionid = answers.questionid AND questions.correctAnswerID = answers.AnswerID INNER join PlayerAnswers on questions.questionid = PlayerAnswers.questionid; ", connection);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // save all values to CorrectAnswersObj and add to the list
                    var player = new CorrectAnswersObj
                    {
                        Question = (string)reader[0],
                        CorrectAnswer = (string)reader[1],
                        RightOrWrong = (string)reader[2],
                        TimeInS = Convert.ToInt32(reader[3]),
                        Points = Convert.ToInt32(reader[4])
                    };
                    correctAnswers.Add(player);
                }
            }
            return correctAnswers;
        }
    }
}

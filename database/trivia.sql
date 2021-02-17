-- 
--  FILE            : GamePage.xaml
--  PROJECT         : Relational Databases PROG2111 - Assignment 4
--  PROGRAMMER      : Andrew Gordon
--  FIRST VERSION   : Dec. 1, 2020
--  LAST UPDATE     : Dec. 9, 2020
--  DESCRIPTION     : Contains the SQL script used to setup the database and
--                    tables with required data and sample data.
--

DROP DATABASE IF EXISTS Trivia_Game;
CREATE DATABASE Trivia_Game;
USE Trivia_Game;

DROP TABLE IF EXISTS Questions;
CREATE TABLE Questions(
    QuestionID INT NOT NULL AUTO_INCREMENT,
    QuestionDesc VARCHAR(255) NOT NULL,
    CorrectAnswerID INT NOT NULL,
    PRIMARY KEY (QuestionID)
);

DROP TABLE IF EXISTS Answers;
CREATE TABLE Answers(
    QuestionID INT NOT NULL,
    AnswerID INT NOT NULL,
    AnswerDesc VARCHAR(255) NOT NULL,
    PRIMARY KEY (QuestionID, AnswerID)
);

DROP TABLE IF EXISTS PlayerAnswers;
CREATE TABLE PlayerAnswers(
    QuestionID INT NOT NULL AUTO_INCREMENT,
    AnswerDesc VARCHAR(255) NOT NULL,
    TimeToAnswer INT,
    PRIMARY KEY (QuestionID)
);

DROP TABLE IF EXISTS Leaderboard;
CREATE TABLE Leaderboard(
    LeaderboardID INT NOT NULL AUTO_INCREMENT,
    PlayerName VARCHAR(255) NOT NULL,
    PlayerScore INT NOT NULL,
    PRIMARY KEY (LeaderboardID)
);

-- Insert questions
INSERT INTO Questions (QuestionDesc, CorrectAnswerID)
VALUES
('What is the national sport of Canada?', 2),
('What is the capital city of Canada?', 3),
('What country is Canada''s biggest trading partner?', 1),
('What symbol is on the Canadian flag?', 2),
('What is the national animal of Canada?', 4),
('How many provinces and territories are in Canada?', 2),
('What famous landmark is in Canada?', 1),
('What is the value of a loonie?', 2),
('What is the leader of Canada called?', 4),
('What famous food is Canadian?', 3);

-- Insert answers
INSERT INTO Answers (QuestionID, AnswerID, AnswerDesc)
VALUES
(1, 1, 'Basketball'), (1, 2, 'Hockey'), (1, 3, 'Soccer'), (1, 4, 'Tennis'),
(2, 1, 'Toronto'), (2, 2, 'Calgary'), (2, 3, 'Ottawa'), (2, 4, 'Vancouver'),
(3, 1, 'USA'), (3, 2, 'China'), (3, 3, 'UK'), (3, 4, 'Germany'),
(4, 1, 'Hockey Stick'), (4, 2, 'Maple Leaf'), (4, 3, 'Goose'), (4, 4, 'Star'),
(5, 1, 'Blue Jay'), (5, 2, 'Eagle'), (5, 3, 'Moose'), (5, 4, 'Beaver'),
(6, 1, '10'), (6, 2, '13'), (6, 3, '15'), (6, 4, '11'),
(7, 1, 'Niagara Falls'), (7, 2, 'Grand Canyon'), (7, 3, 'Mt. Everest'), (7, 4, 'Great Barrier Reef'),
(8, 1, '$2'), (8, 2, '$1'), (8, 3, '$3'), (8, 4, '$10'),
(9, 1, 'The Premier'), (9, 2, 'The King'), (9, 3, 'The President'), (9, 4, 'The Prime Minister'),
(10, 1, 'Sushi'), (10, 2, 'Pizza'), (10, 3, 'Poutine'), (10, 4, 'Fish and Chips');

-- Insert sample leaderboard values
INSERT INTO Leaderboard(PlayerName, PlayerScore)
VALUES
('Jason', 72), ('Tom', 23), ('Bill', 51), ('Dave', 157), ('Jess', 99), ('Ryan', 182), ('George', 111), ('Steve', 99), ('Mike', 137), ('Mary', 83), ('Susie', 67);

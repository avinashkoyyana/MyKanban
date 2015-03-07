using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyKanban;

namespace MyKanbanUnitTests
{
    [TestClass]
    public class TejaUnitTests
    {

        Credential TestCredential
        {
            get
            {
                MyKanban.Data.DatabaseType = Data.DbType.MySql;
                Credential testCredential = new Credential("testuser", "password");
                if (testCredential.Id == 0)
                {
                    Person testUser = new Person(null);
                    testUser.Name = "Test User";
                    testUser.UserName = "testuser";
                    testUser.Password = "password";
                    testUser.Update();

                    testCredential = new Credential("testuser", "password");
                }
                return testCredential;
            }
        }

        #region Teja and Andy's Unit Tests

        [TestMethod]
        public void AddAndyAndTejaAccounts()
        {
            People findAndy = new People("Andy Ding", TestCredential);
            if (findAndy.Count == 0)
            {
                Person andy = new Person(TestCredential);
                andy.Name = "Andy Ding";
                andy.UserName = "ading";
                andy.Password = "password";
                andy.Update();
            }

            People findTeja = new People("Teja Poduri", TestCredential);
            if (findTeja.Count == 0)
            {
                Person teja = new Person(TestCredential);
                teja.Name = "Teja Poduri";
                teja.UserName = "tpoduri";
                teja.Password = "password";
                teja.Update();
            }

            People findMark = new People("Mark Gerow", TestCredential);
            if (findMark.Count == 0)
            {
                Person mark = new Person(TestCredential);
                mark.Name = "Mark Gerow";
                mark.UserName = "mgerow";
                mark.Password = "password";
                mark.Update();
            }
        }

        [TestMethod]
        public void CreateABoardUnderTejasBoardSet()
        {
            // Get credential to use when adding data
            Credential teja = new Credential("tpoduri", "password");

            // Create a new board set and assign status codes
            BoardSet newBoardSet = new BoardSet(teja);
            newBoardSet.Name = "Teja and Andy's new board set";
            newBoardSet.Update();

            // Verify added
            BoardSet boardSetTest = new BoardSet(newBoardSet.Id, teja);
            Assert.IsTrue(boardSetTest.Id == newBoardSet.Id);

            // Add some status codes
            StatusCode statusCode = new StatusCode(teja);
            statusCode.Name = "First State";
            statusCode.ColumnHeading = statusCode.Name;
            newBoardSet.StatusCodes.Add(statusCode);

            statusCode = new StatusCode(teja);
            statusCode.Name = "Second State";
            statusCode.ColumnHeading = statusCode.Name;
            newBoardSet.StatusCodes.Add(statusCode);

            statusCode = new StatusCode(teja);
            statusCode.Name = "Third State";
            statusCode.ColumnHeading = statusCode.Name;
            newBoardSet.StatusCodes.Add(statusCode);

            newBoardSet.StatusCodes.Update();

            // Verify added
            boardSetTest = new BoardSet(newBoardSet.Id, teja);
            Assert.IsTrue(boardSetTest.StatusCodes.Count == newBoardSet.StatusCodes.Count);

            // Create the board and add to the set collection
            Board newBoard = new Board(teja);
            newBoard.Name = "Demo board for Teja";
            newBoardSet.Boards.Add(newBoard);
            newBoardSet.Boards.Update();

            // Verify added
            boardSetTest = new BoardSet(newBoardSet.Id, teja);
            Assert.IsTrue(boardSetTest.Boards.Count == 1);

            // Add a user to this board
            User user = new User(teja);
            user.PersonId = teja.Id;
            newBoard.Users.Add(user);

            Credential andy = new Credential("ading", "password");
            user = new User(teja);
            user.PersonId = andy.Id;
            newBoard.Users.Add(user);

            Credential mark = new Credential("mgerow", "megabase");
            user = new User(teja);
            user.PersonId = mark.Id;
            newBoard.Users.Add(user);

            newBoard.Users.Update();

            // Verify added
            boardSetTest = new BoardSet(newBoardSet.Id, teja);
            Assert.IsTrue(boardSetTest.Boards[0].Users.Count == 3 && boardSetTest.Boards[0].Users[2].PersonId == teja.Id);

            // Add a project to the new board
            Project newProject = new Project(teja);
            newProject.Name = "Demo project";
            newBoard.Projects.Add(newProject);
            newBoard.Projects.Update();

            // Verify added
            boardSetTest = new BoardSet(newBoardSet.Id, teja);
            Assert.IsTrue(boardSetTest.Boards[0].Projects.Count == 1);

            // Add a task to the new project
            Task newTask = new Task(teja);
            newTask.Name = "A new task added to demo project";
            newTask.EstHours = 2;
            newTask.StartDate = DateTime.Now;
            newTask.EndDate = DateTime.Now.AddDays(3);
            newProject.Tasks.Add(newTask);
            newProject.Tasks.Update();

            // Verify added
            boardSetTest = new BoardSet(newBoardSet.Id, teja);
            Assert.IsTrue(boardSetTest.Boards[0].Projects[0].Tasks.Count == 1);

            // Add a sub-task to the new task
            Task subTask = new Task(teja);
            subTask.Name = "a sub-task of demo task";
            newTask.SubTasks.Add(subTask);
            newTask.SubTasks.Update();

            // Verify added
            boardSetTest = new BoardSet(newBoardSet.Id, teja);
            Assert.IsTrue(boardSetTest.Boards[0].Projects[0].Tasks[0].SubTasks.Count == 1);

        }

        #endregion
    }
}

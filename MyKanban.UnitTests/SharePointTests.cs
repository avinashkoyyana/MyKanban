using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyKanban;

namespace MyKanbanUnitTests
{
    [TestClass]
    public class SharePointTests
    {
        Credential TestCredential
        {
            get
            {
                MyKanban.Data.DatabaseType = Data.DbType.SharePoint;
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

        #region SharePoint Unit Tests

        [TestMethod]
        public void SharePointAddBoardSet()
        {
            BoardSet board_set = new BoardSet(TestCredential);
            board_set.Name = "Test add to SP";
            board_set.Update();

            // Verify added
            BoardSet bs2 = new BoardSet(board_set.Id, TestCredential);
            Assert.IsTrue(board_set.Id == bs2.Id &&
                board_set.Name == bs2.Name);

            // Delete the board set
            board_set.Delete();

            // Verify deleted
            BoardSet bs3 = new BoardSet(board_set.Id, TestCredential);
            Assert.IsTrue(bs3.Name == "" && bs3.Id == 0);

            // Cleanup
            Data.DatabaseType = Data.DbType.MySql;
        }

        [TestMethod]
        public void SharePointAddBoard()
        {
            Board board = new Board(TestCredential);
            board.Name = "Zippy";
            board.Update();

            // Verify added
            Board board2 = new Board(board.Id, TestCredential);

            Assert.IsTrue(board.Id > 0 &&
                board.Id == board2.Id &&
                board.Name == board2.Name);

            // Delete the board
            board.Delete();

            // Verify deleted
            Board board3 = new Board(board.Id, TestCredential);

            Assert.IsTrue(board3.Id == 0);
        }

        [TestMethod]
        public void SharePointAddProject()
        {
            Project project = new Project(TestCredential);
            project.Name = "Zippy";
            project.Update();

            // Verify added
            Project project2 = new Project(project.Id, TestCredential);

            Assert.IsTrue(project.Id > 0 &&
                project.Id == project2.Id &&
                project.Name == project2.Name);

            // Delete the board
            project.Delete();

            // Verify deleted
            Project project3 = new Project(project.Id, TestCredential);

            Assert.IsTrue(project3.Id == 0);
        }

        #endregion
    }
}

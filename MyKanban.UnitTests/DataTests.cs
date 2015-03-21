using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyKanban;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System.Threading;

using System.Collections;
using System.Collections.Generic;

using System.Data;

namespace MyKanbanUnitTests
{

    #region Standard Tests

    [TestClass]
    public class DataTests
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

        [TestMethod]
        public void AddAssigneeToTask()
        {
            Credential credential = TestCredential;

            // Create a new task
            Task task = new Task(credential);
            task.Name = "A new task - " + Guid.NewGuid().ToString();
            task.Update();

            // Verify task has been saved
            Tasks tasks = new Tasks(task.Name, credential);

            Assert.IsTrue(tasks.Count == 1 && tasks[0].Id == task.Id
                && tasks[0].Name == task.Name);

            // Create a new person to assign to this task
            Person assignee = new Person(credential);
            assignee.Name = "Jane Doe - " + Guid.NewGuid().ToString();
            assignee.UserName = Guid.NewGuid().ToString();
            assignee.Update();

            // Verify that new assignee has been added to the database
            People assignees = new People(assignee.Name, credential);

            Assert.IsTrue(assignees.Count == 1 && assignees[0].Id == assignee.Id
                && assignees[0].Name == assignee.Name);

            // Add the assignee to the task
            task.Assignees.Add(assignee);
            task.Assignees.Update();

            // Verify that assignee has been saved
            tasks = new Tasks(task.Name, credential);

            Assert.IsTrue(tasks.Count == 1 && tasks[0].Assignees.Count == 1
                && tasks[0].Assignees[0].Id == assignee.Id
                && tasks[0].Assignees[0].Name == assignee.Name);

            // Cleanup
            assignee.Delete();
            task.Delete();

            // Verify deleted
            tasks = new Tasks(task.Name, credential);
            assignees = new People(assignee.Name, credential);

            Assert.IsTrue(tasks.Count == 0 && assignees.Count == 0);
        }

        [TestMethod]
        public void AddBoard()
        {
            Credential credential = TestCredential;

            Board board = new Board(credential);
            board.Name = "Added by Mark " + Guid.NewGuid();
            board.ParentId = 2;
            board.Update();

            board.Name = board.Name + " abc ";
            board.ParentId = 1;
            board.Update();

            // Clean up
            board.Delete();
        }

        [TestMethod]
        public void AddBoardSet()
        {
            Credential credential = TestCredential;

            // Add the boardset
            BoardSet boardSet = new BoardSet(credential);
            boardSet.Name = "My new board set";
            boardSet.Update();

            // Verify added
            BoardSet newBoardSet = new BoardSet(boardSet.Id, credential);

            Assert.IsTrue(boardSet.Name == newBoardSet.Name && boardSet.Id == newBoardSet.Id);

            // Cleanup
            boardSet.Delete();
        }

        [TestMethod]
        public void AddBoardToBoardSet()
        {
            Credential credential = TestCredential;

            BoardSet boardSet = new BoardSet(credential);
            boardSet.Name = "New board set - " + Guid.NewGuid();

            Board board = new Board(credential);
            board.Name = "New board - " + Guid.NewGuid();

            boardSet.Boards.Add(board);
            boardSet.Update();

        }

        [TestMethod]
        public void AddPerson()
        {
            Credential credential = TestCredential;

            // Add a new person
            Person person = new Person(credential);
            person.Name = "Debbie Gerow";
            person.PictureUrl = "http://somedomain.com/pictures/debbie.gif";
            person.Email = "debbie.gerow@comcast.net";
            person.UserName = "dgerow123-" + Guid.NewGuid();
            person.Password = "zippy";
            person.Phone = "650-799-5988";
            person.CanLogin = false;
            person.Update();

            // Read the person back into memory
            Person p2 = new Person(person.Id, credential);
            Assert.IsTrue(
                person.Name == p2.Name &&
                person.PictureUrl == p2.PictureUrl &&
                person.Email == p2.Email &&
                person.Phone == p2.Phone &&
                person.UserName == p2.UserName &&
                //person.Password == p2.Password &&
                person.CanLogin == p2.CanLogin);
        }

        [TestMethod]
        public void AddProject()
        {
            Credential credential = TestCredential;

            Project project = new Project(credential);
            project.Name = "New project - " + Guid.NewGuid();
            project.ProjectLead = 0;
            project.ExpectedStartDate = new DateTime(2015, 3, 12);
            project.ExpectedEndDate = new DateTime(2015, 6, 30);
            project.DefineDone = "Let's see if this works";
            project.Update();
        }

        [TestMethod]
        public void AddProjectToBoard()
        {
            Credential credential = TestCredential;

            Board board = new Board(credential);
            board.Name = "New board - " + Guid.NewGuid().ToString();
            board.Update();

            Project project = new Project(credential);
            project.Name = "New project - " + Guid.NewGuid();
            project.ProjectLead = 0;
            project.ExpectedStartDate = new DateTime(2015, 3, 12);
            project.ExpectedEndDate = new DateTime(2015, 6, 30);
            project.DefineDone = "Let's see if this works";
            project.Update();

            board.Projects.Add(project);
            board.Projects.Update();

            // Read board back in and verify that project successfully added
            Board board2 = new Board(board.Id, credential);

            Assert.IsTrue(board.Id == board2.Id &&
                board.Projects.Count == 1 &&
                board.Projects.Count == board2.Projects.Count &&
                board.Projects[0].Id == board2.Projects[0].Id &&
                board.Projects[0].Name == board2.Projects[0].Name);

            // Clearnup
            project.Delete();
            board.Delete();
        }

        [TestMethod]
        public void AddSprintToBoard()
        {
            Credential credential = TestCredential;

            // Create the board
            Board board = new Board(credential);
            board.Name = "New Board - " + Guid.NewGuid().ToString();

            // Add some sprints
            board.Sprints.Add(new Sprint(board.Id, DateTime.Now, DateTime.Now.AddDays(3), 1, credential));
            board.Sprints.Add(new Sprint(board.Id, DateTime.Now.AddDays(17), DateTime.Now.AddDays(21), 2, credential));
            board.Sprints.Add(new Sprint(board.Id, DateTime.Now.AddDays(22), DateTime.Now.AddDays(23), 3, credential));

            // Save the board
            board.Update(true);

            // Make sure board and all sprints were saved correctly
            Board board2 = new Board(board.Id, credential);

            Assert.IsTrue(board.Name == board2.Name
                && board.Id == board2.Id
                && board.Sprints.Count == board2.Sprints.Count
                && board.Sprints[0].StartDate == board2.Sprints[0].StartDate);

            // Delete the board
            board.Delete();

            // Make sure board and associated sprints have been deleted from database
            Boards boards = new Boards(board.Name, credential);
            Sprints sprints = new Sprints(board, credential);

            Assert.IsTrue(boards.Count == 0 && sprints.Count == 0);
        }

        [TestMethod]
        public void AddComment()
        {
            Credential credential = TestCredential;
            Comment comment = new Comment(credential);
            comment.Text = "abcdef";
            comment.Update();

            // Verify saved
            Comment c2 = new Comment(comment.Id, credential);

            Assert.IsTrue(comment.Id == c2.Id &&
                comment.Text == c2.Text &&
                comment.Name == c2.Name);

            // Cleanup
            comment.Delete();
        }

        [TestMethod]
        public void AddCommentToTask()
        {
            Credential credential = TestCredential;

            // Create a new task
            Task task = new Task(credential);
            task.Name = "New task - " + Guid.NewGuid().ToString();
            task.Update();

            // Create a new Comment
            Comment Comment = new Comment(credential);
            Comment.Text = "New Comment - " + Guid.NewGuid().ToString();
            Comment.Update();

            // Verify task and Comment added to database
            Tasks tasks = new Tasks(task.Name, credential);
            Comments Comments = new Comments(Comment.Name, credential);

            Assert.IsTrue(tasks.Count == 1 && tasks[0].Id == task.Id &&
                Comments.Count == 1 && Comments[0].Id == Comment.Id);

            // Add the Comment to the task
            task.Comments.Add(Comment);
            task.Update();

            // Verify added
            tasks = new Tasks(task.Name, credential);

            Assert.IsTrue(tasks.Count == 1 && tasks[0].Comments.Count == 1 &&
                tasks[0].Comments[0].Id > 0 && tasks[0].Comments[0].Name == Comment.Name);

            // Now delete the Comment
            tasks[0].Comments[0].Delete();

            // Verify deleted
            tasks = new Tasks(task.Name, credential);

            Assert.IsTrue(tasks.Count == 1 && tasks[0].Comments.Count == 0);

            // Cleanup
            task.Delete();
        }

        [TestMethod]
        public void UpdateComment()
        {
            Credential credential = TestCredential;

            // Create a new Comment
            Comment comment = new Comment(credential);
            comment.Text = "New Comment - " + Guid.NewGuid().ToString();
            comment.Update();

            // Verify comment saved to database
            Comments comments = new Comments(comment.Name, credential);

            Assert.IsTrue(comments.Count == 1 && comments[0].Id == comment.Id &&
                comments[0].Text == comment.Text);

            // Now update the comment text
            comment.Text = "This is the updated text - " + Guid.NewGuid().ToString();
            comment.Update();

            // Verify updated
            comments = new Comments(comment.Text, credential);

            Assert.IsTrue(comments.Count == 1 && comments[0].Id == comment.Id &&
                comments[0].Text == comment.Text);

            // Cleanup
            comment.Delete();

        }

        [TestMethod]
        public void AddTag()
        {
            Credential credential = TestCredential;

            Tag tag = new Tag(credential);
            tag.Text = "abcdef";
            tag.Update();

            // Verify added
            Tag tag2 = new Tag(tag.Id, credential);

            Assert.IsTrue(tag.Id == tag2.Id &&
                tag.Text == tag2.Text);

            // Cleanup
            tag.Delete();
        }

        [TestMethod]
        public void AddTagToTask()
        {
            Credential credential = TestCredential;

            // Create a new task
            Task task = new Task(credential);
            task.Name = "New task - " + Guid.NewGuid().ToString();
            task.Update();

            // Create a new tag
            Tag tag = new Tag(credential);
            tag.Text = "New tag - " + Guid.NewGuid().ToString();
            tag.Update();

            // Verify task and tag added to database
            Tasks tasks = new Tasks(task.Name, credential);
            Tags tags = new Tags(tag.Name, credential);

            Assert.IsTrue(tasks.Count == 1 && tasks[0].Id == task.Id &&
                tags.Count == 1 && tags[0].Id == tag.Id);

            // Add the tag to the task
            task.Tags.Add(tag);
            task.Update();

            // Verify added
            tasks = new Tasks(task.Name, credential);

            Assert.IsTrue(tasks.Count == 1 && tasks[0].Tags.Count == 1 &&
                tasks[0].Tags[0].Id > 0 && tasks[0].Tags[0].Name == tag.Name);

            // Now delete the tag
            tasks[0].Tags[0].Delete();

            // Verify deleted
            tasks = new Tasks(task.Name, credential);

            Assert.IsTrue(tasks.Count == 1 && tasks[0].Tags.Count == 0);

            // Cleanup
            task.Delete();
        }

        [TestMethod]
        public void AddDuplicateUser()
        {
            Credential credential = TestCredential;

            Person user = new Person(credential);
            user.Name = "Fred Flintstone";
            user.UserName = "fflintstone";
            user.Password = "yabadabadoo";
            user.Email = "fflintstone@fenwick.com";
            user.Update();

            Person oldUserEntry = new People(user.Name, credential)[0];

            Assert.IsTrue(user.Id == 0 && user.Id != oldUserEntry.Id);
        }

        [TestMethod]
        public void AddUserToBoard()
        {
            Credential credential = TestCredential;

            Person user = new Person(credential);
            user.Name = "George Washington";
            user.UserName = "gwashington_" + Guid.NewGuid().ToString();
            user.Update();

            // Verify user has been saved to the database
            User george = new User(credential);
            george.PersonId = user.Id;

            Assert.IsTrue(user.Id == george.PersonId && user.Name == george.Name);

            // Now create a new board
            Board board = new Board(credential);
            board.Name = "Test - " + Guid.NewGuid().ToString();
            board.Update();

            // Add new user to new board.
            board.Users.Add(george);
            board.Update(true);

            // Verify that user successfully saved to board
            Board boardWithGeorge = new Board(board.Id, credential);

            // Note that the user creating the board is also added to the board,
            // so count should be 2, not 1
            Assert.IsTrue(board.Users.Count == 2 &&
                board.Users.Count == boardWithGeorge.Users.Count &&
                boardWithGeorge.Users[0].PersonId == user.Id &&
                boardWithGeorge.Users[0].Name == "George Washington");

            // Cleanup
            george.Delete();
            boardWithGeorge.Delete();
        }

        [TestMethod]
        public void AddStakeholderToProject()
        {
            Credential credential = TestCredential;

            // Create a new project
            Project project = new Project(credential);
            project.Name = "New Project - " + Guid.NewGuid().ToString();
            project.Update();

            // Verify saved
            Projects projects = new Projects(project.Name, credential);

            Assert.IsTrue(projects.Count == 1 && projects[0].Name == project.Name
                && projects[0].Id == project.Id);

            // Now add a new person
            Person stakeholder = new Person(credential);
            stakeholder.Name = "Jane Stakeholder - " + Guid.NewGuid().ToString();
            stakeholder.UserName = "jstakeholder_" + Guid.NewGuid().ToString();
            stakeholder.Update();

            // Verify stakeholder added to database
            People stakeholders = new People(stakeholder.Name, credential);

            Assert.IsTrue(stakeholders.Count == 1 && stakeholders[0].Name == stakeholder.Name
                && stakeholders[0].Id == stakeholder.Id);

            // Add the new stakeholder to the new project
            project.Stakeholders.Add(stakeholder);
            project.Update();

            // Verify that data has been saved
            projects = new Projects(project.Name, credential);

            Assert.IsTrue(projects.Count == 1 & projects[0].Stakeholders.Count == 1
                && projects[0].Stakeholders[0].Name == project.Stakeholders[0].Name
                && projects[0].Stakeholders[0].Id == project.Stakeholders[0].Id);

            // Cleanup
            project.Stakeholders.Remove(stakeholder);

            // Verify removed
            projects = new Projects(project.Name, credential);

            Assert.IsTrue(projects.Count == 1 && projects[0].Stakeholders.Count == 0
                && projects[0].Name == project.Name && projects[0].Id == project.Id);

            // Finally, delete the sample project and stakeholder
            stakeholder.Delete();
            project.Delete();

            // Verify deleted
            stakeholders = new People(stakeholder.Name, credential);
            projects = new Projects(project.Name, credential);

            Assert.IsTrue(stakeholders.Count == 0 && projects.Count == 0);

        }

        [TestMethod]
        public void AddStatusCodeToBoardSet()
        {
            Credential credential = TestCredential;

            // Create the board set
            BoardSet boardSet = new BoardSet(credential);
            boardSet.Name = Guid.NewGuid().ToString();
            boardSet.Update();

            // Verify saved
            BoardSets boardSets = new BoardSets(boardSet.Name, credential);

            Assert.IsTrue(boardSets.Count == 1);

            // Add some status codes to the board set
            boardSet.StatusCodes.Add(new StatusCode("Code 1", credential));
            boardSet.StatusCodes.Add(new StatusCode("Code 2", credential));
            boardSet.StatusCodes.Add(new StatusCode("Code 3", credential));
            boardSet.Update();

            // Verify status codes were correctly saved

            StatusCodes statusCodes = new StatusCodes(boardSet, credential);

            Assert.IsTrue(statusCodes.Count == boardSet.StatusCodes.Count);
            for (int i = 0; i < statusCodes.Count; i++)
            {
                Assert.IsTrue(statusCodes[i].Status == boardSet.StatusCodes[i].Status);
            }

            // Cleanup
            for (int i = boardSet.StatusCodes.Count - 1; i >= 0; i--)
            {
                boardSet.StatusCodes[i].Delete();
            }
            boardSet.Delete();
        }

        [TestMethod]
        public void AddSubTaskToTask()
        {
            Credential credential = TestCredential;

            // Create a new task
            Task task = new Task(credential);
            task.Name = "New Task - " + Guid.NewGuid().ToString();
            task.Update();

            // Create a new sub-task
            Task subTask = new Task(credential);
            subTask.Name = "sub-task - " + Guid.NewGuid().ToString();
            subTask.Update();

            // Verify both have been saved to database
            Task task2 = new Task(task.Id, credential);
            Task subTask2 = new Task(subTask.Id, credential);

            Assert.IsTrue(task.Id == task2.Id &&
                task.Name == task2.Name &&
                subTask2.Id == subTask.Id &&
                subTask2.Name == subTask.Name);

            // Add the sub-task to the task
            task.SubTasks.Add(subTask);
            task.SubTasks.Update();
            task.Update();

            // Verify sub-task saved to task collection
            Tasks tasks = new Tasks(task.Name, credential);

            Assert.IsTrue(tasks.Count == 1 &&
                tasks[0].Id == task.Id &&
                tasks[0].Name == task.Name &&
                tasks[0].SubTasks.Count == 1 &&
                tasks[0].SubTasks[0].Id == subTask.Id &&
                tasks[0].SubTasks[0].Name == subTask.Name);

            // Cleanup
            subTask.Delete();
            task.Delete();

        }

        [TestMethod]
        public void AddTask()
        {
            Credential adminUser = TestCredential;

            // Create a new user
            Person user = new Person(adminUser);
            user.Name = "New user - " + Guid.NewGuid().ToString();
            user.UserName = Guid.NewGuid().ToString();
            user.Password = "zippy";
            user.Update();

            // Get credential
            Credential credential = new Credential(user.UserName, "zippy");

            // Make sure we have a valid credential
            Assert.IsTrue(credential.Id > 0);

            Task task = new Task(credential);
            task.Name = "My new task - " + Guid.NewGuid().ToString();
            task.DefineDone = "When task successfully saved";
            task.EstHours = 2;
            task.ActHours = 6;
            task.Credential = credential;
            task.Update();

            Tasks findTask = new Tasks(task.Name, credential);

            Assert.IsTrue(findTask.Count == 1 && findTask[0].Id == task.Id);

            // Cleanup
            findTask[0].Delete();
            user.Delete();
        }

        [TestMethod]
        public void VerifyBoardCreatedByAndModifiedBy()
        {
            // Create board under one credential
            Credential cred1 = TestCredential;
            Board board = new Board(cred1);
            board.Name = "New Project - " + Guid.NewGuid().ToString();
            board.Update();

            // Verify created by & modified by 
            Assert.IsTrue(board.Id > 0
                && board.CreatedBy == cred1.Id
                && board.ModifiedBy == cred1.Id);

            // Modify the board under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            Board board2 = new Board(board.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(1000);

            board2.Update(true);

            // Confirm that created by unchanged, but modified set to new id
            Assert.IsTrue(board2.CreatedBy == board.CreatedBy
                && board2.Created == board.Created
                && board2.ModifiedBy == cred2.Id
                && board2.Modified > board.Modified);

            // Cleanup
            board.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void VerifyBoardSetCreatedByAndModifiedBy()
        {
            // Create board set under one credential
            Credential cred1 = TestCredential;
            BoardSet boardSet = new BoardSet(cred1);
            boardSet.Name = "New Project - " + Guid.NewGuid().ToString();
            boardSet.Update();

            // Verify created by & modified by 
            Assert.IsTrue(boardSet.Id > 0
                && boardSet.CreatedBy == cred1.Id
                && boardSet.ModifiedBy == cred1.Id);

            // Modify the board set under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            BoardSet boardSet2 = new BoardSet(boardSet.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(1000);

            boardSet2.Update(true);

            // Confirm that created by unchanged, but modified set to new id
            Assert.IsTrue(boardSet2.CreatedBy == boardSet.CreatedBy
                && boardSet2.Created == boardSet.Created
                && boardSet2.ModifiedBy == cred2.Id
                && boardSet2.Modified > boardSet.Modified);

            // Cleanup
            boardSet.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void VerifyCommentCreatedByAndModifiedBy()
        {
            // Create comment under one credential
            Credential cred1 = TestCredential;
            Comment comment = new Comment(cred1);
            comment.Name = "New Project - " + Guid.NewGuid().ToString();
            comment.Update();

            // Verify created by & modified by 
            Assert.IsTrue(comment.Id > 0
                && comment.CreatedBy == cred1.Id
                && comment.ModifiedBy == cred1.Id);

            // Modify the comment under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            Comment comment2 = new Comment(comment.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(1000);

            comment2.Update(true);

            // Confirm that created by unchanged, but modified set to new id
            Assert.IsTrue(comment2.CreatedBy == comment.CreatedBy
                && comment2.Created == comment.Created
                && comment2.ModifiedBy == cred2.Id
                && comment2.Modified > comment.Modified);

            // Cleanup
            comment.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void VerifyPersonCreatedByAndModifiedBy()
        {
            // Create person under one credential
            Credential cred1 = TestCredential;
            Person person = new Person(cred1);
            person.Name = "New Project - " + Guid.NewGuid().ToString();
            person.UserName = "newuser_" + Guid.NewGuid().ToString();
            person.Update();

            // Verify created by & modified by 
            Assert.IsTrue(person.Id > 0
                && person.CreatedBy == cred1.Id
                && person.ModifiedBy == cred1.Id);

            // Modify the task under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            Person person2 = new Person(person.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(1000);

            person2.Update(true);

            // Confirm that created by unchanged, but modified set to new id
            Assert.IsTrue(person2.CreatedBy == person.CreatedBy
                && person2.Created == person.Created
                && person2.ModifiedBy == cred2.Id
                && person2.Modified > person.Modified);

            // Cleanup
            person.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void VerifyProjectCreatedByAndModifiedBy()
        {
            // Create project under one credential
            Credential cred1 = TestCredential;
            Project project = new Project(cred1);
            project.Name = "New Project - " + Guid.NewGuid().ToString();
            project.Update();

            // Verify created by & modified by 
            Assert.IsTrue(project.Id > 0
                && project.CreatedBy == cred1.Id
                && project.ModifiedBy == cred1.Id);

            // Modify the project under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            Project project2 = new Project(project.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(1000);

            project2.Update(true);

            // Confirm that created by unchanged, but modified set to new id
            Assert.IsTrue(project2.CreatedBy == project.CreatedBy
                && project2.Created == project.Created
                && project2.ModifiedBy == cred2.Id
                && project2.Modified > project.Modified);

            // Cleanup
            project.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void VerifySprintCreatedByAndModifiedBy()
        {
            // Create sprint under one credential
            Credential cred1 = TestCredential;
            Sprint sprint = new Sprint(cred1);
            sprint.StartDate = DateTime.Now;
            sprint.EndDate = DateTime.Now;
            sprint.Update();

            // Verify created by & modified by 
            Assert.IsTrue(sprint.Id > 0
                && sprint.CreatedBy == cred1.Id
                && sprint.ModifiedBy == cred1.Id);

            // Modify the sprint under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            Sprint sprint2 = new Sprint(sprint.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(1000);

            sprint2.Update(true);

            // Confirm that created by unchanged, but modified set to new id
            Assert.IsTrue(sprint2.CreatedBy == sprint.CreatedBy
                && sprint2.Created == sprint.Created
                && sprint2.ModifiedBy == cred2.Id
                && sprint2.Modified > sprint.Modified);

            // Cleanup
            sprint.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void VerifyStatusCodeCreatedByAndModifiedBy()
        {
            // Create status code under one credential
            Credential cred1 = TestCredential;
            StatusCode statusCode = new StatusCode(cred1);
            statusCode.Name = "New Status Code - " + Guid.NewGuid().ToString();
            statusCode.ParentId = 12345;
            statusCode.Update();

            // Verify created by & modified by 
            Assert.IsTrue(statusCode.Id > 0
                && statusCode.CreatedBy == cred1.Id
                && statusCode.ModifiedBy == cred1.Id);

            // Modify the status code under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            StatusCode statusCode2 = new StatusCode(statusCode.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(1000);

            statusCode2.Update(true);

            // Confirm that created by unchanged, but modified set to new id
            Assert.IsTrue(statusCode2.CreatedBy == statusCode.CreatedBy
                && statusCode2.Created == statusCode.Created
                && statusCode2.ModifiedBy == cred2.Id
                && statusCode2.Modified > statusCode.Modified);

            // Cleanup
            statusCode.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void VerifyTagCreatedByAndModifiedBy()
        {
            // Create tag under one credential
            Credential cred1 = TestCredential;
            Tag tag = new Tag(cred1);
            tag.Name = "New Task - " + Guid.NewGuid().ToString();
            tag.Update();

            // Verify created by & modified by 
            Assert.IsTrue(tag.Id > 0 && tag.CreatedBy == cred1.Id && tag.ModifiedBy == cred1.Id);

            // Modify the tag under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            Tag tag2 = new Tag(tag.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(250);

            tag2.Update(true);

            // Confirm that created by AND modified unchanged, because tags aren't updated,
            // if they need to be changed, should be deleted and re-added
            Assert.IsTrue(tag2.CreatedBy == tag.CreatedBy
                && tag2.Created == tag.Created
                && tag2.ModifiedBy == cred1.Id
                && tag2.Modified == tag.Modified);

            // Cleanup
            tag.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void VerifyTaskCreatedByAndModifiedBy()
        {
            // Create task under one credential
            Credential cred1 = TestCredential;
            Task task = new Task(cred1);
            task.Name = "New Task - " + Guid.NewGuid().ToString();
            task.Update();

            // Verify created by & modified by 
            Assert.IsTrue(task.Id > 0 && task.CreatedBy == cred1.Id && task.ModifiedBy == cred1.Id);

            // Modify the task under another credential
            Person newUser = new Person(cred1);
            newUser.Name = "New User - " + Guid.NewGuid().ToString();
            newUser.UserName = "newuser_" + Guid.NewGuid().ToString();
            newUser.Password = "password";
            newUser.Update();

            Credential cred2 = new Credential(newUser.UserName, "password");

            Task task2 = new Task(task.Id, cred2);

            // Wait a short time to ensure that modified date different than
            // Created by date
            System.Threading.Thread.Sleep(1000);

            task2.Update(true);

            // Confirm that created by unchanged, but modified set to new id
            Assert.IsTrue(task2.CreatedBy == task.CreatedBy
                && task2.Created == task.Created
                && task2.ModifiedBy == cred2.Id
                && task2.Modified > task.Modified);

            // Cleanup
            task.Delete();
            newUser.Delete();
        }

        [TestMethod]
        public void AddTaskToProject()
        {
            Credential credential = TestCredential;

            Project p = new Project(credential);
            p.Name = "New Project - " + Guid.NewGuid().ToString();

            Task t = new Task(credential);
            t.Name = "New Task - " + Guid.NewGuid().ToString();

            p.Tasks.Add(t);

            p.Update();

            Project p2 = new Project(p.Id, credential);

            Assert.IsTrue(p.Id == p2.Id && p.Tasks.Count == p2.Tasks.Count && p.Tasks[0].Id == p2.Tasks[0].Id);
        }

        [TestMethod]
        public void AddTaskToTask()
        {
            Credential credential = TestCredential;

            // Verify that parent task id# is being saved
            Task tParent = new Task(credential);
            tParent.Name = "Parent task - " + Guid.NewGuid().ToString();
            tParent.Update();

            Task tChild = new Task(credential);
            tChild.Name = "Child task - " + Guid.NewGuid().ToString();
            tChild.Update();

            tParent.SubTasks.Add(tChild);
            tParent.SubTasks.Update();

            // Get the child and verify that the parent task id is correct
            Task tChild2 = new Task(tChild.Id, credential);

            Assert.IsTrue(tChild.ParentTaskId != 0 && tChild.ParentTaskId == tChild2.ParentTaskId);

            // Cleanup
            tParent.Delete();
            tChild.Delete();
        }

        [TestMethod]
        public void BoardProjects()
        {
            Credential credential = TestCredential;

            Board board = new Board(2, credential);
        }

        [TestMethod]
        public void Boards()
        {
            Credential credential = TestCredential;

            Boards boards = new Boards(credential);
            Board board = new Board(credential);
            board.Name = "new board - " + Guid.NewGuid().ToString();
            boards.Add(board);
            boards.Update();

            Board board2 = boards[0];

            Assert.IsTrue(board.Id == board2.Id &&
                board.Name == board2.Name);

            Boards boards2 = new Boards(board.Name, credential);

            Assert.IsTrue(boards2.Count == 1 &&
                board.Id == boards2[0].Id &&
                board.Name == boards2[0].Name);

            // Cleanup
            board.Delete();

        }

        [TestMethod]
        public void DeletePerson()
        {
            Credential credential = TestCredential;

            Person p1 = new Person(credential);
            p1.Name = "Jane Doe";
            p1.Update();

            People ps1 = new People("Jane Doe", credential);

            Person p2 = new Person(p1.Id, credential);
            p2.Delete();

            People ps2 = new People("Jane Doe", credential);
        }

        [TestMethod]
        public void DeleteProject()
        {
            Credential credential = TestCredential;

            // Create a new project
            Project p = new Project(credential);
            p.Name = "New Project - " + Guid.NewGuid().ToString();
            p.Update();

            // Add a few tasks
            Task t1 = new Task(p.Name + " - task 1", credential);
            p.Tasks.Add(t1);
            p.Tasks.Add(new Task(p.Name + " - task 2", credential));
            p.Tasks.Add(new Task(p.Name + " - task 3", credential));

            // Save the project
            p.Update();

            // Verify saved correctly
            Project p2 = new Project(p.Id, credential);

            Assert.IsTrue(p.Id == p2.Id && p.Name == p2.Name && p.Tasks.Count == p2.Tasks.Count && p.Tasks[0].Name == p2.Tasks[0].Name);

            // Delete the task
            p.Delete();

            // Make sure project and all child tasks were deleted
            Assert.IsTrue(
                    new Projects(p.Name, credential).Count == 0
                    && new Tasks(p.Tasks[0].Name, credential).Count == 0
                    && new Tasks(p.Tasks[1].Name, credential).Count == 0
                    && new Tasks(p.Tasks[2].Name, credential).Count == 0
                );
        }

        [TestMethod]
        public void DeleteTask()
        {
            Credential credential = TestCredential;

            Task t = new Task(credential);
            t.Name = Guid.NewGuid().ToString();
            t.Update();

            Task t2 = new Task(t.Id, credential);

            Assert.IsTrue(t.Id == t2.Id && t.Name == t2.Name);

            Tasks ts = new Tasks(t.Name, credential);

            Assert.IsTrue(ts.Count == 1 && ts[0].Id == t.Id && ts[0].Name == t.Name);

            t.Delete();

            Tasks ts2 = new Tasks(t2.Name, credential);

            Assert.IsTrue(ts2.Count == 0);
        }

        [TestMethod]
        public void GetBoardJSON()
        {
            Credential credential = TestCredential;

            Board board = new Board(credential);
            board.Name = "New Board - " + Guid.NewGuid().ToString();
            string boardJson = Data.GetJson(board);
            Project project = new Project(credential);
            project.Name = "New Project - " + Guid.NewGuid().ToString();
            board.Projects.Add(project);
            string projectJson = Data.GetJson(board.Projects[0]);
            string projectsJson = Data.GetJson(board.Projects);

            // Cleanup
            project.Delete();
            board.Delete();
        }

        [TestMethod]
        public void GetBoardSetsByName()
        {
            Credential credential = TestCredential;

            BoardSets boardSets = new BoardSets("%", credential);
        }

        [TestMethod]
        public void GetTaskJSON()
        {
            Credential credential = TestCredential;

            Task t = new Task(4, credential);
            string taskJson = Data.GetJson(t);
            string taskJson2 = t.JSON();

            // Make sure both methods return the same value
            Assert.IsTrue(taskJson == taskJson2);
        }

        [TestMethod]
        public void GetBoardSet()
        {
            Credential credential = TestCredential;

            BoardSet boardSet = new BoardSet(2, credential);
        }

        [TestMethod]
        public void GetAllPeople()
        {
            Credential credential = TestCredential;

            People people = new People(credential);
        }

        [TestMethod]
        public void VerifyEncryption()
        {
            string pwd = "password";
            string guid = Guid.NewGuid().ToString();
            string encryptedPwd = EncDec.Encrypt(pwd, guid);
            string decryptedPwd = EncDec.Decrypt(encryptedPwd, guid);

            Assert.IsTrue(pwd == decryptedPwd);
        }

        [TestMethod]
        public void GetCredential()
        {
            Credential adminUser = TestCredential;

            // Create a new user
            Person user = new Person(adminUser);
            user.Name = Guid.NewGuid().ToString();
            user.UserName = "zippy - " + Guid.NewGuid().ToString();
            user.Password = "fenwick";
            user.Update();

            // Verify that user saved to database
            Person user2 = (new People(user.Name, adminUser))[0];

            Assert.IsTrue(user.Name == user2.Name && user.Id == user2.Id);

            // Get user and make sure password is correct.
            People people = new People(user.Name, adminUser);
            Person person = people[0];
            person.Password = "yabadabadoo";
            person.Update();

            // Get credential for user/pwd
            Credential credential = new Credential(user.UserName, "yabadabadoo");

            // Verify that credential is for the desired user
            Assert.IsTrue(person.Id == credential.Id);

            // Change the password
            person.Password = "zippity-do-dah!";
            person.Update();

            // Get credential again, should be 0
            credential = new Credential(user.UserName, "yabadabadoo");

            // Confirm it!
            Assert.IsTrue(credential.Id == 0);

            // Change password back
            person.Password = "yabadabadoo";
            person.Update();

            // Verify all is back as it should be
            credential = new Credential(user.UserName, "yabadabadoo");
            Assert.IsTrue(person.Id == credential.Id);

            // Delete the user
            user.Delete();

            // Verify deleted
            people = new People(user.Name, adminUser);

            Assert.IsTrue(people.Count == 0);
        }

        [TestMethod]
        public void GetCredentialBoards()
        {
            Credential credential = TestCredential;

            // Create a new board
            Board board = new Board(credential);
            board.Name = "New board - " + Guid.NewGuid().ToString();
            board.Update();

            // Create a new person entry
            Person p = new Person(TestCredential);
            p.Name = "Jane Doe";
            p.UserName = "jdoe_" + Guid.NewGuid().ToString();
            p.Password = "password";
            p.Update();

            // Create a new user based on the new person
            User user = new User(board.Id, p.Id, credential);
            user.Update();

            // Add the user to the board
            board.Users.Add(user);
            board.Update();

            // Verify user is now a member of the board
            Boards boards = new Boards(board.Name, credential);

            Assert.IsTrue(boards.Count == 1 &&
                boards[0].Name == board.Name &&
                boards[0].Users.Count == 2 &&
                boards[0].Users[0].Id == user.Id);

            // Now get credentials for new user
            Credential newUserCredential = new Credential(user.UserName, "password");

            // Verify that board is in list of user boards
            Assert.IsTrue(newUserCredential.Boards.Count == 1 && newUserCredential.Boards[0].Name == board.Name && newUserCredential.Boards[0].Id == board.Id);

            // Cleanup
            p.Delete();
            user.Delete();
            board.Delete();
        }

        [TestMethod]
        public void GetPeopleForBoard()
        {
            Credential credential = TestCredential;

            Board board = new Board(2, credential);
            People people = new People(board, credential);
        }

        [TestMethod]
        public void GetTaskProjectId()
        {
            Credential credential = TestCredential;

            // Create a new project
            Project p = new Project(credential);
            p.Name = "New project - " + Guid.NewGuid().ToString();
            p.Update();

            // Create a new task
            Task t = new Task(credential);
            t.Name = "New task - " + Guid.NewGuid().ToString();
            t.Update();

            // Add a sub-task
            Task subT = new Task(credential);
            subT.Name = "new sub-task - " + Guid.NewGuid().ToString();
            subT.Update();

            // Add the sub-task to the task
            t.SubTasks.Add(subT);
            t.SubTasks.Update(true);

            // Verify that sub-task has been added
            Tasks tasks = new Tasks(t.Name, credential);

            Assert.IsTrue(tasks.Count == 1 &&
                tasks[0].Id == t.Id &&
                tasks[0].Name == t.Name &&
                tasks[0].SubTasks.Count != 0 &&
                tasks[0].SubTasks[0].Id == subT.Id &&
                tasks[0].SubTasks[0].Name == subT.Name);

            // Now verify that Project Id for both task & sub-task are 0
            // because they haven't been added to the project yet.
            Assert.IsTrue(tasks[0].ProjectId == 0 &&
                tasks[0].SubTasks[0].ProjectId == 0);

            // Add the top-level task to the project and then verify that
            // both top-level and sub-tasks show project id for ProjectId
            p.Tasks.Add(t);
            p.Tasks.Update();

            Projects ps = new Projects(p.Name, credential);

            Assert.IsTrue(ps.Count == 1 &&
                ps[0].Id == p.Id &&
                ps[0].Name == p.Name &&
                ps[0].Tasks.Count == 1 &&
                ps[0].Tasks[0].ProjectId == p.Id &&
                ps[0].Tasks[0].SubTasks.Count == 1 &&
                ps[0].Tasks[0].SubTasks[0].Id == subT.Id &&
                ps[0].Tasks[0].SubTasks[0].ProjectId == p.Id);

            // Cleanup
            subT.Delete();
            t.Delete();
            p.Delete();
        }

        [TestMethod]
        public void UpdateBoard()
        {
            Credential credential = TestCredential;

            Board board = new Board(1, credential);
            board.Name = "zzz Updated by Mark";
            board.ParentId = 2;
            board.Update();
        }

        [TestMethod]
        public void RemoveBoardFromBoardSet()
        {
            Credential credential = TestCredential;

            // Create a new board set
            BoardSet boardSet = new BoardSet(credential);
            boardSet.Name = "My new board set - " + Guid.NewGuid().ToString();
            boardSet.Update();

            // Create a new board
            Board board = new Board(credential);
            board.Name = "My new board" + Guid.NewGuid().ToString();
            board.Update();

            // Add user to board
            User user = new User(512, credential);
            board.Users.Add(user);
            board.Users.Update();

            boardSet.Boards.Add(board);
            boardSet.Update();

            // Verify that the changes have been saved to the database
            BoardSet updatedBoardSet = new BoardSet(boardSet.Id, credential);

            Assert.IsTrue(boardSet.Id == updatedBoardSet.Id
                && boardSet.Boards.Count == updatedBoardSet.Boards.Count
                && boardSet.Boards[0].Id == updatedBoardSet.Boards[0].Id
                && boardSet.Boards[0].Name == updatedBoardSet.Boards[0].Name);

            // Remove the board from the board set
            int originalBoardCount = boardSet.Boards.Count;
            boardSet.Boards.Remove(board);
            int newBoardCount = boardSet.Boards.Count;

            Assert.IsTrue(originalBoardCount == newBoardCount + 1);

            // Put the board back in this board set then verify
            boardSet.Boards.Add(board);
            boardSet.Update();

            Assert.IsTrue(boardSet.Boards.Count == 1 && boardSet.Boards[0].Name == board.Name && boardSet.Boards[0].Id == board.Id);

            // Delete the board set and verify that the board has also been deleted
            boardSet.Delete();

            BoardSets boardSets = new BoardSets(boardSet.Name, credential);
            Boards boards = new Boards(board.Name, credential);

            Assert.IsTrue(boardSets.Count == 0 && boards.Count == 0);
        }

        [TestMethod]
        public void UpdateProject()
        {
            Credential credential = TestCredential;

            Project project = new Project(2, credential);
            project.ActHours = 123.45;
            project.Update();
        }

        [TestMethod]
        public void UpdateTask()
        {
            Credential adminUser = TestCredential;

            Person user = new Person(adminUser);
            user.Name = "New user - " + Guid.NewGuid().ToString();
            user.UserName = Guid.NewGuid().ToString();
            user.Password = "zippy";
            user.Update();

            // Get credential
            Credential credential = new Credential(user.UserName, "zippy");

            // Create a new task
            Task t = new Task(credential);
            t.Name = "New Task - " + Guid.NewGuid().ToString();
            t.EstHours = 123.45;
            t.ActHours = 234.56;
            t.StartDate = DateTime.Now;
            t.EndDate = DateTime.Now.AddDays(15);
            t.Credential = credential;

            double factor = Math.PI;

            // Save it
            t.Update();

            // Get another copy of saved task
            Task t2 = new Task(t.Id, credential);

            // Create another user account
            Person user2 = new Person(credential);
            user2.Name = "New user 2 - " + Guid.NewGuid().ToString();
            user2.UserName = Guid.NewGuid().ToString();
            user2.Password = "zippy";
            user2.Update();

            Credential credential2 = new Credential(user2.UserName, "zippy");

            // Verify that copy in memory and copy in database match
            Assert.IsTrue(t.Name == t2.Name && t.EstHours == t2.EstHours && t.ActHours == t2.ActHours && t.StartDate == t2.StartDate && t.EndDate == t2.EndDate);

            // Modify properties
            t2.Name = t2.Name + Guid.NewGuid().ToString();
            t2.EstHours = t2.EstHours * factor;
            t2.ActHours = t2.ActHours * factor;
            t2.StartDate = t2.StartDate.AddDays(factor);
            t2.EndDate = t2.EndDate.AddDays(factor);
            t2.Credential = credential2;

            // Save updated version
            t2.Update();

            // Get updated version from database
            Task t3 = new Task(t2.Id, credential);

            // Make sure all properties still match
            Assert.IsTrue(t3.Name == t2.Name && t3.EstHours == t2.EstHours && t3.ActHours == t2.ActHours && t3.StartDate == t2.StartDate && t3.EndDate == t2.EndDate);

            // Make sure created by and modified by values are id#s of user and user2 respectively
            Assert.IsTrue(t3.CreatedBy != t3.ModifiedBy);

            // Cleanup
            t3.Delete();
            user.Delete();
            user2.Delete();

            // Make sure successfully deleted
            Assert.IsTrue(new Tasks(t3.Name, credential).Count == 0);
        }

        [TestMethod]
        public void VerifyBoardSetReturnedForProject()
        {
            BoardSet boardSet = new BoardSet(TestCredential);
            boardSet.Name = "A new board set - " + Guid.NewGuid().ToString();
            boardSet.Update();

            // Add some status codes to the board set
            for (int i = 0; i < 3; i++)
            {
                StatusCode s = new StatusCode(boardSet.Credential);
                s.Name = "Status " + i.ToString();
                boardSet.StatusCodes.Add(s);
            }
            boardSet.StatusCodes.Update();

            Board board = new Board(TestCredential);
            board.Name = "A new board - " + Guid.NewGuid().ToString();
            board.Update();

            Project project0 = new Project(TestCredential);
            project0.Name = "A new project - " + Guid.NewGuid().ToString();

            boardSet.Boards.Add(board);
            boardSet.Update();

            board.Projects.Add(project0);
            board.Projects.Update();

            Project project = new Project(project0.Id, TestCredential);

            // Make sure task returned a board set name
            Assert.IsTrue(project.BoardSetId > 0 &&
                !string.IsNullOrEmpty(project.BoardSetName) &&
                project.StatusCodes.Count > 0);

            // Make sure status codes returned by task are identical to those 
            // returned by the underlying board set
            BoardSet boardSet2 = new BoardSet(project.BoardSetId, TestCredential);

            Assert.IsTrue(project.StatusCodes.Count == boardSet2.StatusCodes.Count);

            for (int i = 0; i < project.StatusCodes.Count; i++)
            {
                Assert.IsTrue(project.StatusCodes[i].Id == boardSet.StatusCodes[i].Id);
            }

            // Cleanup
            boardSet.Delete();
            board.Delete();
            project.Delete();
        }

        [TestMethod]
        public void VerifyBoardSetReturnedForTask()
        {
            BoardSet boardSet = new BoardSet(TestCredential);
            boardSet.Name = "A new board set - " + Guid.NewGuid().ToString();
            boardSet.Update();

            // Add some status codes to the board set
            for (int i = 0; i < 3; i++)
            {
                StatusCode s = new StatusCode(boardSet.Credential);
                s.Name = "Status " + i.ToString();
                boardSet.StatusCodes.Add(s);
            }
            boardSet.StatusCodes.Update();

            Board board = new Board(TestCredential);
            board.Name = "A new board - " + Guid.NewGuid().ToString();
            board.Update();

            Project project0 = new Project(TestCredential);
            project0.Name = "A new project - " + Guid.NewGuid().ToString();

            boardSet.Boards.Add(board);
            boardSet.Update();

            board.Projects.Add(project0);
            board.Projects.Update();

            Project project = new Project(project0.Id, TestCredential);

            // Make sure task returned a board set name
            Assert.IsTrue(project.BoardSetId > 0 &&
                !string.IsNullOrEmpty(project.BoardSetName) &&
                project.StatusCodes.Count > 0);

            // Make sure status codes returned by task are identical to those 
            // returned by the underlying board set
            BoardSet boardSet2 = new BoardSet(project.BoardSetId, TestCredential);

            Assert.IsTrue(project.StatusCodes.Count == boardSet2.StatusCodes.Count);

            for (int i = 0; i < project.StatusCodes.Count; i++)
            {
                Assert.IsTrue(project.StatusCodes[i].Id == boardSet.StatusCodes[i].Id);
            }

            Task task = new Task(TestCredential);
            task.Name = "A new task - " + Guid.NewGuid().ToString();
            project.Tasks.Add(task);
            project.Tasks.Update();

            // Make sure task returned a board set name
            Assert.IsTrue(task.BoardSetId > 0 &&
                !string.IsNullOrEmpty(task.BoardSetName) &&
                task.StatusCodes.Count > 0);

            // Make sure status codes returned by task are identical to those 
            // returned by the underlying board set
            boardSet = new BoardSet(task.BoardSetId, TestCredential);

            Assert.IsTrue(task.StatusCodes.Count == boardSet.StatusCodes.Count);

            for (int i = 0; i < task.StatusCodes.Count; i++)
            {
                Assert.IsTrue(task.StatusCodes[i].Id == boardSet.StatusCodes[i].Id);
            }

            // Cleanup
            boardSet.Delete();
            board.Delete();
            project.Delete();
            task.Delete();
        }

        [TestMethod]
        public void GetJsonViaProperty()
        {
            Project project = new Project(TestCredential);
            project.Name = "test project - " + Guid.NewGuid().ToString();
            Task task = new Task(TestCredential);
            task.Name = "test task - " + Guid.NewGuid().ToString();
            project.Tasks.Add(task);
            project.Update();

            string json = project.JSON();
            string json2 = Data.GetJson(project);

            Assert.IsTrue(!string.IsNullOrEmpty(json) && json == json2);
        }

        [TestMethod]
        public void ReturnEstHoursWithoutSubTasks()
        {
            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();
            t.EstHours = 123.45;
            t.Update();

            Task t2 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t.Id == t2.Id &&
                t2.Id > 0 && t.EstHours == 123.45 &&
                t2.EstHours == 123.45);

            // Cleanup
            t.Delete();
        }

        [TestMethod]
        public void ReturnEstHoursWithSubTasks()
        {
            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();
            t.EstHours = 123.45;
            t.Update();

            Task t2 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t.Id == t2.Id &&
                t2.Id > 0 && t.EstHours == 123.45 &&
                t2.EstHours == 123.45);

            Task subTask1 = new Task(TestCredential);
            subTask1.Name = "sub-task - " + Guid.NewGuid().ToString();
            subTask1.EstHours = Math.PI;

            Task subTask2 = new Task(TestCredential);
            subTask2.Name = "sub-task 2 - " + Guid.NewGuid().ToString();
            subTask2.EstHours = Math.E;

            t.SubTasks.Add(subTask1);
            t.SubTasks.Add(subTask2);
            t.Update();

            Task t3 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t3.EstHours == Math.Round(Math.PI + Math.E, 2));

            // Cleanup
            t.Delete();

            // Verify sub-tasks have been deleted
            Task subTask2b = new Task(subTask2.Id, TestCredential);

            Assert.IsTrue(Data.GetTagById(subTask1.Id, TestCredential.Id).Tables[0].Rows.Count == 0 &&
                Data.GetTagById(subTask2.Id, TestCredential.Id).Tables[0].Rows.Count == 0);
        }

        [TestMethod]
        public void ReturnActHoursWithoutSubTasks()
        {
            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();
            t.ActHours = 123.45;
            t.Update();

            Task t2 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t.Id == t2.Id &&
                t2.Id > 0 && t.ActHours == 123.45 &&
                t2.ActHours == 123.45);

            // Cleanup
            t.Delete();
        }

        [TestMethod]
        public void ReturnActHoursWithSubTasks()
        {
            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();
            t.ActHours = 123.45;
            t.Update();

            Task t2 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t.Id == t2.Id &&
                t2.Id > 0 && t.ActHours == 123.45 &&
                t2.ActHours == 123.45);

            Task subTask1 = new Task(TestCredential);
            subTask1.Name = "sub-task - " + Guid.NewGuid().ToString();
            subTask1.ActHours = Math.PI;

            Task subTask2 = new Task(TestCredential);
            subTask2.Name = "sub-task 2 - " + Guid.NewGuid().ToString();
            subTask2.ActHours = Math.E;

            t.SubTasks.Add(subTask1);
            t.SubTasks.Add(subTask2);
            t.Update();

            Task t3 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t3.ActHours == Math.Round(Math.PI + Math.E, 2));

            // Cleanup
            t.Delete();

            // Verify sub-tasks have been deleted
            Task subTask2b = new Task(subTask2.Id, TestCredential);

            Assert.IsTrue(Data.GetTagById(subTask1.Id, TestCredential.Id).Tables[0].Rows.Count == 0 &&
                Data.GetTagById(subTask2.Id, TestCredential.Id).Tables[0].Rows.Count == 0);
        }

        [TestMethod]
        public void SetStatusColor()
        {
            StatusCode statusCode = new StatusCode(TestCredential);
            statusCode.Name = "Test Status";
            statusCode.ColumnHeading = "Test Heading";
            statusCode.ForeColor = "red";
            statusCode.BackColor = "magenta";
            statusCode.ParentId = 12345;
            statusCode.Update();

            StatusCode statusCode2 = new StatusCode(statusCode.Id, TestCredential);

            // Verify that fore and back colors saved correctly
            Assert.IsTrue(statusCode.Id == statusCode2.Id &&
                statusCode.ForeColor == statusCode2.ForeColor &&
                statusCode.BackColor == statusCode2.BackColor);

            // Change the color again
            statusCode.ForeColor = "green";
            statusCode.BackColor = "yellow";
            statusCode.Update();

            StatusCode statusCode3 = new StatusCode(statusCode.Id, TestCredential);

            // Verify that fore and back colors saved correctly
            Assert.IsTrue(statusCode.Id == statusCode3.Id &&
                statusCode.ForeColor == statusCode3.ForeColor &&
                statusCode.BackColor == statusCode3.BackColor);

            // Cleanup
            statusCode2.Delete();
        }

        [TestMethod]
        public void SetUserPermissions()
        {
            Board board = new Board(TestCredential);
            board.Name = "New Board - " + Guid.NewGuid().ToString();
            board.Update();

            // Add a new person
            Person p = new Person(TestCredential);
            p.Name = "new person - " + Guid.NewGuid().ToString();
            p.UserName = p.Name;
            p.Password = "password";
            p.Update();

            User user = new User(board.Id, p.Id, TestCredential);
            user.CanAdd = true;
            user.CanEdit = true;
            user.CanDelete = false;
            user.CanRead = true;

            board.Users.Add(user);
            board.Update();

            // Verify that settings saved
            Board board2 = new Board(board.Id, TestCredential);

            Assert.IsTrue(board.Id == board2.Id &&
                board.Users.Count == board2.Users.Count &&
                board.Users.Count == 2);

            foreach (User u in board.Users.Items)
            {
                if (u.UserName == user.UserName)
                {
                    Assert.IsTrue(
                        u.CanAdd == user.CanAdd &&
                        u.CanEdit == user.CanEdit &&
                        u.CanDelete == user.CanDelete &&
                        u.CanRead == user.CanRead
                        );
                }
            }

            // Cleanup
            p.Delete();
            user.Delete();
            board.Delete();
        }

        [TestMethod]
        public void UpdateTaskStatus()
        {
            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();
            t.Status = 123;
            t.Update();

            // Verify that status was successfully updated
            Task t2 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t.Name == t2.Name &&
                t.Status == t2.Status);

            // Cleanup
            t.Delete();
        }

        [TestMethod]
        public void GetAssignedToValue()
        {
            // Create a new task, assign two individuals to it, then verify
            // that the AssignedTo value returns list of names

            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();
            t.Update();

            Person p1 = new Person(TestCredential);
            p1.Name = "Jane Doe";
            p1.UserName = "jdoe_" + Guid.NewGuid().ToString();

            t.Assignees.Add(p1);

            Person p2 = new Person(TestCredential);
            p2.Name = "John Smith";
            p2.UserName = "jsmith_" + Guid.NewGuid().ToString();

            t.Assignees.Add(p2);

            t.Update();

            // Now retrieve data from database and get
            // value of AssignedTo
            Task t2 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t.Id == t2.Id &&
                t.Assignees.Count == 2 &&
                t.Assignees.Count == t2.Assignees.Count &&
                t.AssignedTo == p1.Name + ", " + p2.Name &&
                t2.AssignedTo == t.AssignedTo);

            // Cleanup
            p1.Delete();
            p2.Delete();
            t.Delete();
        }

        [TestMethod]
        public void VerifyDefaultStatusValue()
        {
            // Create a new board set
            BoardSet bs = new BoardSet(TestCredential);
            bs.Name = "My New Boardset - " + Guid.NewGuid().ToString();
            bs.Update();

            // Add some status codes to the board set
            StatusCode s1 = new StatusCode("Status A", TestCredential);
            s1.Sequence = 0;
            StatusCode s2 = new StatusCode("Status B", TestCredential);
            s2.Sequence = 1;
            StatusCode s3 = new StatusCode("Status C", TestCredential);
            s3.Sequence = 2;
            bs.StatusCodes.Add(s1);
            bs.StatusCodes.Add(s2);
            bs.StatusCodes.Add(s3);

            // Create a board & add to set
            Board b = new Board(TestCredential);
            b.Name = "board - " + Guid.NewGuid().ToString();
            bs.Boards.Add(b);

            // Add a project to the board
            Project p = new Project(TestCredential);
            p.Name = "project - " + Guid.NewGuid().ToString();
            b.Projects.Add(p);

            // Add a task to the project
            Task t = new Task(TestCredential);
            t.Name = "Some new task";
            p.Tasks.Add(t);

            // Save the whole thing
            bs.Update(true);

            // Read back the board set just saved
            BoardSet bs2 = new BoardSet(bs.Id, TestCredential);

            // Verify that task status is set to default of first
            // status in list

            Assert.IsTrue(bs.Boards[0].Projects[0].Tasks[0].Status == bs2.Boards[0].Projects[0].Tasks[0].Status);

            // Cleanup
            t.Delete();
            p.Delete();
            b.Delete();
            bs.Delete();
        }

        [TestMethod]
        public void AddApprover()
        {
            Approver approver = new Approver(TestCredential);
            approver.TaskId = 12345;
            People matches = new People("Fred Flintstone", TestCredential);
            Person person;
            if (matches.Count == 0)
            {
                person = new Person(TestCredential);
                person.Name = "Fred Flintstone";
                person.UserName = "fflintstone_" + Guid.NewGuid().ToString();
                person.Update();
            }
            else
            {
                person = matches[0];
            }

            approver.PersonId = person.Id;
            approver.Update();

            // Verify saved to database
            Approver approver2 = new Approver(approver.Id, TestCredential);

            Assert.IsTrue(approver.Id == approver2.Id &&
                approver.PersonId == approver2.PersonId &&
                approver.TaskId == approver2.TaskId);

            // Cleanup
            approver.Delete();

            // Verify deleted
            Approver approver3 = new Approver(approver.Id, TestCredential);

            Assert.IsTrue(approver3.Id == 0);
        }

        [TestMethod]
        public void CreateApproverCollection()
        {
            // Create a new approver
            Approver approver = new Approver(TestCredential);
            approver.TaskId = 12345;
            approver.PersonId = TestCredential.Id;
            approver.Update();

            // Get collection of approvers for task 12345
            Approvers approvers = new Approvers(12345, TestCredential);

            // Verify collection includes expected approvers
            Assert.IsTrue(approvers.Count == 1 &&
                approvers[0].Id == approver.Id &&
                approvers[0].TaskId == approver.TaskId &&
                approvers[0].PersonId == approver.PersonId);

            // Cleanup
            approver.Delete();
        }

        [TestMethod]
        public void AddApproversToTask()
        {
            // Create a new task
            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();

            // Create two new people to add as approvers
            Person p = new Person(TestCredential);
            p.Name = "Buggs Bunny";
            p.UserName = Guid.NewGuid().ToString();
            p.Password = "password";
            p.Update();

            Person p2 = new Person(TestCredential);
            p2.Name = "Elmer Fudd";
            p2.UserName = Guid.NewGuid().ToString();
            p2.Password = "password";
            p2.Update();

            // Add the approver
            Approver approver = new Approver(TestCredential);
            approver.PersonId = p.Id;
            t.Approvers.Add(approver);

            Approver approver2 = new Approver(TestCredential);
            approver2.PersonId = p2.Id;
            t.Approvers.Add(approver2);

            t.Update();

            // Reload the task
            Task t2 = new Task(t.Id, TestCredential);

            // Verify approver saved
            Assert.IsTrue(t2.Approvers.Count == 2 &&
                t2.Approvers[0].PersonId == p.Id &&
                t2.Approvers[1].PersonId == p2.Id);

            // Cleanup
            approver.Delete();
            approver2.Delete();
            p.Delete();
            p2.Delete();
            t.Delete();
        }

        [TestMethod]
        public void GetBaseList()
        {
            // Get all project objects
            Projects projects = new Projects("", TestCredential);

            // Verify that data in base list is identical to
            // corresponding properties in project object
            List<BaseItem> projectBaseList = projects.GetBaseList();

            for (int i = 0; i < projects.Count; i++)
            {
                Assert.IsTrue(projects[i].Id == projectBaseList[i].Id);
            }
        }

        [TestMethod]
        public void AddUnsavedCommentToTask()
        {
            // Create a new task
            Task t = new Task(TestCredential);
            t.Name = "new task + " + Guid.NewGuid().ToString();

            // Add a new comment to the task
            Comment c = new Comment(TestCredential);
            c.Text = "This is a test comment for task " + t.Name;
            t.Comments.Add(c);

            // Save the task
            t.Update();

            // Verify that data correctly saved
            Task t2 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t.Id == t2.Id &&
                t.Comments.Count == t2.Comments.Count &&
                t.Comments.Count == 1 &&
                t.Comments[0].Text == t2.Comments[0].Text &&
                t.Comments[0].Id == t2.Comments[0].Id &&
                t.Id == c.TaskId &&
                t2.Id == t2.Comments[0].TaskId);

            // Cleanup
            c.Delete();
            t.Delete();
        }

        [TestMethod]
        public void SetTaskSequence()
        {
            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();
            t.Sequence = 12345;
            t.Update();

            // Verify that sequence was saved
            Task t2 = new Task(t.Id, TestCredential);

            Assert.IsTrue(t.Id == t2.Id &&
                t.Sequence == 12345 &&
                t.Sequence == t2.Sequence);

            // Cleanup
            t.Delete();
        }

        [TestMethod]
        public void GetBoardTasks()
        {
            Board b = new Board(TestCredential);
            b.Name = "new board - " + Guid.NewGuid().ToString();

            Project p = new Project(TestCredential);
            p.Name = "new project - " + Guid.NewGuid().ToString();
            b.Projects.Add(p);

            Task t = new Task(TestCredential);
            t.Name = "new task - " + Guid.NewGuid().ToString();
            p.Tasks.Add(t);

            b.Update(true);

            // Verify that can retrieve new task under board
            Board b2 = new Board(b.Id, TestCredential);

            Assert.IsTrue(b.Id == b2.Id &&
                b2.Tasks.Count == 1 &&
                b2.Tasks[0].Id == t.Id);

            // Cleanup
            t.Delete();
            p.Delete();
            b.Delete();
        }

        [TestMethod]
        public void AddPropertyUsingBaseMethod()
        {
            DataSet ds = Data.AddPropertyToObject("A property", 123.45, "task", 123, TestCredential.Id);
            DataRow dr = ds.Tables[0].Rows[0];
            long id = long.Parse(dr["id"].ToString());

            // Verify added correctly
            DataSet ds2 = Data.GetPropertyById(id, TestCredential.Id);
            DataRow dr2 = ds2.Tables[0].Rows[0];
            Assert.IsTrue(dr["id"].ToString() == dr2["id"].ToString()
                && dr["name"].ToString() == dr2["name"].ToString()
                && dr["value"].ToString() == dr2["value"].ToString()
                && dr["value_type"].ToString() == dr2["value_type"].ToString()
                && dr["parent_type"].ToString() == dr2["parent_type"].ToString()
                && dr["parent_id"].ToString() == dr2["parent_id"].ToString());

            // Cleanup
            Data.DeleteProperty(id, TestCredential.Id);
        }

        [TestMethod]
        public void CreatePropertyBag()
        {
            Task task = new Task(TestCredential);
            task.Name = "A new task - " + Guid.NewGuid().ToString();
            task.Update();

            Properties properties = new Properties(task, TestCredential);
            Property p1 = new Property(TestCredential);
            p1.Name = "p1";
            p1.Value = 123.45;
            properties.Add(p1);
            properties.Update();

            // Verify saved
            Properties properties2 = new Properties(task, TestCredential);

            Assert.IsTrue(properties.Count == 1
                && properties[0].Id > 0
                && properties[0].Id == properties2[0].Id
                && properties[0].Name == properties2[0].Name
                && (properties[0].Value.Equals(properties2[0].Value)));

            // Cleanup
            p1.Delete();

            // Verify deleted
            properties = new Properties(task, TestCredential);
            Assert.IsTrue(properties.Count == 0);

            task.Delete();
        }

        [TestMethod]
        public void CreatePropertyForPerson()
        {
            // Find entry for "Mark Gerow", if one doesn't exist, create it
            Person mark;
            People matches = new People("Mark Gerow", TestCredential);
            if (matches.Count == 0)
            {
                mark = new Person(TestCredential);
                mark.Name = "Mark Gerow";
                mark.UserName = "mgerow";
                mark.Password = "password";
                mark.Update();
            }
            else
            {
                mark = matches[0];
            }

            // Add some new properties
            Property favoriteColor = new Property(mark.Credential);
            favoriteColor.Name = "FavoriteColor";
            favoriteColor.Value = "Green";

            Property birthday = new Property(mark.Credential);
            birthday.Name = "Birthday";
            birthday.Value = new DateTime(1958, 3, 12);

            mark.Properties.Add(favoriteColor);
            mark.Properties.Add(birthday);
            mark.Properties["ElementarySchool"].Value = "Ohlones";

            mark.Properties.Update();
        }

        [TestMethod]
        public void SetPropertyByName()
        {
            Task task = new Task(TestCredential);
            task.Name = "A new task - " + Guid.NewGuid().ToString();
            task.Update();

            Properties properties = new Properties(task, TestCredential);
            properties["Size"].Value = 123.45;
            properties["Color"].Value = "Green";
            properties["Model"].Value = "Deluxe";
            properties["Date"].Value = DateTime.Now;
            properties.Update();

            // Verify saved
            Properties properties2 = new Properties(task, TestCredential);

            Assert.IsTrue(properties.Count == 4
                && properties2["Size"].Value.Equals(properties["Size"].Value)
                && properties2["Color"].Value.Equals(properties["Color"].Value)
                && properties2["Model"].Value.Equals(properties["Model"].Value)
                && ((DateTime)properties2["Date"].Value).Date == ((DateTime)properties["Date"].Value).Date);

            // Cleanup
            properties.Clear(true);
            properties = new Properties(task, TestCredential);
            task.Delete();

        }

        [TestMethod]
        public void SetTaskPropertiesByName()
        {
            Task task = new Task(TestCredential);
            task.Name = "A new task - " + Guid.NewGuid().ToString();
            task.Update();

            task.Properties["Size"].Value = 123.45;
            task.Properties["Color"].Value = "Green";
            task.Properties["Model"].Value = "Deluxe";
            task.Properties["Date"].Value = DateTime.Now;
            task.Properties.Update();

            // Verify saved
            Task task2 = new Task(task.Id, TestCredential);

            Assert.IsTrue(task.Properties.Count == 4
                && task2.Properties["Size"].Value.Equals(task.Properties["Size"].Value)
                && task2.Properties["Color"].Value.Equals(task.Properties["Color"].Value)
                && task2.Properties["Model"].Value.Equals(task.Properties["Model"].Value)
                && ((DateTime)task2.Properties["Date"].Value).Date == ((DateTime)task.Properties["Date"].Value).Date);

            // Cleanup
            task.Delete();
        }

        [TestMethod]
        public void SetBoardPropertiesByName()
        {
            Board board = new Board(TestCredential);
            board.Name = "A new board - " + Guid.NewGuid().ToString();
            board.Update();

            board.Properties["Size"].Value = 123.45;
            board.Properties["Color"].Value = "Green";
            board.Properties["Model"].Value = "Deluxe";
            board.Properties["Date"].Value = DateTime.Now;
            board.Properties.Update();

            // Verify saved
            Board board2 = new Board(board.Id, TestCredential);

            Assert.IsTrue(board.Properties.Count == 4
                && board2.Properties["Size"].Value.Equals(board.Properties["Size"].Value)
                && board2.Properties["Color"].Value.Equals(board.Properties["Color"].Value)
                && board2.Properties["Model"].Value.Equals(board.Properties["Model"].Value)
                && ((DateTime)board2.Properties["Date"].Value).Date == ((DateTime)board.Properties["Date"].Value).Date);

            // Cleanup
            board.Delete();

        }

        [TestMethod]
        public void SetPersonPropertiesByName()
        {
            Person person = new Person(TestCredential);
            person.Name = "A new person - " + Guid.NewGuid().ToString();
            person.UserName = "user_" + Guid.NewGuid().ToString();
            person.Update();

            person.Properties["Size"].Value = 123.45;
            person.Properties["Color"].Value = "Green";
            person.Properties["Model"].Value = "Deluxe";
            person.Properties["Date"].Value = DateTime.Now;
            person.Properties.Update();

            // Verify saved
            Person person2 = new Person(person.Id, TestCredential);

            Assert.IsTrue(person.Properties.Count == 4
                && person2.Properties["Size"].Value.Equals(person.Properties["Size"].Value)
                && person2.Properties["Color"].Value.Equals(person.Properties["Color"].Value)
                && person2.Properties["Model"].Value.Equals(person.Properties["Model"].Value)
                && ((DateTime)person2.Properties["Date"].Value).Date == ((DateTime)person.Properties["Date"].Value).Date);

            // Cleanup
            person.Delete();

        }

        [TestMethod]
        public void SetProjectPropertiesByName()
        {
            Project project = new Project(TestCredential);
            project.Name = "A new project - " + Guid.NewGuid().ToString();
            project.Update();

            project.Properties["Size"].Value = 123.45;
            project.Properties["Color"].Value = "Green";
            project.Properties["Model"].Value = "Deluxe";
            project.Properties["Date"].Value = DateTime.Now;
            project.Properties.Update();

            // Verify saved
            Project project2 = new Project(project.Id, TestCredential);

            Assert.IsTrue(project.Properties.Count == 4
                && project2.Properties["Size"].Value.Equals(project.Properties["Size"].Value)
                && project2.Properties["Color"].Value.Equals(project.Properties["Color"].Value)
                && project2.Properties["Model"].Value.Equals(project.Properties["Model"].Value)
                && ((DateTime)project2.Properties["Date"].Value).Date == ((DateTime)project.Properties["Date"].Value).Date);

            // Cleanup
            project.Delete();
        }

        [TestMethod]
        public void ReloadBoard()
        {
            // Create a new board
            Board b = new Board(TestCredential);

            // Add a project
            Project p = new Project(b.Credential);
            b.Projects.Add(p);
            b.Update();
            b.Projects.Update();

            // Create a 2nd instance of the same board
            Board b2 = new Board(b.Id, b.Credential);

            // Verify that they have the same project count
            Assert.IsTrue(b.Projects.Count == 1 &&
                b.Projects.Count == b2.Projects.Count);

            // Now add a 2nd project to the first instance of the board
            Project p2 = new Project(b.Credential);
            b.Projects.Add(p2);
            b.Projects.Update();

            // Verify that first instance has 2 projects and 2nd instance only 1
            Assert.IsTrue(b.Projects.Count == 2 &&
                b2.Projects.Count == 1);

            // Now reload the 2nd instance and verify that both instances 
            // have 2 projects
            b2.Reload();

            Assert.IsTrue(b.Projects.Count == 2 &&
                b.Projects.Count == b2.Projects.Count);

            // Cleanup
            p.Delete();
            p2.Delete();
            b.Delete();
        }

        [TestMethod]
        public void ReloadProject()
        {
            // Create a new project
            Project p = new Project(TestCredential);
            p.Update();

            // Add a task
            Task t = new Task(p.Credential);
            p.Tasks.Add(t);
            p.Tasks.Update();

            // Create a 2nd instance of the same project
            Project p2 = new Project(p.Id, p.Credential);

            // Verify that they have the same sub-project count
            Assert.IsTrue(p.Tasks.Count == 1 &&
                p.Tasks.Count == p2.Tasks.Count);

            // Now add a 2nd task to the first instance of the project
            Task t2 = new Task(p.Credential);
            p.Tasks.Add(t2);
            p.Tasks.Update();

            // Verify that first instance has 2 tasks and 2nd instance only 1
            Assert.IsTrue(p.Tasks.Count == 2 &&
                p2.Tasks.Count == 1);

            // Now reload the 2nd instance and verify that both instances 
            // have 2 tasks
            p2.Reload();

            Assert.IsTrue(p.Tasks.Count == 2 &&
                p.Tasks.Count == p2.Tasks.Count);

            // Cleanup
        }

        [TestMethod]
        public void ReloadTask()
        {
            // Create a new task

            // Add a sub-task

            // Create a 2nd instance of the same task

            // Verify that they have the same sub-task count

            // Now add a 2nd sub-task to the first instance of the task

            // Verify that first instance has 2 sub-tasks and 2nd instance only 1

            // Now reload the 2nd instance and verify that both instances 
            // have 2 sub-tasks

            // Cleanup
        }

        [TestMethod]
        public void ReloadTasks()
        {
            // Create a new project
            Project p1 = new Project(TestCredential);
            p1.Name = "New project - " + Guid.NewGuid().ToString();

            // Add a task
            Task t1 = new Task(p1.Credential);
            t1.Name = "New task # 1";
            p1.Tasks.Add(t1);
            p1.Tasks.Update();
            p1.Update();

            // Create a 2nd instance of the same project
            Project p2 = new Project(p1.Id, p1.Credential);

            // Verify that they have the same task count
            Assert.IsTrue(p1.Tasks.Count == 1 &&
                p1.Tasks.Count == p2.Tasks.Count);

            // Now add a 2nd task to the first instance of the project
            Task t2 = new Task(p1.Credential);
            t2.Name = "New task # 2";
            p1.Tasks.Add(t2);
            p1.Tasks.Update();

            // Verify that first instance has 2 tasks and 2nd instance only 1
            Assert.IsTrue(p1.Tasks.Count == 2 &&
                p2.Tasks.Count == 1);

            // Now reload the 2nd instance and verify that both instances 
            // have 2 tasks
            p2.Tasks.Reload();

            Assert.IsTrue(p1.Tasks.Count == 2 &&
                p2.Tasks.Count == 2);

            // Cleanup
            t1.Delete();
            t2.Delete();
            p1.Delete();

        }

        [TestMethod]
        public void GetPersonByUserName()
        {
            Person fred = new Person("fflintstone", TestCredential);

            Assert.IsTrue(fred.Id > 0 && fred.UserName.ToLower() == "fflintstone");
        }

        [TestMethod]
        public void UpdateUserPassword()
        {
            Person p = new Person(5022, TestCredential);
            p.Password = "megabase";
            p.Update();
        }


        [TestMethod]
        public void AddPropertiesForChecklist()
        {
            Dictionary<int, string> tasks = new Dictionary<int, string>();
            tasks.Add(123, "abc");
            tasks.Add(345, "def");
            tasks.Add(678, "ghi");

            Credential credential = TestCredential;

            // Make sure project has an ID# by updating it
            Project project = new Project(TestCredential);
            project.Name = "Test Project for Teja";
            project.Update();

            // Iterate through items in dictionary, adding a task+property for each
            foreach (var item in tasks)
            {
                // Create a new task under this project to represent the SP item
                Task task = new Task(credential);
                task.Name = "New Kanban task for " + item.Value;

                // Add the new task to the project and save to database
                project.Tasks.Add(task);
                project.Tasks.Update();

                // Create a new property to point back to SP item
                Property QuestionID = new Property(credential);
                QuestionID.Name = "CheckListTaskID";
                QuestionID.Value = item.Value;

                // Add the property to the new Kanban task
                task.Properties.Add(QuestionID);

                // Update the task to save both its data and the associated properties
                task.Update();

                // Verify properties saved
                MyKanban.Task checkTask = new Task(task.Id, TestCredential);
                Assert.IsTrue(checkTask.Id == task.Id &&
                    checkTask.Properties.Count == 1 &&
                    task.Properties.Count == 1 &&
                    checkTask.Properties[0].Name == task.Properties[0].Name &&
                    checkTask.Properties[0].Value.ToString() == item.Value.ToString());
            }

        }

        [TestMethod]
        public void LoginUsingToken()
        {
            Credential fred = new Credential("fflintstone", "password");

            string token = fred.Token;

            Credential fred2 = new Credential(token);

            Assert.IsTrue(fred.Id == fred2.Id &&
                fred.Name == fred2.Name);
        }

        [TestMethod]
        public void GetBoardPermissionsForCurrentUser()
        {
            Boards boards = new Boards("Fenwick Labs", TestCredential);
            Board board = new Board(boards[0].Id, TestCredential);

            // Find which of TestCredential's boards is "Fenwick Labs"
            int matchingIndex = -1;
            List<BoardPermissions> tcBoards = TestCredential.Boards;
            for (int i = 0; i < tcBoards.Count; i++)
            {
                if (tcBoards[i].Id == board.Id) matchingIndex = i;
            }

            Assert.IsTrue(tcBoards[matchingIndex].CanAdd == board.CanAdd &&
                tcBoards[matchingIndex].CanDelete == board.CanDelete &&
                tcBoards[matchingIndex].CanEdit == board.CanEdit &&
                tcBoards[matchingIndex].CanRead == board.CanRead);

        }

    #endregion

    }
}

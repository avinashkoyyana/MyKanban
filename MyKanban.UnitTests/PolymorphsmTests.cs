using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyKanban;

namespace MyKanbanUnitTests
{
    [TestClass]
    public class PolymorphsmTests
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
        public void AddTaskToCollection()
        {
            BaseItem project = new Project(TestCredential);
            project.Name = "New project";
            project.Update();

            BaseList tasks = new Tasks(TestCredential); ;

            for (int i = 0; i < 10; i++)
            {
                MyKanban.Task task = new MyKanban.Task(TestCredential);
                task.Name = "Task #" + i.ToString();
                task.Parent = (Project)project;
                task.ProjectId = project.Id;
                task.Update();
                tasks.Add(task);
                tasks.Update(true);
            }

            //Project project2 = new Project(project.Id, TestCredential);

            project.Reload();

            Assert.IsTrue(((Project)project).Tasks.Count == 10 && tasks.Count == 10);
            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(((Project)project).Tasks[i].Id == tasks[i].Id);
            }

            // Cleanup
            project.Delete();

        }
    }
}

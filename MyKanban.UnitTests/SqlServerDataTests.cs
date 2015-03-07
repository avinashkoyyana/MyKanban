using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyKanban;

namespace MyKanbanUnitTests
{
    [TestClass]
    public class SqlServerDataTests
    {

        Credential TestCredential
        {
            get
            {
                MyKanban.Data.DatabaseType = Data.DbType.SqlServer;
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
    }
}

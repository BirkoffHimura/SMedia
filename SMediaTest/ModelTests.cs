using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountContext;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.Data.SqlClient;
using BLL;
using System.Linq;

namespace SMediaTest
{
    [TestClass]
    public class ModelTests
    {
        TestTools tt;
        public ModelTests()
        {
            tt = new TestTools();
        }
        [TestInitialize()]
        public void ModelTestsInitialize()
        {
            tt.CleanUpDb("Constructor");
            tt.SetupInitialUserAccounts();
        }
        [TestCleanup()]
        public void ModelTestsDestructor()
        {
            tt.CleanUpDb("");
        }
        [ClassInitialize()]
        public static void ClassTestInitialize(TestContext testContext)
        {
                
        }
        [ClassCleanup()]
        public static void ClassTestCleanup()
        {
            TestTools.KillSessions();
            TestTools.DropTestDatabase();
        }

        

        [TestMethod]
        public void TestInsertUser()
        {
            User user = tt.InsertUser("Test User", "test@user.com", "pass123", "my test bio");
            using (SMediaContext sm = new AccountContext.SMediaContext())
            {
                sm.Users.Add(user);
                sm.SaveChanges();

                Assert.IsNotNull(user.ID);
                User retVal = sm.Users.Find(user.ID);
                Assert.AreEqual(retVal.UserName, user.UserName);
            }
        }

        [TestMethod]
        public void TestInsertUserFullInfo()
        {
            User user = tt.InsertUser("Test User", "test@user.com", "pass123", DateTime.Now, DateTime.Now, "my test bio", "3214 Shady Lane", "Suite 32", "Japan", "Tokio", "ST", "12345");

            using (SMediaContext sm = new AccountContext.SMediaContext())
            {
                sm.Users.Add(user);
                sm.SaveChanges();

                Assert.IsNotNull(user.ID);
                User retVal = sm.Users.Find(user.ID);
                Assert.AreEqual(retVal.UserName, user.UserName);
            }
        }

        [DataTestMethod]
        [DataRow("Test User", "testuser.com", "pass123", "test text for bio")]
        [DataRow("Test User", "testuser", "pass123", "test text for bio")]
        [DataRow("Test User", "testuser@", "pass123", "test text for bio")]
        [DataRow("Test User", "testuser@.com", "pass123", "test text for bio")]
        public void TestInvalidEmail(string theName, string uName, string pass, string theBio)
        {
            User user = tt.InsertUser(theName, uName, pass, theBio);

            using (SMediaContext sm = new AccountContext.SMediaContext())
            {
                try
                {
                    sm.Users.Add(user);
                    sm.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    Console.Out.WriteLine(ex.Message);
                    Assert.IsTrue(ex.Message.ToUpper().Contains("VALIDATION FAILED"));
                    return;
                }
            }
            Assert.Fail();
        }
        [TestMethod]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestValidateStateMoreThan2Chars()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                User user = sm.Users.Find(tt.userOne.ID);
                user.State = "ZZZ";
                sm.SaveChanges();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestValidateZipCodeMoreThan5Chars()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                User user = sm.Users.Find(tt.userOne.ID);
                user.ZipCode = "123456";
                sm.SaveChanges();
            }
        }

        [TestMethod]
        public void TestInsertPictureForUser()
        {
            UserPicture up = tt.InsertUserPicture("noimage.jpg", true, DateTime.Now);

            User user = tt.InsertUser("Test User", "test@user.com", "pass123", "my test bio");

            user.UserPictures.Add(up);

            using (SMediaContext sm = new AccountContext.SMediaContext())
            {
                sm.Users.Add(user);
                sm.SaveChanges();
                Assert.IsNotNull(user.ID);
                User retVal = sm.Users.Find(user.ID);
                Assert.AreEqual(retVal.UserName, user.UserName);
            }
        }

        [TestMethod]
        public void TestInsertPictureAndPostForUserWithPicture()
        {
            User user = tt.InsertUser("Test User", "test@user.com", "pass123", "my test bio");

            UserPicture usrP = tt.InsertUserPicture("noimage.jpg", true, DateTime.Now);
            UserPost up = tt.InsertUserPost("Test Subject", "Sample body text", "noimage.jpg", DateTime.Now, usrP);

            user.UserPictures.Add(usrP);
            user.UserPosts.Add(up);

            using (SMediaContext sm = new AccountContext.SMediaContext())
            {
                sm.Users.Add(user);
                sm.SaveChanges();
                Assert.IsNotNull(user.ID);
                User retVal = sm.Users.Find(user.ID);
                Assert.AreEqual(retVal.UserName, user.UserName);
            }
        }

        [TestMethod]
        public void TestPostComments()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                UserPostComment upc = tt.CreatePostComment("Hey this is a comment", DateTime.Now, sm.Users.Find(tt.userTwo.ID));
                User theUser = sm.Users.Find(tt.userOne.ID);
                UserPost userPost = tt.InsertUserPost("test subject", "test body", "noimage.jpg", DateTime.Now, null);
                userPost.UserPostComments.Add(upc);
                theUser.UserPosts.Add(userPost);
                sm.SaveChanges();

                UserPostComment actual = sm.UserPostComments.Find(upc.PostCommentID);
                Assert.IsNotNull(actual);
                Assert.AreEqual(upc.Comment_Body, actual.Comment_Body);
            }
        } 

        [TestMethod]
        public void TestInsertMessageThread()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                MessageThread mt = new MessageThread();
                mt.UserCombination = "1:2";
                sm.MessageThreads.Add(mt);
                sm.SaveChanges();

                MessageThread actual = sm.MessageThreads.Find(mt.MessageThreadID);
                Assert.IsNotNull(actual);
                Assert.AreEqual(mt.UserCombination, actual.UserCombination);
            }
        }
        [TestMethod]
        public void TestInsertMessage()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                UserMessage um = new UserMessage();
                um.User = tt.userOne;
                um.MessageThreadID = tt.messageThreadOne.MessageThreadID;
                um.MessageBody = "Test message 1";
                sm.Entry(tt.userOne).State = System.Data.Entity.EntityState.Unchanged;
                sm.UserMessages.Add(um);
                sm.SaveChanges();

                UserMessage actual = sm.UserMessages.Find(um.UserMessageID);
                Assert.IsNotNull(actual);
                Assert.AreEqual(um.MessageBody, actual.MessageBody);
            }
        }

        [TestMethod]
        public void TestSendUserMessageWithNewMessageThread()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                string theMessageBody = "The test message body";
                long MessageID = sm.SendUserMessage(tt.userOne, tt.userThree, theMessageBody);

                UserMessage actual = sm.UserMessages.Find(MessageID);
                Assert.IsNotNull(actual);
                Assert.AreEqual(theMessageBody, actual.MessageBody);
            }
        }

        [TestMethod]
        public void TestSendUserMessageWithExistingMessageThreadOneAndTwo()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                string theMessageBody = "The test message body sent by user one for an existing message thread";
                long MessageID = sm.SendUserMessage(tt.userOne, tt.userTwo, theMessageBody);

                UserMessage actual = sm.UserMessages.Find(MessageID);
                Assert.IsNotNull(actual);
                Assert.AreEqual(theMessageBody, actual.MessageBody);
                Assert.AreEqual(tt.messageThreadOne.MessageThreadID, actual.MessageThreadID);
            }
        }

        

        [TestMethod]
        public void TestSendUserMessageWithExistingMessageThreadTwoAndOne()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                string theMessageBody = "The test message body sent by user two for an existing message thread";
                long MessageID = sm.SendUserMessage(tt.userTwo, tt.userOne, theMessageBody);

                UserMessage actual = sm.UserMessages.Find(MessageID);
                Assert.IsNotNull(actual);
                Assert.AreEqual(theMessageBody, actual.MessageBody);
                Assert.AreEqual(tt.messageThreadOne.MessageThreadID, actual.MessageThreadID);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void TestInsertMessageThreadsTableIncorrectHigherToLower()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                User FirstUser = tt.InsertUser("Lower", "lower@someemail.com", "12345", "");
                User SecondUser = tt.InsertUser("Higher", "Higher@someemail.com", "12345", "");
                sm.Users.Add(FirstUser);
                sm.Users.Add(SecondUser);

                sm.SaveChanges();

                if(FirstUser.ID < SecondUser.ID)
                {
                    MessageThread messageThread = new MessageThread();
                    messageThread.UserCombination = SecondUser.ID.ToString() + ":" + FirstUser.ID.ToString();
                    sm.MessageThreads.Add(messageThread);
                    sm.SaveChanges();
                }
                else
                {
                    MessageThread messageThread = new MessageThread();
                    messageThread.UserCombination = FirstUser.ID.ToString() + ":" + SecondUser.ID.ToString();
                    sm.MessageThreads.Add(messageThread);
                    sm.SaveChanges();
                }
                
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void TestInsertMessageThreadsTableIncorrectDuplicate()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                MessageThread messageThread = new MessageThread();
                messageThread.UserCombination = tt.userOne.ID.ToString() + ":" + tt.userTwo.ID.ToString();
                sm.MessageThreads.Add(messageThread);
                sm.SaveChanges();
            }
        }
        [TestMethod]
        public void TestGetUserByUserName()
        {
            using (SMediaContext sm = new SMediaContext())
            {

                User retUser = sm.GetUserByUserName(tt.userOne.UserName);

                
                Assert.IsNotNull(retUser);
                Assert.AreEqual(tt.userOne.UserName, retUser.UserName);
                Assert.AreEqual(tt.userOne.ID, retUser.ID);
            }
        }
        [TestMethod]
        public void TestCreatePost()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                string testSubject = "TestCreatePost";
                long idCreated = sm.CreatePost(testSubject, "test body content", null, null, tt.userOne.ID, DateTime.Now.ToString());
                UserPostBs upBs = new UserPostBs();
                UserPost retPost = upBs.GetByID(idCreated);

                Assert.IsNotNull(idCreated);
                Assert.AreEqual(testSubject, retPost.Subject);
            }
        }
        [TestMethod]
        public void TestGetUserPostsByUserID()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                string testSubject = "TestCreatePost";
                long idCreated = sm.CreatePost(testSubject, "test body content", null, null, tt.userOne.ID, DateTime.Now.ToString());
                UserPostBs upBs = new UserPostBs();
                var retPost = sm.GetUserPostsByUserID(tt.userOne.ID);

                var thePost = from pl in retPost
                                where pl.ID == idCreated
                                select pl;

                Assert.IsNotNull(thePost);
                Assert.AreEqual(testSubject, thePost.FirstOrDefault().Subject);
            }
        }
        [TestMethod]
        public void TestGetUserPostCommentByPostID()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                string Comment_body = "Test comment body";

                long id  = sm.CreateUserPostComment(Comment_body, DateTime.Now, tt.userOne.ID, tt.userOneFirstPost.ID);

                var upc = sm.GetUserPostCommentByPostID(tt.userOneFirstPost.ID);
                var comment = from com in upc
                              where com.PostCommentID == id
                              select com;
                Assert.IsNotNull(comment);
                Assert.AreEqual(Comment_body, comment.FirstOrDefault().Comment_Body);
            }
        }
        [TestMethod]
        public void TestCreateUserPostComment()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                string Comment_body = "Test comment body";
                long id = sm.CreateUserPostComment(Comment_body, DateTime.Now, tt.userOne.ID, tt.userOneFirstPost.ID);
                var comment = sm.UserPostComments.Find(id);
                Assert.IsNotNull(comment);
                Assert.AreEqual(Comment_body, comment.Comment_Body);
            }
        }

        [TestMethod]
        public void TestGetUserPicturesByUserID()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                Guid guid = Guid.NewGuid();

                string FileName = guid.ToString() + ".jpg";

                long id = sm.CreateUserPicture(FileName, FileName, false, tt.userOne.ID, DateTime.Now);

                var up = sm.GetUserPicturesByUserID(tt.userOne.ID);
                var pictures = from pic in up
                              where pic.PictureID == id
                              select pic;
                Assert.IsNotNull(pictures);
                Assert.AreEqual(FileName, pictures.FirstOrDefault().FileName);
            }
        }
        [TestMethod]
        public void TestCreateUserPicture()
        {
            using (SMediaContext sm = new SMediaContext())
            {
                Guid guid = Guid.NewGuid();

                string FileName = guid.ToString() + ".jpg";
                long id = sm.CreateUserPicture(FileName, FileName, false, tt.userOne.ID, DateTime.Now);
                var picture = sm.UserPictures.Find(id);
                Assert.IsNotNull(picture);
                Assert.AreEqual(FileName, picture.FileName);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AccountContext;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SMediaTest
{
    [TestClass]
    public class DALTests
    {
        TestTools tt;
        public DALTests()
        {
            tt = new TestTools();
        }
        [TestInitialize()]
        public void DALTestsInitialize()
        {
            tt.CleanUpDb("Constructor");
            tt.SetupInitialUserAccounts();
        }
        [TestCleanup()]
        public void DALTestsDestructor()
        {
            tt.CleanUpDb("Destructor");
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

        #region UserDB Tests
        [TestMethod]
        public void UserDbGetAll()
        {
            UserDb uDb = new UserDb();
            List<AccountContext.User> ret = (List<AccountContext.User>)uDb.GetAll();
            Assert.IsTrue(ret.Count > 0);
        }
        [TestMethod]
        public void UserDbGetByID()
        {
            AccountContext.User tUser;
            UserDb userDb = new UserDb();
            tUser = userDb.GetByID(tt.userOne.ID);
            Assert.AreEqual(tt.userOne.UserName, tUser.UserName);
        }
        [TestMethod]
        public void UserDbGetByUserName()
        {
            AccountContext.User tUser;
            UserDb userDb = new UserDb();
            tUser = userDb.GetByUserName(tt.userOne.UserName);
            Assert.AreEqual(tt.userOne.UserName, tUser.UserName);
            Assert.AreEqual(tt.userOne.ID, tUser.ID);
        }
        [TestMethod]
        public void UserDbInsert()
        {
            AccountContext.User tUser = new AccountContext.User();
            UserDb userDb = new UserDb();

            AccountContext.User cUser;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tUser.BirthDate = DateTime.Now;
            tUser.Name = "UserDbInsert test";
            tUser.Password = "123456789";
            tUser.SignupDate = DateTime.Now;
            tUser.UserName = "UserDbInsert@test.com";
            int i = userDb.Insert(tUser);

            cUser = userDb.GetAll().Where(x => x.UserName == tUser.UserName).FirstOrDefault();

            Assert.IsTrue(i > 0);
            Assert.IsNotNull(cUser);
            Assert.IsTrue(cUser.ID > 0);
        }
        [TestMethod]
        public void UserDbDelete()
        {
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            UserDb uDb = new UserDb();
            uDb.Delete(tt.userThree.ID);
            AccountContext.User tUser = new AccountContext.User();
            tUser = smc.Users.Find(tt.userThree.ID);
            Assert.IsNull(tUser);
        }

        [TestMethod]
        public void UserDbUdate()
        {
            AccountContext.User tUser = new AccountContext.User();
            UserDb userDb = new UserDb();

            AccountContext.User cUser;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tt.userTwo.Name = "UserDbUdate test";
            

            userDb.Update(tt.userTwo);

            cUser = userDb.GetAll().Where(x => x.UserName == tt.userTwo.UserName).FirstOrDefault();
            Assert.IsNotNull(cUser);
            Assert.IsTrue(cUser.ID > 0);
            Assert.AreEqual(tt.userTwo.Name, cUser.Name);
        }
        #endregion

        #region UserPostDB Tests
        [TestMethod]
        public void UserPostDbGetAll()
        {
            UserPostDb upDb = new UserPostDb();
            List<AccountContext.UserPost> ret = (List<AccountContext.UserPost>)upDb.GetAll();
            Assert.IsTrue(ret.Count > 0);
        }
        [TestMethod]
        public void UserPostDbGetAllWithUserData()
        {
            UserPostDb upDb = new UserPostDb();
            List<AccountContext.UserPost> ret = (List<AccountContext.UserPost>)upDb.GetAllWithUserData();
            Assert.IsTrue(ret.Count > 0);
            foreach(UserPost up in ret)
            {
                Assert.IsNotNull(up.User);
            }
        }
        [TestMethod]
        public void UserPostDbGetAllByUserID()
        {
            UserPostDb userPDb = new UserPostDb();
            var tUserPost = userPDb.GetAllByUserID(tt.userOne.ID);
            Assert.IsTrue(tUserPost.Count() > 0);
        }
        [TestMethod]
        public void UserPostDbGetAllByUserIDWithUserData()
        {
            UserPostDb userPDb = new UserPostDb();
            var tUserPost = userPDb.GetAllByUserIDWithUserData(tt.userOne.ID);
            Assert.IsTrue(tUserPost.Count() > 0);
            foreach (AccountContext.UserPost up in tUserPost)
            {
                Assert.IsNotNull(up.User);
            }
        }
        [TestMethod]
        public void UserPostDbGetByID()
        {
            AccountContext.UserPost tUserPost;
            UserPostDb userPDb = new UserPostDb();
            tUserPost = userPDb.GetByID(tt.userOneFirstPost.ID);
            Assert.AreEqual(tt.userOneFirstPost.Subject, tUserPost.Subject);
        }
        [TestMethod]
        public void UserPostDbInsert()
        {
            AccountContext.UserPost tUserP = new AccountContext.UserPost();
            UserPostDb userDb = new UserPostDb();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPost cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tUserP.PostDate = DateTime.Now;
            tUserP.Subject = guid.ToString();
            tUserP.Post_Body = "Test Body post";
            tUserP.User = tt.userOne;
            long i = userDb.Insert(tUserP);

            cUserP = userDb.GetAll().Where(x => x.Subject == tUserP.Subject).FirstOrDefault();

            Assert.IsTrue(i > 0);
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.ID > 0);
        }

        [TestMethod]
        public void UserPostDbDelete()
        {
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            UserPostDb upDb = new UserPostDb();
            upDb.Delete(tt.userTwoFirstPost.ID);
            AccountContext.UserPost tUserP;
            tUserP = upDb.GetByID(tt.userTwoFirstPost.ID);
            Assert.IsNull(tUserP);
        }

        [TestMethod]
        public void UserPostDbUdate()
        {
            AccountContext.UserPost tUserP = new AccountContext.UserPost();
            UserPostDb userPDb = new UserPostDb();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPost cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();

            tt.userOneFirstPost.Subject = guid.ToString();
            
            userPDb.Update(tt.userOneFirstPost);

            cUserP = userPDb.GetAll().Where(x => x.ID == tt.userOneFirstPost.ID).FirstOrDefault();
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.ID > 0);
            Assert.AreEqual(tt.userOneFirstPost.Subject, cUserP.Subject);
        }
        #endregion

        #region UserPostCommentDB Tests
        [TestMethod]
        public void UserPostCommentDbGetAll()
        {
            UserPostCommentDb upDb = new UserPostCommentDb();
            List<AccountContext.UserPostComment> ret = (List<AccountContext.UserPostComment>)upDb.GetAll();
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod]
        public void UserPostCommentDbGetByID()
        {
            AccountContext.UserPostComment tUserPostComment;
            UserPostCommentDb userPDb = new UserPostCommentDb();
            tUserPostComment = userPDb.GetByID(tt.userOneCommentOnFirstPost.PostCommentID);
            Assert.AreEqual(tt.userOneCommentOnFirstPost.Comment_Body, tUserPostComment.Comment_Body);
        }
        [TestMethod]
        public void UserPostCommentDbGetAllByPostID()
        {
            UserPostCommentDb userPDb = new UserPostCommentDb();
            var tUserPostComment = userPDb.GetAllByPostID(tt.userOneFirstPost.ID);
            Assert.IsTrue(tUserPostComment.Count() > 0);        
        }
        [TestMethod]
        public void UserPostCommentDbGetAllByUserIDWithUserData()
        {
            UserPostCommentDb userPDb = new UserPostCommentDb();
            var tUserPostComment = userPDb.GetAllByPostIDWithUserData(tt.userOneFirstPost.ID);
            Assert.IsTrue(tUserPostComment.Count() > 0);
            foreach (AccountContext.UserPostComment up in tUserPostComment)
            {
                Assert.IsNotNull(up.FromUser);
            }
        }
        [TestMethod]
        public void UserPostCommentDbInsert()
        {
            AccountContext.UserPostComment tUserPC = new AccountContext.UserPostComment();
            UserPostCommentDb userDb = new UserPostCommentDb();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPostComment cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tUserPC.CommentDate = DateTime.Now;
            tUserPC.Comment_Body = guid.ToString();
            tUserPC.UserPost = tt.userOneFirstPost;
            tUserPC.FromUser = tt.userOne;
            long i = userDb.Insert(tUserPC);

            cUserP = userDb.GetAll().Where(x => x.Comment_Body == tUserPC.Comment_Body).FirstOrDefault();

            Assert.IsTrue(i > 0);
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.PostCommentID > 0);
        }

        [TestMethod]
        public void UserPostCommentDbDelete()
        {
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            UserPostCommentDb upDb = new UserPostCommentDb();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPostComment tUserPC = new AccountContext.UserPostComment();
            tUserPC.CommentDate = DateTime.Now;
            tUserPC.Comment_Body = guid.ToString();
            tUserPC.UserPost = tt.userOneFirstPost;
            tUserPC.FromUser = tt.userOne;
            long i = upDb.Insert(tUserPC);

            
            upDb.Delete(i);
            AccountContext.UserPostComment tUserP;
            tUserP = upDb.GetByID(i);
            Assert.IsNull(tUserP);
        }

        [TestMethod]
        public void UserPostCommentDbUdate()
        {
            AccountContext.UserPostComment tUserP = new AccountContext.UserPostComment();
            UserPostCommentDb userPDb = new UserPostCommentDb();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPostComment cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();

            tt.userOneCommentOnFirstPost.Comment_Body = guid.ToString();

            userPDb.Update(tt.userOneCommentOnFirstPost);

            cUserP = userPDb.GetAll().Where(x => x.PostCommentID == tt.userOneCommentOnFirstPost.PostCommentID).FirstOrDefault();
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.PostCommentID > 0);
            Assert.AreEqual(tt.userOneCommentOnFirstPost.Comment_Body, cUserP.Comment_Body);
        }
        #endregion

        #region UserPictureDB Tests
        [TestMethod]
        public void UserPictureDbGetAll()
        {
            UserPictureDb upDb = new UserPictureDb();
            List<AccountContext.UserPicture> ret = (List<AccountContext.UserPicture>)upDb.GetAll();
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod]
        public void UserPictureDbGetByID()
        {
            AccountContext.UserPicture tUserPicture;
            UserPictureDb userPDb = new UserPictureDb();
            tUserPicture = userPDb.GetByID(tt.userOnePicture.PictureID);
            Assert.AreEqual(tt.userOnePicture.FileName, tUserPicture.FileName);
        }
        [TestMethod]
        public void UserPictureDbGetAllByUserID()
        {
            UserPictureDb userPDb = new UserPictureDb();
            var tUserPicture = userPDb.GetAllByUserID(tt.userOne.ID);
            Assert.IsTrue(tUserPicture.Count() > 0);
        }
        [TestMethod]
        public void UserPictureDbInsert()
        {
            AccountContext.UserPicture tUserPC = new AccountContext.UserPicture();
            UserPictureDb userDb = new UserPictureDb();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPicture cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tUserPC.UploadDate = DateTime.Now;
            tUserPC.FileName = guid.ToString();
            tUserPC.OriginalFileName = guid.ToString();
            tUserPC.ProfilePicture = true;
            tUserPC.User = tt.userOne;
            long i = userDb.Insert(tUserPC);

            cUserP = userDb.GetAll().Where(x => x.PictureID == i).FirstOrDefault();

            Assert.IsTrue(i > 0);
            Assert.IsNotNull(cUserP);
            Assert.AreEqual(tUserPC.FileName, cUserP.FileName);
        }

        [TestMethod]
        public void UserPictureDbDelete()
        {
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            UserPictureDb upDb = new UserPictureDb();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPicture tUserPC = new AccountContext.UserPicture();
            tUserPC.UploadDate = DateTime.Now;
            tUserPC.FileName = guid.ToString();
            tUserPC.OriginalFileName = guid.ToString();
            tUserPC.ProfilePicture = false;
            tUserPC.User = tt.userOne;
            long i = upDb.Insert(tUserPC);


            upDb.Delete(i);
            AccountContext.UserPicture tUserP;
            tUserP = upDb.GetByID(i);
            Assert.IsNull(tUserP);
        }

        [TestMethod]
        public void UserPictureDbUdate()
        {
            AccountContext.UserPicture tUserP = new AccountContext.UserPicture();
            UserPictureDb userPDb = new UserPictureDb();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPicture cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();

            tt.userOnePicture2.FileName = guid.ToString();

            userPDb.Update(tt.userOnePicture2);

            cUserP = userPDb.GetAll().Where(x => x.FileName == tt.userOnePicture2.FileName).FirstOrDefault();
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.PictureID > 0);
            Assert.AreEqual(tt.userOnePicture2.UploadDate, cUserP.UploadDate);
        }
        #endregion

    }
}

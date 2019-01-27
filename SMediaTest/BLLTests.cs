using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using AccountContext;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SMediaTest
{
    [TestClass]
    public class BLLTests
    {
        TestTools tt;
        public BLLTests()
        {
            tt = new TestTools();
        }
        [TestInitialize()]
        public void BLLTestsInitialize()
        {
            tt.CleanUpDb("Constructor");
            tt.SetupInitialUserAccounts();
        }
        [TestCleanup()]
        public void BLLTestsDestructor()
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
            try
            {
                TestTools tTools = new TestTools();
                tTools.CleanupTestFolder();
            }
            catch { }
        }

        #region MembershipProvider tests
        [TestMethod]
        public void MembershipProviderValidateUser()
        {
            SMediaMembershipProvider smp = new SMediaMembershipProvider();
            Assert.IsTrue(smp.ValidateUser(tt.userOne.UserName, tt.userOne.Password));
        } 
        #endregion

        #region UserBs Tests
        [TestMethod]
        public void UserBsGetAll()
        {
            UserBs uDb = new UserBs();
            List<AccountContext.User> ret = (List<AccountContext.User>)uDb.GetAll();
            Assert.IsTrue(ret.Count > 0);
        }
        [TestMethod]
        public void UserBsGetByID()
        {
            AccountContext.User tUser;
            UserBs UserBs = new UserBs();
            tUser = UserBs.GetByID(tt.userOne.ID);
            Assert.AreEqual(tt.userOne.UserName, tUser.UserName);
        }
        [TestMethod]
        public void UserBsGetByUserName()
        {
            AccountContext.User tUser;
            UserBs UserBs = new UserBs();
            tUser = UserBs.GetByUserName(tt.userOne.UserName);
            Assert.AreEqual(tt.userOne.UserName, tUser.UserName);
            Assert.AreEqual(tt.userOne.ID, tUser.ID);
        }
        [TestMethod]
        public void UserBsInsert()
        {
            AccountContext.User tUser = new AccountContext.User();
            UserBs UserBs = new UserBs();

            AccountContext.User cUser;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tUser.BirthDate = DateTime.Now;
            tUser.Name = "UserBsInsert test";
            tUser.Password = "123456789";
            tUser.SignupDate = DateTime.Now;
            tUser.UserName = "UserBsInsert@test.com";
            int i = UserBs.Insert(tUser);

            cUser = UserBs.GetAll().Where(x => x.UserName == tUser.UserName).FirstOrDefault();

            Assert.IsTrue(i > 0);
            Assert.IsNotNull(cUser);
            Assert.IsTrue(cUser.ID > 0);
        }
        [TestMethod]
        public void UserBsDelete()
        {
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            UserBs uDb = new UserBs();
            uDb.Delete(tt.userThree.ID);
            AccountContext.User tUser = new AccountContext.User();
            tUser = smc.Users.Find(tt.userThree.ID);
            Assert.IsNull(tUser);
        }

        [TestMethod]
        public void UserBsUdate()
        {
            AccountContext.User tUser = new AccountContext.User();
            UserBs UserBs = new UserBs();

            AccountContext.User cUser;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tt.userTwo.Name = "UserBsUdate test";


            UserBs.Update(tt.userTwo);

            cUser = UserBs.GetAll().Where(x => x.UserName == tt.userTwo.UserName).FirstOrDefault();
            Assert.IsNotNull(cUser);
            Assert.IsTrue(cUser.ID > 0);
            Assert.AreEqual(tt.userTwo.Name, cUser.Name);
        }
        #endregion

        #region UserPosts Tests
        [TestMethod]
        public void UserPostBsGetAll()
        {
            UserPostBs upb = new UserPostBs();
            List<AccountContext.UserPost> ret = (List<AccountContext.UserPost>)upb.GetAll();
            Assert.IsTrue(ret.Count > 0);
        }
        [TestMethod]
        public void UserPostBsGetAllWithUserData()
        {
            UserPostBs upb = new UserPostBs();
            List<AccountContext.UserPost> ret = (List<AccountContext.UserPost>)upb.GetAllWithUserData();
            Assert.IsTrue(ret.Count > 0);
            foreach(UserPost up in ret)
            {
                Assert.IsNotNull(up.User);
            }
        }

        [TestMethod]
        public void UserPostBsGetByID()
        {
            AccountContext.UserPost tUserPost;
            UserPostBs userPBs = new UserPostBs();
            tUserPost = userPBs.GetByID(tt.userOneFirstPost.ID);
            Assert.AreEqual(tt.userOneFirstPost.Subject, tUserPost.Subject);
        }
        [TestMethod]
        public void UserPostBsGetAllByUserID()
        {
            UserPostBs userPDb = new UserPostBs();
            var tUserPost = userPDb.GetAllByUserID(tt.userOne.ID);
            Assert.IsTrue(tUserPost.Count() > 0);
        }
        [TestMethod]
        public void UserPostBsGetAllByUserIDWithUserData()
        {
            UserPostBs userPDb = new UserPostBs();
            var tUserPost = userPDb.GetAllByUserIDWithUserData(tt.userOne.ID);
            Assert.IsTrue(tUserPost.Count() > 0);
            foreach(UserPost up in tUserPost)
            {
                Assert.IsNotNull(up.User);
            }
        }
        [TestMethod]
        public void UserPostBsInsert()
        {
            AccountContext.UserPost tUserP = new AccountContext.UserPost();
            UserPostBs userBs = new UserPostBs();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPost cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tUserP.PostDate = DateTime.Now;
            tUserP.Subject = guid.ToString();
            tUserP.Post_Body = "Test Body post";
            tUserP.User = tt.userOne;
            long i = userBs.Insert(tUserP);

            cUserP = userBs.GetAll().Where(x => x.Subject == tUserP.Subject).FirstOrDefault();

            Assert.IsTrue(i > 0);
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.ID > 0);
        }

        [TestMethod]
        public void UserPostBsDelete()
        {
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            UserPostBs upBs = new UserPostBs();
            upBs.Delete(tt.userTwoFirstPost.ID);
            AccountContext.UserPost tUserP;
            tUserP = upBs.GetByID(tt.userTwoFirstPost.ID);
            Assert.IsNull(tUserP);
        }

        [TestMethod]
        public void UserPostBsUdate()
        {
            AccountContext.UserPost tUserP = new AccountContext.UserPost();
            UserPostBs userPBs = new UserPostBs();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPost cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();

            tt.userOneFirstPost.Subject = guid.ToString();

            userPBs.Update(tt.userOneFirstPost);

            cUserP = userPBs.GetAll().Where(x => x.ID == tt.userOneFirstPost.ID).FirstOrDefault();
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.ID > 0);
            Assert.AreEqual(tt.userOneFirstPost.Subject, cUserP.Subject);
        }
        #endregion

        #region UserPostComments Tests
        [TestMethod]
        public void UserPostCommentBsGetAll()
        {
            UserPostCommentBs upb = new UserPostCommentBs();
            List<AccountContext.UserPostComment> ret = (List<AccountContext.UserPostComment>)upb.GetAll();
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod]
        public void UserPostCommentBsGetByID()
        {
            AccountContext.UserPostComment tUserPostComment;
            UserPostCommentBs userPBs = new UserPostCommentBs();
            tUserPostComment = userPBs.GetByID(tt.userOneCommentOnFirstPost.PostCommentID);
            Assert.AreEqual(tt.userOneCommentOnFirstPost.Comment_Body, tUserPostComment.Comment_Body);
        }
        [TestMethod]
        public void UserPostCommentBsGetAllByPostID()
        {
            UserPostCommentBs userPDb = new UserPostCommentBs();
            var tUserPostComment = userPDb.GetAllByPostID(tt.userOneFirstPost.ID);
            Assert.IsTrue(tUserPostComment.Count() > 0);
        }
        [TestMethod]
        public void UserPostCommentBsGetAllByPostIDWithUserData()
        {
            UserPostCommentBs userPDb = new UserPostCommentBs();
            var tUserPostComment = userPDb.GetAllByPostIDWithUserData(tt.userOneFirstPost.ID);
            Assert.IsTrue(tUserPostComment.Count() > 0);
            foreach(UserPostComment upc in tUserPostComment)
            {
                Assert.IsNotNull(upc.FromUser);
            }
        }
        [TestMethod]
        public void UserPostCommentBsInsert()
        {
            AccountContext.UserPostComment tUserPC = new AccountContext.UserPostComment();
            UserPostCommentBs userBs = new UserPostCommentBs();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPostComment cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tUserPC.CommentDate = DateTime.Now;
            tUserPC.Comment_Body = guid.ToString();
            tUserPC.UserPost = tt.userOneFirstPost;
            tUserPC.FromUser = tt.userOne;
            long i = userBs.Insert(tUserPC);

            cUserP = userBs.GetAll().Where(x => x.Comment_Body == tUserPC.Comment_Body).FirstOrDefault();

            Assert.IsTrue(i > 0);
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.PostCommentID > 0);
        }

        [TestMethod]
        public void UserPostCommentBsDelete()
        {
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            UserPostCommentBs upBs = new UserPostCommentBs();
            Guid guid = Guid.NewGuid();
            AccountContext.UserPostComment tUserPC = new UserPostComment();
            tUserPC.CommentDate = DateTime.Now;
            tUserPC.Comment_Body = guid.ToString();
            tUserPC.UserPost = tt.userOneFirstPost;
            tUserPC.FromUser = tt.userOne;
            long i = upBs.Insert(tUserPC);

            upBs.Delete(i);
            AccountContext.UserPostComment tUserP;
            tUserP = upBs.GetByID(i);
            Assert.IsNull(tUserP);
        }

        [TestMethod]
        public void UserPostCommentBsUdate()
        {
            AccountContext.UserPostComment tUserP = new AccountContext.UserPostComment();
            UserPostCommentBs userPBs = new UserPostCommentBs();
            Guid guid = Guid.NewGuid();

            AccountContext.UserPostComment cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();

            tt.userOneCommentOnFirstPost.Comment_Body = guid.ToString();

            userPBs.Update(tt.userOneCommentOnFirstPost);

            cUserP = userPBs.GetAll().Where(x => x.PostCommentID == tt.userOneCommentOnFirstPost.PostCommentID).FirstOrDefault();
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.PostCommentID > 0);
            Assert.AreEqual(tt.userOneCommentOnFirstPost.Comment_Body, cUserP.Comment_Body);
        }
        #endregion

        #region UserPictureBS Tests
        [TestMethod]
        public void UserPictureBsGetAll()
        {
            UserPictureBs upBs = new UserPictureBs();
            List<AccountContext.UserPicture> ret = (List<AccountContext.UserPicture>)upBs.GetAll();
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod]
        public void UserPictureBsGetAllBase64()
        {            
            UserPictureBs upBs = new UserPictureBs();
            List<AccountContext.UserPictureBase64> ret = (List<AccountContext.UserPictureBase64>)upBs.GetAllBase64(System.Configuration.ConfigurationManager.AppSettings["TestImageFolder"]);
            Assert.IsTrue(ret.Count > 0);
            foreach(UserPictureBase64 img in ret)
            {
                Assert.IsNotNull(img.Base64ImageString);
                Assert.AreEqual(TestTools.base64ImageTestData, img.Base64ImageString);
            }
        }

        [TestMethod]
        public void UserPictureBsGetByID()
        {
            AccountContext.UserPicture tUserPicture;
            UserPictureBs userPBs = new UserPictureBs();
            tUserPicture = userPBs.GetByID(tt.userOnePicture.PictureID);
            Assert.AreEqual(tt.userOnePicture.FileName, tUserPicture.FileName);
        }
        [TestMethod]
        public void UserPictureBsGetByIDBase64()
        {
            AccountContext.UserPictureBase64 tUserPicture;
            UserPictureBs userPBs = new UserPictureBs();
            tUserPicture = userPBs.GetByIDBase64(tt.userOnePicture.PictureID, System.Configuration.ConfigurationManager.AppSettings["TestImageFolder"]);

            Assert.AreEqual(tt.userOnePicture.FileName, tUserPicture.FileName);

            Assert.IsNotNull(tUserPicture.Base64ImageString);
            Assert.AreEqual(TestTools.base64ImageTestData, tUserPicture.Base64ImageString);            
        }
        [TestMethod]
        public void UserPictureBsGetAllByUserID()
        {
            UserPictureBs userPBs = new UserPictureBs();
            var tUserPicture = userPBs.GetAllByUserID(tt.userOne.ID);
            Assert.IsTrue(tUserPicture.Count() > 0);
        }
        [TestMethod]
        public void UserPictureBsGetAllByUserIDBase64()
        {
            UserPictureBs userPBs = new UserPictureBs();
            var tUserPicture = userPBs.GetAllByUserIDBase64(tt.userOne.ID, System.Configuration.ConfigurationManager.AppSettings["TestImageFolder"]);
            Assert.IsTrue(tUserPicture.Count() > 0);
            foreach (UserPictureBase64 img in tUserPicture)
            {
                Assert.IsNotNull(img.Base64ImageString);
                Assert.AreEqual(TestTools.base64ImageTestData, img.Base64ImageString);
            }
        }
        [TestMethod]
        public void UserPictureBsInsert()
        {
            AccountContext.UserPicture tUserPC = new AccountContext.UserPicture();
            UserPictureBs userBs = new UserPictureBs();

            AccountContext.UserPicture cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            tUserPC.UploadDate = DateTime.Now;
            tUserPC.FileName = tt.CreateTestImage();
            tUserPC.OriginalFileName = tUserPC.FileName;
            tUserPC.ProfilePicture = true;
            tUserPC.User = tt.userOne;
            long i = userBs.Insert(tUserPC);

            cUserP = userBs.GetAll().Where(x => x.PictureID == i).FirstOrDefault();

            Assert.IsTrue(i > 0);
            Assert.IsNotNull(cUserP);
            Assert.AreEqual(tUserPC.FileName, cUserP.FileName);
        }

        [TestMethod]
        public void UserPictureBsDelete()
        {
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();
            UserPictureBs upBs = new UserPictureBs();

            AccountContext.UserPicture tUserPC = new AccountContext.UserPicture();
            tUserPC.UploadDate = DateTime.Now;
            tUserPC.FileName = tt.CreateTestImage();
            tUserPC.OriginalFileName = tUserPC.FileName;
            tUserPC.ProfilePicture = false;
            tUserPC.User = tt.userOne;
            long i = upBs.Insert(tUserPC);


            upBs.Delete(i);
            AccountContext.UserPicture tUserP;
            tUserP = upBs.GetByID(i);
            Assert.IsNull(tUserP);
        }

        [TestMethod]
        public void UserPictureBsUdate()
        {
            AccountContext.UserPicture tUserP = new AccountContext.UserPicture();
            UserPictureBs userPBs = new UserPictureBs();

            AccountContext.UserPicture cUserP;
            AccountContext.SMediaContext smc = new AccountContext.SMediaContext();

            tt.userOnePicture2.FileName = tt.CreateTestImage();

            userPBs.Update(tt.userOnePicture2);

            cUserP = userPBs.GetAll().Where(x => x.FileName == tt.userOnePicture2.FileName).FirstOrDefault();
            Assert.IsNotNull(cUserP);
            Assert.IsTrue(cUserP.PictureID > 0);
            Assert.AreEqual(tt.userOnePicture2.UploadDate, cUserP.UploadDate);
        }
        #endregion
    }
}

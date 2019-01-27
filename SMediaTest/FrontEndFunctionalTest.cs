using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using TestServerRunner;
using BLL;
using AccountContext;
using OpenQA.Selenium.Support.UI;

namespace SMediaTest
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            try
            {
                if (timeoutInSeconds > 0)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                    return wait.Until(drv => drv.FindElement(by));
                }
            }
            catch { }
            return driver.FindElement(by);
        }
    }
    [TestClass]
    public class FrontEndFunctionalTest
    {
        private ChromeDriver _webDriver;
        
        private const string BaseAddress = "http://localhost";
        private const int Port = 9798;
        private const string WorkingPath = @"G:\TestRuns\";
        private static IISExpress _iISExpress;
        private TestTools tt;
        public FrontEndFunctionalTest()
        {
            tt = new TestTools();
            tt.SetupInitialUserAccounts();
        }
        private void Login()
        {
            _webDriver.Navigate().GoToUrl(BaseAddress + ":" + Port.ToString() + "/Security/Login");
            TestTools tt = new TestTools();
            tt.SetupInitialUserAccounts();

            //System.Threading.Thread.Sleep(3000);

            var UserName = _webDriver.FindElementById("UserName");
            UserName.SendKeys(tt.userOne.UserName);

            var Password = _webDriver.FindElementById("Password");
            Password.SendKeys(tt.userOne.Password);


            var btnLogin = _webDriver.FindElementById("btnLogin");
            btnLogin.Click();
            //System.Threading.Thread.Sleep(3000);            
        }

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            IISExpress.m_UseShellExecute = true;
            IISExpress.m_RedirectStandardOutput = false;

            _iISExpress = IISExpress.Start(System.IO.Directory.GetCurrentDirectory(), Port, true, WorkingPath, true, "SMedia", true, "SMedia");
        }
        [ClassCleanup]
        public static void ClassDestroy()
        {
            try
            {
                _iISExpress.Stop();
                _iISExpress = null;
                TestTools tTools = new TestTools();
                tTools.CleanupTestFolder();
            }
            catch { }
        }
        [TestInitialize]
        public void TestInit()
        {
            _webDriver = new ChromeDriver();
            
        }
        [TestCleanup]
        public void TestCleanup()
        {
            _webDriver.Quit();            
        }
        [TestMethod]
        public void TestAuthenticate_CheckTitle()
        {
            _webDriver.Navigate().GoToUrl(BaseAddress + ":" + Port.ToString() + "/Security/Login");
           
            string title = _webDriver.Title;
            Assert.IsTrue(title.Contains("SMedia Login"));
        }
        [TestMethod]
        public void TestRegister_CheckTitle()
        {
            _webDriver.Navigate().GoToUrl(BaseAddress + ":" + Port.ToString() + "/Security/Register");

            string title = _webDriver.Title;
            Assert.IsTrue(title.Contains("SMedia Register"));
        }

        [TestMethod]
        public void TestRegister_Register()
        {
            _webDriver.Navigate().GoToUrl(BaseAddress + ":" + Port.ToString() + "/Security/Register");
            
            var Name = _webDriver.FindElementById("Name");
            Name.SendKeys("Test Name");

            var UserName = _webDriver.FindElementById("UserName");
            UserName.SendKeys("TestName@someserver.com");

            var Password = _webDriver.FindElementById("Password");
            Password.SendKeys("123567890");

            var BirthDate = _webDriver.FindElementById("BirthDate");
            BirthDate.SendKeys("06/06/1980");

            var SmallBio = _webDriver.FindElementById("SmallBio");
            SmallBio.SendKeys("Small bio sample");

            var btnRegister = _webDriver.FindElementById("Register");
            btnRegister.Click();
            
            var errorMessage = _webDriver.FindElementById("myErrorMessage");
            Assert.IsTrue(errorMessage.Text.Contains("successful"));
        }
        [TestMethod]
        public void TestLogin_Login()
        {
            _webDriver.Navigate().GoToUrl(BaseAddress + ":" + Port.ToString() + "/Security/Login");
            
            //System.Threading.Thread.Sleep(3000);

            var UserName = _webDriver.FindElementById("UserName");
            UserName.SendKeys(tt.userOne.UserName);

            var Password = _webDriver.FindElementById("Password");
            Password.SendKeys(tt.userOne.Password);
                        
            var btnLogin = _webDriver.FindElementById("btnLogin");
            btnLogin.Click();

            //System.Threading.Thread.Sleep(3000);

            string title = _webDriver.Title;
            Assert.IsTrue(title.ToUpper().Contains("HOME PAGE"));
        }
        [TestMethod]
        public void TestCreatePost()
        {
            Login();
            Guid guid = Guid.NewGuid();
            string PostSubjectTest = guid.ToString();
            var Subject = _webDriver.FindElementById("Subject");
            Subject.SendKeys(PostSubjectTest);

            var Post_Body = _webDriver.FindElementById("Post_Body");
            Post_Body.SendKeys("Test Body for the post");

            var btnPost = _webDriver.FindElementById("btnPost");
            btnPost.Click();
             
            UserPostBs uPost = new UserPostBs();
            UserPost up = null;
            DateTime ExecuteTime = DateTime.Now;

            while (up == null)
            {
                up = uPost.GetAll().Where(x => x.Subject == PostSubjectTest).FirstOrDefault();
                if ((DateTime.Now - ExecuteTime) > TimeSpan.FromMilliseconds(2000)) //makes sure we only wait for 2 seconds
                {
                    Assert.IsTrue(false, "Operation GetAll() for Post timed out");
                    return;
                }
            }

            var thePost = _webDriver.FindElement(By.Id(up.ID.ToString()), 45);
            Assert.IsNotNull(up);
            Assert.AreEqual(PostSubjectTest, up.Subject);
            Assert.IsTrue(thePost.Text.Contains(up.Subject));
        }
        [TestMethod]
        public void TestCreatePostAndComment()
        {
            Login();
            Guid guid = Guid.NewGuid();
            string PostSubjectTest = guid.ToString();
            var Subject = _webDriver.FindElementById("Subject");
            Subject.SendKeys(PostSubjectTest);

            var Post_Body = _webDriver.FindElementById("Post_Body");
            Post_Body.SendKeys("Test Body for the post");

            var btnPost = _webDriver.FindElementById("btnPost");
            btnPost.Click();
             
            UserPostBs uPost = new UserPostBs();
            UserPost up = null;
            DateTime ExecuteTime = DateTime.Now;

            while (up == null)
            {
                up = uPost.GetAll().Where(x => x.Subject == PostSubjectTest).FirstOrDefault();
                if ((DateTime.Now - ExecuteTime) > TimeSpan.FromMilliseconds(2000)) //makes sure we only wait for 2 seconds
                {
                    Assert.IsTrue(false, "Operation GetAll() for Post timed out");
                    return;
                }
            }

            var thePost = _webDriver.FindElement(By.Id(up.ID.ToString()), 45);

            Assert.IsNotNull(up);
            // creates comment on newly crated post

            string commentData = Guid.NewGuid().ToString();
            var PostCommentBox = _webDriver.FindElement(By.Id("commentOn" + up.ID.ToString()), 45); 
            PostCommentBox.SendKeys(commentData);

            var btnCommenton = _webDriver.FindElement(By.Id("btnPostComment" + up.ID.ToString()), 45);
            btnCommenton.Click();

            UserPostCommentBs uPostCom = new UserPostCommentBs();
            UserPostComment upc = null;
            ExecuteTime = DateTime.Now;

            while (upc == null)
            {
                upc = uPostCom.GetAll().Where(x => x.Comment_Body == commentData).FirstOrDefault();
                if((DateTime.Now - ExecuteTime) > TimeSpan.FromMilliseconds(2000)) //makes sure we only wait for 2 seconds
                {
                    Assert.IsTrue(false, "Operation GetAll() for comment timed out");
                    return;
                }
            }

            Assert.IsNotNull(upc);

            var theComment = _webDriver.FindElement(By.Id("CoNo" + upc.PostCommentID.ToString()), 45);            

            Assert.AreEqual(PostSubjectTest, up.Subject);
            Assert.IsTrue(thePost.Text.Contains(up.Subject));
            Assert.IsNotNull(theComment);
        }
        [TestMethod]
        public void TestUploadPicture()
        {
            Login();
            _webDriver.Navigate().GoToUrl(BaseAddress + ":" + Port.ToString() + "/User/UserPicture/UserPictureList");
            string img = tt.CreateTestImage();
            string imgPath = System.Configuration.ConfigurationManager.AppSettings["TestImageFolder"];
            var imgUploadBox = _webDriver.FindElementById("imgUpload");
            imgUploadBox.SendKeys(System.IO.Path.Combine(imgPath, img));
            var btnUpload = _webDriver.FindElementById("btnUpload");
            btnUpload.Click();

            UserPictureBs uPicture = new UserPictureBs();
            UserPicture up = null;
            DateTime ExecuteTime = DateTime.Now;

            while (up == null)
            {
                up = uPicture.GetAll().Where(x => x.OriginalFileName == img).FirstOrDefault();
                if ((DateTime.Now - ExecuteTime) > TimeSpan.FromMilliseconds(30000)) //makes sure we only wait for 30 seconds
                {
                    Assert.IsTrue(false, "Operation GetAll() for UserPicture timed out");
                    return;
                }
            }
            var uploadedImg = _webDriver.FindElement(By.Id("img" + up.PictureID), 45);
            Assert.IsNotNull(uploadedImg);
            Assert.IsNotNull(up);
        }
    }
}

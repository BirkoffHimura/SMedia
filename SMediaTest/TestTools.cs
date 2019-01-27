using AccountContext;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMediaTest
{
    public class TestTools
    {
        public User userOne;
        public User userTwo;
        public User userThree;
        public MessageThread messageThreadOne;
        public UserPost userOneFirstPost;
        public UserPost userTwoFirstPost;
        public UserPostComment userOneCommentOnFirstPost;
        public UserPostComment userOneCommentOnFirstPost2;
        public UserPicture userOnePicture;
        public UserPicture userOnePicture2;
        public const string base64ImageTestData = "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAIAAAAlC+aJAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwwAADsMBx2+oZAAAAdJJREFUaEPtl9GtgzAMRTOXB8o8mYZlGCa95OZZJrQPVKGaSD4/TYyhPiQtJtXJCQFvQsCbEPAmBLwJAW9CwJsQ8OZcIKWc9mC+1DVJ6fPGWgTJeUF+py6YvT+9Vhy5h6srUNYqSepaOF1xphSUyClBTkJOC6LAIhggsR+yp9/IzQKfqpxDgEHBDjkU+lCBvqn32zrlBZFBY44VsEhZrcN8AkPCHAK46/wzBfgHnWAF+AMgKN0GH/cbeCwh4E0IeBMC3oSAN5cE0GAuecvUZ+3QRPBFDJ10S+mwLT32S3gqtyd1hwlvH+3M/59vBES2rn+oNedsg+itJWXJOLD7CqqqFdJywnjhlY+2p3wjgAmKs41aToIUK4BCMUV3h0P6qoAPvDu8rfKnAvgaRHSJ0YS2wFaxCjCB+bZF/fRG/2sBLYVbBathBWzv2ervcSuAOPc6I7wyI+CT5xHknXMU0M2gdVsBW7SVsQJEI3rlfuAyOPmcowDG3DncKpiqAHP6nfyDOaqNMfEU4L+h3lEVwGB4m4GqpnHnaKGeAhwzDlRAk5Xhxg/PAS4Or9ZDDcZPuSTwZELAmxDwJgS8CQFvQsCbEPAmBLwJAW9CwJsQ8GZygVpfagSBBcEuQ38AAAAASUVORK5CYII=";

        public void SetupInitialUserAccounts()
        {
            using (SMediaContext sm = new AccountContext.SMediaContext())
            {
                try
                {
                    if (!sm.Database.CreateIfNotExists())
                    {
                        Console.Out.WriteLine("Could not create Database...");
                    }
                    #region Users
                    userOne = InsertUser("The first User", "one@email.com", "12345", "");
                    userTwo = InsertUser("The second User", "two@email.com", "12345", "");
                    userThree = InsertUser("The third User", "three@email.com", "12345", "");

                    sm.Users.Add(userOne);
                    sm.Users.Add(userTwo);
                    sm.Users.Add(userThree);

                    sm.SaveChanges(); 
                    #endregion

                    #region MessageThread
                    //creates a message thread between the previously created users
                    messageThreadOne = new MessageThread();
                    messageThreadOne.UserCombination = userOne.ID.ToString() + ":" + userTwo.ID.ToString();
                    sm.MessageThreads.Add(messageThreadOne);
                    sm.SaveChanges(); 
                    #endregion

                    //creates first post
                    #region UserPosts
                    userOneFirstPost = new UserPost();
                    userOneFirstPost.PostDate = DateTime.Now;
                    userOneFirstPost.Subject = Guid.NewGuid().ToString();
                    userOneFirstPost.Post_Body = Guid.NewGuid().ToString();
                    userOneFirstPost.User = userOne;
                    sm.UserPosts.Add(userOneFirstPost);
                    sm.SaveChanges();

                    userTwoFirstPost = new UserPost();
                    userTwoFirstPost.PostDate = DateTime.Now;
                    userTwoFirstPost.Subject = Guid.NewGuid().ToString();
                    userTwoFirstPost.Post_Body = Guid.NewGuid().ToString();
                    userTwoFirstPost.User = userTwo;
                    sm.UserPosts.Add(userTwoFirstPost);
                    sm.SaveChanges(); 
                    #endregion

                    #region PostComments
                    userOneCommentOnFirstPost = new UserPostComment();
                    userOneCommentOnFirstPost.CommentDate = DateTime.Now;
                    userOneCommentOnFirstPost.Comment_Body = Guid.NewGuid().ToString();
                    userOneCommentOnFirstPost.FromUser = userOne;
                    userOneCommentOnFirstPost.UserPost = userOneFirstPost;
                    sm.UserPostComments.Add(userOneCommentOnFirstPost);
                    sm.SaveChanges();

                    userOneCommentOnFirstPost2 = new UserPostComment();
                    userOneCommentOnFirstPost2.CommentDate = DateTime.Now;
                    userOneCommentOnFirstPost2.Comment_Body = Guid.NewGuid().ToString();
                    userOneCommentOnFirstPost2.FromUser = userOne;
                    userOneCommentOnFirstPost2.UserPost = userOneFirstPost;
                    sm.UserPostComments.Add(userOneCommentOnFirstPost2);
                    sm.SaveChanges(); 
                    #endregion

                    #region UserPictures
                    Guid guid = Guid.NewGuid();
                    userOnePicture = new UserPicture();
                    userOnePicture.FileName = CreateTestImage();
                    userOnePicture.OriginalFileName = userOnePicture.FileName;
                    userOnePicture.ProfilePicture = false;
                    userOnePicture.UploadDate = DateTime.Now;
                    userOnePicture.User = userOne;
                    sm.UserPictures.Add(userOnePicture);
                    sm.SaveChanges();


                    userOnePicture2 = new UserPicture();
                    userOnePicture2.FileName = CreateTestImage();
                    userOnePicture2.OriginalFileName = userOnePicture2.FileName;
                    userOnePicture2.ProfilePicture = false;
                    userOnePicture2.UploadDate = DateTime.Now;
                    userOnePicture2.User = userOne;
                    sm.UserPictures.Add(userOnePicture2);
                    sm.SaveChanges(); 
                    #endregion
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("Creating initial records...: " + ex.Message);
                }
            }
        }
        public static void KillSessions()
        {
            using (SqlConnection con = new SqlConnection("Server=localhost;Database=master;uid=sa;pwd=biowep0909;"))
            {
                string dropDBText = "USE [master]; DECLARE @kill varchar(8000) = '';  SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'   FROM sys.dm_exec_sessions WHERE database_id  = db_id('SMediaTest'); EXEC(@kill);";
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = dropDBText;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public static void DropTestDatabase()
        {
            #region CommentedOut DropDB
            try
            {
                using (SqlConnection con = new SqlConnection("Server=localhost;Database=master;uid=sa;pwd=biowep0909;"))
                {
                    string dropDBText = "DROP DATABASE SMediaTest";
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = dropDBText;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //currentTest.WriteLine("DB Has been deleted");
                }
            }
            catch (Exception ex)
            {
                //currentTest.WriteLine("DROP DB...: " + ex.Message);
            }
            #endregion
        }
        /// <summary>
        /// Cleans up the database, this one is run in the beginning 
        /// and the end of the tests
        /// </summary>
        public void CleanUpDb(string ConsDes)
        {
            try
            {                
                using (SMediaContext sm = new SMediaContext())
                {
                    Console.Out.WriteLine("Executing clean up from [" + ConsDes + "]...");

                    var listOfTables = new List<string> { "UserMessages", "MessageThreads", "UserPostComments", "UserPosts", "UserPictures", "Users" };
                    #region CommentedOut
                    foreach (var tableName in listOfTables)
                    {
                        try
                        {
                            sm.Database.ExecuteSqlCommand("DELETE FROM [" + tableName + "]");
                            sm.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.Out.WriteLine("Delete records from Tables[" + tableName + "]...: " + ex.Message);
                        }
                    }
                    #endregion

                }
            }
            catch { }

        }
        public User InsertUser(string Name, string UserName, string Password, string SmallBio, string AddressLine1, string AddressLine2, string Country, string City, string State, string Zipcode)
        {
            User tmp = new User();
            tmp.Name = Name;
            tmp.UserName = UserName;
            tmp.Password = Password;
            tmp.SignupDate = DateTime.Now;
            tmp.BirthDate = DateTime.Now;
            tmp.AddressLine1 = AddressLine1;
            tmp.AddressLine2 = AddressLine2;
            tmp.Country = Country;
            tmp.City = City;
            tmp.State = State;
            tmp.ZipCode = Zipcode;
            return tmp;
        }
        public User InsertUser(string Name, string UserName, string Password, DateTime SignupDate, DateTime BirthDate, string SmallBio, string AddressLine1, string AddressLine2, string Country, string City, string State, string Zipcode)
        {
            User tmp = new User();
            tmp.Name = Name;
            tmp.UserName = UserName;
            tmp.Password = Password;
            tmp.SignupDate = SignupDate;
            tmp.BirthDate = BirthDate;
            tmp.AddressLine1 = AddressLine1;
            tmp.AddressLine2 = AddressLine2;
            tmp.Country = Country;
            tmp.City = City;
            tmp.State = State;
            tmp.ZipCode = Zipcode;
            return tmp;
        }
        public User InsertUser(string Name, string UserName, string Password, string SmallBio)
        {
            User tmp = new User();
            tmp.Name = Name;
            tmp.UserName = UserName;
            tmp.Password = Password;
            tmp.SignupDate = DateTime.Now;
            tmp.BirthDate = DateTime.Now;
            return tmp;
        }
        public User InsertUser(string Name, string UserName, string Password, DateTime SignupDate, DateTime BirthDate, string SmallBio)
        {
            User tmp = new User();
            tmp.Name = Name;
            tmp.UserName = UserName;
            tmp.Password = Password;
            tmp.SignupDate = SignupDate;
            tmp.BirthDate = BirthDate;
            return tmp;
        }

        public UserPostComment CreatePostComment(string Comment_Body, DateTime CommentDate, User FromUser)
        {
            UserPostComment tmp = new UserPostComment();
            tmp.Comment_Body = Comment_Body;
            tmp.CommentDate = CommentDate;
            tmp.FromUser = FromUser;
            return tmp;
        }

        public UserPicture InsertUserPicture(string FileName, bool ProfilePicture, DateTime UploadDate)
        {
            UserPicture tmp = new UserPicture();
            tmp.FileName = FileName;
            tmp.ProfilePicture = ProfilePicture;
            tmp.UploadDate = UploadDate;
            return tmp;
        }
        public UserPost InsertUserPost(string Subject, string Post_Body, string Img_Extern, DateTime PostDate, UserPicture usrP)
        {
            UserPost tmp = new UserPost();
            tmp.Subject = Subject;
            tmp.Post_Body = Post_Body;
            tmp.Img_Extern = Img_Extern;
            tmp.PostDate = PostDate;
            tmp.UserPicture = usrP;

            return tmp;
        }
        public string CreateTestImage()
        {
            
            string testFolderPath = System.Configuration.ConfigurationManager.AppSettings["TestImageFolder"];
            if (!Directory.Exists(testFolderPath))
            {
                Directory.CreateDirectory(testFolderPath);
            }
            string ImageName = Guid.NewGuid().ToString() + ".png";
            string fileName = Path.Combine(testFolderPath, ImageName);            
            var bytes = Convert.FromBase64String(base64ImageTestData);
            using (var imageFile = new FileStream(fileName, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            return ImageName;
        }
        public void CleanupTestFolder()
        {
            string testFolderToCleanup = System.Configuration.ConfigurationManager.AppSettings["TestFolder"];
            deleteFolderContent(testFolderToCleanup);
        }
        private void deleteFolderContent(string path)
        {
            string[] fileList = Directory.GetFiles(path);
            foreach (string file in fileList)
            {
                File.Delete(file);
            }
            string[] directories = Directory.GetDirectories(path);
            foreach(string directory in directories)
            {
                deleteFolderContent(directory);
                Directory.Delete(directory);
            }
        }
    }
}

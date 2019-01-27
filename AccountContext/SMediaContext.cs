using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContext
{
    public class SMediaContext: DbContext
    {
        public SMediaContext()
        {
            this.Configuration.ProxyCreationEnabled = false;
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPicture> UserPictures { get; set; }
        public virtual DbSet<UserPost> UserPosts { get; set; }
        public virtual DbSet<UserPostComment> UserPostComments { get; set; }
        public virtual DbSet<MessageThread> MessageThreads { get; set; }
        public virtual DbSet<UserMessage> UserMessages { get; set; }
        public long SendUserMessage(User Sender, User Receiver, string MessageText)
        {
            System.Data.SqlClient.SqlParameter MessageID = new System.Data.SqlClient.SqlParameter();
            MessageID.ParameterName = "@MessageID";
            MessageID.Direction = System.Data.ParameterDirection.Output;
            MessageID.SqlDbType = System.Data.SqlDbType.BigInt;
            //Database.ExecuteSqlCommand("Execute SendUserMessage @SenderID, @ReceiverID, @MessageText");
            System.Data.SqlClient.SqlParameter[] paramCollection = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SenderID", Sender.ID),
                new System.Data.SqlClient.SqlParameter("@ReceiverID", Receiver.ID),
                new System.Data.SqlClient.SqlParameter("@MessageText", MessageText),
                MessageID
            };
            var retVal = Database.ExecuteSqlCommand("Execute SendUserMessage @SenderID, @ReceiverID, @MessageText, @MessageID OUT", paramCollection);
            
            return (long)MessageID.Value;
        }
        public User GetUserByUserName(string UserName)
        {
            System.Data.SqlClient.SqlParameter[] paramCollection = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@UserName", UserName)
            };
            IList<User> retUser = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<User>("GetUserByUserName @UserName", paramCollection).ToList<User>();
            if (retUser == null)
            {
                return null;
            }
            else
            {
                return retUser.FirstOrDefault();
            }
        }
        public long CreatePost(string Subject, string Post_Body, object Img_Extern, object UserPicture_PictureID, long User_ID, string PostDate)
        {
            System.Data.SqlClient.SqlParameter ID = new System.Data.SqlClient.SqlParameter();
            ID.ParameterName = "@ID";
            ID.Direction = System.Data.ParameterDirection.Output;
            ID.SqlDbType = System.Data.SqlDbType.BigInt;

            System.Data.SqlClient.SqlParameter[] paramCollection = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@Subject", Subject),
                new System.Data.SqlClient.SqlParameter("@Post_Body", Post_Body),
                new System.Data.SqlClient.SqlParameter("@Img_Extern", Img_Extern == null ? DBNull.Value : Img_Extern),
                new System.Data.SqlClient.SqlParameter("@UserPicture_PictureID", UserPicture_PictureID == null ? DBNull.Value : UserPicture_PictureID),
                new System.Data.SqlClient.SqlParameter("@User_ID", User_ID),
                new System.Data.SqlClient.SqlParameter("@PostDate", PostDate),
                ID
            };
            var retVal = Database.ExecuteSqlCommand("Execute CreatePost @Subject, @Post_Body, @Img_Extern, @UserPicture_PictureID, @User_ID, @PostDate, @ID OUT", paramCollection);
            
            return (long)ID.Value;
        }
        public IEnumerable<UserPost> GetUserPostsByUserID(long User_ID)
        {
            System.Data.SqlClient.SqlParameter[] paramCollection = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@User_ID", User_ID)
            };
            IList<UserPost> retUser = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<UserPost>("GetUserPostsByUserID @User_ID", paramCollection).ToList<UserPost>();
            if (retUser == null)
            {
                return null;
            }
            else
            {
                return retUser;
            }
        }
        public IEnumerable<UserPostComment> GetUserPostCommentByPostID(long UserPost_ID)
        {
            System.Data.SqlClient.SqlParameter[] paramCollection = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@UserPost_ID", UserPost_ID)
            };
            IList<UserPostComment> retUser = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<UserPostComment>("GetUserPostCommentByPostID @UserPost_ID", paramCollection).ToList<UserPostComment>();
            if (retUser == null)
            {
                return null;
            }
            else
            {
                return retUser;
            }
        }
        public long CreateUserPostComment(string Comment_Body, DateTime CommentDate, long FromUser_ID, long UserPost_ID)
        {
            System.Data.SqlClient.SqlParameter PostCommentID = new System.Data.SqlClient.SqlParameter();
            PostCommentID.ParameterName = "@PostCommentID";
            PostCommentID.Direction = System.Data.ParameterDirection.Output;
            PostCommentID.SqlDbType = System.Data.SqlDbType.BigInt;

            System.Data.SqlClient.SqlParameter[] paramCollection = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@Comment_Body", Comment_Body),
                new System.Data.SqlClient.SqlParameter("@CommentDate", CommentDate),
                new System.Data.SqlClient.SqlParameter("@User_ID", FromUser_ID),
                new System.Data.SqlClient.SqlParameter("@PostDate", UserPost_ID),
                PostCommentID
            };
            var retVal = Database.ExecuteSqlCommand("Execute CreateUserPostComment @Comment_Body, @CommentDate, @User_ID, @PostDate, @PostCommentID OUT", paramCollection);

            return (long)PostCommentID.Value;
        }
        public long CreateUserPicture(string FileName, string OriginalFileName, bool ProfilePicture, long User_ID, DateTime UploadDate)
        {
            System.Data.SqlClient.SqlParameter PictureID = new System.Data.SqlClient.SqlParameter();
            PictureID.ParameterName = "@PictureID";
            PictureID.Direction = System.Data.ParameterDirection.Output;
            PictureID.SqlDbType = System.Data.SqlDbType.BigInt;

            System.Data.SqlClient.SqlParameter[] paramCollection = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@FileName", FileName),
                new System.Data.SqlClient.SqlParameter("@OriginalFileName", OriginalFileName),
                new System.Data.SqlClient.SqlParameter("@ProfilePicture", ProfilePicture),
                new System.Data.SqlClient.SqlParameter("@User_ID", User_ID),
                new System.Data.SqlClient.SqlParameter("@UploadDate", UploadDate),
                PictureID
            };
            var retVal = Database.ExecuteSqlCommand("Execute CreateUserPicture @FileName, @OriginalFileName, @ProfilePicture, @User_ID, @UploadDate, @PictureID OUT", paramCollection);

            return (long)PictureID.Value;
        }

        public IEnumerable<UserPicture> GetUserPicturesByUserID(long User_ID)
        {
            System.Data.SqlClient.SqlParameter[] paramCollection = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@User_ID", User_ID)
            };
            IList<UserPicture> retUser = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<UserPicture>("GetUserPicturesByUserID @User_ID", paramCollection).ToList<UserPicture>();
            if (retUser == null)
            {
                return null;
            }
            else
            {
                return retUser;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserMessage>()
                .HasRequired<User>(s => s.User)
                .WithMany(g => g.UserMessages)
                .HasForeignKey<long>(s => s.UserID);

            modelBuilder.Entity<UserMessage>()
                .HasRequired<MessageThread>(s => s.MessageThread)
                .WithMany(g => g.UserMessages)
                .HasForeignKey<long>(s => s.MessageThreadID);
            
        }
    }
}

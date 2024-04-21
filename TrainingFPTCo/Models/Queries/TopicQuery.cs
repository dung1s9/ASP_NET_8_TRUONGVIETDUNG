using Microsoft.Data.SqlClient;

namespace TrainingFPTCo.Models.Queries
{
    public class TopicQuery
    {
        public List<TopicDetail> GetAllDataCourses()
        {
            List<TopicDetail> courses = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = " SELECT  [t].* , [c].[Name] AS CourseName FROM [Topics] AS [t] INNER JOIN [Courses] AS [c] ON [t].[CourseId] = [c].[Id] WHERE [t].[DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TopicDetail detail = new TopicDetail();
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.Name = reader["Name"].ToString() ?? DBNull.Value.ToString();
                        detail.Description = reader["Description"].ToString();
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);
                        detail.ViewDocumentTopic = reader["Documents"].ToString();
                        detail.ViewAttachFileTopic = reader["AttachFile"].ToString();
                        detail.TypeDocument = reader["TypeDocument"].ToString();
                        detail.ViewPosterTopic = reader["PosterTopic"].ToString();
                        detail.ViewCourseName = reader["CourseName"].ToString();
                        detail.Status = reader["Status"].ToString() ?? DBNull.Value.ToString();
                        courses.Add(detail);
                    }
                    connection.Close();
                }
            }
            return courses;
        }

        public List<TopicDetail> GetAllDataTopics(string? keyword, string? filter)
        {
            string dataKeyword = "%" + keyword + "%";
            List<TopicDetail> topics = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT [to].*, [co].[Name] AS CourseName " +
                                  "FROM [Topics] AS [to] " +
                                  "INNER JOIN [Courses] AS [co] ON [to].[CourseId] = [co].[Id] " +
                                  "WHERE ([to].[Name] LIKE @keyword OR [co].[Name] LIKE @keyword) AND [to].[DeletedAt] IS NULL"
;

                if (!string.IsNullOrEmpty(filter))
                {
                    sqlQuery += " AND [to].[Status] = @status";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@keyword", dataKeyword ?? DBNull.Value.ToString());

                if (!string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("@status", filter);
                }

                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TopicDetail detail = new TopicDetail();
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.Name = reader["Name"].ToString() ?? DBNull.Value.ToString();
                        detail.Description = reader["Description"].ToString();
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);
                        detail.ViewDocumentTopic = reader["Documents"].ToString();
                        detail.ViewAttachFileTopic = reader["AttachFile"].ToString();
                        detail.TypeDocument = reader["TypeDocument"].ToString();
                        detail.ViewPosterTopic = reader["PosterTopic"].ToString();
                        detail.ViewCourseName = reader["CourseName"].ToString();
                        detail.Status = reader["Status"].ToString() ?? DBNull.Value.ToString();
                        topics.Add(detail);
                    }
                    connection.Close();
                }
            }
            return topics;
        }
        public int InsertTopic(
            string nameTopic,
            int courseId,
            string? description,
            string? typeDocument,
            string status,
            string documentTopic,
            string? attachFile,
            string posterTopic
        )
        {

            int IdCourse = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Topics] ([CourseId], [Name], [Description],[Documents], [AttachFile], [TypeDocument], [PosterTopic],[Status], [CreatedAt]) VALUES  (@CourseId, @Name, @Description,@Documents,@AttachFile,@TypeDocument,@PosterTopic,@Status, @CreatedAt); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@Name", nameTopic);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Documents", documentTopic);
                cmd.Parameters.AddWithValue("@AttachFile", attachFile ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@PosterTopic", posterTopic);
                cmd.Parameters.AddWithValue("@TypeDocument", typeDocument ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                IdCourse = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return IdCourse;
        }
        public TopicDetail GetDetailTopicById(int id)
        {
            TopicDetail detail = new TopicDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT * FROM [Topics] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.Name = reader["Name"].ToString();
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);
                        detail.Description = reader["Description"].ToString();
                        detail.TypeDocument = reader["TypeDocument"].ToString();
                        detail.Status = reader["Status"].ToString();
                        detail.ViewDocumentTopic = reader["Documents"].ToString();
                        detail.ViewAttachFileTopic = reader["AttachFile"].ToString();
                        detail.ViewPosterTopic = reader["PosterTopic"].ToString();
                    }
                }
                connection.Close();
            }
            return detail;
        }
        public bool DeleteTopicById(int id)
        {
            bool checkDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Topics] SET [DeletedAt] = @DeletedAt WHERE [Id] = @id";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@DeletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                checkDelete = true;
                connection.Close();
            }
            return checkDelete;
        }
        public bool UpdateTopicById(
            int id,
            string nameTopic,
            int courseId,
            string? description,
            string? typeDocument,
            string status,
            string documentTopic,
            string? attachFile,
            string posterTopic

            )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "UPDATE [Topics] SET [CourseId] = @CourseId, [Name] = @Name, [Description] = @Description, [Documents] = @Documents, [AttachFile] = @AttachFile, [TypeDocument] = @TypeDocument,[PosterTopic]=@PosterTopic, [Status] = @Status, [UpdatedAt] = @UpdatedAt WHERE [Id] = @Id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@Name", nameTopic);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Documents", documentTopic);
                cmd.Parameters.AddWithValue("@AttachFile", attachFile ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@PosterTopic", posterTopic);
                cmd.Parameters.AddWithValue("@TypeDocument", typeDocument ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                checkUpdate = true;
                connection.Close();
            }
            return checkUpdate;
        }



    }
}
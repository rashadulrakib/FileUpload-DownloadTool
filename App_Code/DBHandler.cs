using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Data.SqlClient;

/// <summary>
/// Summary description for DBHandler
/// </summary>
public class DBHandler
{
    #region Constructor

    public DBHandler()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #endregion

    #region Private Methods

    private SqlConnection GetDBConection()
    {
        try 
        {
            string dbConnString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlServerConnectionString"].ConnectionString;
            SqlConnection oSqlConn = new SqlConnection(dbConnString);
            return oSqlConn;
        }
        catch(Exception oEx)
        {
            throw oEx;
        }
    }

    #endregion

    #region Public Methods

    public bool SaveFileIntoDB(FileInfo oFileInfo)
    {
        try
        {
            SqlConnection oSqlConn = this.GetDBConection();
            oSqlConn.Open();

            string insertSql = string.Empty;
            insertSql = "if exists(select FilePath from FileInfo where FilePath=@FilePath) begin"
            + " update FileInfo set FileModifierID=@FileModifierID,FileModificationTime=@FileModificationTime"
            + " where FilePath=@FilePath end "
            + " else begin"
            + " insert into FileInfo(FileID,FilePath,FileCreatorID,FileCreationTime)"
            + " values(@FileID,@FilePath,@FileCreatorID,@FileCreationTime) end";

            using (SqlCommand oSqlCmd = new SqlCommand(insertSql, oSqlConn))
            {
                oSqlCmd.Parameters.Add(new SqlParameter("@FileID", oFileInfo.FileID));
                oSqlCmd.Parameters.Add(new SqlParameter("@FilePath", oFileInfo.FilePath));
                oSqlCmd.Parameters.Add(new SqlParameter("@FileCreatorID", oFileInfo.FileCreatorID));
                oSqlCmd.Parameters.Add(new SqlParameter("@FileModifierID", oFileInfo.FileModifierID));
                oSqlCmd.Parameters.Add(new SqlParameter("@FileCreationTime", oFileInfo.FileCreationTime));
                oSqlCmd.Parameters.Add(new SqlParameter("@FileModificationTime", oFileInfo.FileModificationTime));
             
                oSqlCmd.ExecuteNonQuery();
            }

            oSqlConn.Dispose();

            return true;
        }
        catch (Exception oEx)
        {
            throw oEx;
        }

        return false;
    }

    public DataTable LoadAllFiles(bool isAdministratorRole, MembershipUser oMemUser)
    {
        DataTable dtFiles = new DataTable();

        try
        {
            SqlConnection oSqlConn = this.GetDBConection();
            oSqlConn.Open();

            string selectSql = string.Empty;

            if (isAdministratorRole)
            {
                selectSql = "SELECT     Creator, Modifier, FileID, FilePath, FileCreatorID, FileCreationTime, FileDeletionTime, FileModifierID, FileModificationTime "
                            + " FROM         (SELECT     Creatortable.Creator, Modifiertable.Modifier, Creatortable.FileID, Creatortable.FilePath, Creatortable.FileCreatorID, Creatortable.FileCreationTime, "
                            + "         Creatortable.FileDeletionTime, Modifiertable.FileModifierID, Modifiertable.FileModificationTime "
                            + "     FROM          (SELECT     FileInfo.FileID, FileInfo.FilePath, FileInfo.FileCreatorID, FileInfo.FileCreationTime, FileInfo.FileDeletionTime, "
                            + "    aspnet_Users.UserName AS Creator FROM          FileInfo INNER JOIN "
                            + " aspnet_Users ON FileInfo.FileCreatorID = aspnet_Users.UserId) AS Creatortable LEFT OUTER JOIN "
                            + " (SELECT     FileInfo_1.FileID, FileInfo_1.FilePath, FileInfo_1.FileModifierID, FileInfo_1.FileModificationTime, aspnet_Users_1.UserName AS Modifier "
                            + "  FROM          FileInfo AS FileInfo_1 INNER JOIN  aspnet_Users AS aspnet_Users_1 ON FileInfo_1.FileModifierID = aspnet_Users_1.UserId) AS Modifiertable ON "
                            + "    Creatortable.FileID = Modifiertable.FileID) AS JoinTable";
            }
            else
            {
                selectSql = "SELECT     Creator, Modifier, FileID, FilePath, FileCreatorID, FileCreationTime, FileDeletionTime, FileModifierID, FileModificationTime "
                            + " FROM         (SELECT     Creatortable.Creator, Modifiertable.Modifier, Creatortable.FileID, Creatortable.FilePath, Creatortable.FileCreatorID, Creatortable.FileCreationTime, "
                            + "         Creatortable.FileDeletionTime, Modifiertable.FileModifierID, Modifiertable.FileModificationTime "
                            + "     FROM          (SELECT     FileInfo.FileID, FileInfo.FilePath, FileInfo.FileCreatorID, FileInfo.FileCreationTime, FileInfo.FileDeletionTime, "
                            + "    aspnet_Users.UserName AS Creator FROM          FileInfo INNER JOIN "
                            + " aspnet_Users ON FileInfo.FileCreatorID = aspnet_Users.UserId) AS Creatortable LEFT OUTER JOIN "
                            + " (SELECT     FileInfo_1.FileID, FileInfo_1.FilePath, FileInfo_1.FileModifierID, FileInfo_1.FileModificationTime, aspnet_Users_1.UserName AS Modifier "
                            + "  FROM          FileInfo AS FileInfo_1 INNER JOIN  aspnet_Users AS aspnet_Users_1 ON FileInfo_1.FileModifierID = aspnet_Users_1.UserId) AS Modifiertable ON "
                            + "    Creatortable.FileID = Modifiertable.FileID) AS JoinTable WHERE     (FileCreatorID = @FileCreatorID)";
            }
            
            using (SqlCommand oSqlCmd = new SqlCommand(selectSql, oSqlConn))
            {
                if (!isAdministratorRole)
                {
                    oSqlCmd.Parameters.Add(new SqlParameter("@FileCreatorID", oMemUser.ProviderUserKey));
                }
                
                SqlDataReader sqlRdr = oSqlCmd.ExecuteReader();
                dtFiles.Load(sqlRdr);
                sqlRdr.Close();
                sqlRdr.Dispose();
            }

            oSqlConn.Dispose();
        }
        catch (Exception oEx)
        {
            throw oEx;
        }

        return dtFiles;
    }

    public bool DeleteFiles(Guid[] arrFileId)
    {
        try
        {
            SqlConnection oSqlConn = this.GetDBConection();
            oSqlConn.Open();

            foreach (Guid fileID in arrFileId)
	        {
                string deleteSql = "delete from FileInfo where FileID=@FileID";

                using (SqlCommand oSqlCmd = new SqlCommand(deleteSql, oSqlConn))
                {
                    oSqlCmd.Parameters.Add(new SqlParameter("@FileID", fileID));
                    oSqlCmd.ExecuteNonQuery();
                }
	        }

            oSqlConn.Dispose();

            return true;
        }
        catch (Exception oEx)
        {
            throw oEx;
        }

        return false;
    }

    #endregion
}

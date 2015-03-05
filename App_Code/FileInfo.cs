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

/// <summary>
/// Summary description for FileInfo
/// </summary>
public class FileInfo
{
    #region Constructor

    public FileInfo()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #endregion

    #region Private members

    private Guid _fileID = Guid.Empty;
    private string _filePath = string.Empty;
    private Guid _fileCreatorID = Guid.Empty;
    private Guid _fileModifierID = Guid.Empty;
    private DateTime _fileCreationTime = DateTime.MinValue;
    private DateTime _fileModificationTime = DateTime.MinValue;
    private DateTime _fileDeletionTime = DateTime.MinValue;

    #endregion

    #region Public Members
    public Guid FileID { get { return _fileID; } set { _fileID = value; } }
    public string FilePath { get { return _filePath ;} set { _filePath=value ;} }
    public Guid FileCreatorID { get { return _fileCreatorID; } set { _fileCreatorID = value; } }
    public Guid FileModifierID { get { return _fileModifierID; } set { _fileModifierID = value; } }
    public DateTime FileCreationTime { get { return _fileCreationTime; } set { _fileCreationTime = value; } }
    public DateTime FileModificationTime { get { return _fileModificationTime; } set { _fileModificationTime = value; } }
    public DateTime FileDeletionTime { get { return _fileDeletionTime; } set { _fileDeletionTime = value; } }

    #endregion
}

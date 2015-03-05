using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class _Default : System.Web.UI.Page
{
    #region Private variables

    MembershipUser oMemUser = null;
    bool isAdministratorRole = false;

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        oMemUser = Membership.GetUser();

        if (oMemUser != null)
        {
            if (User.IsInRole("Administrator"))
            {
                isAdministratorRole = true;
            }
            else if (User.IsInRole("Member"))
            {
                isAdministratorRole = false;
            }
            
            if (!Page.IsPostBack)
            {
                if (isAdministratorRole)
                {
                    PrepareAdministratorMenues();
                }
                else if (!isAdministratorRole)
                {
                    PrepareMemberMenues();
                }
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    protected void treeMenu_SelectedNodeChanged(object sender, EventArgs e)
    {
        InitializeControls();
        
        if (treeMenu.SelectedValue == "File Administration")
        {
            LoadAllFiles();
            
            MultiViews.SetActiveView(ViewFileAdministration);
        }
        else if (treeMenu.SelectedValue == "Add User")
        {
            MultiViews.SetActiveView(ViewAddUser);
        }
        else if (treeMenu.SelectedValue == "User Administration")
        {
            MultiViews.SetActiveView(ViewUserAdministration);
        }
        else if (treeMenu.SelectedValue.Contains("Folder:"))
        {
            string folderName = treeMenu.SelectedValue.Substring(treeMenu.SelectedValue.IndexOf(":") + 1);

            lblfolderName.Text = "File location-> ./Files/" + folderName+"/";
            MultiViews.SetActiveView(ViewFolder);
        } 
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string folderName = treeMenu.SelectedValue.Substring(treeMenu.SelectedValue.IndexOf(":") + 1);
        string fileDirectory = AppDomain.CurrentDomain.BaseDirectory + "Files" + @"\"+ folderName+@"\";
        
        if (!System.IO.Directory.Exists(fileDirectory))
        {
            System.IO.Directory.CreateDirectory(fileDirectory);
        }

        FileUpControl.SaveAs(fileDirectory + FileUpControl.FileName);

        bool bIsFileInserted = new DBHandler().SaveFileIntoDB(new FileInfo() { FileID = Guid.NewGuid(),
            FilePath = fileDirectory + FileUpControl.FileName, FileCreatorID = new Guid(oMemUser.ProviderUserKey.ToString()),
            FileModifierID = new Guid(oMemUser.ProviderUserKey.ToString()),
            FileCreationTime = DateTime.Now, 
            FileModificationTime = DateTime.Now,
            });

        if (bIsFileInserted)
        {
            lblMessage.Text = "File upoaded succesfully";      
        }
    }

    protected void GridViewFile_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView dtRowView = e.Row.DataItem as DataRowView;

        if (dtRowView != null)
        {
            Label lblFileName = e.Row.FindControl("lblFileName") as Label;
            lblFileName.Text = dtRowView["FilePath"].ToString().Substring(dtRowView["FilePath"].ToString().LastIndexOf(@"\") + 1);

            Label lblCreator = e.Row.FindControl("lblCreator") as Label;
            lblCreator.Text = dtRowView["Creator"].ToString();

            Label lblModifier = e.Row.FindControl("lblModifier") as Label;
            lblModifier.Text = dtRowView["Modifier"] == null ? string.Empty : dtRowView["Modifier"].ToString();

            Label lblCreationTime = e.Row.FindControl("lblCreationTime") as Label;
            lblCreationTime.Text = dtRowView["FileCreationTime"].ToString();

            Label lblModifyTime = e.Row.FindControl("lblModifyTime") as Label;
            lblModifyTime.Text = dtRowView["FileModificationTime"] == null ? string.Empty : dtRowView["FileModificationTime"].ToString();
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btnDelete = e.Row.FindControl("btnDelete") as Button;
            btnDelete.Attributes.Add("onclick", "javascript:return " +
                 "confirm('Are you sure to delete this record?')");

            HyperLink hpLinkDownload = e.Row.FindControl("hpLinkDownload") as HyperLink;
            string physicalPath = dtRowView["FilePath"].ToString();
            string url = "~/"+ physicalPath.Substring(AppDomain.CurrentDomain.BaseDirectory.Length);
            hpLinkDownload.NavigateUrl = url;
           
        }
    }

    protected void GridViewFile_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Guid Key = new Guid(GridViewFile.DataKeys[e.RowIndex].Value.ToString());

        this.DeleteFiles(new Guid[] { Key });
        
        this.LoadAllFiles();
    }

    #endregion

    #region private Methods
    
    private void InitializeControls()
    {
        lblfolderName.Text = string.Empty;
        lblMessage.Text = string.Empty;
        MultiViews.ActiveViewIndex = -1;
    }

    private void PrepareAdministratorMenues()
    {
        TreeNode trNodeHome = new TreeNode("Home", "Home");
        treeMenu.Nodes.Add(trNodeHome);

        TreeNode trNodeFileManagement = new TreeNode("File Management", "File Management");
        trNodeHome.ChildNodes.Add(trNodeFileManagement);

        TreeNode trNodeFolders = new TreeNode("Folders", "Folders");
        trNodeFileManagement.ChildNodes.Add(trNodeFolders);

        #region Load Dynamic menues
        MembershipUserCollection oMemUserCollection = Membership.GetAllUsers();
        foreach (MembershipUser oMemUser in oMemUserCollection)
        {
            TreeNode trNodeFolderName = new TreeNode(oMemUser.UserName, "Folder:" + oMemUser.UserName, "Images/TreeMenues/FolderIcon.gif");
            trNodeFolders.ChildNodes.Add(trNodeFolderName);
        }

        #endregion
        TreeNode trNodeFileAdministration = new TreeNode("File Administration", "File Administration");
        trNodeFileManagement.ChildNodes.Add(trNodeFileAdministration);
        
        TreeNode trNodeUserManagement = new TreeNode("User Management", "User Management");
        trNodeHome.ChildNodes.Add(trNodeUserManagement);

        TreeNode trNodeAddUser = new TreeNode("Add User", "Add User");
        trNodeUserManagement.ChildNodes.Add(trNodeAddUser);

        TreeNode trNodeUserAdministration = new TreeNode("User Administration", "User Administration");
        trNodeUserManagement.ChildNodes.Add(trNodeUserAdministration);
    }

    private void PrepareMemberMenues()
    {
        TreeNode trNodeHome = new TreeNode("Home", "Home");
        treeMenu.Nodes.Add(trNodeHome);

        TreeNode trNodeFileManagement = new TreeNode("File Management", "File Management");
        trNodeHome.ChildNodes.Add(trNodeFileManagement);

        TreeNode trNodeFolders = new TreeNode("Folders", "Folders");
        trNodeFileManagement.ChildNodes.Add(trNodeFolders);

        TreeNode trNodeFolderName = new TreeNode(oMemUser.UserName, "Folder:" + oMemUser.UserName, "Images/TreeMenues/FolderIcon.gif");
        trNodeFolders.ChildNodes.Add(trNodeFolderName);
        
        TreeNode trNodeFileAdministration = new TreeNode("File Administration", "File Administration");
        trNodeFileManagement.ChildNodes.Add(trNodeFileAdministration);
    }

    private void LoadAllFiles()
    {
        GridViewFile.DataSource = new DBHandler().LoadAllFiles(isAdministratorRole,oMemUser);
        GridViewFile.DataBind();
    }

    private void DeleteFiles(Guid[] arrFileId)
    {
        new DBHandler().DeleteFiles(arrFileId);
    }

    #endregion
    
}

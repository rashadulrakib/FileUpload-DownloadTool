<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Upload-Download Tool</title>
</head>

<body>
    <form id="form1" runat="server">
    <table id="Header" style="width: 100%; height: 100px;">
        <tr>
            <td style="background-color: Silver;">
                <center>
                    <h1>
                        File Upload-Download Tool</h1>
                </center>
            </td>
            <td style="background-color: Silver; text-align: right;">
                <asp:LoginStatus ID="LoginStatus1" runat="server" />
            </td>
        </tr>
    </table>
    <table id="loginstatus" style="width: 100%; background-color: Silver">
        <tr>
            <td style="text-align: left; font-style: italic;">
                <asp:LoginView ID="LoginView1" runat="server">
                    <LoggedInTemplate>
                        <span>Logged in. Welcome</span>
                    </LoggedInTemplate>
                    <AnonymousTemplate>
                        <span>You are not logged in. Click the Login link to sign in</span>
                    </AnonymousTemplate>
                </asp:LoginView>
                <strong>
                    <asp:LoginName ID="LoginName1" runat="server" />
                </strong>
            </td>
        </tr>
    </table>
    <table id="Content" style="width: 100%; height: 420px">
        <tr>
            <td style="width: 150px; vertical-align: top; border-right-style: solid; border-right-width: 1px">
                <asp:TreeView ID="treeMenu" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="treeMenu_SelectedNodeChanged">
                    <ParentNodeStyle Font-Bold="False" />
                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                        VerticalPadding="0px" />
                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                        NodeSpacing="0px" VerticalPadding="0px" />
                </asp:TreeView>
            </td>
            <td style="vertical-align: top;">
                <center>
                    <span style="text-align: left">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Text=""></asp:Label></span>
                    <asp:MultiView ID="MultiViews" runat="server">
                        <asp:View ID="ViewFolder" runat="server">
                            <table id="folder">
                                <tr>
                                    <td style="text-align: left">
                                        <strong>
                                            <asp:Label ID="lblfolderName" runat="server" Text=""></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="FileUpControl" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 3px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewFileAdministration" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridViewFile" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellPadding="3" HorizontalAlign="Left" BackColor="White" BorderColor="#CCCCCC"
                                            BorderStyle="None" BorderWidth="1px" 
                                            onrowdatabound="GridViewFile_RowDataBound" 
                                            onrowdeleting="GridViewFile_RowDeleting" DataKeyNames="FileID">
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <RowStyle ForeColor="#000066" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="File Name">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFileName" runat="server" Text="" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" DataField="FilePath" HeaderText="Location" HeaderStyle-HorizontalAlign="Left"/>
                                                <asp:TemplateField HeaderText="Creator">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreator" runat="server" Text="" Width="100px"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Modifier">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModifier" runat="server" Text="" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Creation Time">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreationTime" runat="server" Text="" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Modify Time">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModifyTime" runat="server" Text="" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Download">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hpLinkDownload" runat="server" Text="Download"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnDelete" CommandName="Delete" runat="server" Text="Delete" Width="60px"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewAddUser" runat="server">
                            ViewAddUser
                        </asp:View>
                        <asp:View ID="ViewUserAdministration" runat="server">
                            ViewUserAdministration
                        </asp:View>
                    </asp:MultiView>
                </center>
            </td>
        </tr>
    </table>
    <table id="Footer" style="width: 100%;">
        <tr>
            <td style="background-color: Silver">
                <center>
                    <strong>Copyright@2009 Vintage IT Ltd. All rights reserved</strong>
                </center>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

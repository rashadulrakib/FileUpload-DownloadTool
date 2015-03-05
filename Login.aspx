<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Upload-Download Tool</title>
</head>
<body>
    <form id="form1" runat="server">
     <table id="Header" style="width: 100%; height: 100px">
        <tr>
            <td style="background-color: Silver">
                <center>
                    <h1>File Upload-Download Tool</h1>
                </center>
                
            </td>
        </tr>
    </table>
    <table id="Content" style="width: 100%; height: 450px">
        <tr>
            <td>
                <center>
                    <asp:Login ID="Login1" runat="server">
                    </asp:Login>
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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="QuizApp.Login" EnableSessionState="True" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Login Page
    </div>
        <table style="width: 100%; border-spacing: 0px;" class="loginTable"> 
            <tr>
                <td>
                    <img id="loginImage" src="Images/login_Icon.png" runat="server" alt="Login Logo" height="50" />
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; padding: 5px 5px 0px 5px;">
                    <table id="Login">
                        <tr>
                            <td class="auto-style1">Login: </td>
                            <td class="auto-style1">
                                <asp:TextBox ID="txtLogin" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Password: </td>
                            <td>
                                <input id="txtPassword" runat="server" type="password" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan ="2">
                                <asp:CheckBox ID="checkRememberMe" runat="server" Text="Remember me" Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Literal ID="txtStatus" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="QuizApp.AdminPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">

    <!-- Nav tabs -->
    <ul class="nav nav-tabs" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" data-toggle="tab" href="#addUser" role="tab">Add User</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#profile" role="tab">Manage Users</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#messages" role="tab">Messages</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#settings" role="tab">Settings</a>
        </li>
    </ul>

    <!-- Tab panes -->
    <div class="tab-content">
        <div class="tab-pane active" id="addUser" role="tabpanel">
            <div class="row">
                <fieldset class="col-xs-2 form-group">
                    <label for="userNameTextBox">Username</label>
                    <input type="text" runat="server" class="form-control" id="userNameTextBox" placeholder="UserName" />
                    <small id="userNameErrorLabel" class="text-danger"></small>
                </fieldset>
                <fieldset class="col-xs-2 form-group">
                    <label for="passwordTextBox">Password</label>
                    <input type="password" runat="server" class="form-control" id="passwordTextBox" placeholder="Password" />
                </fieldset>
                <fieldset class="col-xs-2 form-group">
                    <label for="confirmpasswordTextBox">Confirm Password</label>
                    <input type="password" runat="server" class="form-control" id="confirmpasswordTextBox" placeholder="Confirm Password" />
                    <small id="passwordMismatchLabel" class="text-danger"></small>
                </fieldset>
            </div>
            <div class="row">
                <fieldset class="col-xs-2 form-group">
                    <label for="userFirstNameTextBox">First Name</label>
                    <input type="text" runat="server" class="form-control" id="userFirstNameTextBox" placeholder="First Name" />
                    <small id="userFirstNameErrorLabel" class="text-danger"></small>
                </fieldset>
                <fieldset class="col-xs-2 form-group">
                    <label for="userLastNameTextBox">Last Name</label>
                    <input type="text" runat="server" class="form-control" id="userLastNameTextBox" placeholder="Last Name" />
                    <small id="userLastNameErrorLabel" class="text-danger"></small>
                </fieldset>
                <fieldset class="col-xs-2 form-group">
                    <label for="userAccessLevelOption">User Access Level</label>
                    <select class="form-control" id="userAccessLevelOption" runat="server">
                        <option value="10">User</option>
                        <option value="100">Admin</option>
                        <option value="255">Super Admin</option>
                    </select>
                </fieldset>
            </div>
            <div class="row">
                <div class="col-xs-4">
                    <asp:Button runat="server" CssClass="btn btn-primary" OnClick="AddNewUserButton_Click" ID="AddNewUserButton" Text="Submit" />
                </div>

            </div>
        </div>
        <div class="tab-pane" id="profile" role="tabpanel">Profile Tab</div>
        <div class="tab-pane" id="messages" role="tabpanel">...</div>
        <div class="tab-pane" id="settings" role="tabpanel">...</div>
    </div>
    <script src="scripts/jquery-2.2.3.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#<%=userNameTextBox.ClientID%>").focusout(function () {
                var userName = $("#<%=userNameTextBox.ClientID%>").val();
                if (userName == "") {
                    $("#userNameErrorLabel").text("Enter a User Name");
                }
                else {
                    $("#userNameErrorLabel").text("");
                }
            });

            $("#<%=userFirstNameTextBox.ClientID%>").focusout(function () {
                var firstName = $("#<%=userFirstNameTextBox.ClientID%>").val();
                if (firstName == "") {
                    $("#userFirstNameErrorLabel").text("Enter First Name");
                }
                else {
                    $("#userFirstNameErrorLabel").text("");
                }
            });

            $("#<%=userLastNameTextBox.ClientID%>").focusout(function () {
                var firstName = $("#<%=userLastNameTextBox.ClientID%>").val();
                if (firstName == "") {
                    $("#userLastNameErrorLabel").text("Enter Last Name");
                }
                else {
                    $("#userLastNameErrorLabel").text("");
                }
            });

        });

        var password = document.getElementById("<%=passwordTextBox.ClientID%>");
        var confirm_password = document.getElementById("<%=confirmpasswordTextBox.ClientID%>");
        function validatePassword() {
            if (password.value != confirm_password.value) {
                document.getElementById("passwordMismatchLabel").innerHTML = "Passwords Don't Match";
            }
            else {
                document.getElementById("passwordMismatchLabel").innerHTML = "";
            }
        }
        password.onchange = validatePassword;
        confirm_password.onkeyup = validatePassword;

    </script>
</asp:Content>

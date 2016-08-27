<%@ Page Title="Setup / Edit" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="QuizSetup.aspx.cs" Inherits="QuizApp.QuizSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <title>Setup / Edit </title>
    <script type="text/javascript" src="scripts/jquery-2.2.3.min.js"></script>
    <script type="text/javascript">
        //Validation without postback, setup function later once the system is working perfectly
        function btnAddQuestionToDB_ClientClick() {
            var txtQuestion = document.getElementById("<%= txtQuestion.ClientID %>").value;
            var k = 5;
            if (txtQuestion != "")
                return true;
            return true;
        }



    </script>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <div class="col-xs-3" id="quizListSideBar" style="overflow-y: scroll;">
        <asp:Label ID="testLabel" runat="server"></asp:Label>
        <div class="list-group" id="quizQuestionListBar" runat="server">
        </div>
    </div>
    <div class="col-xs-9" id="quizWindow">
        <div class="col-xs-12" id="quizEditWindowTopContainer">
            <asp:Button ID="btnAddNewQuestion" CssClass="btn btn-default" runat="server" OnClick="btnAddNewQuestion_Click" Text="Add New Question" />

        </div>
        <br />
        <div id="questionWindow">
            <div class="panel panel-default" id="quizEditWindowContainer" runat="server" style="display: none; margin: 20px 0px 0px 0px;">
                <div class="panel-heading">
                    <h3 class="panel-title">Add Question</h3>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <textarea id="txtQuestion" runat="server" style="height: 80px; width: 50%; margin: 5px 0px 0px 0px;" class="form-control" onkeydown="return (event.keyCode!=13);" />
                        </tr>
                        <tr>
                            <br />
                        </tr>
                        <div class="input-group" id="questionContainer" runat="server"></div>
                        <tr>
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <input type="checkbox" id="checkbox1" runat="server" aria-label="..." />
                                </span>
                                <input type="text" id="txtanswer1" runat="server" class="form-control" aria-label="..." />
                            </div>
                        </tr>
                        <tr></tr>
                        <tr>
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <input type="checkbox" id="checkbox2" runat="server" aria-label="..." />
                                </span>
                                <input type="text" id="txtanswer2" runat="server" class="form-control" aria-label="..." />
                            </div>
                        </tr>
                        <tr>
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <input type="checkbox" id="checkbox3" runat="server" aria-label="..." />
                                </span>
                                <input type="text" id="txtanswer3" runat="server" class="form-control" aria-label="..." />
                            </div>
                        </tr>
                        <tr>
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <input type="checkbox" id="checkbox4" runat="server" aria-label="..." />
                                </span>
                                <input type="text" id="txtanswer4" runat="server" class="form-control" aria-label="..." />
                            </div>
                        </tr>
                        <tr>
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <input type="checkbox" id="checkbox5" runat="server" aria-label="..." />
                                </span>
                                <input type="text" id="txtanswer5" runat="server" class="form-control" aria-label="..." />
                            </div>
                        </tr>
                        <tr>
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <input type="checkbox" id="checkbox6" runat="server" aria-label="..." />
                                </span>
                                <input type="text" id="txtanswer6" runat="server" class="form-control" aria-label="..." />
                            </div>
                        </tr>
                        <tr>
                            <br />
                        </tr>
                        <tr>
                            <asp:Button ID="btnAddQuestionToDB" CssClass="btn btn-default" runat="server" OnClick="btnAddQuestionToDB_Click" OnClientClick="return btnAddQuestionToDB_ClientClick();" Text="Add" />
                            <asp:Button ID="btnQuestionCancel" CssClass="btn btn-danger" runat="server" OnClick=" btnQuestionCancel_Click" Text="Cancel" />
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

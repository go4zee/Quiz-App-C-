<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="QuizApp.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function validateQuizName() {
            var quizNameTextBox = document.getElementById("<%= QuizNameTextBox.ClientID %>").value;
            if (quizNameTextBox)
                return true;

            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">

    <div class="col-xs-12">
        <button type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#myModal">
            Create New Quiz
        </button>

        <div class="list-group" id="QuizListDiv" runat="server">
        </div>
    </div>




    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Create New Quiz</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="QuizNameLabel">Name:</asp:Label>
                    <asp:TextBox ID="QuizNameTextBox" runat="server" placeholder="Enter Quiz Name"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-primary" OnClick="createNewQuiz" OnClientClick="return validateQuizName();" Text="Create Quiz"></asp:Button>
                </div>
            </div>
        </div>
    </div>




</asp:Content>


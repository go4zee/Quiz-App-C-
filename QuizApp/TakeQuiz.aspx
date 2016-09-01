<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TakeQuiz.aspx.cs" Inherits="QuizApp.TakeQuiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <div id="questionWindow">
        <div class="panel panel-default" id="quizEditWindowContainer" runat="server" style="display: block; margin: 20px 0px 0px 0px;">
            <div class="panel-heading">
                <h3 class="panel-title">Add Question</h3>
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <textarea id="txtQuestion" readonly="readonly" runat="server" style="height: 80px; width: 50%; margin: 5px 0px 0px 0px;" class="form-control" onkeydown="return (event.keyCode!=13);" />
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
                            <input type="text" id="txtanswer1" readonly="true" runat="server" class="form-control" aria-label="..." />
                        </div>
                    </tr>
                    <tr></tr>
                    <tr>
                        <div class="input-group">
                            <span class="input-group-addon">
                                <input type="checkbox" id="checkbox2" runat="server" aria-label="..." />
                            </span>
                            <input type="text" id="txtanswer2" readonly="true" runat="server" class="form-control" aria-label="..." />
                        </div>
                    </tr>
                    <tr>
                        <div class="input-group">
                            <span class="input-group-addon">
                                <input type="checkbox" id="checkbox3" runat="server" aria-label="..." />
                            </span>
                            <input type="text" id="txtanswer3" readonly="true" runat="server" class="form-control" aria-label="..." />
                        </div>
                    </tr>
                    <tr>
                        <div class="input-group">
                            <span class="input-group-addon">
                                <input type="checkbox" id="checkbox4" runat="server" aria-label="..." />
                            </span>
                            <input type="text" id="txtanswer4" readonly="true" runat="server" class="form-control" aria-label="..." />
                        </div>
                    </tr>
                    <tr>
                        <div class="input-group">
                            <span class="input-group-addon">
                                <input type="checkbox" id="checkbox5" runat="server" aria-label="..." />
                            </span>
                            <input type="text" id="txtanswer5" readonly="true" runat="server" class="form-control" aria-label="..." />
                        </div>
                    </tr>
                    <tr>
                        <div class="input-group">
                            <span class="input-group-addon">
                                <input type="checkbox" id="checkbox6" runat="server" aria-label="..." />
                            </span>
                            <input type="text" id="txtanswer6" readonly="true" runat="server" class="form-control" aria-label="..." />
                        </div>
                    </tr>
                    <tr>
                        <br />
                    </tr>
                    <tr>
                        <asp:Button ID="btnPrevious" OnClick="btnPrevious_Click" CssClass="btn btn-default" runat="server"  Text="Previous" />
                        <asp:Button ID="btnNext" OnClick="btnNext_Click" CssClass="btn btn-default" runat="server"  Text="Next" />
                        <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="btn btn-success" runat="server"  Text="Submit" />
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </div>
</asp:Content>

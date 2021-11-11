<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSNotes.aspx.cs" Inherits="VETRIS.Common.VRSNotes" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <title></title>
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />

      <link id="lnkCUSTOM" runat="server" href = "../css/custome-css-style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../scripts/jquery-1.11.3.min.js"></script>
</head>
<body class="nav-sm" style="background: transparent;">
    <form id="form1" runat="server">
        <div class="container">
            <div class="main_container singleTask_page" style="margin: 10px;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12 margin-bottom-10">
                        <div class="x_panel">
                            <div class="x_title">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12">
                                        <h4>
                                            <asp:Label ID="lblMsgHdr" runat="server" Font-Bold="true" Text=""></asp:Label>
                                             <asp:Label ID="lblHelp" runat="server" CssClass="HelpText"></asp:Label>
                                        </h4>
                                       
                                    </div>
                                    
                                    <div class="borderHdr pull-left"></div>
                                </div>

                            </div>
                            <div class="x_content">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Height="150px" Width="100%" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6 col-xs-12 margin-bottom-10 margin-top-10">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        
                                        <button type="button" class="btn btn-primary" id="btnOk" runat="server">OK</button>&nbsp;&nbsp;
                                        <button type="button" class="btn btn-danger" id="btnClose" runat="server">Close</button>
                                       
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnMaxChar" runat="server" value="0" />
    </form>
</body>
<script type="text/javascript">
    var objtxtNotes = document.getElementById('<%=txtNotes.ClientID %>');
    var objhdnMaxChar = document.getElementById('<%=hdnMaxChar.ClientID %>');
</script>
<script src="../scripts/custome-javascript.js"></script>
<script type="text/javascript">
    $(document).ready($(function () {
        parent.adjustGenlSmallFrameHeight();
    }))
    objtxtNotes.maxLength = objhdnMaxChar.value;
    objtxtNotes.value = parent.GsPopupText;
    function btnOk_OnClick() {
        parent.HideGeneralSmall(objtxtNotes.value);
    }
</script>
</html>


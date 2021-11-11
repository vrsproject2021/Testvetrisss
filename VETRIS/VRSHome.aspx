<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSHome.aspx.cs" Inherits="VETRIS.VRSHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .text-center {text-align:center;}
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
        <div class="text-center">
            <img id="imgBg" runat="server" alt ="" style="height:543px;width:1200px;"/>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var Divider = parent.objhdnDivider.value;
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    parent.adjustFrameHeight(); parent.HideLoad(); CheckError();
    function CheckError() {
        var arrErr = new Array();
        if (parent.Trim(objhdnError.value) != "") {
            arrErr = objhdnError.value.split(Divider);
            switch (arrErr[0]) {
                case "catch":
                case "false":
                    parent.PopupMessage(RootDirectory, strForm, "OnLoad()", arrErr[1], "true");
                    break;
            }
        }
        objhdnError.value = "";
        parent.GsRetStatus = "false";
    }
</script>
<script src="scripts/custome-javascript.js" type="text/javascript"></script>
</html>

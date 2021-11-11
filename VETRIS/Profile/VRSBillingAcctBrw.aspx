<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSBillingAcctBrw.aspx.cs" Inherits="VETRIS.Profile.VRSBillingAcctBrw" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        </div>
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var strForm = "VRSBillingAcctBrw";
    var URL = "Profile/VRSBillingAcctDlg.aspx";
    var UserID = parent.objhdnUserID.value;
    var UserRoleID = parent.objhdnUserRoleID.value;
    var MenuID = parent.objhdnMenuID.value;
    parent.HideLoad();

    objhdnID.value = parent.objhdnBillAcctID.value;
    parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
    parent.PopupLoad();
    parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&th=" + parent.GsTheme;

</script>
</html>

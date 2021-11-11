$(document).ready(function(){

    $("#chatIcon").click(function () {
        $(".chatBoxArea").animate({ "top": "70px" }, "slow");
    });

    $("#chatCloseBtn").click(function () {
        $(".chatBoxArea").animate({ "top": "-440px" }, "slow");
    });
    
});

//var UserRoleCode = objhdnUserRoleCode.value;

//if ((UserRoleCode == "IU") || (UserRoleCode == "AU")) {
if (objhdnENBLCHAT.value == "Y") {
    var acctName = "";
    if (objhdnInstName.value == "NA") acctName = objhdnBillAcctName.value;
    if (acctName == "") acctName = objhdnInstName.value;


    objiframeChat.src = objhdnCHATURL.value + "institute_name=" + acctName + "&client_name=" + objhdnUserName.value + "&contact_number=" + objhdnUserContNo.value + "&uid=" + objhdnUserID.value;
    document.getElementById("divChat").style.display="block";
}

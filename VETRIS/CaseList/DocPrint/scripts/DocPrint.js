function LoadDoc() {
    if (objhdnErr.value == "Y") {
        document.getElementById("divProcess").style.display = "none";
        document.getElementById("divError").style.display = "inline";
    }
    else {
        if (objhdnDirect.value == "N") {
            parent.GsFilePath = objhdnDocPath.value;
            parent.GsLaunchURL = objhdnServerDocPath.value;
            parent.GsFileType = objhdnFmt.value;
            if (objhdnFmt.value == "P") parent.objiframeRptView.src = objhdnServerDocPath.value;
            else if (objhdnFmt.value == "R") {
                document.getElementById("divProcess").style.display = "none";
                document.getElementById("divFile").style.display = "block";
                var dlURL = objhdnServerDocPath.value;
                document.getElementById("flLink").innerHTML = "<a href='" + dlURL + "' id='dlLink' class='txtLink' download='" + dlURL + "'>Click Here</a>";
            }
            else if (objhdnFmt.value == "B") {
                document.getElementById("divProcess").style.display = "none";
                document.getElementById("divConfirm").style.display = "block";
            }
        }
        else {
            document.getElementById("divProcess").style.display = "none";
            if (objhdnFmt.value == "P") {
                window.location = objhdnServerDocPath.value;
            }
            else if (objhdnFmt.value == "R") {
                document.getElementById("divFile").style.display = "block";
                var dlURL = objhdnServerDocPath.value;
                document.getElementById("flLink").innerHTML = "<a href='" + dlURL + "' id='dlLink' class='txtLink' download='" + dlURL + "'>Click Here</a>";
            }
            else if (objhdnFmt.value == "B") {
                document.getElementById("divConfirm").style.display = "block";
            }
        }
    }
}

function Format_OnClick(Type) {
    var strURL = objhdnReqURL.value;
    strURL = strURL.replace("FMT=B", "FMT=" + Type);
    window.location = strURL;
}
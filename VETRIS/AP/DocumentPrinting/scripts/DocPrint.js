function LoadDoc() {
    if (objhdnErr.value == "Y") {
        document.getElementById("divProcess").style.display = "none";
        document.getElementById("divError").style.display = "inline";
    }
    else {
        parent.GsFilePath = objhdnDocPath.value;
        parent.objiframeRptView.src = objhdnServerDocPath.value;
    }
}
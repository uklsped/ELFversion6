function findControl(parent, tagName, serverId) {
    var items = parent.getElementsByTagName(tagName);
    // walk the items looking for the right guy
    for (var i = 0; i < items.length; i++) {
        var ctl = items[i];
        if (ctl && ctl.id) {
            // check the end of the name.
            //
            var subId = ctl.id.substring(ctl.id.length - serverId.length);
            if (subId == serverId) {
                return ctl;
            }
        }
    }
    return null;
}



function loadTabPanel(sender, e) {

    var tabContainer = sender;

    if (tabContainer) {

        var updateControlId = "TabButton" + tabContainer.get_activeTabIndex();
        // get the active tab and find our button
        //
        var activeTab = tabContainer.get_activeTab();


        // check to see if we've already loaded
        //
        if (findControl(activeTab.get_element(), "div", "TabContent" + tabContainer.get_activeTabIndex())) return;


        var updateControl = findControl(activeTab.get_element(), "input", updateControlId);

        if (updateControl) {

            // fire the update

            updateControl.click();

        }

    }

}
function windowOpener(url, name, args) {
    if (typeof (popupWin) != "object") {
        popupWin = window.open(url, name, args);
    } else {
        if (!popupWin.closed) {
            popupWin.location.href = url;
        } else {
            popupWin = window.open(url, name, args);
        }
    }
    popupWin.focus();
}

<%@ Page Language="VB" AutoEventWireup="false" CodeFile="test repeatfault.aspx.vb" Inherits="test_repeatfault" %>

<%@ Register src="controls/DeviceRepeatFaultuc.ascx" tagname="DeviceRepeatFaultuc" tagprefix="uc1" %>

<%@ Register src="controls/ErrorTextuc.ascx" tagname="ErrorTextuc" tagprefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:PlaceHolder ID="PlaceHolderRepeatFault" runat="server"></asp:PlaceHolder>
        </div>
        
        
        
    </form>
</body>
</html>

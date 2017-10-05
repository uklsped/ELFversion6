<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Preclinusercontrol.ascx.vb" Inherits="Preclinusercontrol" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

      
    <%@ Register src="ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc1" %>
<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc2" %>
<%@ Register src="ConfirmPage.ascx" tagname="ConfirmPage" tagprefix="uc3" %>
<%@ Register src="Singlemachinefaultuc.ascx" tagname="Singlemachinefaultuc" tagprefix="uc4" %>
<%@ Register src="ViewCommentsuc.ascx" tagname="ViewCommentsuc" tagprefix="uc5" %>
<%@ Register src="DefectSave.ascx" tagname="DefectSave" tagprefix="uc6" %>
<%@ Register src="TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc7" %>
<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />
      
    <script type="text/javascript">
//    function pageLoad() {
//    var popup = $find('check_ModalPopupExtender');
//popup.add_showing(calledWhenShown);
//        if (popup != null) {
//            popup.show();
//        }
//     }
    function AcceptClin() {
        var pipup = $find('clinHandoverButton_ModalPopupExtender');
        return true;
    }
     function runup() {
         var apopup = $find('check_ModalPopupExtender');

         apopup.add_showing(rkunup);

}
function rkunup() {
    alert('I am called!');
}

   

    function calledWhenShown() {

        alert('I am called when the ModalPopup is shown');

    }

function ClearUI() {
    $find("textvalidator1").hide();
    $get("txtUsername").value = "";
}

function IsValid() {
    var textbox = $get("txtUsername");
    if (textbox.value == "") {
        return false;
    }
    else
        return true;
}


function DoClose() {

    
   
    
    // return true so that submit will happen
    return true;
}


</script>        
   <div>
       <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC" 
        BorderStyle="Solid">
        <div id="header_wrapper">
    <div id="temp_header">
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="HandoverId"
        BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" 
        CellPadding="4"  EditRowStyle-BorderStyle="NotSet" Width="906px">
            <RowStyle BackColor="White" ForeColor="#330099" />
            <Columns>
            <asp:BoundField DataField="HandoverId" HeaderText="HandoverId" 
                    InsertVisible="False" ReadOnly="True" SortExpression="HandoverId" />
                   <asp:TemplateField HeaderText="6 MV"  >
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MV6"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="6 MV FFF"  >
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MV6FFF"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="10 MV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MV10"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="10 MV FFF">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MV10FFF"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="4 MeV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MeV4"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="6 MeV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MeV6"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="8 MeV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MeV8"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="10 MeV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MeV10"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="12 MeV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MeV12"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="15 MeV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MeV15"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="18 MeV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MeV18"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="20 MeV">
                    <ItemTemplate >
                    <img src= "<%#FormatImage(Eval("MeV20"))%>" />
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comment" HeaderText="Comment" 
                    SortExpression="CComment" />
                                <asp:BoundField DataField="LogOutName" HeaderText="Approved By" 
                    SortExpression="LogOutName" />
                <asp:BoundField DataField="LogOutDate" HeaderText="Date Approved" 
                    SortExpression="LogOutDate" />
                <asp:BoundField DataField="linac" HeaderText="linac" SortExpression="linac" />
<%--                    <asp:BoundField DataField="ApprovedBy" HeaderText="ApprovedBy" 
                    SortExpression="ApprovedBy" />--%>
                
                    
            </Columns>
            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
            <HeaderStyle BackColor="#330000" Font-Bold="True" ForeColor="#FFFFCC" />
        </asp:GridView>
        
        <br />
        <br />
        <br />
        <br />
        </div>
        </div>

        <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1430px">
               <asp:TableRow ID="r1" runat="server">
                   
                   <asp:TableCell ID="c1" runat="server" Width="50px">
                       
                   <asp:Button ID="clinHandoverButton" Height="150px"  runat="server" 
    Text="Approve For Clinical Use" CausesValidation="false" BackColor="#FFCC00" />


</asp:TableCell>
<asp:TableCell ID="c2" runat="server" HorizontalAlign="Left" Width="100px">
 <%--<asp:TableCell ID="TableCell1" runat="server" Width="250px" HorizontalAlign="Left">--%>
                   <asp:GridView ID="GridViewImage" runat="server" AutoGenerateColumns="False" >
            <Columns>
            
                <asp:BoundField DataField="Energy" HeaderText="Select Imaging" 
                    SortExpression="Energy" />
                    <asp:TemplateField>  
                             
            <ItemTemplate>
            <asp:CheckBox runat="server" ID="RowlevelCheckBox" AutoPostBack="true"  
                    OncheckedChanged="checked"/>
            </ItemTemplate>
                      </asp:TemplateField>
            </Columns>
        </asp:GridView>
  <%--  <asp:CheckBoxList ID="Imaging" runat="server" RepeatColumns="1">
    <asp:ListItem Text="iView"  />
    <asp:ListItem Text="XVI" Enabled="False" />
    </asp:CheckBoxList>--%>
</asp:TableCell>
                   
            
                   <asp:TableCell ID="PreclinicalComments" runat="server" HorizontalAlign="left" Width="250px">
                               <legend align="left" style="font-family: Arial, Helvetica, sans-serif; font-weight: bold">Pre-clinical Comments</legend>
                              <asp:TextBox ID="CommentBox" runat="server" MaxLength="250" Rows="5" 
                TextMode="MultiLine" Width="250px" Height="150px" ReadOnly="false" AutoPostBack="true"></asp:TextBox>

                       </asp:TableCell>
                       
                       <asp:TableCell ID="c3" runat="server" HorizontalAlign="Left" Width="250px" VerticalAlign="Top" BackColor="White">
                                                        <div style =" background-color:Green;  
        height:30px;width:355px; margin:0;padding:0">
        <table cellspacing="0" cellpadding = "0" rules="all" border="1" id="Table3" 
         style="font-family:Arial;font-size:10pt;width:350px;color:white;
         border-collapse:collapse;height:100%;">
            <tr>
               <td style ="width:175px;text-align:center">Engineering Comment</td>
            </tr>
        </table>
        </div>
 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
           <ContentTemplate>
           
        <div style ="height:175px; width:355px; overflow:auto;">
        <asp:GridView ID="GridViewComments" runat="server" AutoGenerateColumns="False" showheader="false"
        DataKeyNames="Comment" BackColor="White" 
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
        GridLines="Horizontal" >
        <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
                <asp:BoundField DataField="comment" HeaderText="Engineering" ItemStyle-Width="350px" 
                SortExpression="Engineering" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" />
            </Columns>
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>              
               </div>    
                       
</ContentTemplate>         
</asp:UpdatePanel>
                       
                       
                       <%--<div>--%>
                       
                       
                       <%--<asp:Repeater ID="Repeater1" runat="server">
                       <HeaderTemplate><table width="250px" >
                       <tr><th>Engineering Runup Comments</th></tr></HeaderTemplate>
        <ItemTemplate><tr><td>
                        <%#Eval("Comment")%></td></tr></ItemTemplate>
                        <FooterTemplate></table></FooterTemplate>
                </asp:Repeater>--%>
                       <%--</div>--%>
                               <%--<asp:TextBox ID="TextBox2" runat="server" MaxLength="250" Rows="5" 
                TextMode="MultiLine" Width="250px" Height="150px" ReadOnly="true"></asp:TextBox>--%>
                </asp:TableCell>
                <asp:TableCell ID = "c4" runat="server" HorizontalAlign="Left" Width="375px">
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:PlaceHolder ID="PlaceHolder3" runat="server">
</asp:PlaceHolder>
</ContentTemplate>

</asp:UpdatePanel>
                       </asp:TableCell>

                       <asp:TableCell VerticalAlign="Top">
<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
               <ContentTemplate>
                   <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                   <br></br>
               <asp:PlaceHolder ID="PlaceHolder5" runat="server">
                </asp:PlaceHolder>
                </ContentTemplate></asp:UpdatePanel>
               </asp:TableCell>

               </asp:TableRow>
               
                          </asp:Table>
                          
                          <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" Width="1038px">
         <asp:TableRow ID ="t2r1" runat="server">
         <asp:TableCell ID="t2c1" runat="server" Width="160px" HorizontalAlign="Left">
                     
 </asp:TableCell>
                              <asp:TableCell ID="t2c2" runat="server">
               <asp:Button ID="LogOff" runat="server" Text="Log Off Without Approving For Clinical Use" CausesValidation="False" />
               </asp:TableCell>
                        </asp:TableRow>
               </asp:Table>

                       <asp:UpdatePanel ID="faultupdatePanel" runat="server" Visible="true">
                       
        <ContentTemplate>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
         </ContentTemplate>
        </asp:UpdatePanel>
        <asp:PlaceHolder ID="PlaceHolder2" runat="server">
        <uc2:WriteDatauc ID="WriteDatauc1" LinacName="" UserReason="2"  Tabby="2"  WriteName="PreClinData" visible="false" runat="server" />
            <uc3:ConfirmPage ID="ConfirmPage1" Visible="false" runat="server" />
         </asp:PlaceHolder>
                          
            <br />
           
           <uc5:ViewCommentsuc ID="ViewCommentsuc1" linacName="" CommentSort="pcr" runat="server" />
                          
            </asp:Panel>
           
        </div>   
    
        
    
        <br />
        

   
    


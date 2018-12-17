<%@ Page Title="" Language="VB" MasterPageFile="~/Elf.master" AutoEventWireup="false" CodeFile="OneTable.aspx.vb" Inherits="OneTable" %>

<%@ Register src="controls/CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Table ID="Table1" runat="server">
        <asp:TableRow>
    <asp:TableCell ID="Clinicalcomments" runat="server" HorizontalAlign="left" Width="250px">
                   <div>
                       <table style="width: 100%;">
                           <tr>
                               <td>
                                   Clinical Comment
                               </td>
                           </tr>
                           <tr>
                               <td>
                               <asp:UpdatePanel ID="UpdatePanelcomments" runat="server" UpdateMode="Conditional"><ContentTemplate>
                    <uc1:CommentBoxuc ID="CommentBox" runat="server" /><br /><asp:Button ID="SaveText" runat="server" Text="Save" CausesValidation="False" />
                                       </ContentTemplate><Triggers><asp:AsyncPostBackTrigger ControlID ="SaveText" eventname="click"/></Triggers></asp:UpdatePanel>
                               
                                   </td>
                               <td>

                               </td>
                           </tr>
                           <tr>
                               <td>
 <div style =" background-color:Green;  
        height:30px;width:355px; margin:0;padding:0">
        <table cellspacing="0" cellpadding = "0" rules="all" border="1" id="Table2" 
         style="font-family:Arial;font-size:10pt;width:350px;color:white;
         border-collapse:collapse;height:100%;">
            <tr>
                <td style ="width:175px;text-align:center">Time</td>
               <td style ="width:175px;text-align:center">Clinical Comment</td>
            </tr>
        </table>
        </div>
 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
           <ContentTemplate>
           
        <div style ="height:175px; width:355px; overflow:auto;">
        <asp:GridView ID="GridViewComments" runat="server" AutoGenerateColumns="False" showheader="false"
        DataKeyNames="Clincomment" BackColor="White" 
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
        GridLines="Horizontal" >
        <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="DateTime" HeaderText="Time" ItemStyle-Width="175px" 
                SortExpression="Time" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" HtmlEncode="False" HtmlEncodeFormatString="True" />
      <asp:BoundField DataField="Clincomment" HeaderText="Clinical" ItemStyle-Width="175px" 
                SortExpression="Clinical" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" HtmlEncode="False" HtmlEncodeFormatString="True" />

            </Columns>
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>              
               </div>    
                       
</ContentTemplate>         
</asp:UpdatePanel>
        </td>
      </tr>
    </table>
 </div>
 </asp:TableCell>
 <asp:TableCell>
     <table>
         <tr>
           <td>
             <div style =" background-color:Green; height:30px;width:355px; margin:0;padding:0">
                <table cellspacing="0" cellpadding = "0" rules="all" border="1" id="Table5" style="font-family:Arial;font-size:10pt;width:350px;color:white;
                        border-collapse:collapse;height:100%;">
            <tr>
                <td style ="width:175px;text-align:center">Time</td>
               <td style ="width:175px;text-align:center">Pre-Clinical Comment</td>             
            </tr>
                    </table>
            </div>
               </td>
             </tr>
            <tr>
                <td>
                 <div style ="height:175px; width:355px; overflow:auto;">
        <asp:GridView ID="GridPre" runat="server" AutoGenerateColumns="False" showheader="false"
        DataKeyNames="Comment" BackColor="White" 
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
        GridLines="Horizontal" >
        <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
     
            <asp:BoundField DataField="Ccomment" HeaderText="PreClinical" ItemStyle-Width="350px" 
                SortExpression="PreClinical" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" />
               
            </Columns>
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>              
               </div>
                    </td>
            </tr>
          
            <tr>
                <td>
                  <div style =" background-color:Green;  
        height:30px;width:355px; margin:0;padding:0">
        <table cellspacing="0" cellpadding = "0" rules="all" border="1" id="Table6" 
         style="font-family:Arial;font-size:10pt;width:350px;color:white;
         border-collapse:collapse;height:100%;">
            <tr>
                <td style ="width:175px;text-align:center">Time</td>
               <td style ="width:175px;text-align:center">Engineering Comment</td>
            </tr>
        </table>
        </div>
           
        <div style ="height:175px; width:355px; overflow:auto;">
        <asp:GridView ID="GridViewEng" runat="server" AutoGenerateColumns="False" showheader="false"
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
                       
</td>
            </tr>
        </table>
                   </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    
</asp:Content>


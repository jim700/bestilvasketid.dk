<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="bestilvasketid.dk.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Vores Kontakt Info</h3>
    <address>
        JHC Group International<br />
        TEC Ballerup<br />
        Telegrafvej 7-9<br />
        2750 Ballerup<br />
        <abbr title="Phone">Telefon: </abbr>
        <a href="tel:004561733755">+45 61 733 755</a>       
    </address>

    <address>
        <strong>Support:</strong>   <a href="mailto:bestilvasketid@gmail.com">bestilvasketid@gmail.com</a><br />
        <%--<strong>Marketing:</strong> <a href="mailto:Marketing@example.com">Marketing@example.com</a>--%>
    </address>

    <br />
    <h3>Indstillinger</h3>
    <asp:Label ID="OpenTimeLabel" runat="server" Text="Start tid: "></asp:Label><br />
    <asp:TextBox ID="OpenTime" runat="server" placeholder="i hele timer"></asp:TextBox><br />
    <asp:Label ID="CloseTimeLabel" runat="server" Text="Slut tid"></asp:Label><br />
    <asp:TextBox ID="CloseTime" runat="server" placeholder="i hele timer"></asp:TextBox><br /><br />

    <asp:Label ID="MaxScheduleTimeLabel" runat="server" Text="Vasketidslængde"></asp:Label><br />
    <asp:TextBox ID="MaxScheduleTime" runat="server" placeholder="i minutter"></asp:TextBox><br /><br />

    <asp:Label ID="MachinesLabel" runat="server" Text="Antal Maskiner"></asp:Label><br />
    <asp:TextBox ID="Machines" runat="server" placeholder="i hele maskiner"></asp:TextBox><br /><br />

    <asp:Label ID="IdentityLabel" runat="server" Text="Hvis identitet?"></asp:Label>
    <asp:CheckBox ID="IdentityCheckBox" runat="server" /><br />

    <asp:Button ID="ConfirmButton" runat="server" Text="Gem indstillinger" OnClick="ConfirmButton_Click" />

</asp:Content>

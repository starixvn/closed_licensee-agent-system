<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsertMultipleMember.aspx.cs"
    Inherits="LicenseeAgent.Site.InsertMultipleMember" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div>
        Insert member for yq355uus_agent111. ID:
        <%=AgentId %>. Currency:<%=CurrencyId %>. Licensee:<%=LicenseeName%>
    </div>
    <form id="form1" runat="server">
    <div>
        <label>
            Username prefix</label>
        <input name="prefix" value="<%=Prefix %>" />
    </div>
    <div>
        <label>
            Start Index</label>
        <input name="start" value="<%=StartIndex %>" />
    </div>
    <div>
        <label>
            Member Count</label>
        <input name="count" value="<%=MemberCount %>" />
    </div>
    <input type="submit" value="Start" />
    </form>
    <div>
        Error Message
    </div>
    <div>
        <%=Message %>
    </div>
</body>
</html>

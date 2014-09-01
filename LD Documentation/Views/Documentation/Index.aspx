<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<% Response.Write(this.ViewData["title"]); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <% LD_Documentation.Models.DocumentationLink link = (LD_Documentation.Models.DocumentationLink)this.ViewData["nav"]; %>
    <% foreach(LD_Documentation.Models.DocumentationLink current in link.GetParents()) { %>
    <li><a href="<%: current.Uri %>"><%: current.Title %></a></li>
    <% } %>
    <li><a href="<%: link.Uri %>"><%: link.Title %></a></li>
    <li><ul>
    <% foreach (LD_Documentation.Models.DocumentationLink current in link.GetChildren()) { %>
        <li><a href="<%: current.Uri %>"><%: current.Title %></a></li>
    <% } %>
    </ul></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Response.Write(this.ViewData["content"]); %>
<pre style="display: none">
<% Response.Write(Html.Encode(this.ViewData["xml"])); %>
</pre>
</asp:Content>

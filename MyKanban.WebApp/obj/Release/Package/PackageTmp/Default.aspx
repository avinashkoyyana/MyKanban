<%@ Page Language="C#" AutoEventWireup="true" Inherits="_Default" %>
<!DOCTYPE html>

<!-- -------------------------------------------------------------------
// File:    Default.aspx
// Purpose: Display MyKanban.Web sample Kanban board
// Date:    3/20/2015
// By:      Mark E. Gerow
// ---------------------------------------------------------------------
// Mods:
------------------------------------------------------------------------
    Copyright (c) 2015 Mark E. Gerow

    This file is part of MyKanban.

    MyKanban is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MyKanban is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MyKanban.  If not, see <http://www.gnu.org/licenses/>.
// ----------------------------------------------------------------- -->
<html lang="en">
<head runat="server">

    <title>MyKanban Web</title>
    <link rel="shortcut icon" type="image/x-icon" href="images/StatusAnnotations_Complete_and_ok_32xLG_color.png" />

    <link rel="stylesheet" href="chosen/chosen.css" />
    <link rel="stylesheet" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/themes/smoothness/jquery-ui.css" />

    <!-- Standard jQuery includes -->
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>

    <!-- jQuery plug-in which allows smooth operation with touch screens -->
    <script src="js/jquery.ui.touch-punch.min.js"></script>

    <!-- Support for cookies -->
    <script type="text/javascript" src="js/jquery.cookie.js"></script>

    <!-- Support for chosen, which improves UI of SELECT drop-downs -->
    <script src="chosen/chosen.jquery.js"></script>

    <!-- Support for spinner, which displays animated icon when filtering -->
    <script src="js/spin.js"></script>

    <!-- MyKanban JavaScript -->
    <script src="js/MyKanban.js"></script>

    <!-- MyKanban Styles, includes tweaks to jQuery CSS classes -->
    <link rel="stylesheet" href="css/MyKanban.css" />

</head>

<body>

    <!-- This DIV will be loaded with Kanban board after retrieved from database -->
    <div id="divBoard" style='background-color: #FFFFFF;'>
    </div>

    <!-- Login dialog -->
    <div id="divLogin" title="Login to MyKanban Web Demo" style="display: none; font-size: 10pt;">
        <div>
            <div>For demonstration purposes, please select "Azure" as the server, then "TESTUSER" as the user name, and "password" as the password (case sensitive).
                This will connect you to the Azure instance of the web application and database (SQL Server in this case).<br /><br />
                <i style="color: red; font-size: smaller;">Please note: The performance I've gotten from Azure is not good.  Issue seems to be in the database not website.  This
                    is most likely because I'm using the lowest level of Azure servers to minimize my cost.
                    When connected to an in-house database server performance is almost instantaneous.  So please don't be
                    too concerned about performance.
                </i>
                <br /><br />
            </div>
            <table>
                <tr>
                    <td>Server:</td>
                    <td>
                        <select id="selServer">
                            <option value="Azure" selected="selected">Azure</option>
                            <option value="Local">Local</option>
                        </select></td>
                </tr>
                <tr>
                    <td>Username:</td>
                    <td>
                        <input style="width: 150px;" type="text" name="user" id="txtUserName" /></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td>
                        <input style="width: 150px;" type="password" name="password" id="txtPwd" /></td>
                </tr>
            </table>
        </div>
    </div>

    <!-- Floating toolbox -->
    <div id="divToolbox" title="Filters & Tools" style="display: none; font-size: 10pt;">
        <table cellpadding="3" cellspacing="3">
            <tr>
                <td>Board:</td>
                <td>
                    <select id="selAuthorizedBoards" onchange="displayBoard();" style="width: 200px;"></select></td>
                <td>
                    <div id="divSpinner" style="position: relative; top: 8px; left: 6px; width: 20px;"></div>
                </td>
            </tr>
            <tr>
                <td>Sprint:</td>
                <td>
                    <select id="selSprint" onchange="filterBySprint();" style="width: 200px;">
                        <option>[All Sprints]</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Project:</td>
                <td>
                    <select id="selProject" onchange="filterByProject();" style="width: 200px;">
                        <option>[All Projects]</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Assignee:</td>
                <td>
                    <select id="selAssignee" onchange="filterByAssignee();" style="width: 200px;">
                        <option>[All Assignees]</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Approver:</td>
                <td>
                    <select id="selApprover" onchange="filterByApprover();" style="width: 200px;">
                        <option>[All Approvers]</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Search for:</td>
                <td valign="middle">
                    <table style="width: 200px;" cellpadding="3">
                        <tr>
                            <td>
                                <input id="txtSearch" />
                            </td>
                            <td valign="middle" align="left">
                                <a href="javascript:filterByText();" style="text-decoration: none">
                                    <img src="images/Find_5650.png" border="0" style="position: relative; top: 3px; left: -4px;" /></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <!-- Task edit form -->
    <div id="divEditTask" title="Edit Task" style="display: none;" taskid="0">
        <table cellpadding="3" cellspacing="3">
            <tr>
                <td>Project:</td>
                <td>
                    <div id="txtProject" style="border: none; color: gray;"></div>
                    <select id="selTaskProjects"></select>
                </td>
            </tr>
            <tr>
                <td>Name:</td>
                <td>
                    <input id="txtName" size="50" /></td>
            </tr>
            <tr>
                <td>Define Done:</td>
                <td>
                    <textarea id="txtDefineDone" rows="6" style="width: 98%;"></textarea>
                </td>
            </tr>
            <tr>
                <td>Est Hours:</td>
                <td>
                    <input id="txtEstHours" style="width: 50px;" /></td>
            </tr>
            <tr>
                <td>Act Hours:</td>
                <td>
                    <input id="txtActHours" style="width: 50px;" /></td>
            </tr>
            <tr>
                <td>Start Date:</td>
                <td>
                    <input id="txtStartDate" style="width: 100px;" /></td>
            </tr>
            <tr>
                <td>End Date:</td>
                <td>
                    <input id="txtEndDate" style="width: 100px;" /></td>
            </tr>
            <tr>
                <td>Assignees: </td>
                <td>
                    <select id="selAssignees" data-placeholder="Choose an Assignee..." class="chosen-select" multiple style="width: 350px;" tabindex="4"></select>
                </td>
            </tr>
            <tr>
                <td>Approvers: </td>
                <td>
                    <select id="selApprovers" data-placeholder="Choose an Approver..." class="chosen-select" multiple style="width: 350px;" tabindex="4"></select>
                </td>
            </tr>
            <tr>
                <td>Status: </td>
                <td>
                    <select id="selStatus"></select>
                </td>
            </tr>
        </table>
    </div>

</body>
</html>

/* ----------------------------------------------------------------------------- /
// File:        MyKanban.js
// Purpose:     Contains core routines for MyKanban.Web demo app
// By:          Mark E. Gerow
// Date:        3/20/2015
// ----------------------------------------------------------------------------- /
// Mode:
// ----------------------------------------------------------------------------- /
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
// -----------------------------------------------------------------------------*/

// Global variables
var boardId = 0;        // Current board id
var sprintId = 0;       // Sprint filter value
var projectId = 0;      // Project filter value
var assigneeId = 0;     // Assignee filter value
var approverId = 0;     // Approver filter value
var filterText = '';    // Text filter value
var token = '';         // User token returned by call to GetUserData.aspx
var canAdd = false;     // Can current user add tasks to current board
var canEdit = false;    // Can current user edit tasks on current board
var canDelete = false;  // Can current user delete tasks from current board
var statusId = 0        // Current task status id

// Variable used to determine which server and database
// should process request
var proxyPath = '';         // Path to MyKanban proxy
var rootPath = '';          // Path to MyKanban web server serving data
var dbType = '';            // Either "MySql" or "SqlServer"
var connectionString = '';  // Database connection string

// Set spinner options
var spinner = null;
var opts = {
    lines: 7, // The number of lines to draw
    length: 5, // The length of each line
    width: 2, // The line thickness
    radius: 5, // The radius of the inner circle
    corners: 1, // Corner roundness (0..1)
    rotate: 0, // The rotation offset
    direction: 1, // 1: clockwise, -1: counterclockwise
    color: 'green', // #rgb or #rrggbb or array of colors
    speed: 1.2, // Rounds per second
    trail: 60, // Afterglow percentage
    shadow: true, // Whether to render a shadow
    hwaccel: true, // Whether to use hardware acceleration
    className: 'spinner', // The CSS class to assign to the spinner
    zIndex: 2e9, // The z-index (defaults to 2000000000)
    top: '50%', // Top position relative to parent
    left: '50%' // Left position relative to parent
};

// jQuery startup routines - after all of page is loaded into browser
$(function () {

    // If cookie for user data exists, set default
    $('#txtUserName').val($.cookie("userName"));
    $('#txtPwd').val($.cookie("pwd"));

    // Prompt user to login
    $('#divLogin').dialog({
        resizable: false,
        autoResize: true,
        height: 'auto',
        width: '450',
        dialogClass: 'dialog',
        closeOnEscape: false,
        modal: true,
        open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); },
        buttons: [
            {
                text: "Login", click: function () {
                    $(this).dialog("close");
                    var userName = $('#txtUserName').val();
                    var pwd = $('#txtPwd').val();
                    $.cookie('userName', userName, { expires: 30 });
                    $.cookie('pwd', pwd, { expires: 30 });
                    getUserData(userName, pwd);
                }
            }
        ]
    });
});

// Get list of boards this user has access to
function getUserData(userName, pwd) {

    var timeStamp = new Date().getTime();

    // Determine which proxy to use based on login, this info will remain
    // in affect for duration of this user session.
    if ($('#selServer').val() == 'Local') {

        proxyPath = "http://localhost:50312/proxy.aspx?mode=";
        rootPath = 'http://localhost:50312/';
        dbType = 'MySql';
        connectionString = encodeURI('Server=localhost;Database=mykanban;Uid=mykanban;Pwd=megabase;');
        //connectionString = encodeURI('Server=mykanban.ctcu3iejefqp.us-west-2.rds.amazonaws.com;Database=mykanban;Uid=mykanban;Pwd=MyK2nb2n2015;');
        //dbType = 'SqlServer';
        //connectionString = encodeURI('Server=tcp:lnfk7armd0.database.windows.net,1433;Database=mykanban;User ID=mykanban@lnfk7armd0;Password=MyK@b@n2015;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;');

    } else if ($('#selServer').val() == 'Azure') {

        // Azure cloud settings
        // --------------------
        proxyPath = "http://gerow1.azurewebsites.net/proxy.aspx?mode=";
        rootPath = 'http://gerow1.azurewebsites.net/';
        dbType = 'SqlServer';
        connectionString = encodeURI('Server=tcp:lnfk7armd0.database.windows.net,1433;Database=mykanban;User ID=mykanban@lnfk7armd0;Password=MyK@b@n2015;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;');

    } else if ($('#selServer').val() == 'Work' && userName.toLowerCase() != 'testuser') {

        proxyPath = "https://mobile.fenwick.com/proxy.aspx?mode=";
        rootPath = "http://fw-8s50l02/MyKanban/";
        dbType = "MySql";
        connectionString = encodeURI('Server=fw-8s50l02;Database=mykanban;Uid=mykanban;Pwd=megabase;');

    } else {
        
        alert('Login denied.  If you believe you received this message in error, please contact your system administrator.');
        window.location.reload();
    }

    $.ajax({
        url: proxyPath + "GetUserData&userName=" + userName 
            + "&pwd=" + pwd 
            + '&rootPath=' + rootPath 
            + '&dbType=' + dbType
            + '&t=' + timeStamp
            + '&connectionString=' + connectionString,
        dataType: "jsonp",
        type: 'GET',
        crossDomain: true,
        jsonpCallback: "getUserDataCallback",
        success: function (data) {
            // For some reason, the success method is getting called
            // instead of callback3(), so in this case just pass
            // control to callback3() explicitly.
            //callback3(data);
        },
        error: function (e) {
            alert(e.message);
        }
    });
}

// Populate the drop-down list of boards and display
function getUserDataCallback(data) {

    // Fill list of boards
    $('#selAuthorizedBoards').html('<option value="0">[Select a Board]</option>');
    for (var i = 0; i < data.length; i++) {
        $('#selAuthorizedBoards').append('<option value=' + data[i].Id + '>' + data[i].Name + '</option>');
    }

    // Save user token
    if (data.length > 0) token = encodeURIComponent(data[0].Token);

    $('#divToolbox').dialog({
        resizable: false,
        autoResize: true,
        height: 'auto',
        width: 'auto',
        dialogClass: 'dialog',
        closeOnEscape: false,
        modal: false,
        open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); },
        buttons: [
            {
                text: "Collapse",
                click: function () {
                    collapse();
                }
            },
            {
                text: "Expand",
                click: function () {
                    expand();
                }
            },
            {
                text: "New Task",
                click: function () {
                    addTask();
                }
            },
            {
                text: "Refresh",
                click: function () {
                    displayBoard(true);
                }
            }
        ]
    });
}

// Collapse all tasks down to just headings
function collapse() {
    $('.ui-icon-minusthick').parents(".portlet").find(".portlet-content").toggle();
    $('.ui-icon-minusthick').toggleClass("ui-icon-minusthick ui-icon-plusthick");
}

// Expand all tasks to show more details
function expand() {
    $('.ui-icon-plusthick').parents(".portlet").find(".portlet-content").toggle();
    $('.ui-icon-plusthick').toggleClass("ui-icon-minusthick ui-icon-plusthick");
}

// Get data for selected board
function displayBoard(refresh) {

    var timeStamp = new Date().getTime();

    // Show spinner while getting board
    var target = document.getElementById('divSpinner');
    spinner = new Spinner(opts).spin(target);

    if (typeof (refresh) === 'undefined') refresh = false;

    boardId = $('#selAuthorizedBoards').val();
    sprintId = $('#selSprint').val();
    projectId = $('#selProject').val();
    assigneeId = $('#selAssignee').val();
    approverId = $('#selApprover').val();
    filterText = $('#txtSearch').val();

    $.ajax({
        url: proxyPath + "GetBoardData&boardId=" + boardId 
            + "&sprintId=" + sprintId 
            + '&projectId=' + projectId 
            + '&assigneeId=' + assigneeId 
            + '&approverId=' + approverId 
            + '&filterText=' + filterText 
            + '&refresh=' + refresh 
            + '&rootPath=' + rootPath 
            + '&dbType=' + dbType 
            + '&connectionString=' + connectionString
            + '&t=' + timeStamp
            + '&token=' + token,
        dataType: "jsonp",
        type: 'GET',
        crossDomain: true,
        jsonpCallback: "displayBoardCallback",
        success: function (data) {
            // For some reason, the success method is getting called
            // instead of callback(), so in this case just pass
            // control to callback() explicitly.
            //callback(data);
        },
        error: function (e) {
            alert(e.message);
        }
    });
}

// Take JSON returned by GetBoardData.aspx (through proxy) and display all tasks
function displayBoardCallback(data) {

    // Clear board before filling
    $('#divBoard').html('');
    $('#selStatus').html('');

    // Create columns for each status & populate status drop-down on edit form
    //for (var i = 0; i < data[0].Items.length; i++) {
    //    $('#divBoard').append('<div id="divStatus_' + data[0].Items[i].Id + '" class="column"><h3 class="columnHeader" style="padding-bottom: 10px;"><span class="columnHeaderText" style="background-color: ' + data[0].Items[i].BackColor + '; color: ' + data[0].Items[i].ForeColor + ';">' + data[0].Items[i].ColumnHeading + '</span></h3></div>');
    //    $('#selStatus').append('<option value="' + data[0].Items[i].Id + '">' + data[0].Items[i].Name + '</option>');
    //}
    for (var i = 0; i < data.board_set_status.length; i++) {
        $('#divBoard').append('<div id="divStatus_' + data.board_set_status[i].id + '" class="column"><h3 class="columnHeader" style="padding-bottom: 10px;"><span class="columnHeaderText" style="background-color: ' + data.board_set_status[i].back_color + '; color: ' + data.board_set_status[i].fore_color + ';">' + data.board_set_status[i].column_heading + '</span></h3></div>');
        $('#selStatus').append('<option value="' + data.board_set_status[i].id + '">' + data.board_set_status[i].column_heading + '</option>');
    }

    // Fill filter list for sprints
    //$('#selSprint').html('<option value="0" selected="selected">[All Sprints]</option>');
    //for (var i = 0; i < data[4].Items.length; i++) {
    //    $('#selSprint').append('<option value="' + data[4].Items[i].Id + '">' + data[4].Items[i].Name + '</option>');
    //}
    //$("#selSprint option[value='" + sprintId + "']").prop("selected", true);
    $('#selSprint').html('<option value="0" selected="selected">[All Sprints]</option>');
    for (var i = 0; i < data.sprint.length; i++) {
        $('#selSprint').append('<option value="' + data.sprint[i].id + '">' + data.sprint[i].name + '</option>');
    }
    $("#selSprint option[value='" + sprintId + "']").prop("selected", true);


    // Load tasks
    //for (var i = 0; i < data[1].length; i++) {

    //    var projectName = data[1][i].ProjectName;

    //    var task = data[1][i];

    //    $('#divStatus_' + task.Status).append(getPortlet(task));
    //}
    for (var i = 0; i < data.task.length; i++) {

        var projectName = data.task[i].project_name;

        var task = data.task[i];

        $('#divStatus_' + task.status_id).append(getPortlet(task));
    }

    // Now fill in the assignees multi-select list
    // in the edit task dialog with list of available
    // users, also fill filter drop-down
    //$('#selAssignees').html('');
    //$('#selApprovers').html('');
    //$('#selAssignee').html('<option value="0" selected="selected">[All Assignees]</option>');
    //$('#selApprover').html('<option value="0" selected="selected">[All Approvers]</option>');

    //for (var i = 0; i < data[2].Items.length; i++) {

    //    // For add/edit form
    //    $('#selAssignees').append('<option value="' + data[2].Items[i].PersonId + '">' + data[2].Items[i].Name + '</option>');
    //    $('#selApprovers').append('<option value="' + data[2].Items[i].PersonId + '">' + data[2].Items[i].Name + '</option>');

    //    // For filter
    //    $('#selAssignee').append('<option value="' + data[2].Items[i].PersonId + '">' + data[2].Items[i].Name + '</option>');
    //    $('#selApprover').append('<option value="' + data[2].Items[i].PersonId + '">' + data[2].Items[i].Name + '</option>');
    //}
    //$('#selAssignees').chosen();
    //$('#selAssignees').trigger("chosen:updated");
    //$('#selApprovers').chosen();
    //$('#selApprovers').trigger("chosen:updated");
    //$("#selAssignee option[value='" + assigneeId + "']").prop("selected", true);
    //$("#selApprover option[value='" + approverId + "']").prop("selected", true);
    $('#selAssignees').html('');
    $('#selApprovers').html('');
    $('#selAssignee').html('<option value="0" selected="selected">[All Assignees]</option>');
    $('#selApprover').html('<option value="0" selected="selected">[All Approvers]</option>');

    for (var i = 0; i < data.board_person.length; i++) {

        // For add/edit form
        $('#selAssignees').append('<option value="' + data.board_person[i].id + '">' + data.board_person[i].name + '</option>');
        $('#selApprovers').append('<option value="' + data.board_person[i].id + '">' + data.board_person[i].name + '</option>');

        // For filter
        $('#selAssignee').append('<option value="' + data.board_person[i].id + '">' + data.board_person[i].name + '</option>');
        $('#selApprover').append('<option value="' + data.board_person[i].id + '">' + data.board_person[i].name + '</option>');
    }
    $('#selAssignees').chosen();
    $('#selAssignees').trigger("chosen:updated");
    $('#selApprovers').chosen();
    $('#selApprovers').trigger("chosen:updated");
    $("#selAssignee option[value='" + assigneeId + "']").prop("selected", true);
    $("#selApprover option[value='" + approverId + "']").prop("selected", true);

    $(".column").sortable({
        connectWith: ".column",
        handle: ".portlet-header",
        cancel: ".portlet-toggle",
        placeholder: "portlet-placeholder ui-corner-all",

        remove: function (event, ui) {

            var taskId = ui.item[0].id.split('_')[1];
            var content = document.getElementById('content_' + taskId);
            $("body").css("cursor", "progress");

        },

        receive: function (event, ui) {

            var timeStamp = new Date().getTime();

            var taskId = ui.item[0].id.split('_')[1];
            statusId = event.target.id.split('_')[1];

            // Update the status
            $.ajax({
                url: proxyPath + "UpdateTaskStatus&taskId=" + taskId 
                    + '&statusId=' + statusId 
                    + '&rootPath=' + rootPath 
                    + '&dbType=' + dbType 
                    + '&connectionString=' + connectionString
                    + '&t=' + timeStamp
                    + '&token=' + token,
                dataType: "jsonp",
                type: 'GET',
                async: false,
                crossDomain: true,
                jsonpCallback: "updateTaskStatusCallback",
                success: function (data) {
                    // For some reason, the success method is getting called
                    // instead of callback2(), so in this case just pass
                    // control to callback2() explicitly.
                    updateTaskStatusCallback(data);
                },
                error: function (e) {
                    alert(e.message);
                }
            });
        },

        stop: function (event, ui) {

            var timeStamp = new Date().getTime();

            // If we got here, then a status update occurred.  Need
            // to insert some time here so that preceeding update
            // can complete before we start this one

            var divId = event.target.id;
            var idsInOrder = $("#" + divId).sortable("toArray");
            var serIdsInOrder = serializeIds(idsInOrder);
            statusId = event.target.id.split('_')[1];

            setTimeout(function () { processSequenceChange(serIdsInOrder, statusId, timeStamp) }, 1000);

            //$.ajax({
            //    url: proxyPath + "UpdateTaskSequence&boardId=" + boardId 
            //        + '&taskIds=' + serIdsInOrder 
            //        + '&rootPath=' + rootPath 
            //        + '&dbType=' + dbType 
            //        + '&connectionString=' + connectionString
            //        + '&statusId=' + statusId
            //        + '&t=' + timeStamp
            //        + '&token=' + token,
            //    dataType: "jsonp",
            //    type: 'GET',
            //    async: false,
            //    crossDomain: true,
            //    jsonpCallback: "updateTaskSequenceCallback",
            //    success: function (data) {
            //        // For some reason, the success method is getting called
            //        updateTaskSequenceCallback(data);
            //    },
            //    error: function (e) {
            //        alert(e.message);
            //    }
            //});
        }
    });

    function processSequenceChange(serIdsInOrder, statusId, timeStamp) {

        $.ajax({
            url: proxyPath + "UpdateTaskSequence&boardId=" + boardId 
                + '&taskIds=' + serIdsInOrder 
                + '&rootPath=' + rootPath 
                + '&dbType=' + dbType 
                + '&connectionString=' + connectionString
                + '&statusId=' + statusId
                + '&t=' + timeStamp
                + '&token=' + token,
            dataType: "jsonp",
            type: 'GET',
            async: false,
            crossDomain: true,
            jsonpCallback: "updateTaskSequenceCallback",
            success: function (data) {
                // For some reason, the success method is getting called
                updateTaskSequenceCallback(data);
            },
            error: function (e) {
                alert(e.message);
            }
        });

    }

    function updateTaskSequenceCallback(data) {
        // NO OP
    }

    // Return a semicolon-delimited list of task IDs in the current column
    function serializeIds(ids) {
        var serializedIds = '';
        for (var i = 0; i < ids.length; i++) {
            if (ids[i].length > 0 && ids[i].indexOf('_') != -1) {
                if (serializedIds.length > 0) serializedIds += ';';
                serializedIds += ids[i].split('_')[1];
            }
        }
        return serializedIds;
    }

    // Set the fore/back color of the task to the new status colors
    function updateTaskStatusCallback(task) {
        var header = document.getElementById('header_' + task.task[0].id);
        var content = document.getElementById('content_' + task.task[0].id);
        $(header).replaceWith('<div class="portlet-header" id="header_' + task.task[0].id + '" style="background-color: ' + task.task[0].back_color + '; color: ' + task.task[0].fore_color + ';">' + task.task[0].name + ' (' + task.task[0].id + ')</div>');
        $("body").css("cursor", "default");

        restorePlusMinusIcon(task);
    }

    // TODO: Figure out why below doesn't prevent portlets from being dropped on header row
    $(".columnHeader").disableSelection();

    // Add +/- for expand/collapse functionality
    $(".portlet")
      .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
      .find(".portlet-header")
        .addClass("ui-widget-header ui-corner-all")
        .prepend("<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

    $(".portlet-toggle").click(function () {
        var icon = $(this);
        icon.toggleClass("ui-icon-minusthick ui-icon-plusthick");
        icon.closest(".portlet").find(".portlet-content").toggle();
    });

    // Load project list for Add version of edit form, and for filter dialog
    //$('#selTaskProjects').html('');
    //$('#selProject').html('');
    //$('#selProject').html('<option value="0" selected="selected">[All Projects]</option>');
    //for (var i = 0; i < data[3].length; i++) {
    //    $('#selProject').append('<option value="' + data[3][i].Id + '">' + data[3][i].Name + '</option>');
    //    $('#selTaskProjects').append('<option value="' + data[3][i].Id + '">' + data[3][i].Name + '</option>');
    //}
    //$("#selProject option[value='" + projectId + "']").prop("selected", true);
    $('#selTaskProjects').html('');
    $('#selProject').html('');
    $('#selProject').html('<option value="0" selected="selected">[All Projects]</option>');
    for (var i = 0; i < data.project.length; i++) {
        $('#selProject').append('<option value="' + data.project[i].id + '">' + data.project[i].name + '</option>');
        $('#selTaskProjects').append('<option value="' + data.project[i].id + '">' + data.project[i].name + '</option>');
    }
    $("#selProject option[value='" + projectId + "']").prop("selected", true);

    // Make edit links look like buttons
    $('.editButton').button({ icons: { primary: "ui-icon-pencil" }, text: false });

    // Save user permissions for current board
    //canAdd = data[5].CanAdd;
    //canDelete = data[5].CanDelete;
    //canEdit = data[5].CanEdit;
    canAdd = data.user[0].can_add;
    canDelete = data.user[0].can_delete;
    canEdit = data.user[0].can_edit;

    // Disable add button if user does not have add permissions
    if (!canAdd) {
        $(".ui-dialog-buttonpane button:contains('New Task')").button("disable");
    } else {
        $(".ui-dialog-buttonpane button:contains('New Task')").button("enable");
    }

    // If user does not have edit permissions, disable ability to drag/drop items
    $(".column").sortable("option", "disabled", !canEdit);

    spinner.stop();

}

// Edit the selected task
function editTask(taskId) {

    var timeStamp = new Date().getTime();

    // Get task data
    $.ajax({
        url: proxyPath + "GetTaskData&taskId=" + taskId 
            + '&rootPath=' + rootPath 
            + '&dbType=' + dbType 
            + '&connectionString=' + connectionString
            + '&t=' + timeStamp
            + '&token=' + token,
        dataType: "jsonp",
        type: 'GET',
        crossDomain: true,
        jsonpCallback: "editTaskCallback",
        success: function (task) {
            // For some reason, the success method is getting called
            // instead of callback2(), so in this case just pass
            // control to callback2() explicitly.
            //callback4(data);
        },
        error: function (e) {
            alert(e.message);
        }
    });
}

// Populate edit dialog with JSON data returned by call to
// GetTaskData.aspx (via proxy)
function editTaskCallback(task) {

    // Set field values
    //$('#txtProject').html(task.ProjectName);
    //$('#selTaskProjects').hide();
    //$('#txtName').val(task.Name);
    //$('#txtDefineDone').val(task.DefineDone.replace(/\\n/g, "\n"));
    //$('#txtEstHours').val(task.EstHours);
    //$('#txtEstHours').spinner();
    //$('#txtActHours').val(task.ActHours);
    //$('#txtActHours').spinner();
    //$('#txtStartDate').val(formatDate(task.StartDate));
    //$("#txtStartDate").datepicker();
    //$('#txtEndDate').val(formatDate(task.EndDate));
    //$("#txtEndDate").datepicker();
    //$('#selStatus').val(task.Status);

    $('#txtProject').html(task.task[0].project_name);
    $('#selTaskProjects').hide();
    $('#txtName').val(task.task[0].name);
    $('#txtDefineDone').val(task.task[0].define_done.replace(/\\n/g, "\n"));
    $('#txtEstHours').val(task.task[0].est_hours);
    $('#txtEstHours').spinner();
    $('#txtActHours').val(task.task[0].act_hours);
    $('#txtActHours').spinner();
    $('#txtStartDate').val(formatDate(task.task[0].start_date));
    $("#txtStartDate").datepicker();
    $('#txtEndDate').val(formatDate(task.task[0].end_date));
    $("#txtEndDate").datepicker();
    $('#selStatus').val(task.task[0].status_id);

    // TODO: Set value(s) for assignees
    //$('#selAssignees').val('').trigger('chosen:updated');
    //for (var i = 0; i < task.Assignees.Items.length; i++) {
    //    $("#selAssignees option[value='" + task.Assignees.Items[i].Id + "']").prop("selected", true);
    //}
    //$('#selAssignees').trigger('chosen:updated');
    $('#selAssignees').val('').trigger('chosen:updated');
    for (var i = 0; i < task.task_assignee.length; i++) {
        $("#selAssignees option[value='" + task.task_assignee[i].id + "']").prop("selected", true);
    }
    $('#selAssignees').trigger('chosen:updated');

    // TODO: Set value(s) for approvers
    //$('#selApprovers').val('').trigger('chosen:updated');
    //for (var i = 0; i < task.Approvers.Items.length; i++) {
    //    $("#selApprovers option[value='" + task.Approvers.Items[i].PersonId + "']").prop("selected", true);
    //}
    //$('#selApprovers').trigger('chosen:updated');
    $('#selApprovers').val('').trigger('chosen:updated');
    for (var i = 0; i < task.task_approver.length; i++) {
        $("#selApprovers option[value='" + task.task_approver[i].id + "']").prop("selected", true);
    }
    $('#selApprovers').trigger('chosen:updated');

    // Set the ID# being edited
    $('#divEditTask').attr('taskId', task.task[0].id);

    $('#divEditTask').dialog({
        resizable: false,
        autoResize: true,
        height: 'auto',
        width: 'auto',
        dialogClass: 'dialog',
        closeOnEscape: true,
        modal: true,
        title: 'Edit Task (' + task.task[0].id + ')',
        open: function () {

            // Disable delete button if user does not have permission to delete
            if (!canDelete) {
                $(".ui-dialog-buttonpane button:contains('Delete')").button("disable");
            } else {
                $(".ui-dialog-buttonpane button:contains('Delete')").button("enable");
            }

            // Disable edit button if user does not have permission to edit
            if (!canEdit) {
                $(".ui-dialog-buttonpane button:contains('Save')").button("disable");
            } else {
                $(".ui-dialog-buttonpane button:contains('Save')").button("enable");
            }
        },
        buttons: [
            {
                text: "Delete", click: function () {
                    if (confirm("Are you sure you want to permanently delete this task?")) {
                        var timeStamp = new Date().getTime();
                        var taskId = $('#divEditTask').attr('taskId');
                        $.ajax({
                            url: proxyPath + "DeleteTask&taskId=" + taskId 
                                + '&rootPath=' + rootPath 
                                + '&dbType=' + dbType 
                                + '&connectionString=' + connectionString
                                + '&t=' + timeStamp
                                + '&token=' + token,
                            dataType: "jsonp",
                            type: 'POST',
                            crossDomain: true,
                            jsonpCallback: "deleteTaskCallback",
                            success: function (data) {
                                // Deleted
                            },
                            error: function (e) {
                                alert(e.message);
                            }
                        });

                        $(this).dialog('close');
                    }
                }
            },
            {
                text: "Save", click: function () {

                    var timeStamp = new Date().getTime();

                    var assignees = '';
                    $('#selAssignees :selected').each(function (i, selected) {
                        if (assignees.length > 0) assignees += ';';
                        assignees += $(selected).val();
                    });

                    var approvers = '';
                    $('#selApprovers :selected').each(function (i, selected) {
                        if (approvers.length > 0) approvers += ';';
                        approvers += $(selected).val();
                    });

                    var taskId = $('#divEditTask').attr('taskId');
                    var name = specialEncoding($('#txtName').val());
                    var defineDone = specialEncoding($('#txtDefineDone').val().replace(/\n/g, "\\n"));
                    var startDate = $('#txtStartDate').val();
                    var endDate = $('#txtEndDate').val();
                    var estHours = $('#txtEstHours').val();
                    var actHours = $('#txtActHours').val();
                    var statusId = $('#selStatus').val();

                    $.ajax({
                        url: proxyPath + "UpdateTask&taskId=" + taskId
                            + '&projectId=0'
                            + '&name=' + name
                            + '&defineDone=' + defineDone
                            + '&assignees=' + assignees
                            + '&approvers=' + approvers
                            + '&startDate=' + startDate
                            + '&endDate=' + endDate
                            + '&estHours=' + estHours
                            + '&actHours=' + actHours
                            + '&statusId=' + statusId
                            + '&rootPath=' + rootPath 
                            + '&dbType=' + dbType 
                            + '&connectionString=' + connectionString 
                            + '&t=' + timeStamp 
                            + '&token=' + token,
                        dataType: "jsonp",
                        type: 'POST',
                        crossDomain: true,
                        jsonpCallback: "saveTaskCallback",
                        success: function (data) {
                            // Saved
                        },
                        error: function (e) {
                            alert(e.message);
                        }
                    });

                    $(this).dialog("close");
                }
            },
            {
                text: "Cancel", click: function () {
                    $(this).dialog('close');
                }
            }
        ],
    });

}

// Rolled my own routine to handle some common characters that 
// present issues for ASP.NET
function specialEncoding(s) {
    return s.replace(/&/g, "~")
        .replace(/#/g, "`").replace(/\+/g, "^")
        .replace(/</g, '[').replace(/>/g, ']');
}

// After task has been deleted from MyKanban database, also
// need to hide it on the board
function deleteTaskCallback(data) {
    $('#task_' + data.TaskId).hide();
}

// Display the edit dialog with empty fields so user can
// create a new task
function addTask() {

    // Set field values
    $('#txtProject').html('');
    $('#selTaskProjects').show();
    $('#txtName').val('');
    $('#txtDefineDone').val('');
    $('#txtEstHours').val(8);
    $('#txtEstHours').spinner();
    $('#txtActHours').val(0);
    $('#txtActHours').spinner();
    $('#txtStartDate').val();
    $("#txtStartDate").datepicker();
    $('#txtEndDate').val();
    $("#txtEndDate").datepicker();

    $('#divEditTask').dialog({
        resizable: false,
        autoResize: true,
        height: 'auto',
        width: 'auto',
        dialogClass: 'dialog',
        closeOnEscape: true,
        modal: true,
        title: 'Add a New Task',
        open: function () {

            // If user is able to open this form, they have rights to add a new
            // task, but the save button may have been previously disabled if
            // they don't also have edit rights, so need to make sure it's enabled
            $(".ui-dialog-buttonpane button:contains('Save')").button("enable");
        },
        buttons: [
            {
                text: "Save", click: function () {

                    var timeStamp = new Date().getTime();

                    var assignees = '';
                    $('#selAssignees :selected').each(function (i, selected) {
                        if (assignees.length > 0) assignees += ';';
                        assignees += $(selected).val();
                    });

                    var approvers = '';
                    $('#selApprovers :selected').each(function (i, selected) {
                        if (approvers.length > 0) approvers += ';';
                        approvers += $(selected).val();
                    });

                    var taskId = 0;
                    var name = specialEncoding($('#txtName').val());
                    var defineDone = specialEncoding($('#txtDefineDone').val());
                    var startDate = $('#txtStartDate').val();
                    var endDate = $('#txtEndDate').val();
                    var estHours = $('#txtEstHours').val();
                    var actHours = $('#txtActHours').val();
                    var projectId = $('#selTaskProjects').val();
                    var statusId = $('#selStatus').val();

                    $.ajax({
                        url: proxyPath + "UpdateTask&taskId=" + taskId
                            + '&projectId=' + projectId
                            + '&name=' + name
                            + '&defineDone=' + defineDone
                            + '&assignees=' + assignees
                            + '&approvers=' + approvers
                            + '&startDate=' + startDate
                            + '&endDate=' + endDate
                            + '&estHours=' + estHours
                            + '&actHours=' + actHours
                            + '&statusId=' + statusId
                            + '&rootPath=' + rootPath 
                            + '&dbType=' + dbType 
                            + '&connectionString=' + connectionString 
                            + '&t=' + timeStamp
                            + '&token=' + token,
                        dataType: "jsonp",
                        type: 'POST',
                        crossDomain: true,
                        jsonpCallback: "addTaskCallback",
                        success: function (data) {
                            // Saved
                        },
                        error: function (e) {
                            alert(e.message);
                        }
                    });

                    $(this).dialog("close");
                }
            },
            {
                text: "Cancel", click: function () {
                    $(this).dialog('close');
                }
            }
        ]
    });

}

// If task edited and saved, update the task portlet to reflect any changes
function saveTaskCallback(task) {
    //var prevStatus = $('#task_' + task.Id)[0].parentElement.id.split('_')[1];
    //if (prevStatus != task.Status) {
    //    $('#task_' + task.Id).remove();
    //    $('#divStatus_' + task.Status).append(getPortlet(task));
    //} else {
    //    $('#task_' + task.Id).replaceWith(getPortlet(task));
    //}
    debugger;
    var prevStatus = $('#task_' + task.task[0].id)[0].parentElement.id.split('_')[1];
    if (prevStatus != task.task[0].status_id) {
        $('#task_' + task.task[0].id).remove();
        $('#divStatus_' + task.task[0].status_id).append(getPortlet(task.task[0]));
    } else {
        $('#task_' + task.task[0].id).replaceWith(getPortlet(task.task[0]));
    }

    // When the portlet is redrawn, need to format the edit button again,
    // otherwise it will revert to simple text without icon
   // $('#task_' + task.Id + ' .editButton').button({ icons: { primary: "ui-icon-pencil" }, text: false });
    $('#task_' + task.task[0].id + ' .editButton').button({ icons: { primary: "ui-icon-pencil" }, text: false });

    // When portlet is redrawn, the +/- icon in the upper right
    // corner disappears, so need to add that back as well
    restorePlusMinusIcon(task);
}

// If task added and saved, update the task portlet to reflect any changes
// TODO: need add task to return all data for task just added in same format at sp_get_task
function addTaskCallback(task) {
    //debugger;
    $('#divStatus_' + task.task[0].status_id).append(getPortlet(task.task[0]));

    $('#task_' + task.task[0].id + ' .editButton').button({ icons: { primary: "ui-icon-pencil" }, text: false });

    restorePlusMinusIcon(task.task[0]);
}

// Add back that pesky +/- icon
function restorePlusMinusIcon(task) {
    // Set the +/- icons on the portlet just moved
    $("#task_" + task.Id)
      .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
      .find(".portlet-header")
        .addClass("ui-widget-header ui-corner-all")
        .prepend("<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

    $("#task_" + task.Id + " .portlet-toggle").click(function () {
        var icon = $(this);
        icon.toggleClass("ui-icon-minusthick ui-icon-plusthick");
        icon.closest(".portlet").find(".portlet-content").toggle();
    });
}

// Display a single task (aka "portlet" in jQuery sortable lingo)
//function getPortlet(task) {

//    var portletHtml = '<div class="portlet" id="task_'
//            + task.Id + '" style="xpadding: 0px;"><div class="portlet-header" id="header_' + task.Id + '" style="background-color: ' + task.BackColor + '; color: ' + task.ForeColor + ';">'
//            + task.Name + ' ('
//            + task.Id + ')</div><div id="content_'
//            + task.Id + '" class="portlet-content">'

//            // Body of task goes here:
//            + '<table cellpadding="3" cellpadding="3" width="100%" style="padding-bottom: 10px;">'
//            + '<tr><td>Project:</td><td>' + task.ProjectName + '</td></tr>'
//            + '<tr><td>Est Hours:</td><td>' + task.EstHours + '</td></tr>'
//            + '<tr><td>Assignees:</td><td>' + task.AssignedTo + '</td></tr>'
//            + '<tr><td>Approvers:</td><td>' + (task.ApprovedBy != undefined ? task.ApprovedBy : '?') + '</td></tr>';

//    if (task.DefineDone != null && task.DefineDone != undefined && task.DefineDone != '') {
//        portletHtml += '<tr><td colspan="2">Define Done:</td></tr>'
//        + '<tr><td colspan="2" style="border: solid silver 1px; background-color: #F8F8F4; padding: 8px;">' + task.DefineDone.replace(/\\n/g, "<br/>") + '</td></tr>'
//    }

//    portletHtml += '</table>'
//            + '<div style="width: 100%; text-align: right;"><a href=\'javascript:editTask(' + task.Id + ');\' class="editButton">Edit</a></div>'
//            + '</div></div>';

//    return portletHtml;
//}
function getPortlet(task) {

    var portletHtml = '<div class="portlet" id="task_'
            + task.id + '" style="xpadding: 0px;"><div class="portlet-header" id="header_' + task.id + '" style="background-color: ' + task.back_color + '; color: ' + task.fore_color + ';">'
            + task.name + ' ('
            + task.id + ')</div><div id="content_'
            + task.id + '" class="portlet-content">'

            // Body of task goes here:
            + '<table cellpadding="3" cellpadding="3" width="100%" style="padding-bottom: 10px;">'
            + '<tr><td>Project:</td><td>' + task.project_name + '</td></tr>'
            + '<tr><td>Est Hours:</td><td>' + task.est_hours + '</td></tr>'
            + '<tr><td>Assignees:</td><td>' + task.assigned_to + '</td></tr>'
            + '<tr><td>Approvers:</td><td>' + (task.approved_by != undefined ? task.approved_by : '?') + '</td></tr>';

    if (task.define_done != null && task.define_done != undefined && task.define_done != '') {
        portletHtml += '<tr><td colspan="2">Define Done:</td></tr>'
        + '<tr><td colspan="2" style="border: solid silver 1px; background-color: #F8F8F4; padding: 8px;">' + task.define_done.replace(/\\n/g, "<br/>") + '</td></tr>'
    }

    portletHtml += '</table>'
            + '<div style="width: 100%; text-align: right;"><a href=\'javascript:editTask(' + task.id + ');\' class="editButton">Edit</a></div>'
            + '</div></div>';

    return portletHtml;
}

// In future, may do something more creative with these filters,
// for right now, just redisplay the board
function filterBySprint() {
    displayBoard();
}

function filterByProject() {
    displayBoard();
}

function filterByAssignee() {
    displayBoard();
}

function filterByApprover() {
    displayBoard();
}

function filterByText() {
    displayBoard();
}

function formatDate(date) {
    return date.split('-')[1] + '/' + date.split('-')[2].split('T')[0] + '/' + date.split('-')[0];
}

// Extract named querystring parameter
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
var pageNumber = 1;
var pageSize = 10;
var startCount = 0;
var endCount = 0;

$(function () {
    GetEmployeesList();
});

$("#searchEmployee").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        GetEmployeesList();
    }
});

//#region CRUD Function
function GetEmployeesList(pageNo = 0) {
    if (pageNo != 0) pageNumber = pageNo;
    var data = new FormData();
    data.append("pageNo", Number(pageNumber));
    data.append("pageSize", pageSize);
    data.append("search", $("#searchEmployee").val().trim());

    $.ajax({
        url: `employee/Pagination`,
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.success === true) {
                $("#employeeRecordBody").html(data.html);
                $(".custom-pagination").html(data.paginationHtml);
                $("#startValue_Employee").html(data.startValuesCount);
                $("#endValue_Employee").html(data.endValuesCount);
                $("#totalRecord_Employee").html(data.totalRecords);
                startCount = data.startValuesCount;
                endCount = data.endValuesCount;
            }
        },
        error: function (data) {
            toastr.error(data.error);
        }
    });
}

function saveEmployee() {
    if (!validateForm()) return;

    var id = Number($("#hfEmployeeId").val());
    let name = $("#txtEmployeeName").val();
    let email = $("#txtEmployeeEmail").val();
    let department = $("#txtEmployeeDept").val();
    let dob = $("#txtEmployeeDob").val();

    var data = new FormData();
    data.append("Id", id);
    data.append("Name", name);
    data.append("Email", email);
    data.append("Department", department);
    data.append("DateOfBirth", dob);

    $.ajax({
        url: "employee/Save",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.success === true) {
                $("#employeeModal").modal('hide');
                GetEmployeesList();
                toastr.success(`Record ${id > 0 ? "Updated" : "Saved"} Successfully.`);
            }
            else {
                toastr.error(data.error);
            }
        },
        error: function (data) {
            toastr.error(data.error);
        }
    });
}

function addEmployee() {
    $.ajax({
        url: "employee/add",
        success: function (data) {
            if (data.success === true) {
                $("#customModal").html(data.html);
                $("#employeeModal").modal('show');
            }
        },
        error: function (data) {
            toastr.error(data.error);
        }
    });
}

function editEmployee(id) {
    $.ajax({
        url: `employee/edit/${id}`,
        success: function (data) {
            if (data.success === true) {
                $("#customModal").html(data.html);
                $("#employeeModal").modal('show');
            }
        },
        error: function (data) {
            toastr.error(data.error);
        }
    });
}
function addDeleteConfirm(id) {
    $.ajax({
        url: `base/addconfirmdelete?method=deleteEmployee()`,
        success: function (html) {
            $("#addConfirmModel").html(html);
            $("#deleteConfirm").modal('show');
            $("#deleteRecordId").val(id);
        },
        error: function (data) {
            toastr.error(data.error);
        }
    });
}
function deleteEmployee() {
    var id = $("#deleteRecordId").val();
    $.ajax({
        url: `employee/delete?id=${id}`,
        type: "delete",
        success: function (data) {
            $("#deleteConfirm").modal('hide');
            if (data.success === true) {
                if (startCount == endCount) pageNumber = pageNumber - 1;
                if (pageNumber < 0) pageNumber = 1;
                GetEmployeesList();
                toastr.success(`Record Deleted Successfully.`);
            }
            else {
                toastr.error(data.error);
            }
        },
        error: function (data) {
            toastr.error(data.error);
        }
    });
}
//#endregion CRUD Function

//#region Form Validation
function validateForm() {
    var isValidStep = true;

    var isValidField = validateEmptyField("txtEmployeeName", "errorName");
    if (isValidStep && !isValidField) isValidStep = false;

    isValidField = validateEmailField("txtEmployeeEmail", "errorEmail", "errorEmailValid");
    if (isValidStep && !isValidField) isValidStep = false;

    isValidField = validateEmptyField("txtEmployeeDept", "errorDepartment");
    if (isValidStep && !isValidField) isValidStep = false;

    isValidField = validateEmptyField("txtEmployeeDob", "errorDOB");
    if (isValidStep && !isValidField) isValidStep = false;

    return isValidStep;
}

function validateEmptyField(fieldId, errorSpanId) {
    if ($("#" + fieldId).val().length == 0 && $("#" + fieldId).val().trim().length == 0) {
        $("#" + errorSpanId).removeClass("d-none");
        return false;
    }
    else {
        $("#" + errorSpanId).addClass("d-none");
        return true;
    }
}

function validateEmailField(fieldId, requiredErrorId, inValidErrorId) {
    var email = $("#" + fieldId).val();
    if (email.length == 0 && email.trim().length == 0) {
        if (requiredErrorId && requiredErrorId.length > 0) {
            $("#" + requiredErrorId).removeClass("d-none");
        }
        if (inValidErrorId && inValidErrorId.length > 0) {
            $("#" + inValidErrorId).addClass("d-none");
        };
        return false;
    }
    else if (!validateEmail(email)) {
        if (requiredErrorId && requiredErrorId.length > 0) {
            $("#" + requiredErrorId).addClass("d-none");
        }
        if (inValidErrorId && inValidErrorId.length > 0) {
            $("#" + inValidErrorId).removeClass("d-none");
        }
        return false;
    }
    else {
        if (requiredErrorId && requiredErrorId.length > 0) {
            $("#" + requiredErrorId).addClass("d-none");
        }
        if (inValidErrorId && inValidErrorId.length > 0) {
            $("#" + inValidErrorId).addClass("d-none");
        }
        return true;
    }
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email.trim()).toLowerCase());
}

//#endregion Form Validation
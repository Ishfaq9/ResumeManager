// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    $(document).on('input', 'input[name$=".YearsWorked"]', function () {
        updateTotalYears();
        });




    function DeleteItem(btn) {
            
            var table = document.getElementById('ExpTable');
    var rows = table.getElementsByTagName('tr');
    if(rows.length==2)
    {
        alert("This row can not be deleted ! ");
    return;
            }
    $(btn).closest('tr').remove();
        updateTotalYears();


        var btnIdx = btn.id.replaceAll('btnremove-', '');

        var idofIsDeleted = btnIdx + "_IsDeleted";

        var hidIsDelId = document.querySelector("[id$='" + idofIsDeleted + "']").id;

        document.getElementById(hidIsDelId).value = "true";

        //$(btn).closest("tr").remove();

        $(btn).closest('tr').hide();


        }

    function updateTotalYears() {

            var sum = 0;

    $("#ExpTable tbody tr").each(function () {

                //console.log(parseFloat($(this).find("input[name$='.YearsWorked']").val()));
                var yearsWorked = parseInt($(this).find("input[name$='.YearsWorked']").val());
    sum += yearsWorked;
                
            });

    $("#totalYears").html(sum);
    document.getElementById('TotalExperience').value = sum;


        }

    function AddItem(btn) {
         
    // var table = document.getElementById('ExpTable');
    // var rows = table.getElementsByTagName('tr');

    // var rowOuterHtml = rows[rows.length - 1].outerHTML;

    // var lastrowIdx = document.getElementById('hdnLastIndex').value;

    // var nextrowIdx = eval(lastrowIdx) + 1;

    // document.getElementById('hdnLastIndex').value = nextrowIdx;

    // rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
    // rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
    // rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);


    // var newRow = table.insertRow();
    // newRow.innerHTML = rowOuterHtml;            

    var table = document.getElementById('ExpTable');
    var rows = table.getElementsByTagName('tr');
    var rowOuterHtml = table.rows[table.rows.length - 1].outerHTML;
    var lastrowIdx = document.getElementById('hdnLastIndex').value;
    //var lastrowIdx = rows.length - 2;
    var nextrowIdx = eval(lastrowIdx) + 1;

    document.getElementById('hdnLastIndex').value = nextrowIdx;

    var rowOuterHtml = table.rows[table.rows.length - 1].outerHTML;
    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
    rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
    rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);


    var newRow = table.insertRow();
    newRow.innerHTML = rowOuterHtml;


    updateTotalYears();
    // var totalYears = 0;
    // var yearsInputs = document.querySelectorAll('input[name$=".YearsWorked"]');
    // yearsInputs.forEach(input => totalYears += parseInt(input.value));
    // document.getElementById('totalYears').textContent = totalYears;


    // var btnAddID = btn.id;
    // var btnDeleteid = btnAddID.replaceAll('btnadd', 'btnremove');

    // var delbtn = document.getElementById(btnDeleteid);
    // delbtn.classList.add("visible");
    // delbtn.classList.remove("invisible");


    // var addbtn = document.getElementById(btnAddID);
    // addbtn.classList.remove("visible");
        // addbtn.classList.add("invisible");
        var x = document.getElementsByTagName("Input");
        for (var cnt = 0; cnt < x.length; cnt++) {
            if (x[cnt].type == "text" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > -1) {
                x[cnt].value = '';
            } else if (x[cnt].type == "number" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > -1) {
                x[cnt].value = 0;
            }
        }

    rebindvalidators();

        }

    function rebindvalidators() {
            var $form = $("#ApplicantForm");
    $form.data("validator", null);
    $.validator.unobtrusive.parse($form);
    $form.validate($form.data("unobtrusiveValidation").options);
    }

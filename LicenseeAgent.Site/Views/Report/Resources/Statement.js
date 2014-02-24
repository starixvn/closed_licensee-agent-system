var STATEMENT = function ($) {
    return {
        init: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
            $('#search').click(function () {
                STATEMENT.dataGridBinding();
                return false;
            });
        },
        dataGridBinding: function () {

            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: CORE.resolveUrl("~/Report/GetStatementData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                        $('#grid').show().fanexGrid(gridOption);
                        CORE.unblockUI();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        }
    }
} (jQuery);

$(function () {
    STATEMENT.init();
});
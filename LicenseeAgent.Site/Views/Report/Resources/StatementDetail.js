var STATEMENTDETAIL = function ($) {
    return {
        init: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
            $('#search').click(function () {
                STATEMENTDETAIL.dataGridBinding();
                return false;
            });
            STATEMENTDETAIL.dataGridBinding();
        },
        dataGridBinding: function () {
            if (!$('#Username').val().trim()) {
                alert(LANGUAGE.getString('UsernameRequired'));
                return;
            }
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: CORE.resolveUrl("~/Report/GetStatementDetailData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        gridOption.pagerOption = {
                            itemsPerPage: CORE.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: CORE.pageSizes,
                            totalItem: data.List.length
                        };

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
    STATEMENTDETAIL.init();
});
var STATEMENT = function ($) {
    return {
        init: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
            $('#search').click(function () {
                STATEMENT.dataGridBinding();
                return false;
            });
            STATEMENT.dataGridBinding();
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
        },
        ViewDetail: function (agentId, username) {
            location.href = CORE.resolveUrl("~/Report/StatementDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}&isMember={4}&parentUsername={5}".format(agentId, $('#FromDate').val(), $('#ToDate').val(), username, pageContext.IsLastLevel, $('#Username').val()));
        },
        TransferDetail: function (agentId, username) {
            var url = "Report/TransferDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}".format(agentId, $('#FromDate').val(), $('#ToDate').val(), username);
            POPUP.openDialogFrame(url, { width: 465, height: 400 });
        }
    }
} (jQuery);

$(function () {
    STATEMENT.init();
});
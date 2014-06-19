/*global gridOptions, core, pageContext, popup*/

var statementReport = (function ($) {
    'use strict';
    return {
        init: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
            $('#search').click(function () {
                statementReport.dataGridBinding();
                return false;
            });
            statementReport.dataGridBinding();
        },
        dataGridBinding: function () {
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: core.resolveUrl("~/Report/GetStatementData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        gridOptions.pagerOption = {
                            itemsPerPage: core.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: core.pageSizes,
                            totalItem: data.List.length
                        };

                        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                        $('#grid').show().fanexGrid(gridOptions);
                        core.unblockUI();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        ViewDetail: function (agentId, username) {
            location.href = core.resolveUrl("~/Report/StatementDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}&isMember={4}&parentUsername={5}"
                                            .format(agentId, $('#FromDate').val(), $('#ToDate').val(), username,
                                                    pageContext.IsLastLevel, $('#Username').val()));
        },
        TransferDetail: function (agentId, username) {
            var url = "Report/TransferDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}&isMember={4}"
                      .format(agentId, $('#FromDate').val(), $('#ToDate').val(), username, pageContext.IsLastLevel);
            popup.openFrame(url, {
                width: 550,
                height: 400
            });
        }
    };
})(jQuery);

$(function () {
    'use strict';
    statementReport.init();
});
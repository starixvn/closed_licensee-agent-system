/*global gridOptions, CORE, pageContext, POPUP*/

var statementReport = (function ($) {
    /*jshint strict:false */
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
                url: CORE.resolveUrl("~/Report/GetStatementData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {

                        gridOptions.pagerOption = {
                            itemsPerPage: CORE.pageSizes[2],
                            currentPage: 0,
                            itemsPerPageArray: CORE.pageSizes,
                            totalItem: data.List.length
                        };

                        gridOptions.bodyRows = CORE.genGridRows(data.List, gridOptions);
                        gridOptions.columns[1].Width = 100; //Resize username column
                        $('#grid').show().fanexGrid(gridOptions);
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
            location.href = CORE.resolveUrl("~/Report/StatementDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}&isMember={4}&parentUsername={5}"
                                            .format(agentId, $('#FromDate').val(), $('#ToDate').val(), username,
                                                    pageContext.IsLastLevel, $('#Username').val()));
        },
        TransferDetail: function (agentId, username) {
            var url = "Report/TransferDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}&isMember={4}"
                      .format(agentId, $('#FromDate').val(), $('#ToDate').val(), username, pageContext.IsLastLevel);
            POPUP.openDialogFrame(url, { width: 465, height: 400 });
        }
    };
} (jQuery));

$(function () {
    /*jshint strict:false */
    statementReport.init();
});
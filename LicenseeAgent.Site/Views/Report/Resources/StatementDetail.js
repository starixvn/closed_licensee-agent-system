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
        },
        TransferDetail: function (agentId, username) {
            var url = "Report/TransferDetail?agentId={0}&fromDate={1}&toDate={2}&username={3}".format(agentId, $('#FromDate').val(), $('#ToDate').val(), username);
            POPUP.openDialogFrame(url, { width: 465, height: 400 });
        },
        BetList: function (custId, statusIDs, date) {
            var dateTime = moment(date).format("MM/DD/YYYY");
            var url = "BetList/BetListWinlossReport?custId={0}&fromDate={1}&toDate={2}&statusIDs={3}".format(custId, dateTime, dateTime, statusIDs);
            POPUP.openDialogFrame(url);
        }
    }
} (jQuery);

$(function () {
    STATEMENTDETAIL.init();
});
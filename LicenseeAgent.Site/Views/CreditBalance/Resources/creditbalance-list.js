var creditBalanceList = function ($) {
    var records;

    return {
        init: function () {
            $('.selectpicker').val(1);
            $('.selectpicker').selectpicker('refresh');

            context.init({
                preventDoubleContext: false
            });

            $('#search-form').submit(function (e) {
                creditBalanceList.submit();
                e.preventDefault();
            });

            $('#search-form').submit();
        },
        submit: function () {
            var params = {
                username: $.trim($("#Username").val()),
                status: $("#Status").val()
            };
            $.ajax({
                url: $('#search-form').attr('action'),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode != 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    records = data.List;
                    creditBalanceList.pagerSelectCallBack(0, CORE.pageSizes[2]);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        pagerSelectCallBack: function (pageIndex, itemsPerPage) {
            gridOptions.pagerOption = {
                itemsPerPage: itemsPerPage,
                currentPage: pageIndex,
                totalItem: records.length,
                itemsPerPageArray: CORE.pageSizes,
                showDetail: true,
                customRenderFooter: function (bodyRows, pageIndex, itemsPerPage) {
                    return CORE.genGridTotalRows(bodyRows, gridOptions);
                }
            };
            gridOptions.tdCssClass = function (rowIndex, colName, rowData) {
                if (colName == 'YesterdayBalance' || colName == 'TransferIcon') {
                    return rowData.IsHighlighted ? 'text-highlighted' : '';
                }
            }
            gridOptions.bodyRows = CORE.genGridRows(records, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
        },
        editAgentCredit: function (agentId, agentName, givenCredit) {
            var url = '~/CreditBalance/EditAgentCredit?agentId={0}&agentName={1}&givenCredit={2}'.format(agentId, agentName, givenCredit);
            POPUP.openDialogFrame(url, { width: 428, height: 200 });
        },
        agentEditMemberCredit: function (custId, custName, givenCredit) {
            var url = '~/CreditBalance/AgentEditMemberCredit?custId={0}&memberName={1}&givenCredit={2}'.format(custId, custName, givenCredit);
            POPUP.openDialogFrame(url, { width: 428, height: 200 });
        }
    }
} (jQuery);

$(function () {
    creditBalanceList.init();
});
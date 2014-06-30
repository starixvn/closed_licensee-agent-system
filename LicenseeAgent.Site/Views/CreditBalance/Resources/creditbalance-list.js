/*global popup, gridOptions, core, context*/

var creditBalanceList = (function ($) {
    'use strict';
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
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    records = data.List;
                    creditBalanceList.pagerSelectCallBack(0, core.pageSizes[2]);
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        pagerSelectCallBack: function (pageIndex, itemsPerPage) {
            gridOptions.pagerOption = {
                itemsPerPage: itemsPerPage,
                currentPage: pageIndex,
                totalItem: records.length,
                itemsPerPageArray: core.pageSizes,
                showDetail: true,
                customRenderFooter: function (bodyRows) {
                    return core.genGridTotalRows(bodyRows, gridOptions);
                }
            };

            gridOptions.tdCssClass = function (rowIndex, colName, rowData) {
                if (colName === 'YesterdayBalance' || colName === 'TransferIcon') {
                    return rowData.IsHighlighted ? 'text-highlighted' : '';
                }
            };

            gridOptions.bodyRows = core.genGridRows(records, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
        },
        editAgentCredit: function (agentId) {
            var url = '~/CreditBalance/EditAgentCredit?agentId={0}'.format(agentId);
            popup.openFrame(url);
        },
        agentEditMemberCredit: function (custId) {
            var url = '~/CreditBalance/AgentEditMemberCredit?custId={0}'.format(custId);
            popup.openFrame(url);
        }
    };
} (jQuery));

$(function () {
    'use strict';
    creditBalanceList.init();
});
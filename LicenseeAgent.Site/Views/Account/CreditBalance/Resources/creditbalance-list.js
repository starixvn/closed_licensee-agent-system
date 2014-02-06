var TRANSFER_LIST = function ($) {
    var _pageindex = 0;
    var _itemsperpage = CORE.pageSizes[0];

    return {
        init: function () {
            $('.selectpicker').val(1);
            $('.selectpicker').selectpicker('refresh');

            context.init({
                preventDoubleContext: false
            });
            context.attach('#select-dropdown', [
                { text: LANGUAGE.getString('All'), href: '#', action: TRANSFER_LIST.checkAll },
                { text: LANGUAGE.getString('None'), href: '#', action: TRANSFER_LIST.uncheckAll }
            ]);

            $('#search-form').submit(function (e) {
                TRANSFER_LIST.reRenderAfterPaged();
                e.preventDefault();
            });

            $('#search-form').submit();
        },
        reRenderAfterPaged: function () {
            var params = {
                username: $.trim($("#Username").val()),
                status: $("#Status").val(),
                pageindex: _pageindex,
                itemsperpage: _itemsperpage
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
                    }

                    gridOption.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
                        currentPage: 0,
                        itemsPerPageArray: CORE.pageSizes,
                        showDetail: true,
                        totalItem: data.TotalRecords,
                        pagerSelectCallBack: TRANSFER_LIST.doPaging
                    };

                    gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                    gridOption.footerRows = CORE.genGridFooterRows(data.TotalData, gridOption);
                    $('#grid').show().fanexGrid(gridOption);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        doPaging: function (page_index, itemsPerPage, jq) {
            gridOption.pagerOption.itemsPerPage = itemsPerPage;
            gridOption.pagerOption.currentPage = page_index;
            _pageindex = page_index;
            _itemsperpage = itemsPerPage;

            var params = {
                username: $.trim($("#Username").val()),
                status: $("#Status").val(),
                pageindex: _pageindex,
                itemsperpage: _itemsperpage
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
                    }

                    gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                    gridOption.footerRows = CORE.genGridFooterRows(data.TotalData, gridOption);
                    $('#grid').show().fanexGrid(gridOption);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });

            return false;
        },
        reload: function () {
            var params = {
                username: $.trim($("#Username").val()),
                status: $("#Status").val(),
                pageindex: _pageindex,
                itemsperpage: _itemsperpage
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
                    }

                    gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                    gridOption.footerRows = CORE.genGridFooterRows(data.TotalData, gridOption);
                    $('#grid').show().fanexGrid(gridOption);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        editAgentCredit: function (agentId, agentName, givenCredit) {
            var url = '~/CreditBalance/EditAgentCredit?agentId={0}&agentName={1}&givenCredit={2}'.format(agentId, agentName, givenCredit);
            POPUP.openDialogFrame(url, { width: 428, height: 200 });
        },
        agentEditMemberCredit: function (custId, custName, givenCredit) {
            var url = '~/CreditBalance/AgentEditMemberCredit?custId={0}&custName={1}&givenCredit={2}'.format(custId, custName, givenCredit);
            POPUP.openDialogFrame(url, { width: 428, height: 200 });
        }
    }
} (jQuery);

$(function () {
    TRANSFER_LIST.init();
});
var TRANSFER_LIST = function ($) {
    return {
        init: function () {
            $('.selectpicker').selectpicker();

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

                    gridOption.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
                        currentPage: 0,
                        itemsPerPageArray: CORE.pageSizes,
                        showDetail: true
                    };
                    gridOption.tdCssClass = function (rowIndex, colName, rowData) {
                        if (colName == 'YesterdayBalance' || colName == 'TransferIcon') {
                            return rowData.IsHighlighted ? 'text-highlighted' : '';
                        }
                    }

                    gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                    gridOption.footerRows = CORE.genFooterRows(data.Total, gridOption);
                    $('#grid').show().fanexGrid(gridOption);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        toggleCheck: function (target) {
            if ($(target).hasClass("icon-uncheck-all")) {
                TRANSFER_LIST.checkAll();
            }
            else {
                TRANSFER_LIST.uncheckAll();
            }
        },
        checkAll: function () {
            $(".icon-uncheck-all").attr("class", "icon-checked-all");
            $(".icon-uncheck").attr("class", "icon-checked");
        },
        uncheckAll: function () {
            $(".icon-checked-all").attr("class", "icon-uncheck-all");
            $(".icon-checked").attr("class", "icon-uncheck");
        },
        transferAgent: function (agentId, agentName, amount) {
            var url = '~/Transfer/Agent?agentId={0}&agentName={1}&amount={2}'.format(agentId, agentName, amount);
            POPUP.openDialogFrame(url);
        },
        transferMember: function (memberId, amount) {
            var url = '~/Transfer/Member?memberId={0}&amount={1}'.format(memberId, amount);
            POPUP.openDialogFrame(url, { width: 480, height: 300 });
        }
    }
} (jQuery);

$(function () {
    TRANSFER_LIST.init();
});
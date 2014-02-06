var TRANSFER_LIST = function ($) {
    var records;
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
                TRANSFER_LIST.submit();
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
                    TRANSFER_LIST.pagerSelectCallBack(0, CORE.pageSizes[2]);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        pagerSelectCallBack: function (pageIndex, itemsPerPage) {
            gridOption.pagerOption = {
                itemsPerPage: itemsPerPage,
                currentPage: pageIndex,
                totalItem: records.length,
                itemsPerPageArray: CORE.pageSizes,
                showDetail: true,
                pagerSelectCallBack: function (pageIndex, itemsPerPage) {
                    TRANSFER_LIST.pagerSelectCallBack(pageIndex, itemsPerPage);
                }
            };
            gridOption.tdCssClass = function (rowIndex, colName, rowData) {
                if (colName == 'YesterdayBalance' || colName == 'TransferIcon') {
                    return rowData.IsHighlighted ? 'text-highlighted' : '';
                }
            }

            if (gridOption.bodyRows) {
                var startIndex = pageIndex * itemsPerPage;
                var endIndex = (pageIndex + 1) * itemsPerPage;
                var newRecords = records.slice(startIndex, endIndex);
                gridOption.bodyRows = CORE.genGridRows(newRecords, gridOption);
                gridOption.footerRows = CORE.genTotalRows(newRecords, gridOption);
            }
            else {
                gridOption.bodyRows = CORE.genGridRows(records, gridOption);
                gridOption.footerRows = CORE.genTotalRows(records, gridOption, itemsPerPage);
            }
            
            $('#grid').show().fanexGrid(gridOption);
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
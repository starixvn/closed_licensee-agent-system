/*global popup, gridOptions, context, core, language*/

var transferList = (function ($) {
    'use strict';
    var records;
    return {
        init: function () {
            $('.selectpicker').selectpicker();

            context.init({
                preventDoubleContext: false
            });
            context.attach('#select-dropdown', [
                { text: language.getString('All'), href: '#', action: transferList.checkAll },
                { text: language.getString('None'), href: '#', action: transferList.uncheckAll }
            ]);

            $('#search-form').submit(function (e) {
                transferList.submit();
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
                    transferList.pagerSelectCallBack(0, core.pageSizes[2]);
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
        toggleCheck: function (target) {
            if ($(target).hasClass("icon-uncheck")) {
                $(target).attr("class", "icon-checked");
            }
            else {
                $(target).attr("class", "icon-uncheck");
            }

            if ($('.icon-uncheck').length === 0) {
                $(".icon-uncheck-all").attr("class", "icon-checked-all");
            }
            else {
                $(".icon-checked-all").attr("class", "icon-uncheck-all");
            }
        },
        toggleAll: function (target) {
            if ($(target).hasClass("icon-uncheck-all")) {
                transferList.checkAll();
            }
            else {
                transferList.uncheckAll();
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
        transferAgent: function (agentId) {
            var url = '~/Transfer/Agent?agentId={0}'.format(agentId);
            popup.openFrame(url);
        },
        transferMember: function (memberId) {
            var url = '~/Transfer/Member?memberId={0}'.format(memberId);
            popup.openFrame(url);
        }
    };
} (jQuery));

$(function () {
    'use strict';
    transferList.init();
});
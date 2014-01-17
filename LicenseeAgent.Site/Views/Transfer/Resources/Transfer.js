var TRANSFER = function ($) {
    return {
        init: function () {
            $('.selectpicker').selectpicker();

            context.init({
                preventDoubleContext: false
            });
            context.attach('#select-dropdown', [
                { text: LANGUAGE.getString('All'), href: '#', action: TRANSFER.checkAll },
                { text: LANGUAGE.getString('None'), href: '#', action: TRANSFER.uncheckAll }
            ]);

            $('#search-form').submit(function (e) {
                TRANSFER.reRenderAfterPaged();
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
                    }

                    gridOption.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
                        currentPage: 0,
                        itemsPerPageArray: CORE.pageSizes,
                        showDetail: true
                    };

                    gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                    $('#grid').show().fanexGrid(gridOption);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        toggleCheck: function (target) {
            if ($(target).hasClass("icon-uncheck-all")) {
                TRANSFER.checkAll();
            }
            else {
                TRANSFER.uncheckAll();
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
            POPUP.openDialogFrame(url, { width: 428, height: 200 });
        }
    }
} (jQuery);

$(function () {
    TRANSFER.init();
});
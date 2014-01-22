var TRANSFER_LIST = function ($) {
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
        licenseeEditAgentCredit: function (agentId, agentName, givenCredit) {
            var url = '~/CreditBalance/LicenseeEditAgentCredit?agentId={0}&agentName={1}&givenCredit={2}'.format(agentId, agentName, givenCredit);
            POPUP.openDialogFrame(url, { width: 428, height: 200 });
        }
    }
} (jQuery);

$(function () {
    TRANSFER_LIST.init();
});
var resetPassword = function ($) {
    return {
        init: function () {
        },
        updatePassword: function () {
            var params = {
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                leagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: CORE.resolveUrl("~/Customer/ResetPassword"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    gridOption.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
                        currentPage: 0,
                        itemsPerPageArray: CORE.pageSizes,
                        showDetail: true,
                        afterPagedFunction: function () {
                            customerList.gridAfterBinding();
                        }
                    };

                    gridOption.bodyRows = CORE.genGridRows(data.List, gridOption);
                    $('#grid').show().fanexGrid(gridOption);

                    customerList.gridAfterBinding();
                    customerList.resizeBox();
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        }
    }
} (jQuery);
$(function () {
    resetPassword.init();
});
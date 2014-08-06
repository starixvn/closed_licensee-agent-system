/*global core, language*/
var pendingMemberList = (function ($) {
    'use strict';
    var gridOptions = {
        columns: [
            { HeaderText: '#', Width: 50, Name: "No", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString('Username'), Width: 300, HeaderAlign: "center", Name: "Username", CellAlign: "left" }
        ],
        showPager: true,
        pagerOption: {
            itemsPerPage: core.pageSizes[2],
            currentPage: 0,
            itemsPerPageArray: core.pageSizes,
            showDetail: true
        },
        indexNumberColumn: 0,
        autoResizeColumn: true
    };
    return {
        init: function () {
            pendingMemberList.hideGrid();

            $('form').submit(function (e) {
                if ($(this).valid()) {
                    core.hideErrors();
                    pendingMemberList.submit();
                } else {
                    pendingMemberList.hideGrid();
                }
                e.preventDefault();
            });

            if ($('#DirectAgentUserName').length === 0) {
                pendingMemberList.submit();
            }
        },
        hideGrid: function () {
            $('#grid').hide();
        },
        submit: function () {
            $.ajax({
                url: $('form').attr('action'),
                type: 'post',
                data: $('form').serializeObject(),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        core.showErrorMsg(data.ErrMsg);
                        pendingMemberList.hideGrid();
                        return;
                    }
                    gridOptions.pagerOption.currentPage = 0;
                    gridOptions.bodyRows = core.genGridRows(data.Data, gridOptions);
                    $('#grid').show().fanexGrid(gridOptions);
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        }
    };
} (jQuery));

$(function () {
    'use strict';
    pendingMemberList.init();
});
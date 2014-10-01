/*global core, language*/
var customerList = customerList || {};
customerList.pendingMember = (function ($) {
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
            customerList.pendingMember.hideGrid();

            $('form').submit(function (e) {
                if ($(this).valid()) {
                    customerList.pendingMember.submit();
                } else {
                    customerList.pendingMember.hideGrid();
                }
                e.preventDefault();
            });

            if ($('#DirectAgentUserName').length === 0) {
                customerList.pendingMember.submit();
            }
        },
        hideGrid: function () {
            $('#grid').hide();
        },
        submit: function () {
            core.ajaxJSON({
                url: $('form').attr('action'),
                data: $('form').serializeObject()
            }, function (data) {
                gridOptions.pagerOption.currentPage = 0;
                gridOptions.bodyRows = core.genGridRows(data.Data, gridOptions);
                $('#grid').show().fanexGrid(gridOptions);
            }, function (data, errorHandler) {
                errorHandler(data);
                customerList.pendingMember.hideGrid();
            });
        }
    };
} (jQuery));
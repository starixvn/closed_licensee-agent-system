/*global core, language, pageContext*/
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
                var url = $(this).attr('action');
                if ($(this).find('#AgentUserName').length > 0) {
                    if (!$('#AgentUserName').val().trim()) {
                        core.showErrorMsg(language.getString('AgentUserNameIsEmpty'));
                        customerList.pendingMember.hideGrid();
                    }
                    else {
                        customerList.pendingMember.submit(url);
                    }
                } else {
                    customerList.pendingMember.submit(url);
                }
                e.preventDefault();
            });

            $('a[data-toggle="tab"]').on('shown', function (e) {
                core.hideErrors();
                if ($(e.target).attr('href') === "#direct-agent") {
                    customerList.pendingMember.hideGrid();
                    $('#directMember').submit();
                }
                else {
                    customerList.pendingMember.hideGrid();
                }
            });

            if (!pageContext.IsAdmin) {
                $('#directMember').submit();
            }
        },
        hideGrid: function () {
            $('#grid').hide();
        },
        submit: function (url) {
            core.ajaxJSON({
                url: url,
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
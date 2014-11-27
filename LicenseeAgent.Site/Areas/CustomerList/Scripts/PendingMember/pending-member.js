/*global core, language, pageContext*/
'use strict';

var customerList = customerList || {};
customerList.pendingMember = (function ($this, $) {
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

    var hideGrid = function () {
        $('#grid').hide();
    };

    var submit = function (url) {
        core.ajaxJSON({
            url: url,
            data: $('form').serializeObject()
        }, function (data) {
            gridOptions.pagerOption.currentPage = 0;
            gridOptions.bodyRows = core.genGridRows(data.Data, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
        }, function (data, errorHandler) {
            errorHandler(data);
            hideGrid();
        });
    };

    $this.init = function () {
        hideGrid();

        $('form').submit(function (e) {
            var url = $(this).attr('action');
            if ($(this).find('#AgentUserName').length > 0) {
                if (!$('#AgentUserName').val().trim()) {
                    core.showErrorMsg(language.getString('AgentUserNameIsEmpty'));
                    hideGrid();
                }
                else {
                    submit(url);
                }
            } else {
                submit(url);
            }
            e.preventDefault();
        });

        $('a[data-toggle="tab"]').on('shown', function (e) {
            core.hideErrors();
            if ($(e.target).attr('href') === "#direct-agent") {
                hideGrid();
                $('#directMember').submit();
            }
            else {
                hideGrid();
            }
        });

        if (!pageContext.IsAdmin) {
            $('#directMember').submit();
        }
    };

    return $this;
} (customerList.pendingMember || {}, jQuery));
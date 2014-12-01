/*global CORE, LANGUAGE, GRID, pageContext*/
'use strict';

var CUSTOMER_LIST = CUSTOMER_LIST || {};
CUSTOMER_LIST.PENDING_MEMBER_LIST = (function ($this, $) {
    var gridOptions = {
        columns: [
            { HeaderText: '#', Width: 50, Name: "No", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString('Username'), Width: 300, HeaderAlign: "center", Name: "Username", CellAlign: "left" }
        ],
        showPager: true,
        pagerOption: {
            itemsPerPage: GRID.PageSizes[2],
            currentPage: 0,
            itemsPerPageArray: GRID.PageSizes,
            showDetail: true
        },
        showNumberColumn: true,
        autoResizeColumn: true
    };

    var hideGrid = function () {
        $('#grid').hide();
    };

    var submit = function (url) {
        CORE.ajaxJSON({
            url: url,
            data: $('form').serializeObject()
        }, function (data) {
            gridOptions.pagerOption.currentPage = 0;
            gridOptions.bodyRows = GRID.generateBodyRows(data.Data, gridOptions);
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
                    CORE.showErrorMsg(LANGUAGE.getString('AgentUserNameIsEmpty'));
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
            CORE.hideErrors();
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
} (CUSTOMER_LIST.PENDING_MEMBER_LIST || {}, jQuery));
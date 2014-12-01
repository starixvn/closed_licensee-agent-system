/*global CORE, GRID*/
/*exported MESSAGE*/
'use strict';

var MESSAGE = (function ($this, $) {
    var MAX_ROW_PER_PAGE = 1;
    var ITEMS_PER_PAGE_ARRAY = [1, 2, 3, 4];

    var storageParams = {
        PageIndex: 0,
        PageSize: MAX_ROW_PER_PAGE
    };

    var showAnnouncements = function (messages) {
        var gridOptions = {
            columns: [
                { HeaderText: "#", Name: "No", Width: 30, HeaderAlign: "center", CellAlign: "center", CssClass: 'bold' },
                { HeaderText: "ID", Name: "ID", Width: 50, HeaderAlign: "center", CellAlign: "center", CssClass: 'bold' },
                { HeaderText: "Post Date", Name: "PostDate", Width: 70, HeaderAlign: "center", CellAlign: "center", CssClass: 'bold' },
                { HeaderText: "Message", Name: "Message", Width: 822, HeaderAlign: "center", CellAlign: "center", CssClass: 'bold' }
            ],
            showTree: true,
            hideYBackGroundScroller: true,
            fullHeight: false,
            gridHeight: 600,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: storageParams.PageSize,
                currentPage: storageParams.PageIndex,
                itemsPerPageArray: ITEMS_PER_PAGE_ARRAY,
                pagerSelectCallBack: function (pageIndex, pageSize) {
                    renderPagingEvent(pageIndex, pageSize);
                }
            },
            marginRight: 0
        };

        if (messages != null && messages.length > 0) {
            gridOptions.pagerOption.totalItem = messages.length - 1;
            gridOptions.bodyRows = GRID.generateBodyRows(messages, gridOptions);
        }
        $('#MessageArea').fanexGrid(gridOptions);
    };

    var renderPagingEvent = function (pageIndex, pageSize) {
        storageParams.PageIndex = pageIndex;
        storageParams.PageSize = pageSize;
        showAnnouncements();
        return false;
    };

    var getMessage = function (disabled) {
        $("#search-form :input").attr("disabled", disabled);
        var type = "0";
        if ($(".tab-special").hasClass("active")) {
            type = "1";
        }
        else if ($(".tab-personal").hasClass("active")) {
            type = "2";
        }
        showAnnouncements(null);
        CORE.ajaxJSON({
            url: CORE.resolveUrl("~/Message/GetMessage")
        }, function () {
        });
    };

    $this.init = function () {
        $('.datepicker').datepicker();

        $('li.tab-public').click(function () {
            $("#tabs-area a:first").tab('show');
            getMessage(false);
        });

        $('li.tab-special').click(function () {
            $("#tabs-area a").eq(1).tab('show');
            getMessage(true);
        });

        $('li.tab-personal').click(function () {
            $("#tabs-area a:last").tab('show');
            getMessage(false);
        });

        showAnnouncements(null);
    };

    return $this;
} (MESSAGE || {}, jQuery));
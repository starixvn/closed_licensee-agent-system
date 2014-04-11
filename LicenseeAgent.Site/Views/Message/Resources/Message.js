/*global core*/

var Message = (function ($) {
    'use strict';
    var MAX_ROW_PER_PAGE = 1;
    var ITEMS_PER_PAGE_ARRAY = [1, 2, 3, 4];

    return {
        storageParams: {
            PageIndex: 0,
            PageSize: MAX_ROW_PER_PAGE
        },
        initialize: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
            Message.initData();
        },
        initData: function () {
            $('li.tab-public').click(function () {
                $("#tabs-area a:first").tab('show');
                Message.getMessage(false);
            });
            $('li.tab-special').click(function () {
                $("#tabs-area a").eq(1).tab('show');
                Message.getMessage(true);
            });
            $('li.tab-personal').click(function () {
                $("#tabs-area a:last").tab('show');
                Message.getMessage(false);
            });
            Message.showAnnouncements(null);
        },
        getMessage: function (disabled) {
            $("#search-form :input").attr("disabled", disabled);
            var type = "0";
            if ($(".tab-special").hasClass("active")) {
                type = "1";
            }
            else if ($(".tab-personal").hasClass("active")) {
                type = "2";
            }
            Message.showAnnouncements(null);
            //            $.ajax({
            //                url: core.resolveUrl("~/Message/GetMessage"),
            //                type: 'POST',
            //                data: params,
            //                dataType: "json",
            //                beforeSend: function () {
            //                    core.blockUI();
            //                },
            //                success: function (data) {
            //                },
            //                complete: function () {
            //                    core.unblockUI();
            //                }
            //            });
        },
        showAnnouncements: function (messages) {
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
                    itemsPerPage: Message.storageParams.PageSize,
                    currentPage: Message.storageParams.PageIndex,
                    itemsPerPageArray: ITEMS_PER_PAGE_ARRAY,
                    pagerSelectCallBack: function (pageIndex, pageSize) {
                        Message.renderPagingEvent(pageIndex, pageSize);
                    }
                },
                marginRight: 0
            };

            if (messages != null && messages.length > 0) {
                gridOptions.pagerOption.totalItem = messages.length - 1;
                gridOptions.bodyRows = core.genGridRows(messages, gridOptions);
            }
            $('#MessageArea').fanexGrid(gridOptions);
        },
        renderPagingEvent: function (pageIndex, pageSize) {
            Message.storageParams.PageIndex = pageIndex;
            Message.storageParams.PageSize = pageSize;

            Message.showAnnouncements();

            return false;
        }
    };
}) (jQuery);

$(function () {
    'use strict';
    Message.initialize();
});
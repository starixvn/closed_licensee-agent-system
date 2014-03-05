var MESSAGE = (function ($) {
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
            MESSAGE.initData();
        },
        initData: function () {
            $('li.tab-public').click(function () {
                $("#tabs-area a:first").tab('show');
                MESSAGE.getMessage(false);
            });
            $('li.tab-special').click(function () {
                $("#tabs-area a").eq(1).tab('show');
                MESSAGE.getMessage(true);
            });
            $('li.tab-personal').click(function () {
                $("#tabs-area a:last").tab('show');
                MESSAGE.getMessage(false);
            });
            MESSAGE.showAnnouncements(null);
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

            var params = {
                FromDate: $("#FromDate").val(),
                ToDate: $("#ToDate").val(),
                Type: type
            };
            MESSAGE.showAnnouncements(null);
            //            $.ajax({
            //                url: CORE.resolveUrl("~/Message/GetMessage"),
            //                type: 'POST',
            //                data: params,
            //                dataType: "json",
            //                beforeSend: function () {
            //                    CORE.blockUI();
            //                },
            //                success: function (data) {
            //                },
            //                complete: function () {
            //                    CORE.unblockUI();
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
                    itemsPerPage: MESSAGE.storageParams.PageSize,
                    currentPage: MESSAGE.storageParams.PageIndex,
                    itemsPerPageArray: ITEMS_PER_PAGE_ARRAY,
                    pagerSelectCallBack: function (pageIndex, pageSize) {
                        MESSAGE.renderPagingEvent(pageIndex, pageSize);
                    }
                },
                marginRight: 0
            };

            if (messages != null && messages.length > 0) {
                gridOptions.pagerOption.totalItem = messages.length - 1;
                gridOptions.bodyRows = CORE.genGridRows(messages, gridOptions);
            }
            $('#MessageArea').fanexGrid(gridOptions);
        },
        renderPagingEvent: function (pageIndex, pageSize) {
            MESSAGE.storageParams.PageIndex = pageIndex;
            MESSAGE.storageParams.PageSize = pageSize;

            MESSAGE.showAnnouncements();

            return false;
        }
    }
} (jQuery));
$(function () {
    MESSAGE.initialize();
});
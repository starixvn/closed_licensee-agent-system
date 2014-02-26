var MESSAGE = (function ($) {
    return {
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
        }
    }
} (jQuery));
$(function () {
    MESSAGE.initialize();
});
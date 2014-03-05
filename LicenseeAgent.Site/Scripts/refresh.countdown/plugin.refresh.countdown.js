var refreshCountDownMethods = function ($) {
    var timeInterval = $("#time-interval").val();
    var timer = null;

    return {
        showTimeList: function () {
            $(".dropdown-time-list").show();
        },

        hideTimeList: function () {
            $(".dropdown-time-list").hide();
        },

        onTimeSelected: function (context, formName) {
            var newInterval = $(context).html();
            $("#server-time-interval").val(newInterval);
            $(".refresh-count-down div.timer").html(newInterval);
            $("#time-interval").val(newInterval);
            timeInterval = newInterval;
            $(".dropdown-time-list").hide();
            clearInterval(timer);
            refreshCountDownMethods.startCountDown(timeInterval, formName);
        },

        startCountDown: function (timeInterval, formName) {
            timer = setInterval(function () {
                if (timeInterval <= 0) {
                    clearInterval(timer);
                    $("#" + formName).submit();
                };
                $(".refresh-count-down div.timer").html(timeInterval);
                timeInterval -= 1;
            }, 1000);
        },

        resetCountDown: function (timeInterval, formName) {
            window.clearInterval(timer);
            refreshCountDownMethods.startCountDown(timeInterval, formName);
        },

        stopCountDown: function () {
            window.clearInterval(timer);
        },

        submitForm: function (formName) {
            $("#" + formName).submit();
        }
    }
} (jQuery);

$.fn.refreshCountDown = function (options) {

    var timerList = '<div class="dropdown-time-list" onmouseleave="refreshCountDownMethods.hideTimeList()"><ul>';

    for (var i = 0; i < options.listTimeInterval.length; i++) {
        timerList += '<li onclick="refreshCountDownMethods.onTimeSelected(this,\'' + options.formName + '\')">' + options.listTimeInterval[i] + '</li>';
    };
    timerList += '</ul></div>';
    timerList += '<input type="hidden" value="' + options.timeIntervalValue + '" name="' + options.timeIntervalName + '" id="time-interval" />';

    $(this).append('<div class="timer"></div>');
    var buttons = $('<div class="buttons"></div>');
    $(buttons).append('<div onclick="refreshCountDownMethods.submitForm(\'' + options.formName + '\')" class="refresh-button"></div>');
    $(buttons).append('<div onclick="refreshCountDownMethods.showTimeList()" class="dropdown-button"></div>');
    $(this).append(buttons);
    $(this).after(timerList);

    refreshCountDownMethods.startCountDown(options.timeIntervalValue, options.formName);
};

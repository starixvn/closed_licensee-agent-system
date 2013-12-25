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
                    $("#" + formName).submit();
                };
                $(".refresh-count-down div.timer").html(timeInterval);
                timeInterval -= 1;
            }, 1000);
        }
    }
} (jQuery);

$(function () {

});

$.fn.refreshCountDown = function (options) {

    var timerList = '<div class="dropdown-time-list" onmouseleave="refreshCountDownMethods.hideTimeList()"><ul>';

    for (var i = 0; i < options.listTimeInterval.length; i++) {
        timerList += '<li onclick="refreshCountDownMethods.onTimeSelected(this,\'' + options.formName + '\')">' + options.listTimeInterval[i] + '</li>';
    };
    timerList += '</ul></div>';
    timerList += '<input type="hidden" value="' + options.timeIntervalValue + '" name="' + options.timeIntervalName + '" id="time-interval" />';

    $(this).append('<div class="timer"></div>').click(function () { refreshCountDownMethods.showTimeList(); });
    $(this).append('<div class="refresh-button"></div>').click(function () { refreshCountDownMethods.showTimeList(); });
    $(this).append('<div class="dropdown-button"></div>').click(function () { refreshCountDownMethods.showTimeList(); });
    $(this).after(timerList);

    refreshCountDownMethods.startCountDown(options.timeIntervalValue, options.formName); 
};

var HomePage = function ($) {
    return {
        init: function () {
            HomePage.initEvent();
        },

        initEvent: function () {            
            $("#top-winning").click(function () {
                if ($(this).hasClass("expand")) {
                    HomePage.expandCommon(this);
                    HomePage.topWinningMembers();
                } else {
                    $(this).removeClass("collapse");
                    $(this).addClass("expand");
                    $("#top-winning-content").hide();
                }
            });

            $("#top-turnover").click(function () {
                if ($(this).hasClass("expand")) {
                    HomePage.expandCommon(this);
                    HomePage.topTurnoverMembers();
                } else {
                    $(this).removeClass("collapse");
                    $(this).addClass("expand");
                    $("#top-turnover-content").hide();
                }
            });

            $("#new-members").click(function () {
                if ($(this).hasClass("expand")) {
                    HomePage.expandCommon(this);
                    HomePage.newMembers();
                } else {
                    $(this).removeClass("collapse");
                    $(this).addClass("expand");
                    $("#new-members-content").hide();
                }
            });
        },

        expandCommon: function (context) {
            $(".dropdown-popup").hide();
            $(".dropdown-button").removeClass("collapse");
            $(".dropdown-button").addClass("expand");
            $(context).removeClass("expand");
            $(context).addClass("collapse");
        },

        topWinningMembers: function () {
            $.ajax({
                url: CORE.resolveUrl("~/Account/TopWinning"),
                type: 'get',
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    $("#top-winning-content").html(data);
                    $("#top-winning-content").show();
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        topTurnoverMembers: function () {
            $.ajax({
                url: CORE.resolveUrl("~/Account/TopTurnover"),
                type: 'get',
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    $("#top-turnover-content").html(data);
                    $("#top-turnover-content").show();
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        newMembers: function () {
            $.ajax({
                url: CORE.resolveUrl("~/Account/NewMembers"),
                type: 'get',
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    $("#new-members-content").html(data);
                    $("#new-members-content").show();
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        }
    };
} (jQuery);
$(function () {
    HomePage.init();
});
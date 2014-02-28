var HEADER = function ($) {
    var currentTime = Date.now;
    var licenseeId, currencyId;
    return {
        init: function () {
            currentTime = moment($('#clock').html());
            licenseeId = $('#licensee').data('licensee-id');
            currencyId = $('#currency').data('currency-id');

            var orginalUsername = $.trim($('.username').text());
            $('.username').dotdotdot().trigger("isTruncated", function (isTruncated) {
                $('.username').attr('title', orginalUsername);
            });

            // kickout
            setTimeout(function () { CORE.reCheckIn(); }, 1000);

            // clock
            setTimeout(HEADER.clockTick, 1000);

            $('#changePwd').click(function () {
                MENU.activeMenu($('#menuSecurity'));
            });

            $('#language').hover(function () {
                $(this).find('.language-box').show();
            }, function () {
                $(this).find('.language-box').hide();
            });

            $('#language li').click(function () {
                var data = {
                    Language: $(this).find('a').attr('data-language')
                };
                $.ajax({
                    type: "POST",
                    data: $.param(data),
                    url: CORE.resolveUrl("~/Account/ChangeLanguage"),
                    dataType: "json",
                    async: false,
                    timeout: 20000
                })
                .done(function (result) {
                    if (result.isChanged) {
                        location.reload();
                    }
                });
            });
            $('.personalmessage').click(function () {
                HEADER.updateCountPersonalMessage();
            });
            HEADER.initScrollMessage();
        },
        signOut: function () {
            try {
                parent.location = CORE.resolveUrl('~/Account/SignOut');
            } catch (err) {
                CORE.console.warn(err);
            }
        },
        clockTick: function () {
            currentTime.add('seconds', 1);
            var sTime = currentTime.format('h:mm:ss A MMM D, YYYY') + ' GMT-4';
            $('#clock').html(sTime);
            setTimeout(HEADER.clockTick, 1000);
        },
        initScrollMessage: function () {
            $('.marquee').marquee({ speed: 20000 });
            $('.marquee').mouseover(function () {
                $(this).trigger('pause');
            }).mouseout(function () {
                $(this).trigger('resume');
            });
            setInterval("HEADER.newestMessage();", 60000);
        },
        newestMessage: function () {
            $('.marquee').marquee('destroy');
            $('.message-content').text("ABC");
            $('.marquee').marquee({ speed: 20000 });
        },
        updateCountPersonalMessage: function () {
            $('.badge').text(0);
        }
    };
} (jQuery);
$(function () {
    HEADER.init();
});
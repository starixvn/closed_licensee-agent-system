/*global core, moment, homeMenu*/

var homeHeader = (function ($) {
    'use strict';
    var currentTime = Date.now;
    var licenseeId, currencyId;
    return {
        init: function () {
            currentTime = moment($('#clock').html());
            licenseeId = $('#licensee').data('licensee-id');
            currencyId = $('#currency').data('currency-id');

            var orginalUsername = $.trim($('.username').text());
            $('.username').dotdotdot().trigger("isTruncated", function () {
                $('.username').attr('title', orginalUsername);
            });

            // kickout
            setTimeout(function () { core.reCheckIn(); }, 1000);

            // clock
            setTimeout(homeHeader.clockTick, 1000);

            $('#changePwd').click(function () {
                homeMenu.activeMenu($('#menuSecurity'));
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
                    url: core.resolveUrl("~/Account/ChangeLanguage"),
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
                homeHeader.updateCountPersonalMessage();
            });
        },
        signOut: function () {
            try {
                parent.location = core.resolveUrl('~/Account/SignOut');
            } catch (err) {
                core.console.warn(err);
            }
        },
        clockTick: function () {
            currentTime.add('seconds', 1);
            var sTime = currentTime.format('h:mm:ss A MMM D, YYYY') + ' GMT-4';
            $('#clock').html(sTime);
            setTimeout(homeHeader.clockTick, 1000);
        },
        initScrollMessage: function () {
            $('.marquee').marquee({ speed: 20000 });
            $('.marquee').mouseover(function () {
                $(this).trigger('pause');
            }).mouseout(function () {
                $(this).trigger('resume');
            });

            setInterval(function () { homeHeader.newestMessage(); }, 60000);
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
} (jQuery));

$(function () {
    'use strict';
    homeHeader.init();
});
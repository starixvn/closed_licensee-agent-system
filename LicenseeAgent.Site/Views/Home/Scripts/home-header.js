/*global CORE, HOME_MENU, moment*/
'use strict';

var HOME_HEADER = (function ($this, $) {
    var currentTime = Date.now;
    var licenseeId, currencyId;

    $this.init = function () {
        currentTime = moment($('#clock').html());
        licenseeId = $('#licensee').data('licensee-id');
        currencyId = $('#currency').data('currency-id');

        var orginalUsername = $.trim($('.username').text());
        $('.username').dotdotdot().trigger("isTruncated", function () {
            $('.username').attr('title', orginalUsername);
        });

        // kickout
        setTimeout(function () {
            CORE.reCheckIn();
        }, 1000);

        // clock
        setInterval(function () {
            currentTime.add('seconds', 1);
            var sTime = currentTime.format('h:mm:ss A MMM D, YYYY') + ' GMT-4';
            $('#clock').html(sTime);
        }, 1000);

        $('#changePwd').click(function () {
            HOME_MENU.activeMenu($('#menuSecurity'));
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
            }).done(function (result) {
                if (result.isChanged) {
                    location.reload();
                }
            });
        });

        $('.personalmessage').click(function () {
            $('.badge').text(0);
        });
    };

    $this.signOut = function () {
        try {
            parent.location = CORE.resolveUrl('~/Account/SignOut');
        } catch (err) {
            CORE.Console.warn(err);
        }
    };

    //    var initScrollMessage = function () {
    //        $('.marquee').marquee({ speed: 20000 });
    //        $('.marquee').mouseover(function () {
    //            $(this).trigger('pause');
    //        }).mouseout(function () {
    //            $(this).trigger('resume');
    //        });

    //        setInterval(function () { newestMessage(); }, 60000);
    //    };

    //    var newestMessage = function () {
    //        $('.marquee').marquee('destroy');
    //        $('.message-content').text("ABC");
    //        $('.marquee').marquee({ speed: 20000 });
    //    };

    return $this;
} (HOME_HEADER || {}, jQuery));

$(function () {
    HOME_HEADER.init();
});
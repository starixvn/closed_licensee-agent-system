/*global CORE, LANGUAGE*/
'use strict';

var HOME = HOME || {};
HOME.TOP_MENU = (function ($this, $) {
    var isSelected, licenseeId, newLicenseeId, currencyId, newCurrencyId, userName;

    $this.init = function () {
        isSelected = $('#is-selected').val() === "True";
        licenseeId = $('#licensee').data('licensee-id');
        newLicenseeId = licenseeId;
        currencyId = $('#currency').data('currency-id');
        newCurrencyId = currencyId;
        userName = $('.username b').text();

        if (!isSelected) {
            enableSearch(false);
        }
        if ($("#licensee").length > 0) {
            $('#licensee').hover(function () {
                $(this).find('.drop-box').show();
            }, function () {
                $(this).find('.drop-box').hide();
            });

            $('#licensee li:not(.dropdown-submenu)').click(function () {
                var subMenu = $(this).parent().parent();
                var dropBox = subMenu.parent();
                dropBox.find('li').removeClass('selected');
                subMenu.addClass('selected');
                $(this).addClass('selected');
                newLicenseeId = $(this).children('a').data('id');
                newCurrencyId = 0;
                onChange();
                $('#licensee').find('span:first').html($(this).find('a').html());
                $("#currency").find("span:first").html(LANGUAGE.getString('ChooseCurrency'));
                loadCurrency(newLicenseeId);
            });
        }

        if ($("#currency").length > 0) {
            $('#currency').hover(function () {
                $(this).find('.drop-box').show();
            }, function () {
                $(this).find('.drop-box').hide();
            });
        }

        $('#agentName').bind('keypress keyup change paste', function () {
            setTimeout(function () {
                if ($.trim($('#agentName').val()) !== '') {
                    $('#submit').removeClass('disabled');
                    $('#agentName').prop('disabled', false);
                }
                else {
                    if (!$('#submit').hasClass('disabled')) {
                        $('#submit').addClass('disabled');
                    }
                }
            }, 1);
        });

        $('#submit').click(function () {
            if (!$('#submit').hasClass('disabled')) {
                submit();
            }
        });

        initCurrencyBox();

        $('#internal-form').submit(function (e) {
            e.preventDefault();
            if (!$('#submit').hasClass('disabled')) {
                submit();
            }
        });

        if ($('#licensee').length > 0 && (!licenseeId || !currencyId)) {
            unactiveMain();
            var isShowIntro = $.cookie('{0}_show_intro'.format(userName));
            if (isShowIntro !== '0') {
                beginIntro();
            }
        }
    };

    var enableSearch = function (enabled) {
        $('#agentName').val('');
        if (enabled) {
            $('#submit').removeClass('disabled');
            $('#agentName').prop('disabled', false);
        }
        else {
            $('#agentName').prop('disabled', true);
        }
    };

    var submit = function () {
        if (newLicenseeId > 0 && newCurrencyId > 0) {
            var agentName = $.trim($('#agentName').val());
            var data = {
                licenseeid: newLicenseeId,
                currencyId: newCurrencyId,
                userName: agentName
            };
            CORE.ajaxJSON({
                url: $('#internal-form').attr('action'),
                data: data
            }, function (response) {
                if (response.DirectUrl) {
                    var url = response.DirectUrl;
                    if (agentName === '') {
                        location.href = url;
                    }
                    else {
                        CORE.openWindow(url);
                        CORE.unblockUI();
                    }
                }
                else {
                    location.reload();
                }
            });
        }
    };

    var calculateIntro = function () {
        var x = 0;
        if ($('#licensee').length > 0 && $('#licensee').css('display') !== 'none') {
            x = $('#licensee').offset().left;
        }
        else {
            x = $('#currency').offset().left;
        }
        var y = $('#currency').offset().left + $('#currency').outerWidth();
        $('#intro').modal({
            backdrop: false,
            show: true
        });
        var width = $('#intro').outerWidth();
        $('#intro').css('left', x - (width - (y - x)) / 2);
    };

    var onChange = function () {
        endIntro();
        if (newCurrencyId > 0 && newCurrencyId > 0) {
            enableSearch(true);
            submit();
        }
        else {
            enableSearch(false);
            unactiveMain();
        }
    };

    //    var activeMain = function () {
    //        $('.message-box').show();
    //        $('#overlay').hide();
    //    };

    var unactiveMain = function () {
        $('.message-box').hide();
        $('#overlay').show();
    };

    var loadCurrency = function (licenseeId) {
        var url = $('#licensee').data('href');
        CORE.ajaxJSON({
            url: url,
            data: {
                licenseeId: licenseeId
            }
        }, function (response) {
            parseCurrency(response.List);
        });
    };

    var parseCurrency = function (list) {
        var html = '';
        for (var index in list) {
            html += '<li><a href="javascript:void(0);" data-id="' + list[index].Currency +
                        '"  number-agent=' + list[index].NumberAgentLevel + '>' + list[index].CurrencyName + '</a></li>';
        }
        if ($("#currency").find('ul').length <= 0) {
            $("#currency").append('<ul class="drop-box">');
        }
        $("#currency").find('ul').html(html);

        initCurrencyBox();
    };

    var initCurrencyBox = function () {
        $('#currency li').click(function () {
            $(this).parent().find('li').removeClass('selected');
            $(this).addClass('selected');
            newCurrencyId = $(this).children('a').data('id');
            onChange();
            $('#currency').find('span:first').html($(this).find('a').html());
            $('#currency').find('ul').hide();
        });
    };

    $this.setCurrency = function (currencyId) {
        newCurrencyId = currencyId;
        submit();
    };

    var beginIntro = function () {
        if ($('#intro').length === 0) {
            return;
        }
        $('#overlay').addClass('intro');
        calculateIntro();

        // create & amimate intro arrow
        var arrow = $('<div class="intro-arrow"></div>');
        $('body').append(arrow);
        animateArrow(arrow, $('#licensee'));

        $('#intro .checkbox').change(function () {
            var isChecked = $(this).find('input').is(':checked');
            if (isChecked) {
                $.cookie('{0}_show_intro'.format(userName), '0', { path: '/' });
                endIntro();
            }
        });

        $('body').bind('click', function (e) {
            if (!$.contains($('#intro')[0], e.target)) {
                endIntro();
                $(this).unbind(e);
            }
        });

        $(window).bind('resize', function () {
            calculateIntro();
        });
    };

    var endIntro = function () {
        $('.intro-arrow').remove();
        $('#overlay').removeClass('intro');
        $('#intro').modal('hide');
        $(window).unbind('resize');
    };

    var animateArrow = function (arrow, element) {
        if (element.length === 0 || element.css('display') === 'none') {
            element = $('#currency');
        }
        arrow.css('opacity', 1);
        var x = element.offset().left + (element.outerWidth() - arrow.width()) / 2;
        var y = $('#intro').offset().top - arrow.height();
        arrow.css({
            'left': x,
            'top': y
        });
        var animateTop = '{0}px'.format(element.offset().top + element.outerHeight());
        arrow.animate({ "top": animateTop, opacity: 0 }, 900, function () {
            setTimeout(function () {
                if (element[0] === $('#currency')[0]) {
                    animateArrow(arrow, $('#licensee'));
                }
                else {
                    animateArrow(arrow, $('#currency'));
                }
            }, 1000);
        });
    };

    return $this;
} (HOME.TOP_MENU || {}, jQuery));
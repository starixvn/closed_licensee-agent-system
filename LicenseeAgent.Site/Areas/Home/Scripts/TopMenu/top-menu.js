/*global core, language*/

var home = home || {};
home.topMenu = (function ($) {
    'use strict';
    var isSelected, licenseeId, newLicenseeId, currencyId, newCurrencyId, userName;
    return {
        init: function () {
            isSelected = $('#is-selected').val() === "True";
            licenseeId = $('#licensee').data('licensee-id');
            newLicenseeId = licenseeId;
            currencyId = $('#currency').data('currency-id');
            newCurrencyId = currencyId;
            userName = $('.username b').text();

            if (!isSelected) {
                home.topMenu.enableSearch(false);
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
                    home.topMenu.onChange();
                    $('#licensee').find('span:first').html($(this).find('a').html());
                    $("#currency").find("span:first").html(language.getString('ChooseCurrency'));
                    home.topMenu.loadCurrency(newLicenseeId);
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
                    home.topMenu.submit();
                }
            });

            home.topMenu.initCurrencyBox();

            $('#internal-form').submit(function (e) {
                e.preventDefault();
                if (!$('#submit').hasClass('disabled')) {
                    home.topMenu.submit();
                }
            });

            if ($('#licensee').length > 0 && (!licenseeId || !currencyId)) {
                home.topMenu.unactiveMain();
                var isShowIntro = $.cookie('{0}_show_intro'.format(userName));
                if (isShowIntro !== '0') {
                    home.topMenu.beginIntro();
                }
            }
        },
        enableSearch: function (enabled) {
            $('#agentName').val('');
            if (enabled) {
                $('#submit').removeClass('disabled');
                $('#agentName').prop('disabled', false);
            }
            else {
                $('#agentName').prop('disabled', true);
            }
        },
        submit: function () {
            if (newLicenseeId > 0 && newCurrencyId > 0) {
                var agentName = $.trim($('#agentName').val());
                var data = {
                    licenseeid: newLicenseeId,
                    currencyId: newCurrencyId,
                    userName: agentName
                };
                var url = $('#internal-form').attr('action');
                $.ajax({
                    url: url,
                    async: false,
                    type: 'post',
                    data: data,
                    beforeSend: function () {
                        core.blockUI();
                    },
                    success: function (data) {
                        if (data.ErrCode !== 0) {
                            alert(data.ErrMsg);
                            core.unblockUI();
                            return;
                        }

                        if (data.DirectUrl) {
                            var url = data.DirectUrl;
                            if (agentName === '') {
                                location.href = url;
                            }
                            else {
                                core.openWindow(url);
                                core.unblockUI();
                            }
                        }
                        else {
                            location.reload();
                        }
                    },
                    error: function () {
                        core.unblockUI();
                    }
                });
            }
        },
        onChange: function () {
            home.topMenu.endIntro();
            if (newCurrencyId > 0 && newCurrencyId > 0) {
                home.topMenu.enableSearch(true);
                home.topMenu.submit();
            }
            else {
                home.topMenu.enableSearch(false);
                home.topMenu.unactiveMain();
            }
        },
        activeMain: function () {
            $('.message-box').show();
            $('#overlay').hide();
        },
        unactiveMain: function () {
            $('.message-box').hide();
            $('#overlay').show();
        },
        loadCurrency: function (lsId) {
            var url = $('#licensee').data('href');
            $.ajax({
                url: url,
                type: 'post',
                data: { licenseeId: lsId },
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    home.topMenu.parseCurrency(data.List);
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        parseCurrency: function (list) {
            var html = '';
            for (var index in list) {
                html += '<li><a href="javascript:void(0);" data-id="' + list[index].Currency +
                        '"  number-agent=' + list[index].NumberAgentLevel + '>' + list[index].CurrencyName + '</a></li>';
            }
            if ($("#currency").find('ul').length <= 0) {
                $("#currency").append('<ul class="drop-box">');
            }
            $("#currency").find('ul').html(html);

            home.topMenu.initCurrencyBox();
        },
        initCurrencyBox: function () {
            $('#currency li').click(function () {
                $(this).parent().find('li').removeClass('selected');
                $(this).addClass('selected');
                newCurrencyId = $(this).children('a').data('id');
                home.topMenu.onChange();
                $('#currency').find('span:first').html($(this).find('a').html());
                $('#currency').find('ul').hide();
            });
        },
        setCurrency: function (currencyId) {
            newCurrencyId = currencyId;
            home.topMenu.submit();
        },
        beginIntro: function () {
            if ($('#intro').length === 0) {
                return;
            }
            $('#overlay').addClass('intro');
            home.topMenu.calculateIntro();

            // create & amimate intro arrow
            var arrow = $('<div class="intro-arrow"></div>');
            $('body').append(arrow);
            home.topMenu.animateArrow(arrow, $('#licensee'));

            $('#intro .checkbox').change(function () {
                var isChecked = $(this).find('input').is(':checked');
                if (isChecked) {
                    $.cookie('{0}_show_intro'.format(userName), '0', { path: '/' });
                    home.topMenu.endIntro();
                }
            });

            $('body').bind('click', function (e) {
                if (!$.contains($('#intro')[0], e.target)) {
                    home.topMenu.endIntro();
                    $(this).unbind(e);
                }
            });

            $(window).bind('resize', function () {
                home.topMenu.calculateIntro();
            });
        },
        endIntro: function () {
            $('.intro-arrow').remove();
            $('#overlay').removeClass('intro');
            $('#intro').modal('hide');
            $(window).unbind('resize');
        },
        calculateIntro: function () {
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
        },
        animateArrow: function (arrow, element) {
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
                        home.topMenu.animateArrow(arrow, $('#licensee'));
                    }
                    else {
                        home.topMenu.animateArrow(arrow, $('#currency'));
                    }
                }, 1000);
            });
        }
    };
} (jQuery));
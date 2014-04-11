/*global core, language*/

var internalHeader = (function ($) {
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
                internalHeader.enableSearch(false);
            }
            if ($("#licensee").length > 0) {
                $('#licensee').hover(function () {
                    $(this).find('.drop-box').show();
                }, function () {
                    $(this).find('.drop-box').hide();
                });

                $('#licensee li').click(function () {
                    $(this).parent().find('li').removeClass('selected');
                    $(this).addClass('selected');
                    newLicenseeId = $(this).children('a').data('id');
                    newCurrencyId = 0;
                    internalHeader.onChange();
                    $('#licensee').find('span:first').html($(this).find('a').html());
                    $('#licensee').find('ul').hide();
                    $("#currency").find("span:first").html(language.getString('ChooseCurrency'));
                    internalHeader.loadCurrency(newLicenseeId);
                });
            }

            if ($("#currency").length > 0) {
                $('#currency').hover(function () {
                    $(this).find('.drop-box').show();
                }, function () {
                    $(this).find('.drop-box').hide();
                });
            }

            $('#agentName').keyup(function () {
                if ($.trim($('#agentName').val()) !== '') {
                    $('#submit').removeClass('disabled');
                    $('#agentName').prop('disabled', false);
                }
                else {
                    if (!$('#submit').hasClass('disabled')) {
                        $('#submit').addClass('disabled');
                    }
                }
            });

            $('#submit').click(function () {
                if (!$('#submit').hasClass('disabled')) {
                    internalHeader.submit();
                }
            });

            internalHeader.initCurrencyBox();

            $('#internal-form').submit(function (e) {
                e.preventDefault();
                if (!$('#submit').hasClass('disabled')) {
                    internalHeader.submit();
                }
            });

            if ($('#licensee').length > 0 && (!licenseeId || !currencyId)) {
                internalHeader.unactiveMain();
                var isShowIntro = $.cookie('{0}_show_intro'.format(userName));
                if (isShowIntro !== '0') {
                    internalHeader.beginIntro();
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
                            if (agentName === '') {
                                location.href = data.DirectUrl;
                            }
                            else {
                                core.openWindow(data.DirectUrl);
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
            internalHeader.endIntro();
            if (newCurrencyId > 0 && newCurrencyId > 0) {
                internalHeader.enableSearch(true);
                internalHeader.submit();
            }
            else {
                internalHeader.enableSearch(false);
                internalHeader.unactiveMain();
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
            var url = core.resolveUrl($('#licensee').data('href'));
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
                    internalHeader.parseCurrency(data.List);
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

            internalHeader.initCurrencyBox();
        },
        initCurrencyBox: function () {
            $('#currency li').click(function () {
                $(this).parent().find('li').removeClass('selected');
                $(this).addClass('selected');
                newCurrencyId = $(this).children('a').data('id');
                internalHeader.onChange();
                $('#currency').find('span:first').html($(this).find('a').html());
                $('#currency').find('ul').hide();
            });
        },
        setCurrency: function (currencyId) {
            newCurrencyId = currencyId;
            internalHeader.submit();
        },
        beginIntro: function () {
            if ($('#intro').length === 0) {
                return;
            }
            $('#overlay').addClass('intro');
            internalHeader.calculateIntro();

            // create & amimate intro arrow
            var arrow = $('<div class="intro-arrow"></div>');
            $('body').append(arrow);
            internalHeader.animateArrow(arrow, $('#licensee'));

            $('#intro .checkbox').change(function () {
                var isChecked = $(this).find('input').is(':checked');
                if (isChecked) {
                    $.cookie('{0}_show_intro'.format(userName), '0', { path: '/' });
                    internalHeader.endIntro();
                }
            });

            $('body').bind('click', function (e) {
                if (!$.contains($('#intro')[0], e.target)) {
                    internalHeader.endIntro();
                    $(this).unbind(e);
                }
            });

            $(window).bind('resize', function () {
                internalHeader.calculateIntro();
            });
        },
        endIntro: function () {
            $('.intro-arrow').remove();
            $('#overlay').removeClass('intro');
            $('#intro').modal('hide');
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
                        internalHeader.animateArrow(arrow, $('#licensee'));
                    }
                    else {
                        internalHeader.animateArrow(arrow, $('#currency'));
                    }
                }, 1000);
            });
        }
    };
} (jQuery));

$(function () {
    'use strict';
    internalHeader.init();
});
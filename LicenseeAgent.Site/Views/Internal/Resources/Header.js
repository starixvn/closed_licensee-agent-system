var INTERNAL_HEADER = function ($) {
    var isSelected = $('#is-selected').val() == "True";
    var licenseeId = $('#licensee').data('licensee-id');
    var newLicenseeId = licenseeId;
    var currencyId = $('#currency').data('currency-id');
    var newCurrencyId = currencyId;
    var userName = $('.username b').text();
    return {
        init: function () {
            if (!isSelected) {
                INTERNAL_HEADER.enableSearch(false);
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
                    INTERNAL_HEADER.onChange();
                    $('#licensee').find('span:first').html($(this).find('a').html());
                    $('#licensee').find('ul').hide();
                    $("#currency").find("span:first").html(LANGUAGE.getString('ChooseCurrency'));
                    INTERNAL_HEADER.loadCurrency(newLicenseeId);
                });
            }

            if ($("#currency").length > 0) {
                $('#currency').hover(function () {
                    $(this).find('.drop-box').show();
                }, function () {
                    $(this).find('.drop-box').hide();
                });
            }

            $('#submit').addClass('disabled');
            $('#agentName').keyup(function () {
                if ($.trim($('#agentName').val()) != '') {
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
                    INTERNAL_HEADER.submit();
                }
            });

            INTERNAL_HEADER.initCurrencyBox();

            $('#internal-form').submit(function (e) {
                e.preventDefault();
                if (!$('#submit').hasClass('disabled')) {
                    INTERNAL_HEADER.submit();
                }
            });

            if ($('#licensee').length > 0 && (!licenseeId || !currencyId)) {
                INTERNAL_HEADER.unactiveMain();
                var isShowIntro = $.cookie('{0}_show_intro'.format(userName));
                if (isShowIntro !== '0') {
                    INTERNAL_HEADER.beginIntro();
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
                    type: 'post',
                    data: data,
                    beforeSend: function () {
                        CORE.blockUI();
                    },
                    success: function (data) {
                        if (data.ErrCode != 0) {
                            alert(data.ErrMsg);
                            return;
                        }
                        if (data.DirectUrl) {
                            if (agentName == '') {
                                location.href = data.DirectUrl;
                            }
                            else {
                                POPUP.openWindow(data.DirectUrl);
                            }
                        }
                        else {
                            location.reload();
                        }
                    },
                    complete: function () {
                        CORE.unblockUI();
                    }
                });
            }
        },
        onChange: function () {
            INTERNAL_HEADER.endIntro();
            if (newCurrencyId > 0 && newCurrencyId > 0) {
                INTERNAL_HEADER.enableSearch(true);
                INTERNAL_HEADER.submit();
            }
            else {
                INTERNAL_HEADER.enableSearch(false);
                INTERNAL_HEADER.unactiveMain();
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
            var url = CORE.resolveUrl($('#licensee').data('href'));
            $.ajax({
                url: url,
                type: 'post',
                data: { licenseeId: lsId },
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode != 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    INTERNAL_HEADER.parseCurrency(data.List);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        parseCurrency: function (list) {
            var html = '';
            for (var index in list) {
                html += '<li><a href="javascript:void(0);" data-id="' + list[index].Currency + '"  number-agent=' + list[index].NumberAgentLevel + '>' + list[index].CurrencyName + '</a></li>';
            }
            if ($("#currency").find('ul').length <= 0) {
                $("#currency").append('<ul class="drop-box">')
            }
            $("#currency").find('ul').html(html);

            INTERNAL_HEADER.initCurrencyBox();
        },
        initCurrencyBox: function () {
            $('#currency li').click(function () {
                $(this).parent().find('li').removeClass('selected');
                $(this).addClass('selected');
                newCurrencyId = $(this).children('a').data('id');
                INTERNAL_HEADER.onChange();
                $('#currency').find('span:first').html($(this).find('a').html());
                $('#currency').find('ul').hide();
            });
        },
        setCurrency: function (currencyId) {
            newCurrencyId = currencyId;
            INTERNAL_HEADER.submit();
        },
        beginIntro: function () {
            if ($('#intro').length == 0) return;

            $('#overlay').addClass('intro');

            // calculate intro position
            var x = 0;
            if ($('#licensee').length > 0 && $('#licensee').css('display') != 'none') {
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

            // create & amimate intro arrow
            var arrow = $('<div class="intro-arrow"></div>');
            $('body').append(arrow);
            INTERNAL_HEADER.animateArrow(arrow, $('#licensee'));

            $('#intro .checkbox').change(function () {
                var isChecked = $(this).find('input').is(':checked');
                if (isChecked) {
                    $.cookie('{0}_show_intro'.format(userName), '0', { path: '/' });
                    INTERNAL_HEADER.endIntro();
                }
            });

            $('body').bind('click', function (e) {
                if (!$.contains($('#intro')[0], e.target)) {
                    INTERNAL_HEADER.endIntro();
                    $(this).unbind(e);
                }
            });
        },
        endIntro: function () {
            $('.intro-arrow').remove();
            $('#overlay').removeClass('intro');
            $('#intro').modal('hide');
        },
        animateArrow: function (arrow, element) {
            if (element.length == 0 || element.css('display') == 'none') element = $('#currency');
            arrow.css('opacity', 1);
            var x = element.offset().left + (element.outerWidth() - arrow.width()) / 2;
            var y = $('#intro').offset().top - arrow.height();
            arrow.css({
                'left': x,
                'top': y
            });
            var animate_top = '{0}px'.format(element.offset().top + element.outerHeight());
            arrow.animate({ "top": animate_top, opacity: 0 }, 900, function () {
                setTimeout(function () {
                    if (element[0] == $('#currency')[0]) {
                        INTERNAL_HEADER.animateArrow(arrow, $('#licensee'));
                    }
                    else {
                        INTERNAL_HEADER.animateArrow(arrow, $('#currency'));
                    }
                }, 1000);
            });
        }
    };
} (jQuery);
$(function () {
    INTERNAL_HEADER.init();
});
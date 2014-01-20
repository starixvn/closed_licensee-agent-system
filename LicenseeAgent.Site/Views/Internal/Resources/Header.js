var INTERNAL_HEADER = function ($) {
    var isSelected = $('#is-selected').val() == "True";
    var licenseeId = $('#licensee').data('licensee-id');
    var newLicenseeId = licenseeId;
    var currencyId = $('#currency').data('currency-id');
    var newCurrencyId = currencyId;
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

            INTERNAL_HEADER.initCurrencyBox();

            $('#internal-form').submit(function (e) {
                e.preventDefault();
                if (!$('#submit').hasClass('disabled')) {
                    INTERNAL_HEADER.submit();
                }
            });

            if ($('#licensee').length > 0 && (!licenseeId || !currencyId)) {
                INTERNAL_HEADER.unactiveMain();
            }
        },
        enableSearch: function (enabled) {
            $('#agentName').val('');
            if (enabled) {
                $('#submit').removeClass('disabled');
                $('#agentName').prop('disabled', false);
                $('#submit').click();
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
                            POPUP.openWindow(data.DirectUrl);
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
            if (newCurrencyId > 0 && newCurrencyId > 0) {
                INTERNAL_HEADER.enableSearch(true);
            }
            else {
                INTERNAL_HEADER.enableSearch(false);
            }

            if (licenseeId == newLicenseeId && currencyId == newCurrencyId) {
                return;
            }

            INTERNAL_HEADER.unactiveMain();
        },
        activeMain: function () {
            $('#overlay').hide();
        },
        unactiveMain: function () {
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
        setCurrency: function (switchedCurrencyId, switchedCurrencyName) {
            newCurrencyId = switchedCurrencyId;
            currencyId = switchedCurrencyId;
            $('#currency').data('currency-id', switchedCurrencyId);
            $('#currency').find('span:first').html(switchedCurrencyName);
        }
    };
} (jQuery);
$(function () {
    INTERNAL_HEADER.init();
});
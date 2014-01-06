var INTERNAL_HEADER = function ($) {
    var licenseeId = $('#licensee').data('licensee-id');
    var newLicenseeId = licenseeId;
    var currencyId = $('#currency').data('currency-id');
    var newCurrencyId = currencyId;
    return {
        init: function () {
            if (("#licensee").length > 0) {
                $('#licensee').hover(function () {
                    $(this).find('.drop-box').show();
                }, function () {
                    $(this).find('.drop-box').hide();
                });

                $('#licensee li').click(function () {
                    newLicenseeId = $(this).children('a').data('id');
                    newCurrencyId = 0;
                    INTERNAL_HEADER.onChange();
                    $('#licensee').find('span:first').html($(this).find('a').html());
                    $('#licensee').find('ul').hide();
                    $("#currency").find("span:first").html($("#defaultCurrency").val());
                    INTERNAL_HEADER.loadCurrency(newLicenseeId);
                });
            }

            if (("#currency").length > 0) {
                $('#currency').hover(function () {
                    $(this).find('.drop-box').show();
                }, function () {
                    $(this).find('.drop-box').hide();
                });
            }

            INTERNAL_HEADER.initCurrencyBox();

            $('#internal-form').submit(function (e) {
                e.preventDefault();
                INTERNAL_HEADER.submit();
            });

            if ($('#licensee').length > 0 && !licenseeId) {
                INTERNAL_HEADER.unactiveMain();
            }
        },
        submit: function () {
            if (newLicenseeId > 0 && newCurrencyId > 0) {
                var data = {
                    licenseeid: newLicenseeId,
                    currencyId: newCurrencyId,
                    userName: $('#agentName').val()
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
                        location.reload();
                    },
                    complete: function () {
                        CORE.unblockUI();
                    }
                });
            }
        },
        onChange: function () {
            if (licenseeId == newLicenseeId && currencyId == newCurrencyId) {
                INTERNAL_HEADER.activeMain();
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
                newCurrencyId = $(this).children('a').data('id');
                INTERNAL_HEADER.onChange();
                $('#currency').find('span:first').html($(this).find('a').html());
                $('#currency').find('ul').hide();
            });
        }
    };
} (jQuery);
$(function () {
    INTERNAL_HEADER.init();
});
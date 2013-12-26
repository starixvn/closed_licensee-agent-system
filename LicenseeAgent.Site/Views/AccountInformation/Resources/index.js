var accountInformation = function ($) {
    var isCalling = false
        , timerAutocomplete = null
        , preUserName
        , lengthUsername = 2;

    var urlSuggestion = $("#searchUrl").data("suggest");
    var urlDownline = $("#searchUrl").data("downline");
    var urlCurrency = $("#searchUrl").data("currency");

    return {
        init: function () {
            // username suggestion
            $('#UserName').keyup(function (event) {
                if (timerAutocomplete == null) {
                    timerAutocomplete = setInterval(accountInformation.autoComplete, 200);
                }
            });

            //form submit
            $('#formSearch').submit(function (event) {
                if ($.trim($("#UserName").val()).length > lengthUsername) {
                    $(this).ajaxSubmit({
                        target: '#searchResult',
                        beforeSubmit: CORE.blockUI,
                        success: accountInformation.searchResultBinding
                    });
                }
                event.preventDefault();
            });

            //search click
            $('#search').click(function () {
                $("#formSearch").submit();
            });
        },
        searchResultBinding: function () {
            CORE.unblockUI();

            $("#downlineContent").niceScroll();

            var pageIndex = $('#paging').data('page-index') + 1;
            var totalPages = $('#paging').data('total-pages');

            if (totalPages > 1) {
                $('#paging').bootstrapPaginator({
                    size: 'mini',
                    currentPage: pageIndex,
                    numberOfPages: 3,
                    totalPages: totalPages,
                    pageUrl: function () { return '#'; },
                    onPageClicked: function (e, originalEvent, type, page) {
                        accountInformation.getDownline(page);
                    }
                });
            }

            $(".agent-inline").click(function () {
                $("#UserName").val($(this).html());
                $("#formSearch").submit();

                event.preventDefault();
            });
        },
        //suggest agent name
        autoComplete: function () {
            var userName = $('#UserName').val();
            accountInformation.clearTimer();
            if ($.trim(userName).length >= lengthUsername) {
                if (userName != preUserName) {
                    timerAutocomplete = setInterval(accountInformation.autoComplete, 200);
                }
                else {
                    accountInformation.getAutoComplete(userName);
                }
            }
            preUserName = userName;
        },
        clearTimer: function () {
            clearInterval(timerAutocomplete);
            timerAutocomplete = null;
        },
        getAutoComplete: function (userName) {
            if ($.trim(userName).length <= lengthUsername) {
                return;
            }
            if (!isCalling) {
                $.ajax({
                    url: urlSuggestion,
                    type: 'post',
                    data: {
                        username: userName
                    },
                    beforeSend: function () {
                        isCalling = true;
                    },
                    success: function (data) {
                        if (data.MsgId == 0) {
                            $("#UserName").autocomplete({
                                source: data.List
                            });
                        }
                    },
                    complete: function () {
                        isCalling = false;
                    }
                });
            }
        },
        //get downline when click paging
        getDownline: function (pageIndex) {
            $.ajax({
                url: urlDownline,
                type: 'post',
                data: {
                    agentid: $("#AgentId").val(),
                    pageindex: pageIndex - 1
                },
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.MsgId == 0) {
                        accountInformation.bindDownline(data.List);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        bindDownline: function (downlines) {
            var numberDownline = downlines.length;
            var downlineHtml = '';
            for (var index = 0; index < numberDownline; index++) {
                downlineHtml += '<li><a class="agent-inline" href="#" data-id="' + downlines[index].DownlineCustId + '">' + downlines[index].UserName + '</a></li>';
            }
            $("#downlineContent ul").html(downlineHtml);
        },
        //show alert when search username not exist
        showNoUserAlert: function (message) {
            var buttons = new Array();
            buttons.push({
                text: 'OK',
                click: function () {
                    accountInformation.closeDialog();
                }
            });
            $('#dialog-confirm').find('p').html(message);
            $("#dialog-confirm").dialog({
                resizable: false,
                width: 400,
                modal: true,
                buttons: buttons,
                close: function (event, ui) { }
            });
        },
        //show alert when search username exist in another currency
        showCurrencyAlert: function (currencyId, msgQuestion, msgAlert) {
            var buttons = new Array();
            buttons.push({
                text: 'YES',
                click: function () {
                    accountInformation.changeCurrencyPage(currencyId);
                    accountInformation.closeDialog();
                }
            });
            buttons.push({
                text: 'NO',
                click: function () {
                    accountInformation.showNoUserAlert(msgAlert);
                }
            });
            $('#dialog-confirm').find('p').html(msgQuestion);
            $("#dialog-confirm").dialog({
                resizable: false,
                width: 400,
                modal: true,
                buttons: buttons,
                close: function (event, ui) { }
            });
        },
        closeDialog: function () {
            $('#dialog-confirm').dialog('close');
        },
        //change currency site
        changeCurrencyPage: function (currencyId) {
            var data = {
                licenseeid: $("#licensee").data("licensee-id"),
                currencyid: currencyId
            };
            $.ajax({
                url: urlCurrency,
                type: 'post',
                data: data,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.MsgId != 0) {
                        alert(data.MsgContent);
                        return;
                    }
                    location.reload();
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        }
    }
} (jQuery);

$(function () {
    accountInformation.init();
});

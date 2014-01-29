var accountInformation = function ($) {
    var urlSuggestion = $("#searchUrl").data("suggest");
    var urlDownline = $("#searchUrl").data("downline");
    var urlCurrency = $("#searchUrl").data("currency");
    return {
        init: function () {
            //scroll left menu
            $(".leftmenu-box").niceScroll();

            // username suggestion
            $("#UserName").typeahead({ source: accountInformation.getSuggestion, minLength: 2 });

            //form submit
            $('#formSearch').submit(function (event) {
                var userName = $.trim($("#UserName").val());
                $("#UserName").val(userName);
                $(this).ajaxSubmit({
                    target: '#searchResult',
                    beforeSubmit: accountInformation.beforeSearchUser,
                    success: accountInformation.searchResultBinding
                });
                event.preventDefault();
            });

            //search click
            $('#search').click(function () {
                $("#formSearch").submit();
            });
        },
        beforeSearchUser: function () {
            CORE.blockUI();
            $("#searchAlert").hide();
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
            //add trigger event
            $('.btn-inverse').click(function () {
                $("#frmMain").attr("src", $(this).data("url"));
            });

            //add popover for edit indirect member
            if ($('.popover-link').length > 0) {
                $('.popover-link').popover({
                    container: 'body',
                    html: true,
                    placement: 'right'
                });

                $(document).click(function (e) {
                    $('.popover-link').each(function () {
                        if (!$(this).is(e.target) &&
                            $(this).has(e.target).length === 0 &&
                            $('.popover').has(e.target).length === 0) {
                            $(this).popover('hide');
                            return;
                        }
                    });
                });
            }

            accountInformation.pagingBinding();
        },
        pagingBinding: function () {
            $(".agent-inline").click(function (event) {
                $("#UserName").val($(this).attr('title'));
                $("#formSearch").submit();

                event.preventDefault();
            });
        },
        hidePopover: function () {
            $('.popover-link').popover('hide');
        },
        getSuggestion: function (query, process) {
            return $.ajax({
                url: urlSuggestion,
                type: 'post',
                data: {
                    username: query
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        return process(data.List);
                    }
                }
            });
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
                    if (data.ErrCode == 0) {
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
                downlineHtml += '<li><a class="agent-inline" href="#" data-id="' + downlines[index].DownlineCustId + '" title="' + downlines[index].UserName + '">' + downlines[index].UserName + '</a></li>';
            }
            $("#downlineContent ul").html(downlineHtml);
            accountInformation.pagingBinding();
        },
        //show alert when search username not exist
        showNoUserAlert: function (message) {
            $("#searchAlert").show();
        },
        //show alert when search username exist in another currency
        showCurrencyAlert: function (currencyId, currencyName, msgQuestion, msgAlert) {
            var buttons = new Array();
            buttons.push({
                text: 'YES',
                click: function () {
                    accountInformation.changeCurrencyPage(currencyId, currencyName);
                    accountInformation.closeDialog();
                }
            });
            buttons.push({
                text: 'NO',
                click: function () {
                    accountInformation.showNoUserAlert(msgAlert);
                    accountInformation.closeDialog();
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
        changeCurrencyPage: function (currencyId, currencyName) {
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
                    if (data.ErrCode != 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    if (typeof INTERNAL_HEADER != 'undefined') {
                        INTERNAL_HEADER.setCurrency(currencyId);
                    }
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
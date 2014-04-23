/*global core, language, popup, internalHeader */

var accountInformation = (function ($) {
    'use strict';
    var urlSuggestion, urlDownline, urlCurrency;
    return {
        init: function () {
            urlSuggestion = $("#searchUrl").data("suggest");
            urlDownline = $("#searchUrl").data("downline");
            urlCurrency = $("#searchUrl").data("currency");

            var userName;

            //scroll left menu
            $(".leftmenu-box").niceScroll({ cursoropacitymax: 0.7, autohidemode: "leave" });

            // username suggestion
            $("#UserName").typeahead({
                source: accountInformation.getSuggestion,
                minLength: 2
            });

            //form submit
            $('#formSearch').submit(function (event) {
                userName = $.trim($("#UserName").val());
                if (userName !== '') {
                    $("#UserName").val(userName);
                    $(this).ajaxSubmit({
                        target: '#searchResult',
                        beforeSubmit: accountInformation.beforeSearchUser,
                        success: accountInformation.searchResultBinding
                    });
                }
                event.preventDefault();
            });

            //search click
            $('#search').click(function () {
                $("#formSearch").submit();
            });

            //check exist cookie username. If exist, call search username
            userName = $.cookie('username');
            if (userName !== undefined) {
                $("#UserName").val(userName);
                $("#formSearch").submit();

                $.removeCookie('username', { path: '/' });

                accountInformation.changeAccountInfoActive();
            }
        },
        beforeSearchUser: function () {
            core.blockUI();
            $("#searchAlert").hide();
        },
        searchResultBinding: function () {
            core.unblockUI();

            $('#tabDownline').on('shown', function () {
                accountInformation.showScroll();
            });

            var pageIndex = $('#paging').data('page-index') + 1,
                totalPages = $('#paging').data('total-pages');

            if (totalPages > 1) {
                $('#paging').bootstrapPaginator({
                    size: 'mini',
                    currentPage: pageIndex,
                    numberOfPages: 3,
                    totalPages: totalPages,
                    pageUrl: function () { return '#'; },
                    onPageChanged: function (e, oldPage, newPage) {
                        if (oldPage !== newPage && e) {
                            accountInformation.getDownline(newPage);
                        }
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
                    if (data.ErrCode === 0) {
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
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        accountInformation.bindDownline(data.List);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        bindDownline: function (downlines) {
            var numberDownline = downlines.length,
                downlineHtml = '',
                index = 0;
            for (index = 0; index < numberDownline; index++) {
                downlineHtml += '<li><a class="agent-inline" href="#" data-id="' +
                    downlines[index].DownlineCustId +
                    '" title="' + downlines[index].UserName +
                    '"><i class="icon-downline-arrow"></i>' +
                    downlines[index].UserName + '</a></li>';
            }
            $("#downlineContent ul").html(downlineHtml);
            accountInformation.pagingBinding();
            accountInformation.showScroll();
        },
        //show alert when search username not exist
        showNoUserAlert: function (message) {
            $("#searchAlert").html(message);
            $("#searchAlert").show();
        },
        //show alert when search username exist in another currency
        showCurrencyAlert: function (currencyId, currencyName, msgQuestion, msgAlert) {
            var message = msgQuestion;
            var title = language.getString('Attention');
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    popup.close(0);
                    accountInformation.changeCurrencyPage(currencyId, currencyName);
                }
            }, {
                label: language.getString('No'),
                action: function () {
                    popup.close(0);
                    accountInformation.showNoUserAlert(msgAlert);
                }
            }];

            popup.confirm({
                title: title,
                message: message,
                buttons: buttons
            });
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
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    if (internalHeader !== undefined) {
                        $.cookie('username', $("#UserName").val(), { path: '/', expires: 1 });
                        internalHeader.setCurrency(currencyId);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        changeAccountInfoActive: function () {
            var tab, mainMenuId, accountInfoId;
            tab = $('.leftmenu-box').find('.nav-tabs').first();
            tab.find('li:first').removeClass('active');
            tab.find('li:nth-child(2)').addClass('active');

            mainMenuId = tab.find('li:first').find('a:first').attr('href');
            accountInfoId = tab.find('li:nth-child(2)').find('a:first').attr('href');

            $(mainMenuId).removeClass('active in');
            $(accountInfoId).removeClass('fade');
            $(accountInfoId).addClass('active in');
        },
        showScroll: function () {
            var niceScroll = $("#downlineContent").getNiceScroll();
            if (niceScroll.length === 0) {
                $("#downlineContent").niceScroll({ cursoropacitymax: 0.7, autohidemode: "leave" });
            }
            $("#downlineContent").getNiceScroll().resize();
        },
        editAgentCredit: function (agentId, agentName, agentLevel, givenCredit) {
            var url = '~/CreditBalance/EditAgentCredit?agentId={0}&agentName={1}&givenCredit={2}&agentLevel={3}&isAccountInfo=true';
            var urlFormat = url.format(agentId, agentName, givenCredit, agentLevel);
            popup.openFrame(urlFormat);
        },
        agentEditMemberCredit: function (custId, custName, givenCredit) {
            var url = '~/CreditBalance/AgentEditMemberCredit?custId={0}&memberName={1}&givenCredit={2}&isAccountInfo=true'.format(
                custId, custName, givenCredit);
            popup.openFrame(url);
        },
        refreshGivenCredit: function () {
            setTimeout(function () { $('#formSearch').submit(); }, core.RELOAD_INTERVAL);
        }
    };
} (jQuery));

$(function () {
    'use strict';
    accountInformation.init();
});
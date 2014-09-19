/*global core, popup */

var home = home || {};
home.accountInformation = home.accountInformation || {};
home.accountInformation.result = (function ($) {
    'use strict';
    var getDownLinesUrl = '~/Home/AccountInformation/GetDownLines';
    var editAgentCreditUrl = '~/CreditBalance/EditAgentCredit';
    var editMemberCreditUrl = '~/CreditBalance/AgentEditMemberCredit';
    return {
        init: function () {
            $('#tabDownline').on('shown', function () {
                home.accountInformation.result.showScrollDownLine();
                home.accountInformation.result.showScrollSearch();
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
                            home.accountInformation.result.getDownLines(newPage);
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
                $('.popover-link').each(function () {
                    $(this).popover({
                        container: 'body',
                        html: true,
                        content: $(this).next().html(),
                        placement: 'right'
                    });
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

            //add popover for more upline
            if ($('.icon-more-arrow').length > 0) {
                $('.icon-more-arrow').popover({
                    container: 'body',
                    trigger: 'manual',
                    html: true,
                    content: $('.icon-more-arrow').next().html(),
                    placement: 'bottom'
                }).parent().click(function () {
                    $('.icon-more-arrow').popover('toggle');
                });

                $(document).click(function (e) {
                    $('.icon-more-arrow').parent().each(function () {
                        if (!$(this).is(e.target) &&
                            $(this).has(e.target).length === 0 &&
                            $('.popover').has(e.target).length === 0) {
                            $('.icon-more-arrow').popover('hide');
                            return;
                        }
                    });
                });
            }

            home.accountInformation.info.init();
            home.accountInformation.product.init();
            home.accountInformation.result.showScrollSearch();
            home.accountInformation.result.pagingBinding();
        },
        showScrollSearch: function () {
            $("#searchResult").niceScroll({
                cursorcolor: "#8a8a8a",
                autohidemode: "leave",
                horizrailenabled: false
            });
        },
        pagingBinding: function () {
            $(".agent-inline").click(function (event) {
                var userName = $(this).attr('title');
                $("#accountInfo #UserName").val(userName);
                $("#accountInfo form").submit();
                event.preventDefault();
            });
        },
        //get downline when click paging
        getDownLines: function (pageIndex) {
            $.ajax({
                url: core.resolveUrl(getDownLinesUrl),
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
                        home.accountInformation.result.bindDownLines(data.List);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        bindDownLines: function (downlines) {
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
            home.accountInformation.result.pagingBinding();
            home.accountInformation.result.showScrollDownLine();
        },
        showScrollDownLine: function () {
            var niceScroll = $("#downlineList").getNiceScroll();
            if (niceScroll.length === 0) {
                niceScroll = $("#downlineList").niceScroll({
                    cursorcolor: "#8a8a8a",
                    autohidemode: "leave"
                });
            }
            niceScroll.resize();
        },
        editAgentCredit: function (agentId, agentName, agentLevel, givenCredit) {
            var url = core.resolveUrl(editAgentCreditUrl, {
                agentId: agentId,
                agentName: agentName,
                givenCredit: givenCredit,
                agentLevel: agentLevel,
                isAccountInfo: true
            });
            popup.openFrame(url);
        },
        editMemberCredit: function (custId, custName, givenCredit) {
            var url = core.resolveUrl(editMemberCreditUrl, {
                custId: custId,
                memberName: custName,
                givenCredit: givenCredit,
                isAccountInfo: true
            });
            popup.openFrame(url);
        },
        refreshGivenCredit: function (hasReplication) {
            var timeout = 0;
            if (hasReplication === true) {
                timeout = core.REPLICATION_INTERVAL;
            }
            popup.block();
            setTimeout(function () {
                $('#accountInfo form').submit();
                popup.unblock();
            }, timeout);
        }
    };
} (jQuery));
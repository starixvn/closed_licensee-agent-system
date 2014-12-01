/*global CORE, POPUP */
'use strict';

var HOME = HOME || {};
HOME.ACCOUNT_INFORMATION = HOME.ACCOUNT_INFORMATION || {};
HOME.ACCOUNT_INFORMATION.RESULT = (function ($this, $) {
    $this.init = function () {
        $('#tabDownline').on('shown', function () {
            showScrollDownLine();
            showScrollSearch();
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
                        getDownLines(newPage);
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

        HOME.ACCOUNT_INFORMATION.INFO.init();
        HOME.ACCOUNT_INFORMATION.PRODUCT.init();
        showScrollSearch();
        pagingBinding();
    };

    var showScrollSearch = function () {
        $("#searchResult").niceScroll({
            cursorcolor: "#8a8a8a",
            autohidemode: "leave",
            horizrailenabled: false
        });
    };

    var pagingBinding = function () {
        $(".agent-inline").click(function (event) {
            var userName = $(this).attr('title');
            $("#accountInfo #UserName").val(userName);
            $("#accountInfo form").submit();
            event.preventDefault();
        });
    };

    //get downline when click paging
    var getDownLines = function (pageIndex) {
        CORE.ajaxJSON({
            url: CORE.resolveUrl('~/Home/AccountInformation/GetDownLines'),
            data: {
                agentid: $("#AgentId").val(),
                pageindex: pageIndex - 1
            }
        }, function (response) {
            bindDownLines(response.List);
        }, null);
    };

    var bindDownLines = function (downlines) {
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
        pagingBinding();
        showScrollDownLine();
    };

    var showScrollDownLine = function () {
        var niceScroll = $("#downlineContent").getNiceScroll();
        if (niceScroll.length === 0) {
            $("#downlineContent").niceScroll({
                cursorcolor: "#8a8a8a",
                autohidemode: "leave"
            });
        }
        $("#downlineContent").getNiceScroll().resize();
    };

    var refreshGivenCredit = function (hasReplication) {
        var timeout = 0;
        if (hasReplication === true) {
            timeout = CORE.ReplicationInterval;
        }
        CORE.blockUI();
        setTimeout(function () {
            $('#accountInfo form').submit();
            CORE.unblockUI();
        }, timeout);
    };

    $this.editAgentCredit = function (agentId, agentName, agentLevel, givenCredit) {
        var url = CORE.resolveUrl('~/CreditBalance/EditAgent', {
            agentId: agentId,
            agentName: agentName,
            givenCredit: givenCredit,
            agentLevel: agentLevel,
            isAccountInfo: true
        });
        POPUP.openFrame(url);
        POPUP.on('edit-agent-credit', function () {
            refreshGivenCredit(false);
        });
    };

    $this.editMemberCredit = function (custId, custName, givenCredit) {
        var url = CORE.resolveUrl('~/CreditBalance/EditMember', {
            custId: custId,
            memberName: custName,
            givenCredit: givenCredit,
            isAccountInfo: true
        });
        POPUP.openFrame(url);
        POPUP.on('edit-member-credit', function () {
            refreshGivenCredit(true);
        });
    };

    return $this;
} (HOME.ACCOUNT_INFORMATION.RESULT || {}, jQuery));
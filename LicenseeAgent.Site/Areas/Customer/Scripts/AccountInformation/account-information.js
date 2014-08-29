/*global core, language, popup, internalHeader, pageContext */

var customer = customer || {};

customer.accountInformation = (function ($) {
    'use strict';
    var statusObj = { Close: 0, Open: 1 };
    var urlSuggestion, urlDownline, urlCurrency;
    return {
        init: function () {
            urlSuggestion = $("#searchUrl").data("suggest");
            urlDownline = $("#searchUrl").data("downline");
            urlCurrency = $("#searchUrl").data("currency");
            var userName;

            // username suggestion
            $("#UserName").typeahead({
                source: customer.accountInformation.getSuggestion,
                minLength: 2
            });

            //form submit
            $('#formSearch').submit(function (event) {
                userName = $.trim($("#UserName").val());
                if (userName !== '') {
                    $("#UserName").val(userName);
                    customer.accountInformation.search($(this).attr('action'), userName);
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

                customer.accountInformation.changeAccountInfoActive();
            }
        },
        search: function (url, userName) {
            $.ajax({
                url: url,
                type: 'post',
                data: { username: userName },
                beforeSend: function () {
                    $("#searchAlert").hide();
                    core.blockUI();
                },
                success: function (data) {
                    if (typeof data === 'object' && data.ErrCode !== 0) {
                        customer.accountInformation.showNoUserAlert(data.ErrMsg);
                        return;
                    }
                    $('#searchResult').html(data);
                    customer.accountInformation.searchResultBinding();
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        searchResultBinding: function () {
            core.unblockUI();
            $('#tabDownline').on('shown', function () {
                customer.accountInformation.showScrollDownline();
                customer.accountInformation.showScrollSearch();
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
                            customer.accountInformation.getDownline(newPage);
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

            //add popover for more upline
            if ($('.popover-more').length > 0) {
                $('.popover-more').popover({
                    container: '.parent-popover', //'body',
                    html: true,
                    placement: 'bottom'
                });

                $(document).click(function (e) {
                    $('.popover-more').each(function () {
                        if (!$(this).is(e.target) &&
                            $(this).has(e.target).length === 0 &&
                            $('.popover').has(e.target).length === 0) {
                            $(this).popover('hide');
                            return;
                        }
                    });
                });
            }

            $("#customer-status").click(function () {
                customer.accountInformation.changeStatus(this);
            });

            $("#show-hide").click(function () {
                if ($(this).hasClass("icon-collapse")) {
                    $(this).parents(".cust-info").find("table").hide();
                    $(this).removeClass("icon-collapse")
                            .addClass("icon-expand");
                }
                else {
                    $(this).parents(".cust-info").find("table").show();
                    $(this).removeClass("icon-expand")
                            .addClass("icon-collapse");
                }
            });

            customer.accountInformation.showScrollSearch();
            customer.accountInformation.pagingBinding();

            $("#searchResult").mouseover(function () {
                $("#searchResult").getNiceScroll().resize();
            });
        },
        showScrollSearch: function () {
            //scroll search menu
            $("#searchResult").niceScroll({
                cursorcolor: "#8a8a8a",
                autohidemode: "leave",
                horizrailenabled: false
            });
            $("#searchResult").getNiceScroll().resize();
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
                        customer.accountInformation.bindDownline(data.List);
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
            customer.accountInformation.pagingBinding();
            customer.accountInformation.showScrollDownline();
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
                    popup.close();
                    customer.accountInformation.changeCurrencyPage(currencyId, currencyName);
                }
            }, {
                label: language.getString('No'),
                action: function () {
                    popup.close();
                    customer.accountInformation.showNoUserAlert(msgAlert);
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
        showScrollDownline: function () {
            var niceScroll = $("#downlineContent").getNiceScroll();
            if (niceScroll.length === 0) {
                $("#downlineContent").niceScroll({
                    cursorcolor: "#8a8a8a",
                    autohidemode: "leave"
                });
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
        refreshGivenCredit: function (hasReplication) {
            var timeout = 0;
            if (hasReplication === true) {
                timeout = core.REPLICATION_INTERVAL;
            }
            popup.block();
            setTimeout(function () {
                $('#formSearch').submit();
                popup.unblock();
            }, timeout);
        },
        changeStatus: function (target) {
            if (!(pageContext.IsDirectDownLine || pageContext.IsAdmin)) {
                return;
            }

            var currentStatus, newStatus;
            var agentId = parseInt($("#AgentId").val());
            if (!pageContext.IsAdmin && pageContext.AgentId === agentId) {
                return;
            }

            currentStatus = $(target).hasClass('icon-status-open') ? 1 : 0;
            newStatus = currentStatus === 0 ? 1 : 0;
            customer.accountInformation.showAlert(target, agentId, newStatus);
        },
        showAlert: function (target, custId, statusId) {
            var message = '';
            if (statusId === statusObj.Close) {
                if (pageContext.IsMember) {
                    message = language.getString('ChangeStatusMember');
                } else {
                    message = language.getString('ChangeStatusAgent');
                }
            } else {
                if (pageContext.IsMember) {
                    message = language.getString('ChangeCloseToOpenMember');
                } else {
                    message = language.getString('ChangeCloseToOpenAgent');
                }
            }
            var title = language.getString('ChangeStatus');
            var buttons = [{
                label: language.getString('Yes'),
                action: function () {
                    popup.close();
                    customer.accountInformation.updateStatus(target, custId, statusId);
                }
            }, {
                label: language.getString('No'),
                action: function () {
                    popup.close();
                }
            }];

            popup.confirm({
                title: title,
                message: message,
                buttons: buttons
            });
        },
        updateStatus: function (target, custId, statusId) {
            var params, url;
            if (pageContext.IsMember) {
                params = {
                    memberid: custId,
                    status: statusId
                };
            } else {
                params = {
                    agentid: custId,
                    status: statusId
                };
            }

            url = pageContext.IsMember ? "~/CustomerList/ChangeMemberStatus"
                : "~/CustomerList/ChangeAgentStatus";
            $.ajax({
                url: core.resolveUrl(url),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }
                    if (statusId === 0) {
                        $(target).addClass('icon-status-closed')
                            .removeClass('icon-status-open')
                            .find('span')
                            .text('Closed');
                        $("#show-hide").parents(2)
                            .addClass('closed')
                            .removeClass('open');
                    } else {
                        $(target).addClass('icon-status-open')
                            .removeClass('icon-status-closed')
                            .find('span')
                            .text('Open');
                        $("#show-hide").parents(2)
                            .addClass('open')
                            .removeClass('closed');
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        }
    };
} (jQuery));
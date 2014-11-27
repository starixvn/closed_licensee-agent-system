/*global core, popup, language */
'use strict';

var home = home || {};
home.accountInformation = (function ($this, $) {
    $this.init = function () {
        var userName;

        // username suggestion
        $("#accountInfo #UserName").typeahead({
            source: home.accountInformation.getSuggestion,
            minLength: 2
        });

        //form submit
        $('#accountInfo form').submit(function (event) {
            userName = $.trim($("#UserName").val());
            if (userName !== '') {
                home.accountInformation.search(userName);
            }
            event.preventDefault();
        });

        //search click
        $('#accountInfo #search').click(function () {
            $("#accountInfo form").submit();
        });

        //check exist cookie username. If exist, call search username
        userName = $.cookie('username');
        if (userName !== undefined) {
            $("#accountInfo #UserName").val(userName);
            $("#accountInfo form").submit();

            $.removeCookie('username', { path: '/' });
            home.accountInformation.changeAccountInfoActive();
        }
    };

    $this.changeAccountInfoActive = function () {
        var tab, mainMenuId, accountInfoId;
        tab = $('.leftmenu-box').find('.nav-tabs').first();
        tab.find('li:first').removeClass('active');
        tab.find('li:nth-child(2)').addClass('active');

        mainMenuId = tab.find('li:first').find('a:first').attr('href');
        accountInfoId = tab.find('li:nth-child(2)').find('a:first').attr('href');

        $(mainMenuId).removeClass('active in');
        $(accountInfoId).removeClass('fade');
        $(accountInfoId).addClass('active in');
    };

    $this.getSuggestion = function (query, process) {
        return $.ajax({
            url: core.resolveUrl('~/Home/AccountInformation/Suggest'),
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
    };

    $this.search = function (userName) {
        home.accountInformation.hidePopover();
        $("#accountInfo #UserName").val(userName);
        $.ajax({
            url: $('#accountInfo form').attr('action'),
            type: 'post',
            data: {
                username: userName
            },
            beforeSend: function () {
                $("#searchAlert").hide();
                $("#searchResult").html('');
                core.blockUI();
            },
            success: function (data) {
                if (typeof data === 'object') {
                    if (data.ErrCode === -4) {
                        home.accountInformation.showNoUserAlert(data.ErrMsg);
                    }
                    else {
                        core.handleError(data);
                    }
                }
                else {
                    $('#searchResult').html(data);
                    home.accountInformation.result.init();
                }
            },
            complete: function () {
                core.unblockUI();
            }
        });
    };

    $this.hidePopover = function () {
        $('.popover-link').popover('hide');
        $('.icon-more-arrow').popover('hide');
    };

    //show alert when search username not exist
    $this.showNoUserAlert = function (message) {
        $("#searchAlert").html(message);
        $("#searchAlert").show();
    };

    //show alert when search username exist in another currency
    $this.showCurrencyAlert = function (currencyId, currencyName, msgQuestion, msgAlert) {
        var message = msgQuestion;
        var title = language.getString('Attention');
        var buttons = [{
            label: language.getString('Yes'),
            action: function () {
                popup.close();
                home.accountInformation.changeCurrencyPage(currencyId, currencyName);
            }
        }, {
            label: language.getString('No'),
            action: function () {
                popup.close();
                home.accountInformation.showNoUserAlert(msgAlert);
            }
        }];

        popup.confirm({
            title: title,
            message: message,
            buttons: buttons
        });
    };

    //change currency site
    $this.changeCurrencyPage = function (currencyId) {
        var data = {
            licenseeid: $("#licensee").data("licensee-id"),
            currencyid: currencyId
        };
        $.ajax({
            url: $('#internal-form').attr('action'),
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
                if (home.topMenu !== undefined) {
                    $.cookie('username', $("#UserName").val(), { path: '/', expires: 1 });
                    home.topMenu.setCurrency(currencyId);
                }
            },
            complete: function () {
                core.unblockUI();
            }
        });
    };

    return $this;
} (home.accountInformation || {}, jQuery));
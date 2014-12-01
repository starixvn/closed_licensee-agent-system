/*global CORE, LANGUAGE, POPUP*/
'use strict';

var HOME = HOME || {};
HOME.ACCOUNT_INFORMATION = (function ($this, $) {
    $this.init = function () {
        var userName;

        // username suggestion
        $("#accountInfo #UserName").typeahead({
            source: getSuggestion,
            minLength: 2
        });

        //form submit
        $('#accountInfo form').submit(function (event) {
            userName = $.trim($("#UserName").val());
            if (userName !== '') {
                search(userName);
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
            changeAccountInfoActive();
        }
    };

    var changeAccountInfoActive = function () {
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

    var getSuggestion = function (query, process) {
        return $.ajax({
            url: CORE.resolveUrl('~/Home/AccountInformation/Suggest'),
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

    var search = function (userName) {
        $this.hidePopover();
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
                CORE.blockUI();
            },
            success: function (data) {
                if (typeof data === 'object') {
                    if (data.ErrCode === -4) {
                        $this.showNoUserAlert(data.ErrMsg);
                    }
                    else {
                        CORE.handleError(data);
                    }
                }
                else {
                    $('#searchResult').html(data);
                    HOME.ACCOUNT_INFORMATION.RESULT.init();
                }
            },
            complete: function () {
                CORE.unblockUI();
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
        var title = LANGUAGE.getString('Attention');
        var buttons = [{
            label: LANGUAGE.getString('Yes'),
            action: function () {
                POPUP.close();
                $this.changeCurrencyPage(currencyId, currencyName);
            }
        }, {
            label: LANGUAGE.getString('No'),
            action: function () {
                POPUP.close();
                $this.showNoUserAlert(msgAlert);
            }
        }];

        POPUP.confirm({
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
        CORE.ajaxJSON({
            url: $('#internal-form').attr('action'),
            data: data
        }, function () {
            if (HOME.TOP_MENU !== undefined) {
                $.cookie('username', $("#UserName").val(), { path: '/', expires: 1 });
                HOME.TOP_MENU.setCurrency(currencyId);
            }
        });
    };

    return $this;
} (HOME.ACCOUNT_INFORMATION || {}, jQuery));
/*global CORE, LANGUAGE, BET_LIST, refreshCountDownMethods*/
/*exported TOTAL_BETS*/
'use strict';

var TOTAL_BETS = (function ($this, $) {
    var $betListGridOptions;

    $this.init = function (betListGridOptions) {
        $betListGridOptions = betListGridOptions;
        $this.initCountDown();
    };

    $this.initCountDown = function () {
        $("#refresh-count-down").refreshCountDown({
            listTimeInterval: [15, 30, 45, 60, 75],
            timeIntervalName: "TimeInterval",
            timeIntervalValue: $("#server-time-interval").val(),
            formName: "main-form"
        });
    };

    $this.initSportMultiSelect = function (sportList, onChange) {
        $("#sport-multi-select").fanexMultiSelect({
            listData: sportList,
            showTooltip: true,
            maxHeight: 500,
            minWidth: 150,
            checkAllText: LANGUAGE.getString('AllSports'),
            noItemText: LANGUAGE.getString('NoSelectedSport'),
            noneSelectedText: LANGUAGE.getString('PleaseSelectSport'),
            selectedText: LANGUAGE.getString('NumberOfSelectedSports'),
            tooltipNoItem: LANGUAGE.getString('NoSelectedSport'),
            useCustomCheckAllText: true,
            customCheckAllText: LANGUAGE.getString('AllSports'),
            showParentSelectedText: false,
            showFooter: true,
            footerButtonText: LANGUAGE.getString('OK'),
            onLeave: function () {
                onChange();
            },
            onFooterButtonClick: function () {
                onChange();
            },
            onCheck: function () {
            },
            onCheckAll: function () {
            }
        });
    };

    $this.initLeagueMultiSelect = function (leagueList, onChange) {
        $("#league-multi-select").fanexMultiSelect({
            listData: leagueList,
            showTooltip: true,
            maxHeight: 500,
            minWidth: 300,
            checkAllText: LANGUAGE.getString('AllLeagues'),
            noItemText: LANGUAGE.getString('NoSelectedLeague'),
            noneSelectedText: LANGUAGE.getString('PleaseSelectLeague'),
            selectedText: LANGUAGE.getString('NumberOfSelectedLeagues'),
            tooltipNoItem: LANGUAGE.getString('NoSelectedLeague'),
            useCustomCheckAllText: true,
            customCheckAllText: LANGUAGE.getString('AllLeagues'),
            showParentSelectedText: false,
            showFooter: true,
            footerButtonText: LANGUAGE.getString('OK'),
            onLeave: function () {
                onChange();
            },
            onFooterButtonClick: function () {
                onChange();
            },
            onCheck: function () {
            },
            onCheckAll: function () {
            }
        });
    };

    $this.initEventMultiSelect = function (eventList, onChange) {
        $("#event-multi-select").fanexMultiSelect({
            listData: eventList,
            showTooltip: true,
            maxHeight: 500,
            minWidth: 250,
            checkAllText: LANGUAGE.getString('AllEvents'),
            noItemText: LANGUAGE.getString('NoSelectedEvent'),
            noneSelectedText: LANGUAGE.getString('PleaseSelectEvent'),
            selectedText: LANGUAGE.getString('NumberOfSelectedEvents'),
            tooltipNoItem: LANGUAGE.getString('NoSelectedEvent'),
            useCustomCheckAllText: true,
            customCheckAllText: LANGUAGE.getString('AllEvents'),
            showParentSelectedText: false,
            showFooter: true,
            footerButtonText: LANGUAGE.getString('OK'),
            onLeave: function () {
                onChange();
            },
            onFooterButtonClick: function () {
                onChange();
            },
            onCheck: function () {
            },
            onCheckAll: function () {
            }
        });
    };

    $this.initTeamMultiSelect = function (teamList, onChange) {
        $("#team-multi-select").fanexMultiSelect({
            listData: teamList,
            showTooltip: true,
            maxHeight: 500,
            minWidth: 250,
            checkAllText: LANGUAGE.getString('AllTeams'),
            noItemText: LANGUAGE.getString('NoSelectedTeam'),
            noneSelectedText: LANGUAGE.getString('PleaseSelectTeam'),
            selectedText: LANGUAGE.getString('NumberOfSelectedTeams'),
            tooltipNoItem: LANGUAGE.getString('NoSelectedTeam'),
            useCustomCheckAllText: true,
            customCheckAllText: LANGUAGE.getString('AllTeams'),
            showParentSelectedText: false,
            showFooter: true,
            footerButtonText: LANGUAGE.getString('OK'),
            onLeave: function () {
                onChange();
            },
            onFooterButtonClick: function () {
                onChange();
            },
            onCheck: function () {
            },
            onCheckAll: function () {
            }
        });
    };

    $this.reRenderAfterPaged = function () {
        CORE.ajaxJSON({
            url: $('form').attr('action'),
            data: {
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                MatchList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
            }
        }, function (data) {
            refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
            $('.freegrid').trigger('data', [data]);
        });
    };

    $this.generateBetListLink = function (matchId, bettype, betteam, showliveticket, linkvalue) {
        var linkHtmlString = "<span class=\"data-link\" onclick=\"TOTAL_BETS.showBetList(";
        linkHtmlString += matchId + ",'" + bettype + "','" + betteam + "'," + showliveticket + ")\">";
        linkHtmlString += linkvalue + "</span>";
        return linkHtmlString;
    };

    $this.showBetList = function (matchId, betType, betTeam, showliveticket) {
        refreshCountDownMethods.stopCountDown();
        $('form').hide();
        $("#totalbets-back").attr("class", "history-back");

        CORE.ajaxJSON({
            url: CORE.resolveUrl("~/BetList/TotalBets"),
            data: {
                MatchId: matchId,
                BetType: betType,
                BetTeam: betTeam,
                ShowLiveTicket: showliveticket
            }
        }, function (data) {
            BET_LIST.reRenderAfterPagedSuccessfully($betListGridOptions, data);
        });
    };

    $this.submitForm = function () {
        refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
        $("#totalbets-back").attr("class", "history-back hide");
        $('form').show().submit();
    };

    return $this;
} (TOTAL_BETS || {}, jQuery));
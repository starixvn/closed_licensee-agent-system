/*global core, language, refreshCountDownMethods*/

var totalBets = (function ($) {
    'use strict';
    var $betListGridOptions;
    return {
        init: function (betListGridOptions) {
            $betListGridOptions = betListGridOptions;
            totalBets.initCountDown();
        },
        initCountDown: function () {
            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: core.TimeInterval,
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });
        },
        initSportMultiSelect: function (sportList, onChange) {
            $("#sport-multi-select").fanexMultiSelect({
                listData: sportList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 150,
                checkAllText: language.getString('AllSports'),
                noItemText: language.getString('NoSelectedSport'),
                noneSelectedText: language.getString('PleaseSelectSport'),
                selectedText: language.getString('NumberOfSelectedSports'),
                tooltipNoItem: language.getString('NoSelectedSport'),
                useCustomCheckAllText: true,
                customCheckAllText: language.getString('AllSports'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: language.getString('OK'),
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
        },
        initLeagueMultiSelect: function (leagueList, onChange) {
            $("#league-multi-select").fanexMultiSelect({
                listData: leagueList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 300,
                checkAllText: language.getString('AllLeagues'),
                noItemText: language.getString('NoSelectedLeague'),
                noneSelectedText: language.getString('PleaseSelectLeague'),
                selectedText: language.getString('NumberOfSelectedLeagues'),
                tooltipNoItem: language.getString('NoSelectedLeague'),
                useCustomCheckAllText: true,
                customCheckAllText: language.getString('AllLeagues'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: language.getString('OK'),
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
        },
        initEventMultiSelect: function (eventList, onChange) {
            $("#event-multi-select").fanexMultiSelect({
                listData: eventList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 250,
                checkAllText: language.getString('AllEvents'),
                noItemText: language.getString('NoSelectedEvent'),
                noneSelectedText: language.getString('PleaseSelectEvent'),
                selectedText: language.getString('NumberOfSelectedEvents'),
                tooltipNoItem: language.getString('NoSelectedEvent'),
                useCustomCheckAllText: true,
                customCheckAllText: language.getString('AllEvents'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: language.getString('OK'),
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
        },
        initTeamMultiSelect: function (teamList, onChange) {
            $("#team-multi-select").fanexMultiSelect({
                listData: teamList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 250,
                checkAllText: language.getString('AllTeams'),
                noItemText: language.getString('NoSelectedTeam'),
                noneSelectedText: language.getString('PleaseSelectTeam'),
                selectedText: language.getString('NumberOfSelectedTeams'),
                tooltipNoItem: language.getString('NoSelectedTeam'),
                useCustomCheckAllText: true,
                customCheckAllText: language.getString('AllTeams'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: language.getString('OK'),
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
        },
        reRenderAfterPaged: function () {
            core.ajaxJSON({
                url: $('form').attr('action'),
                data: {
                    LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                    MatchList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
                }
            }, function (data) {
                refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
                $('#BetListGrid').trigger('data', [data]);
            });
        },
        generateBetListLink: function (matchId, bettype, betteam, showliveticket, linkvalue) {
            var linkHtmlString = "<span class=\"data-link\" onclick=\"totalBets.showBetList(";
            linkHtmlString += matchId + ",'" + bettype + "','" + betteam + "'," + showliveticket + ")\">";
            linkHtmlString += linkvalue + "</span>";
            return linkHtmlString;
        },
        showBetList: function (matchId, bettype, betteam, showliveticket) {
            refreshCountDownMethods.stopCountDown();
            $('form').hide();
            $("#totalbets-back").attr("class", "history-back");

            core.ajaxJSON({
                url: core.resolveUrl("~/Totalbets/Common/BetList"),
                data: {
                    MatchId: matchId,
                    Bettype: bettype,
                    Betteam: betteam,
                    ShowLiveTicket: showliveticket
                }
            }, function (data) {
                fanex.betlist.core.reRenderAfterPagedSuccessfully($betListGridOptions, data);
            });
        },
        submitForm: function () {
            refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
            $("#totalbets-back").attr("class", "history-back hide");
            $('form').show().submit();
        }
    };
} (jQuery));
var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "center", CanResize: true },
                { HeaderText: LANGUAGE.getString("TransTime"), Width: 100, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("Choice"), Width: 345, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Stake"), Width: 70, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("WinLoss"), Width: 60, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
            ],
    showPager: true,
    gridExpandHeight: 100,
    bodyRowHeight: 29,
    autoResizeColumn: true
};

var fanex = fanex || {};
fanex.totalBets = fanex.totalBets || {};
fanex.totalBets.parlay = {
    init: function () {

        $('.selectpicker').selectpicker();

        // search
        $('#search-form').submit(function (e) {
            fanex.totalBets.parlay.reRenderAfterPaged(0, CORE.pageSizes[0]);
            e.preventDefault();
        });

        $("#mixParlayTab").click(function () {
            fanex.totalBets.parlay.switchParlayType(true);
        });

        $("#systemParlayTab").click(function () {
            fanex.totalBets.parlay.switchParlayType(false);
        });

        // get total bets mix parlay 
        fanex.totalBets.parlay.switchParlayType(true);
    },
    reRenderAfterPaged: function (currentPage, itemsPerPage, betType) {
        if (gridOptions && gridOptions.pagerOption) {
            currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
            itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
        }

        var params = {
            BetType: betType
        };

        $.ajax({
            type: 'post',
            data: params,
            beforeSend: function () {
                CORE.blockUI();
            },
            success: function (data) {
                fanex.betlist.core.reRenderAfterPagedSuccessfully(data, [10]);
            },
            complete: function () {
                CORE.unblockUI();
            }
        });
    },
    switchParlayType: function (isMixParlay) {
        if (isMixParlay) {
            $("#mixParlayTab").removeClass('inactive-tab');
            $("#mixParlayTab").addClass('active-tab');
            $("#systemParlayTab").removeClass('active-tab');
            $("#systemParlayTab").addClass('inactive-tab');
            fanex.totalBets.parlay.reRenderAfterPaged(0, CORE.pageSizes[0], 9);
        }
        else {
            $("#mixParlayTab").removeClass('active-tab');
            $("#mixParlayTab").addClass('inactive-tab');
            $("#systemParlayTab").removeClass('inactive-tab');
            $("#systemParlayTab").addClass('active-tab');
            fanex.totalBets.parlay.reRenderAfterPaged(0, CORE.pageSizes[0], 29);
        }
    },
    showParlayDetails: function (transId, status) {
        fanex.betlist.core.showDetails(transId, status);
    },
    showSystemParlayDetails: function (refNo, custId, betTeam) {
        fanex.betlist.core.showSystemParlayDetails(refNo, custId, betTeam);
    }
};

$(function () {
    fanex.totalBets.parlay.init();
});
/*global CORE, LANGUAGE, GRID, pageContext*/
'use strict';

var gridOptions = {
    columns: [
        { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
        { HeaderText: LANGUAGE.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
        { HeaderText: LANGUAGE.getString("Choice"), Width: 335, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: LANGUAGE.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: LANGUAGE.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: LANGUAGE.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: LANGUAGE.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
        { HeaderText: LANGUAGE.getString("AgentPT"), Width: 75, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: LANGUAGE.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: LANGUAGE.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
    ],
    showPager: true,
    enableHiddenColumns: false
};

var BET_LIST = BET_LIST || {};
BET_LIST.WINLOSS = (function ($this, $) {
    $.fn.setBreadCrumb = function (agentId, agentLevel, username) {
        var currentBreadCrumb = $(this).val();
        var breadCrumb = agentId + "," + agentLevel + "," + username;
        var newBreadCrumb = "";
        if (currentBreadCrumb.indexOf(breadCrumb) >= 0) {
            {
                newBreadCrumb = currentBreadCrumb.substring(0, currentBreadCrumb.indexOf(breadCrumb) + breadCrumb.length);
            }
        } else {
            newBreadCrumb = currentBreadCrumb + ";" + breadCrumb;
        }
        $(this).val(newBreadCrumb);
    };

    $this.init = function () {
        // breadcrum
        $('.breadcrumb a').click(function () {
            var agentId = $(this).data('agent-id');
            var agentLevel = $(this).data('agent-level');
            var agentName = $(this).text();
            viewDownline(agentId, agentLevel, agentName);
        });

        var hiddenColumns = [];
        if (pageContext.LegendType === 1) {
            hiddenColumns.push(8);
        }
        if (pageContext.IsLastLevel) {
            hiddenColumns.push(9);
        }
        gridOptions.hiddenColumns = hiddenColumns;

        // get bet list
        reRenderAfterPaged(0, GRID.PageSizes[0]);
    };

    var reRenderAfterPaged = function (currentPage, itemsPerPage) {
        if (gridOptions && gridOptions.pagerOption) {
            currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
            itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
        }

        CORE.ajaxJSON({
            url: $('#search-form').attr('action'),
            data: $('form').serialize()
        }, function (data) {
            BET_LIST.reRenderAfterPagedSuccessfully(gridOptions, data, true);
        });
    };

    $this.showBetListDetails = function (transId, status) {
        BET_LIST.showDetails(transId, status);
    };

    $this.showMixParlayDetail = function (transId, divEvent) {
        var loadMixParlayDetail = function (transId) {
            var url = CORE.resolveUrl('~/BetList/MixParlayDetails', {
                transId: transId
            });
            CORE.ajaxHTML({
                url: url
            }, function (response) {
                divEvent.html(response);
                $(".line:last", divEvent).remove();
                $('.freegrid').fanexGrid('resizeGrid');
            });
        };

        divEvent.toggle();
        var dataExisted = $("#divEvent_" + transId).text().length > 0;
        if (!dataExisted) {
            loadMixParlayDetail(transId);
        } else {
            $('.freegrid').fanexGrid('resizeGrid');
        }
    };

    var viewDownline = function (agentId, agentLevel, username) {
        $("#BreadCrumb").setBreadCrumb(agentId, agentLevel, username);
        var url = CORE.resolveUrl('~/Report/AgentWinLoss', {
            agentId: agentId,
            agentLevel: agentLevel,
            username: username,
            drawFilter: $('#DrawFilter').val(),
            breadcrumb: $("#BreadCrumb").val(),
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            Product: $("#ProductIds").val()
        });
        location.href = url;
    };

    return $this;
} (BET_LIST.WINLOSS || {}, jQuery));
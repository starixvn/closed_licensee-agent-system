/*global core, language, showMixParlayDetailurl */

var outstandingGridOptions = {
    /*jshint strict:false */
    columns: [
                { HeaderText: language.getString("Agent"), Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter:
                    function (rowIndex, rowData, cell) {
                        return '<a href="#" onclick="outstandingReport.outstandingDrilldown(\'' + rowData.Username
                                + '\',' + rowData.Id + ',' + cell.IsLastLevel + ')">' + rowData.Username + '</a>';
                    }
                },
                { HeaderText: language.getString("Outstanding"), Width: 200, Name: "Outstanding",
                    HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)'
                },
                { HeaderText: language.getString("AgentShare"), Width: 200, Name: "Positiontaking",
                    HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)'
                }
            ],
    showPager: false,
    showFooter: true,
    gridExpandHeight: 60,
    bodyRowHeight: 29
};

var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: language.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: language.getString("Choice"), Width: 345, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Stake"), Width: 70, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("WinLoss"), Width: 60, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: language.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
            ],
    showPager: true,
    gridExpandHeight: 100,
    bodyRowHeight: 29
};

var originalBreadcrumb = '';
var agentIdLevelN = 0;
var outstandingReport = (function ($) {
    /*jshint strict:false */
    var breadCrumbItems = [
    {
        agentId: $('#loginAgent').data('id'),
        agentName: $('#loginAgent').data('name')
    }];

    return {
        init: function () {
            outstandingReport.reRenderAfterPaged(0, core.pageSizes[0], -1);
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage, agentId) {
            if (outstandingGridOptions && outstandingGridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                                                : outstandingGridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || outstandingGridOptions.pagerOption.itemsPerPage;
            }

            var params = {
                AgentId: agentId
            };

            $.ajax({
                url: "Outstanding",
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        $("#breadcrumb > span").html(originalBreadcrumb);
                        alert(data.ErrMsg);
                        return;
                    }

                    var footerRows = [
                            {
                                Columns: [
                                    { Value: language.getString("Total"), Name: "Username", CellAlign: "left" },
                                    { Value: data.SumOutstanding, Name: "Outstanding" },
                                    { Value: data.SumPositiontaking, Name: "Positiontaking" }
                                ]
                            }
                    ];

                    outstandingGridOptions.pagerOption = {
                        itemsPerPage: core.pageSizes[0],
                        itemsPerPageArray: core.pageSizes,
                        currentPage: 0,
                        showDetail: true
                    };
                    outstandingGridOptions.bodyRows = core.genGridRows(data.List, outstandingGridOptions);

                    outstandingGridOptions.footerRows = footerRows;
                    if (data.IsLastLevel === true) {
                        outstandingGridOptions.columns[0].HeaderText = language.getString("Member");
                    }
                    else {
                        outstandingGridOptions.columns[0].HeaderText = language.getString("Agent");
                    }

                    for (var i = 0; i < outstandingGridOptions.bodyRows.length; i++) {
                        var currentRow = outstandingGridOptions.bodyRows[i];
                        currentRow.Columns[0].IsLastLevel = data.IsLastLevel;
                    }

                    $('#BetListGrid').show().fanexGrid(outstandingGridOptions);
                    $("#breadcrumb").attr("class", "breadcrumb");
                    originalBreadcrumb = $("#breadcrumb > span").html();
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        reRenderBetListAfterPaged: function (currentPage, itemsPerPage, custId, agentId, hidedColumn) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }

            var params = {
                AgentId: agentId,
                CustId: custId
            };

            $.ajax({
                url: "OutstandingBetList",
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        $("#breadcrumb > span").html(originalBreadcrumb);
                        alert(data.ErrMsg);
                        return;
                    }

                    gridOptions.pagerOption = {
                        itemsPerPage: core.pageSizes[0],
                        itemsPerPageArray: core.pageSizes,
                        currentPage: 0,
                        showDetail: true
                    };
                    gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                    $('#BetListGrid').show().fanexGrid(gridOptions);

                    if (data.IsLastLevel === true) {
                        $("#BetListGrid").fanexGrid("hideColumns", gridOptions, hidedColumn);
                        $("#breadcrumb").attr("class", "breadcrumb");
                    }
                    else {
                        $("#breadcrumb").attr("class", "breadcrumb");
                    }
                    originalBreadcrumb = $("#breadcrumb > span").html();
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        showBetListDetails: function (transId, status) {
            fanex.betlist.core.showDetails(transId, status);
        },
        showMixParlayDetail: function (transId, divEvent) {
            var loadMixParlayDetail = function (transId) {
                var url = showMixParlayDetailurl + transId;
                $.ajax({
                    type: "GET",
                    contentType: "text/html",
                    cache: false,
                    url: core.resolveUrl(url),
                    error: function (xhRequest, errorText) {
                        alert(errorText);
                    },
                    success: function (response) {
                        divEvent.html(response);
                        $(".line:last", divEvent).remove();
                        $('#BetListGrid').fanexGrid('resizeGrid');
                    },
                    beforeSend: function () {
                        core.blockUI();
                    },
                    complete: function () {
                        core.unblockUI();
                    }
                });
            };

            var dataExisted = $("#divEvent_" + transId).text().length > 0;
            if (!dataExisted) {
                loadMixParlayDetail(transId);
            }

            divEvent.toggle();
        },
        showSystemParlayResult: function (refNo, custId, betTeam) {
            fanex.betlist.core.showSystemParlayDetails(refNo, custId, betTeam);
        },
        outstandingDrilldown: function (agentName, agentId, isLastLevel) {
            if (isLastLevel) {
                outstandingReport.reRenderBetListAfterPaged(0, core.pageSizes[0], agentId, agentIdLevelN, [9]);
            }
            else {
                breadcrumb += agentId + ')" class="link-disable">' + agentName + '</a>';
                agentIdLevelN = agentId;
                outstandingReport.reRenderAfterPaged(0, core.pageSizes[0], agentId);
            }
            if (typeof isLastLevel === 'undefined') {
                outstandingReport.createBreadcrumb(agentId, agentName, false);
            } else {
                outstandingReport.createBreadcrumb(agentId, agentName, true);   
            }
        },
        breadcrumbDrilldown: function (agentName, agentId) {
            outstandingReport.outstandingDrilldown(agentName, agentId);
        },
        createBreadcrumb: function(agentId, agentName, isDownline) {
            var index,
                numberAgent = breadCrumbItems.length - 1,
                breadCrumbHtml = '',
                matchIndex = numberAgent;
            if (isDownline) {
                breadCrumbItems.push({
                    agentId: agentId,
                    agentName: agentName
                });
            } else {
                for (index = numberAgent; index >= 0; index--) {
                    if (breadCrumbItems[index].agentId === agentId) {
                        matchIndex = index;
                        break;
                    }
                }
                breadCrumbItems.splice(matchIndex + 1);
            }

            numberAgent = breadCrumbItems.length - 1;
            if (numberAgent > 0) {
                for (index = 0; index < numberAgent; index ++) {
                    breadCrumbHtml += '<li><a onclick="javascript:outstandingReport.breadcrumbDrilldown(\''
                        + breadCrumbItems[index].agentName + '\','
                        + breadCrumbItems[index].agentId + ');" href="#">'
                        + breadCrumbItems[index].agentName
                        + '</a><span class="divider">&gt;&gt;</span></li>';
                }
                breadCrumbHtml += '<li><span>' + breadCrumbItems[index].agentName + '</span></li>';
            }
            $(".breadcrumb").html(breadCrumbHtml);
        }
    };
} (jQuery));

$(function () {
    /*jshint strict:false */
    outstandingReport.init();
});
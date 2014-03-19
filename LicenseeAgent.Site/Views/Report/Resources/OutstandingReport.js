/*global CORE, LANGUAGE, showMixParlayDetailurl*/

var outstandingGridOptions = {
    /*jshint strict:false */
    columns: [
                { HeaderText: LANGUAGE.getString("Agent"), Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter:
                    function (rowIndex, rowData, cell) {
                        return '<a href="#" onclick="Outstanding.outstandingDrilldown(\'' + rowData.Username 
                                + '\',' + rowData.Id + ',' + cell.IsLastLevel + ')">' + rowData.Username + '</a>';
                    } 
                },
                { HeaderText: LANGUAGE.getString("Outstanding"), Width: 200, Name: "Outstanding", 
                  HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)' },
                { HeaderText: LANGUAGE.getString("AgentShare"), Width: 200, Name: "Positiontaking", 
                  HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)' }
            ],
    showPager: false,
    showFooter: true,
    gridExpandHeight: 60,
    bodyRowHeight: 29
};

var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
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
    bodyRowHeight: 29
};

var originalBreadcrumb = '';
var agentIdLevelN = 0;
var Outstanding = (function ($) {
    /*jshint strict:false */
    var breadCrumbItems = [
    {
        agentId: $('#loginAgent').data('id'),
        agentName: $('#loginAgent').data('name')
    }];

    return {
        init: function () {
            Outstanding.reRenderAfterPaged(0, CORE.pageSizes[0], -1);
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
                    CORE.blockUI();
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
                                    { Value: LANGUAGE.getString("Total"), Name: "Username", CellAlign: "left" },
                                    { Value: data.SumOutstanding, Name: "Outstanding" },
                                    { Value: data.SumPositiontaking, Name: "Positiontaking" }
                                ]
                            }
                    ];

                    outstandingGridOptions.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
                        itemsPerPageArray: CORE.pageSizes,
                        currentPage: 0,
                        showDetail: true
                    };
                    outstandingGridOptions.bodyRows = CORE.genGridRows(data.List, outstandingGridOptions);

                    outstandingGridOptions.footerRows = footerRows;
                    if (data.IsLastLevel === true) {
                        outstandingGridOptions.columns[0].HeaderText = LANGUAGE.getString("Member");
                    }
                    else {
                        outstandingGridOptions.columns[0].HeaderText = LANGUAGE.getString("Agent");
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
                    CORE.unblockUI();
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
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        $("#breadcrumb > span").html(originalBreadcrumb);
                        alert(data.ErrMsg);
                        return;
                    }

                    gridOptions.pagerOption = {
                        itemsPerPage: CORE.pageSizes[0],
                        itemsPerPageArray: CORE.pageSizes,
                        currentPage: 0,
                        showDetail: true
                    };
                    gridOptions.bodyRows = CORE.genGridRows(data.List, gridOptions);
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
                    CORE.unblockUI();
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
                    url: CORE.resolveUrl(url),
                    error: function (xhRequest, errorText) {
                        alert(errorText);
                    },
                    success: function (response) {
                        divEvent.html(response);
                        $(".line:last", divEvent).remove();
                        $('#BetListGrid').fanexGrid('resizeGrid');
                    },
                    beforeSend: function () {
                        CORE.blockUI();
                    },
                    complete: function () {
                        CORE.unblockUI();
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
                Outstanding.reRenderBetListAfterPaged(0, CORE.pageSizes[0], agentId, agentIdLevelN, [9]);
            }
            else {
                breadcrumb += agentId + ')" class="link-disable">' + agentName + '</a>';
                agentIdLevelN = agentId;
                Outstanding.reRenderAfterPaged(0, CORE.pageSizes[0], agentId);
            }
            if (typeof isLastLevel === 'undefined') {
                OUTSTANDING.createBreadcrumb(agentId, agentName, false);
            } else {
                OUTSTANDING.createBreadcrumb(agentId, agentName, true);   
            }
        },
        breadcrumbDrilldown: function (agentName, agentId) {
            Outstanding.outstandingDrilldown(agentName, agentId);
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
                    breadCrumbHtml += '<li><a onclick="javascript:OUTSTANDING.breadcrumbDrilldown(\''
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
    Outstanding.init();
});
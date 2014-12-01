/*global CORE, LANGUAGE, GRID, BREADCRUMB*/
'use strict';

var REPORT = REPORT || {};
REPORT.AGENT_OUTSTANDING = (function ($this, $) {
    var isShowBetList = false;

    /* jshint -W101 */
    var outstandingGridOptions = {
        columns: [
            { HeaderText: LANGUAGE.getString("Agent"), Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter:
                function (rowIndex, rowData) {
                    if (rowData.Id === 0) {
                        return '<a href="#" class="text-direct-member" onclick="REPORT.AGENT_OUTSTANDING.showMember()">{0}</a>'.format(LANGUAGE.getString('DirectMembers'));
                    } else {
                        if (isShowBetList) {
                            return '<a href="#" onclick="REPORT.AGENT_OUTSTANDING.showBetList({0}, \'{1}\')">{1}</a>'.format(rowData.Id, rowData.Username);
                        } else {
                            return '<a href="#" onclick="REPORT.AGENT_OUTSTANDING.showAgent({0}, \'{1}\')">{1}</a>'.format(rowData.Id, rowData.Username);
                        }
                    }
                }
            },
            { HeaderText: LANGUAGE.getString("Outstanding"), Width: 200, Name: "Outstanding",
                HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)'
            },
            { HeaderText: LANGUAGE.getString("AgentShare"), Width: 200, Name: "Positiontaking",
                HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)'
            }
        ],
        pagerOption: {
            currentPage: 0,
            itemsPerPage: GRID.PageSizes[0],
            itemsPerPageArray: GRID.PageSizes,
            showDetail: true
        },
        showPager: false,
        showFooter: true
    };

    var gridOptions = {
        columns: [
            { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
            { HeaderText: LANGUAGE.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: LANGUAGE.getString("Choice"), Width: 345, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: LANGUAGE.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: LANGUAGE.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
        ],
        pagerOption: {
            itemsPerPage: GRID.PageSizes[0],
            itemsPerPageArray: GRID.PageSizes,
            currentPage: 0,
            showDetail: true
        },
        showPager: true,
        enableHiddenColumns: false
    };
    /* jshint +W101 */

    $this.init = function () {
        var breadcrumb = BREADCRUMB.init();
        breadcrumb.on('itemClicked', function (item, data) {
            if (data.AgentId > 0) {
                showData(data.AgentId, false);
                var lastItem = BREADCRUMB.getLastItem();
                lastItem.IsLink = false;
                BREADCRUMB.render();
            }
        });
        var agentId = $('#AgentId').val();
        showData(agentId, false);
    };

    $this.showMember = function () {
        var agentId = $('#AgentId').val();
        var item = BREADCRUMB.getLastItem();
        item.IsLink = true;
        BREADCRUMB.render();
        showData(agentId, true);
    };

    $this.showAgent = function (agentId, userName) {
        BREADCRUMB.addItem({
            Text: userName,
            Data: {
                AgentId: agentId
            }
        });

        showData(agentId, false);
    };

    var showData = function (agentId, showMembers) {
        if (showMembers) {
            isShowBetList = true;
            outstandingGridOptions.columns[0].HeaderText = LANGUAGE.getString("Member");
        }
        else {
            isShowBetList = false;
            outstandingGridOptions.columns[0].HeaderText = LANGUAGE.getString("Agent");
        }

        // update form input
        $('#AgentId').val(agentId);
        $('#ShowMembers').val(showMembers);
        CORE.ajaxJSON({
            url: $('form').attr('action'),
            data: $('form').serializeObject()
        }, function (data) {
            var footerRows = [
                {
                    Columns: [
                        { Value: LANGUAGE.getString("Total"), Name: "Username", CellAlign: "left" },
                        { Value: data.SumOutstanding, Name: "Outstanding" },
                        { Value: data.SumPositiontaking, Name: "Positiontaking" }
                    ]
                }
            ];

            outstandingGridOptions.bodyRows = GRID.generateBodyRows(data.List, outstandingGridOptions);
            outstandingGridOptions.footerRows = footerRows;
            $('.freegrid').show().fanexGrid(outstandingGridOptions);
        });
    };

    $this.showBetList = function (custId, userName) {
        BREADCRUMB.addItem({
            Text: userName,
            Data: {
                CustId: custId
            }
        });

        CORE.ajaxJSON({
            url: CORE.resolveUrl('~/BetList/Outstanding'),
            data: {
                CustId: custId
            }
        }, function (data) {
            var hiddenColumns = [];
            if (data.LegendType === 1) {
                hiddenColumns.push(8);
            }
            if (data.IsLastLevel) {
                hiddenColumns.push(9);
            }
            gridOptions.hiddenColumns = hiddenColumns;
            gridOptions.bodyRows = GRID.generateBodyRows(data.List, gridOptions);
            $('.freegrid').show().fanexGrid(gridOptions);
        });
    };

    $this.showBetListDetails = function (transId, status) {
        fanex.betlist.core.showDetails(transId, status);
    };

    return $this;
} (REPORT.AGENT_OUTSTANDING || {}, jQuery));
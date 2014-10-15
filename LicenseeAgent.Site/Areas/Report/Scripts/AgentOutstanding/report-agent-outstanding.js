/*global core, language */

var report = report || {};
report.agentOutstanding = (function ($) {
    'use strict';

    var isShowBetList = false;

    /* jshint -W101 */
    var outstandingGridOptions = {
        columns: [
            { HeaderText: language.getString("Agent"), Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left", Formatter:
                function (rowIndex, rowData) {
                    if (rowData.Id === 0) {
                        return '<a href="#" class="text-direct-member" onclick="report.agentOutstanding.showMember()">{0}</a>'.format(language.getString('DirectMembers'));
                    } else {
                        if (isShowBetList) {
                            return '<a href="#" onclick="report.agentOutstanding.showBetList({0}, \'{1}\')">{1}</a>'.format(rowData.Id, rowData.Username);
                        } else {
                            return '<a href="#" onclick="report.agentOutstanding.showAgent({0}, \'{1}\')">{1}</a>'.format(rowData.Id, rowData.Username);
                        }
                    }
                }
            },
            { HeaderText: language.getString("Outstanding"), Width: 200, Name: "Outstanding",
                HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)'
            },
            { HeaderText: language.getString("AgentShare"), Width: 200, Name: "Positiontaking",
                HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '($)'
            }
        ],
        pagerOption: {
            currentPage: 0,
            itemsPerPage: core.pageSizes[0],
            itemsPerPageArray: core.pageSizes,
            showDetail: true
        },
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
            { HeaderText: language.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: language.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: language.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: language.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: language.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: language.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
        ],
        pagerOption: {
            itemsPerPage: core.pageSizes[0],
            itemsPerPageArray: core.pageSizes,
            currentPage: 0,
            showDetail: true
        },
        showPager: true,
        enableHiddenColumns: false,
        gridExpandHeight: 100,
        bodyRowHeight: 29
    };
    /* jshint +W101 */

    return {
        init: function () {
            var breadcrumb = core.breadcrumb.init();
            breadcrumb.on('itemClicked', function (item, data) {
                if (data.AgentId > 0) {
                    report.agentOutstanding.showData(data.AgentId, false);
                    var lastItem = core.breadcrumb.getLastItem();
                    lastItem.IsLink = false;
                    core.breadcrumb.render();
                }
            });
            var agentId = $('#AgentId').val();
            report.agentOutstanding.showData(agentId, false);
        },
        showMember: function () {
            var agentId = $('#AgentId').val();
            var item = core.breadcrumb.getLastItem();
            item.IsLink = true;
            core.breadcrumb.render();
            report.agentOutstanding.showData(agentId, true);
        },
        showAgent: function (agentId, userName) {
            core.breadcrumb.addItem({
                Text: userName,
                Data: {
                    AgentId: agentId
                }
            });

            report.agentOutstanding.showData(agentId, false);
        },
        showData: function (agentId, showMembers) {
            if (showMembers) {
                isShowBetList = true;
                outstandingGridOptions.columns[0].HeaderText = language.getString("Member");
            }
            else {
                isShowBetList = false;
                outstandingGridOptions.columns[0].HeaderText = language.getString("Agent");
            }

            // update form input
            $('#AgentId').val(agentId);
            $('#ShowMembers').val(showMembers);
            core.ajaxJSON({
                url: $('form').attr('action'),
                data: $('form').serializeObject()
            }, function (data) {
                var footerRows = [
                    {
                        Columns: [
                            { Value: language.getString("Total"), Name: "Username", CellAlign: "left" },
                            { Value: data.SumOutstanding, Name: "Outstanding" },
                            { Value: data.SumPositiontaking, Name: "Positiontaking" }
                        ]
                    }
                ];

                outstandingGridOptions.bodyRows = core.genGridRows(data.List, outstandingGridOptions);
                outstandingGridOptions.footerRows = footerRows;
                $('#BetListGrid').show().fanexGrid(outstandingGridOptions);
            });
        },
        showBetList: function (custId, userName) {
            core.breadcrumb.addItem({
                Text: userName,
                Data: {
                    CustId: custId
                }
            });

            core.ajaxJSON({
                url: core.resolveUrl('~/Report/AgentOutstanding/BetList'),
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
                gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                $('#BetListGrid').show().fanexGrid(gridOptions);
            });
        },
        showBetListDetails: function (transId, status) {
            fanex.betlist.core.showDetails(transId, status);
        }
    };
} (jQuery));
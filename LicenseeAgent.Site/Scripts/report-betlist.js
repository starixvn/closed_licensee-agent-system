/*global core, language*/

var report = report || {};
report.betlist = (function ($) {
    'use strict';

    var gridOptions = {
        columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: language.getString("TransTime"), Width: 90, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: language.getString("Choice"), Width: 335, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: language.getString("AgentPT"), Width: 75, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: language.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
            ],
        showPager: true,
        pagerOption: {
            currentPage: 0,
            itemsPerPage: core.pageSizes[0],
            itemsPerPageArray: core.pageSizes,
            showDetail: true
        },
        enableHiddenColumns: false,
        bodyRowHeight: 29
    };

    return {
        render: function (setting) {
            var grid;
            if (typeof setting.Container === 'undefined') {
                grid = $('.freegrid');
            } else {
                grid = $(setting.Container);
            }
            core.ajaxJSON({
                url: core.resolveUrl('~/BetList'),
                data: setting.Input
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
                core.genGridTotalRows(gridOptions.bodyRows, gridOptions);

                if (setting.ShowTotalRow) {
                    gridOptions.showFooter = true;
                    var footerTitle = '<div>{0}:</div><div>{1}:</div><div>{2}:</div>'.format(
                        language.getString('SubTotalWinLost'),
                        language.getString('SubTotalComm'),
                        language.getString('Total')
                    );
                    var footerValue = '<div>{0}</div><div>{1}</div><div>{2}</div>'.format(
                        data.SubTotalWinLost.formatNegative(),
                        data.SubTotalComm.formatNegative(),
                        data.TotalWinLost.formatNegative()
                    );
                    gridOptions.footerRows = [
                        {
                            Columns: [
                                { Value: footerTitle, Name: "Index", CellAlign: "right", MergeColNumber: 5 },
                                { Value: footerValue, Name: "WinLoss", CellAlign: "right" }
                            ]
                        }
                    ];
                }

                grid.show().fanexGrid(gridOptions);
            });
        }
    };
} (jQuery));
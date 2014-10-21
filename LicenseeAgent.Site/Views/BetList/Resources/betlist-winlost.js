/*global core, language, showMixParlayDetailurl, pageContext*/

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
    enableHiddenColumns: false,
    bodyRowHeight: 29
};

var betList = (function ($) {
    'use strict';
    return {
        init: function () {
            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id');
                var agentLevel = $(this).data('agent-level');
                var agentName = $(this).text();
                betList.ViewDownline(agentId, agentLevel, agentName);
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
            betList.reRenderAfterPaged(0, core.pageSizes[0]);
        },
        reRenderAfterPaged: function (currentPage, itemsPerPage) {
            if (gridOptions && gridOptions.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage
                                                : gridOptions.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
            }

            var urlGetBetList = $('#search-form').attr('action');
            var submitOptions = $('form').serializeObject();

            $.ajax({
                url: urlGetBetList,
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data, true);
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
                    beforeSend: function () {
                        core.blockUI();
                    },
                    success: function (response) {
                        divEvent.html(response);
                        $(".line:last", divEvent).remove();
                        $('.freegrid').fanexGrid('resizeGrid');
                    },
                    complete: function () {
                        core.unblockUI();
                    }
                });
            };

            divEvent.toggle();
            var dataExisted = $("#divEvent_" + transId).text().length > 0;
            if (!dataExisted) {
                loadMixParlayDetail(transId);
            } else {
                $('.freegrid').fanexGrid('resizeGrid');
            }
        },
        ViewDownline: function (agentId, agentLevel, username) {
            $("#BreadCrumb").setBreadCrumb(agentId, agentLevel, username);
            location.href = core.resolveUrl("~/Report/AgentWinLoss?agentId=" + agentId + "&agentLevel=" + agentLevel
                                    + "&username=" + username + "&drawFilter=" + $('#DrawFilter').val() + "&breadcrumb=" + $("#BreadCrumb").val()
                                    + "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val() + "&Product=" + $("#ProductIds").val());
        }
    };
} (jQuery));

$(function () {
    'use strict';
    betList.init();
});
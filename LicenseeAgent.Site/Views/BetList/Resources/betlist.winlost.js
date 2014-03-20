/*global CORE, LANGUAGE, showMixParlayDetailurl*/

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

var betList = (function ($) {
    /*jshint strict:false */
    return {
        init: function () {
            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id');
                var agentLevel = $(this).data('agent-level');
                var agentName = $(this).text();
                betList.ViewDownline(agentId, agentLevel, agentName);
            });

            // get bet list
            betList.reRenderAfterPaged(0, CORE.pageSizes[0]);
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
                    CORE.blockUI();
                },
                success: function (data) {
                    fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data, [9]);
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
                    /*jshint unused:false*/
                    error: function (xhRequest, errorText, thrownError) {
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
        ViewDownline: function (agentId, agentLevel, username) {
            $("#BreadCrumb").setBreadCrumb(agentId, agentLevel, username);
            location.href = CORE.resolveUrl("~/Report/AgentWinLoss?agentId=" + agentId + "&agentLevel=" + agentLevel
                                            + "&username=" + username + "&drawFilter=" + $('#DrawFilter').val() + "&breadcrumb=" 
                                            + $("#BreadCrumb").val() + "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val());
        }
    };
} (jQuery));

$(function () {
    /*jshint strict:false */
    betList.init();
});
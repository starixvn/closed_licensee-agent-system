/*global gridOptions, CORE, pageContext, LANGUAGE*/

var agentWinLoss = (function ($) {
    /*jshint strict:false */
    return {
        init: function () {
            $("#tabs-area a:first").tab('show');
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();

            $('#search').click(function () {
                agentWinLoss.dataGridBinding();
                return false;
            });
            $('#today').click(function () {
                var today = $(this).data('date');
                $('#fromDatePicker').datepicker('update', today);
                $('#toDatePicker').datepicker('update', today);
                agentWinLoss.dataGridBinding();
                return false;
            });
            $('#yesterday').click(function () {
                var yesterday = $(this).data('date');
                $('#fromDatePicker').datepicker('update', yesterday);
                $('#toDatePicker').datepicker('update', yesterday);
                agentWinLoss.dataGridBinding();
                return false;
            });

            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id');
                var agentLevel = $(this).data('agent-level');
                var agentName = $(this).text();
                agentWinLoss.ViewDownline(agentId, agentLevel, agentName);
            });

            agentWinLoss.dataGridBinding();
        },

        dataGridBinding: function () {
            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: CORE.resolveUrl("~/Report/GetAgentWinLossData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        gridOptions.bodyRows = CORE.genGridRows(data.List, gridOptions);
                        if (data.List.length > 0) {
                            var totalRow = data.Total[0];
                            if (pageContext.IsLastLevel) {
                                gridOptions.footerRows = agentWinLoss.footerMember(totalRow);
                            }
                            else {
                                if (pageContext.AgentLevel === 1) {
                                    gridOptions.footerRows = agentWinLoss.footerLicensee(totalRow);
                                }
                                else {
                                    gridOptions.footerRows = agentWinLoss.footerAgent(totalRow);
                                }
                            }
                        }
                        $('#grid').show().fanexGrid(gridOptions);
                        CORE.unblockUI();
                        //customerList.resizeBox();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        footerMember: function (total) {
            var footerRows = [
                                {
                                    Columns: [
                                        { Value: LANGUAGE.getString("Total"), Name: "Username", CellAlign: "center" },
                                        { Value: total.BetCount, Name: "BetCount" },
                                        { Value: total.Turnover, Name: "Turnover" },
                                        { Value: total.Winlost, Name: "Winlost" },
                                        { Value: total.PlayerComm, Name: "PlayerComm" },
                                        { Value: total.MemberTotal, Name: "MemberTotal" },
                                        { Value: total.AWinLost, Name: "AWinLost" },
                                        { Value: total.AComm, Name: "AComm" },
                                        { Value: total.ATotal, Name: "ATotal" },
                                        { Value: total.UplineStake, Name: "UplineStake" },
                                        { Value: total.UplineTotal, Name: "UplineTotal" }
                                    ]
                                }
                        ];
            return footerRows;
        },

        footerAgent: function (total) {
            var footerRows = [
                    {
                        Columns: [
                            { Value: LANGUAGE.getString("Total"), Name: "Username", CellAlign: "center" },
                            { Value: total.BetCount, Name: "BetCount" },
                            { Value: total.Turnover, Name: "Turnover" },
                            { Value: total.MemberTotal, Name: "MemberTotal" },
                            { Value: total.DownlineStake, Name: "DownlineStake" },
                            { Value: total.DownlineTotal, Name: "DownlineTotal" },
                            { Value: total.AWinLost, Name: "AWinLost" },
                            { Value: total.AComm, Name: "AComm" },
                            { Value: total.ATotal, Name: "ATotal" },
                            { Value: total.UplineStake, Name: "UplineStake" },
                            { Value: total.UplineTotal, Name: "UplineTotal" }
                        ]
                    }
                ];
            return footerRows;
        },
        footerLicensee: function (total) {
            var footerRows = [
                {
                    Columns: [
                            { Value: LANGUAGE.getString("Total"), Name: "Username", CellAlign: "center" },
                            { Value: total.BetCount, Name: "BetCount" },
                            { Value: total.Turnover, Name: "Turnover" },
                            { Value: total.MemberTotal, Name: "MemberTotal" },
                            { Value: total.DownlineStake, Name: "DownlineStake" },
                            { Value: total.DownlineTotal, Name: "DownlineTotal" },
                            { Value: total.AWinLost, Name: "AWinLost" },
                            { Value: total.AComm, Name: "AComm" },
                            { Value: total.ATotal, Name: "ATotal" }
                        ]
                }
            ];
            return footerRows;
        },

        ViewDownline: function (agentId, agentLevel, username) {
            $("#BreadCrumb").setBreadCrumb(agentId, agentLevel, username);
            location.href = CORE.resolveUrl("~/Report/AgentWinLoss?agentId=" + agentId + "&agentLevel=" + agentLevel + "&username=" + username
                                            + "&drawFilter=" + $('input[name=DrawFilter]:checked', '#search-form').val() + "&breadcrumb=" 
                                            + $("#BreadCrumb").val() + "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val());
        },
        ViewBetList: function (custId, agentLevel, username) {
            $("#BreadCrumb").setBreadCrumb(custId, agentLevel, username);
            var drawFilter = $('input[name=DrawFilter]:checked', '#search-form').val();
            var status = (drawFilter === "1" ? "112,113" : "111,112,113");
            location.href = CORE.resolveUrl("~/BetList/BetListWinlossReport?custId=" + custId + "&breadcrumb=" + $("#BreadCrumb").val() + "&FromDate="
                                            + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val() + "&drawFilter=" + drawFilter + "&statusIDs=" 
                                            + status + "&title=" + document.title);
        }
    };
}) (jQuery);

$(function () {
    /*jshint strict:false */
    agentWinLoss.init();
});
/*global gridOptions, core, report*/

var agentWinLossReport = (function ($) {
    'use strict';
    return {
        init: function () {
            $("#tabs-area a:first").tab('show');
            agentWinLossReport.initDatePicker();
            agentWinLossReport.registerButtonClickEvent();
            report.registerCheckboxProductEvent();
            agentWinLossReport.dataGridBinding();
        },

        initDatePicker: function () {
            $('#fromDatePicker').datepicker();
            $('#toDatePicker').datepicker();
        },

        registerButtonClickEvent: function () {
            $('#search').click(function () {
                agentWinLossReport.dataGridBinding();
                return false;
            });

            $('#today').click(function () {
                var today = $(this).data('date');
                $('#fromDatePicker').datepicker('update', today);
                $('#toDatePicker').datepicker('update', today);
                agentWinLossReport.dataGridBinding();
                return false;
            });
            $('#yesterday').click(function () {
                var yesterday = $(this).data('date');
                $('#fromDatePicker').datepicker('update', yesterday);
                $('#toDatePicker').datepicker('update', yesterday);
                agentWinLossReport.dataGridBinding();
                return false;
            });

            // breadcrum
            $('.breadcrumb a').click(function () {
                var agentId = $(this).data('agent-id');
                var agentLevel = $(this).data('agent-level');
                var agentName = $(this).text();
                agentWinLossReport.ViewDownline(agentId, agentLevel, agentName);
            });
        },

        dataGridBinding: function () {
            report.setProductValue();

            var submitOptions = $('form').serializeObject();
            $.ajax({
                url: core.resolveUrl("~/Report/GetAgentWinLossData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        gridOptions.bodyRows = core.genGridRows(data.List, gridOptions);
                        core.genGridTotalRows(gridOptions.bodyRows, gridOptions);
                        $('#grid').show().fanexGrid(gridOptions);
                        core.unblockUI();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        ViewDownline: function (agentId, agentLevel, username) {
            report.setProductValue();
            $("#BreadCrumb").setBreadCrumb(agentId, agentLevel, username);
            location.href = core.resolveUrl("~/Report/AgentWinLoss?agentId=" + agentId + "&agentLevel=" + agentLevel + "&username=" + username
                                + "&drawFilter=" + $('input[name=DrawFilter]:checked', '#search-form').val() + "&breadcrumb=" + $("#BreadCrumb").val()
                                + "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val() + "&Product=" + $("#Product").val());
        },
        ViewBetList: function (custId, agentLevel, username) {
            report.setProductValue(); // Set Product selected values.
            $("#BreadCrumb").setBreadCrumb(custId, agentLevel, username);
            var drawFilter = $('input[name=DrawFilter]:checked', '#search-form').val();
            var status = (drawFilter === "1" ? report.SettledNoDraw : report.SettledHasDraw);
            location.href = core.resolveUrl("~/BetList/BetListWinlossReport?custId=" + custId + "&breadcrumb=" + $("#BreadCrumb").val() + "&FromDate="
                                            + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val() + "&drawFilter=" + drawFilter + "&statusIDs="
                                            + status + "&ProductIds=" + $("#Product").val() + "&title=" + document.title);
        }
    };
})(jQuery);

$(function () {
    'use strict';
    agentWinLossReport.init();
});
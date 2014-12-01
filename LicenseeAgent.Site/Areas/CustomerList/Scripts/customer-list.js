/*global CORE, LANGUAGE, GRID, pageContext, context */
'use strict';

var CUSTOMER_LIST = (function ($this, $) {
    var gridOptions;
    var createCheckBoxContext = function () {
        context.attach('#select-dropdown', [
            {
                text: LANGUAGE.getString('All'),
                href: '#',
                action: CUSTOMER_LIST.checkAll
            },
            {
                text: LANGUAGE.getString('None'),
                href: '#',
                action: CUSTOMER_LIST.uncheckAll
            }
        ]);
    };

    var reRenderAfterPaged = function (currentPage, itemsPerPage) {
        if (gridOptions && gridOptions.pagerOption) {
            currentPage = currentPage >= 0 ? currentPage : gridOptions.pagerOption.currentPage;
            itemsPerPage = itemsPerPage || gridOptions.pagerOption.itemsPerPage;
        }

        var params = $('#search-form').serializeObject();
        params.pageIndex = currentPage;
        params.pageSize = itemsPerPage;
        CORE.ajaxJSON({
            url: $('#search-form').attr('action'),
            data: params
        }, function (data) {
            gridOptions.pagerOption = {
                itemsPerPage: data.ItemsPerPage,
                itemsPerPageArray: GRID.PageSizes,
                currentPage: data.CurrentPage,
                totalItem: data.TotalItem,
                pagerSelectCallBack: reRenderAfterPaged
            };

            gridOptions.bodyRows = GRID.generateBodyRows(data.List, gridOptions);
            $('#grid').show().fanexGrid(gridOptions);
            $("#boxsearch").width($("#grid").width());
            $("#breadcrumb").width($("#grid").width());
            if (!pageContext.IsDirectDownLine) {
                $(".cust-status").addClass('cust-status-none');
            }
        });
    };

    var getSuggestion = function (query, process) {
        return CORE.ajaxJSON({
            url: $('#Username').data('autocomplete-url'),
            data: {
                userName: query,
                agentId: $("#AgentId").val(),
                agentLevel: $("#AgentLevel").val()
            }
        }, function (response) {
            return process(response.List);
        });
    };

    $this.init = function (_gridOptions) {
        gridOptions = _gridOptions;
        $('.selectpicker').selectpicker();

        createCheckBoxContext();

        // search
        $('#search-form').submit(function (e) {
            reRenderAfterPaged(0, GRID.PageSizes[2]);
            e.preventDefault();
        });

        // username suggestion
        $('#Username').typeahead({
            source: getSuggestion,
            minLength: 2
        });

        // breadcrum
        $('.breadcrumb a').click(function () {
            var agentId = $(this).data('agent-id');
            $this.showAgent(agentId);
        });

        // get customer list
        $('#search-form').submit();
    };

    $this.showAgent = function (agentId) {
        $('#agent-form #AgentId').val(agentId);
        $('#agent-form').submit();
    };

    $this.showMember = function (agentId) {
        $('#member-form #AgentId').val(agentId);
        $('#member-form').submit();
    };

    $this.hidePopover = function () {
        $('.popover-link').each(function () {
            $(this).popover('hide');
        });
    };

    $this.checkAllItem = function (itemAll) {
        if ($(itemAll).hasClass("icon-uncheck-all")) {
            CUSTOMER_LIST.checkAll();
        } else {
            CUSTOMER_LIST.uncheckAll();
        }
    };

    $this.checkAll = function () {
        $(".icon-uncheck-all").attr("class", "icon-checked-all");
        $(".icon-uncheck").attr("class", "icon-checked");
    };

    $this.uncheckAll = function () {
        $(".icon-checked-all").attr("class", "icon-uncheck-all");
        $(".icon-checked").attr("class", "icon-uncheck");
    };

    $this.selectClick = function (itemSelect) {
        if ($(itemSelect).hasClass("icon-uncheck")) {
            $(itemSelect).removeClass("icon-uncheck");
            $(itemSelect).addClass("icon-checked");
            if ($(".cell-select").length === $(".icon-checked").length) {
                $(".icon-uncheck-all").attr("class", "icon-checked-all");
            }
        } else {
            $(itemSelect).removeClass("icon-checked");
            $(itemSelect).addClass("icon-uncheck");
            $(".icon-checked-all").attr("class", "icon-uncheck-all");
        }
    };

    return $this;
} (CUSTOMER_LIST || {}, jQuery));
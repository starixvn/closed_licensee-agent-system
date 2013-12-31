$(document).ready(function () {
    renderDatePicker();
});

var renderDatePicker = function () {
    var lastdate = '';
    lastdate = lastMonth + '/' + lastDay + '/' + lastYear;
    $('#month-picker-control').monPickr({
        plugInId: '#month-picker-control',
        today: lastdate,
        maxDate: _today,
        minDate: _minDate,
        showDateRange: true,
        fromDayId: 'spFrom',
        toDayId: 'spTo',
        fromDayLabel: 'From',
        toDayLabel: 'To',
        log: function (msg) {
            console.warn(msg);
        }
    });
};

function OpenIPInfo(ipString) {
    alert(ipString);
    var url = "Customer/GetIpInformation?IpAddress=" + ipString;
    POPUP.openDialogFrame(url, { width: 360, height: 300 });
};

var app = angular.module('betlist', ['ngGrid', 'ngSanitize']);
app.controller('BetListController', function ($scope, $http) {
    // STATUS
    var statusData = [{ "id": 0, "status": "All" },
                        { "id": 1, "status": "Won" },
                        { "id": 2, "status": "Lose" },
                        { "id": 3, "status": "Draw"}];
    $scope.statusOptions = statusData;
    $scope.selectedStatus = statusData[0];

    $scope.changeStatus = function (selectedItem) {
        //alert(selectedItem.status);
    }

    // GRID
    $scope.betlistData = null;
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [100, 200, 500],
        pageSize: 100,
        currentPage: 1
    };
    $scope.setPagingData = function (data, page, pageSize) {
        var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
        $scope.myData = pagedData;
        $scope.totalServerItems = data.length;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    $scope.gridOptions = {
        data: 'betlistData',
        rowHeight: 65,
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        multiSelect: false,
        columnDefs: [
                        { field: 'Index', displayName: 'No.', width: 30 },
                        { field: 'TransTime', displayName: 'TransTime', width: 120, style: 'class=\"alignRight\"', cellTemplate: '<div ng-bind-html=\"row.getProperty(col.field)\"></div>' },
                        { field: 'Choice', displayName: 'Choice', width: 370, cellTemplate: '<div ng-bind-html=\"row.getProperty(col.field)\"></div>' },
                        { field: 'Odds', displayName: 'Odds', width: 40, cellTemplate: '<div ng-bind-html=\"row.getProperty(col.field)\"></div>' },
                        { field: 'Stake', displayName: 'Stake', width: 60, cellTemplate: '<div ng-bind-html=\"row.getProperty(col.field)\"></div>' },
                        { field: 'WinLoss', displayName: 'Winloss', width: 60, cellTemplate: '<div ng-bind-html=\"row.getProperty(col.field)\"></div>' },
                        { field: 'Status', displayName: 'Status', width: 100, cellTemplate: '<div ng-bind-html=\"row.getProperty(col.field)\"></div>' },
                        { field: 'AgentPT', displayName: 'AgentPT', width: 80, cellTemplate: '<div ng-bind-html=\"row.getProperty(col.field)\"></div>' },
                        { field: 'AgentComm', displayName: 'AgentComm', width: 80, cellTemplate: '<div ng-bind-html=\"row.getProperty(col.field)\"></div>' },
                        { field: 'Feature', displayName: 'Feature', width: 60, cellTemplate: '<a href=\"javascript:;\" ng-click=\"ViewBetListDetails()\">Details</a>' }
                    ]
    };

    $scope.SubmitForm = function () {
        CORE.blockUI();
        var fromDate = $('month-picker-control').monPickr("getFromDate", "MM/dd/yyyy");
        var toDate = $('month-picker-control').monPickr("getFromDate", "MM/dd/yyyy");
        var queryString = "FromDate=" + fromDate + "&";
        queryString += "ToDate=" + fromDate + "&";

        $http({
            method: 'POST',
            url: "BetList/Index" + queryString(),
            headers: { 'Content-Type': 'application/json' }
        }).
        success(function (data, status, headers, config) {
            $scope.betlistData = data;
            CORE.unblockUI();
        }).
        error(function (data, status, headers, config) {
            alert("Error");
        });
    };

    $scope.ViewBetListDetails = function () {
        var url = "BetList/Details" + queryString();
        POPUP.openDialogFrame(url, { width: 360, height: 300 });
    }

    var queryString = function () {
        return "";
    };

    $scope.OpenIPInfo = function (ipString) {
        alert(ipString);
        var url = "Customer/GetIpInformation?IpAddress=" + ipString;
        POPUP.openDialogFrame(url, { width: 360, height: 300 });
    };
});

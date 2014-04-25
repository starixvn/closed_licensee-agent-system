/*global settingContext, sportsBookSetting */

'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.controller('CustomerInfoController',
    [
        '$scope',
        '$filter',
        'CustomerInfoService',
        function($scope, $filter) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];
            if (settingContext.FormId === fanex.customerSetting.FormType.Add) {
                $scope.isShowLimitMaxCredit = sportsBookSetting.CustInfo.MaxCredit !== undefined ? true : false;
                if ($scope.isShowLimitMaxCredit) {
                    $scope.$watch('custInfo.credit', function(newValue) {
                        if (parseFloat(newValue) > $scope.custInfo.maxCredit) {
                            $scope.formCustInfo.credit.hintMessage = $scope.language['customerInfo_Message_MaxCredit']
                                .format($filter('number')($scope.custInfo.maxCredit, 0));
                            $scope.formCustInfo.credit.$setValidity('', false);
                        } else {
                            $scope.formCustInfo.credit.hintMessage = '';
                            $scope.formCustInfo.credit.$setValidity('', true);
                        }
                    });
                }
                $scope.custInfo = {
                    userName: '',
                    password: '',
                    firstName: '',
                    lastName: '',
                    phone: '',
                    mobile: '',
                    fax: '',
                    credit: sportsBookSetting.CustInfo.Credit,
                    maxCredit: sportsBookSetting.CustInfo.MaxCredit,
                    status: sportsBookSetting.CustInfo.Status
                };
            } else {
                $scope.custInfo = {
                    userName: sportsBookSetting.CustInfo.UserName,
                    firstName: sportsBookSetting.CustInfo.FirstName,
                    lastName: sportsBookSetting.CustInfo.LastName,
                    credit: sportsBookSetting.CustInfo.Credit,
                    status: sportsBookSetting.CustInfo.Status
                };
            }
        }
    ]);
}
/*global settingContext */

'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.controller('CustomerInfoController',
    [
        '$scope',
        '$filter',
        function ($scope, $filter) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];
            if (settingContext.FormId === fanex.customerSetting.FormType.Add) {
                $scope.isShowLimitMaxCredit = $scope.custInfo.MaxCredit !== undefined ? true : false;
                if ($scope.isShowLimitMaxCredit) {
                    $scope.$watch('custInfo.Credit', function (newValue) {
                        if (parseFloat(newValue) > $scope.custInfo.MaxCredit) {
                            $scope.formCustInfo.credit.hintMessage = $scope.language['customerInfo_Message_MaxCredit']
                                .format($filter('number')($scope.custInfo.MaxCredit, 0));
                            $scope.formCustInfo.credit.$setValidity('', false);
                        } else {
                            $scope.formCustInfo.credit.hintMessage = '';
                            $scope.formCustInfo.credit.$setValidity('', true);
                        }
                    });
                }


                $scope.custInfo = {
                    UserName: '',
                    Password: '',
                    FirstName: '',
                    LastName: '',
                    Phone: '',
                    Mobile: '',
                    Fax: ''
                };
            } else {
                $scope.custInfo = {};
            }
            angular.extend($scope.custInfo, $scope.$parent.custInfo);
        }
    ]);
}
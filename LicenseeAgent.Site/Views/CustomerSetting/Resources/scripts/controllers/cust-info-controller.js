/*global settingContext */

if (fanex.customerSetting) {
    fanex.customerSetting.controller('CustomerInfoController',
    [
        '$scope',
        '$filter',
        'Fanex.CustomerSetting.LanguageService',
        function ($scope, $filter, languageService) {
            'use strict';
            if (settingContext.FormId === fanex.customerSetting.FormType.Add) {
                $scope.isShowLimitMaxCredit = $scope.custInfo.MaxCredit !== undefined ? true : false;
                if ($scope.isShowLimitMaxCredit) {
                    $scope.$watch('custInfo.Credit', function (newValue) {
                        if (parseFloat(newValue) > $scope.custInfo.MaxCredit) {
                            $scope.formCustInfo.credit.hintMessage = languageService.getString('GivenMaxCredit')
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
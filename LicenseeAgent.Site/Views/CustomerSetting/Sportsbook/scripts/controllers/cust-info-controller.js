if (fanex.customerSetting) {
    var custInfoController = fanex.customerSetting.controller('customerSetting.custInfoController',
    [
        '$scope',
        '$filter',
        'fanex.customerSetting.custInfoService',
        function CustInfoController($scope, $filter, custInfoService) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];

            if (settingContext.FormId == fanex.customerSetting.FormType.Add) {
                var isShowMaxCredit = true;
                if (custInfo.MaxCredit == undefined) {
                    isShowMaxCredit = false;
                }
                $scope.isShowLimitMaxCredit = isShowMaxCredit;
                if ($scope.isShowLimitMaxCredit) {
                    $scope.$watch('custInfo.credit', function (newValue) {
                        if (parseFloat(newValue) > $scope.custInfo.maxCredit) {
                            $scope.formCustInfo.credit.hintMessage = $scope.language['customerInfo_Message_MaxCredit'].format($filter('number')($scope.custInfo.maxCredit, 0));
                            $scope.formCustInfo.credit.$setValidity('', false);
                        }
                        else {
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
                    credit: custInfo.Credit,
                    maxCredit: custInfo.MaxCredit,
                    status: custInfo.Status
                };
            }
            else {
                $scope.custInfo = {
                    userName: custInfo.UserName,
                    firstName: custInfo.FirstName,
                    lastName: custInfo.LastName,
                    credit: custInfo.Credit,
                    status: custInfo.Status
                };
            }
        } ]);
}
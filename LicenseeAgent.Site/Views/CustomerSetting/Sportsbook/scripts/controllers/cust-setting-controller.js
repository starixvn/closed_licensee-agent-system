if (fanex.customerSetting) {
    var custSettingController = fanex.customerSetting.controller('customerSetting.custSettingController', ['$scope', '$http', function CustSettingController($scope, $http) {
        CORE.blockUI();
        if (typeof result != 'undefined') {
            $('#custSetting').html(result.ErrMsg);
            $('#custSetting').show();
            CORE.unblockUI();
            return;
        }

        CORE.initInstruction();

        // reload setting mode from query string
        settingContext.IsSimpleMode = CORE.getParameterByName('isSimpleMode') !== 'false' && settingContext.IsSimpleMode;
        $('#modeSwitcher').bootstrapSwitch('setState', !settingContext.IsSimpleMode);

        $scope.settingMode = settingContext.IsSimpleMode;
        $scope.formId = settingContext.FormId;
        $('#modeSwitcher').on('switch-change', function (e, data) {
            var isSimpleMode = !data.value;
            fanex.hideHint();
            $scope.settingMode = isSimpleMode;
            var positionTakingScope = angular.element('[ng-controller="customerSetting.positionTakingController"]').scope();
            positionTakingScope.settingContext.IsSimpleMode = isSimpleMode;
            $scope.$apply();
        });

        var encodeMessages = function (messages) {
            var array = [];
            var length = messages.length;
            for (var i = 0; i < length; i++) {
                var message = messages[i];
                array.push(message.ErrCode.toString());
                array.push(message.ErrMsg);
            }
            return encodeURIComponent(array.join('|'));
        }
        var decodeMessages = function (string) {
            var array = decodeURIComponent(string).split('|');
            if (array.length % 2 != 0) return;
            var length = array.length / 2;
            var messages = [];
            for (var i = 0; i < length; i++) {
                var errCode = parseInt(array[i * 2]);
                var errMsg = array[i * 2 + 1];
                var message = {
                    ErrCode: errCode,
                    ErrMsg: errMsg
                };
                messages.push(message);
            }
            return messages;
        }

        $scope.insertCustomer = function () {
            var custInfoScope = angular.element('#custInfoController').scope();
            if (custInfoScope.formCustInfo.$invalid) {
                var keepGoing = true;
                angular.forEach(custInfoScope.formCustInfo.$error, function (field) {
                    if (angular.isArray(field)) {
                        if (keepGoing) {
                            var element = $('#' + field[0].$name);
                            element.focus();
                            keepGoing = false;
                        }
                    }
                });
            }
            else {
                // Commission Setting
                var commissionSettingScope = angular.element('[ng-controller="fanex.customersetting.commissioncontroller"]').scope();
                if (!commissionSettingScope.isValidData()) {
                    return;
                }
                var commissionSettingsModel = commissionSettingScope.saveData();

                // Betting Limit
                var bettingLimitScope = angular.element('[ng-controller="fanex.customersetting.betlimitcontroller"]').scope();
                if (!bettingLimitScope.isValidData()) {
                    return;
                }
                var bettingLimits = bettingLimitScope.saveData();

                // position taking
                var positionTakingScope = angular.element('[ng-controller="customerSetting.positionTakingController"]').scope();
                if (!positionTakingScope.isValidData()) {
                    return;
                }
                var positionTakingUpdated = positionTakingScope.saveData();

                $scope.Messages = null;
                CORE.blockUI();
                $http({
                    method: 'POST',
                    url: CORE.resolveUrl($('#custSetting').data('href')),
                    headers: { 'Content-Type': 'application/json' },
                    data: {
                        settingContext: settingContext,
                        updateDataSportsbookCustomer:
                        {
                            PositionTakingSetting: positionTakingUpdated,
                            CustInfo: custInfoScope.custInfo,
                            CommissionSetting: {
                                CommissionItems: commissionSettingsModel,
                                UserGroup: bettingLimitScope.settingContext.TargetRoleId == 1 ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup : ''
                            },
                            BettingLimits: bettingLimits
                        }
                    }
                }).
                success(function (data, status, headers, config) {
                    CORE.unblockUI();

                    if (data.ErrCode != 0) {
                        $scope.Messages = [{
                            ErrCode: -1,
                            ErrMsg: data.ErrMsg
                        }];
                        return;
                    }

                    $scope.Messages = data.Messages;
                    CORE.scrollTop();
                    var isSuccess = true;
                    for (var i = 0; i < data.Messages.length; i++) {
                        var message = data.Messages[i];
                        if (message.ErrCode != 0) {
                            isSuccess = false;
                            break;
                        }
                    }
                    if (isSuccess) {
                        POPUP.close();
                        if (window.parent.customerList)
                            window.parent.customerList.reRenderAfterPaged();
                    }
                }).
                error(function (data, status, headers, config) {
                    CORE.unblockUI();
                    alert(data.ErrMsg);
                });
            }
        }

        $scope.updateCustomer = function () {
            $('.messages').hide();
            $scope.Messages = null;

            var custInfoScope = angular.element('[ng-controller="customerSetting.custInfoController"]').scope();

            // Commission Setting
            var commissionSettingsModel = null;
            if (settingContext.FormId != fanex.customerSetting.FormType.EditIndirectMember) {
                var commissionSettingScope = angular.element('[ng-controller="fanex.customersetting.commissioncontroller"]').scope();
                if (!commissionSettingScope.isValidData()) {
                    return;
                }
                commissionSettingsModel = commissionSettingScope.saveData();
            }

            // Betting Limit
            var bettingLimits = null;
            if (settingContext.FormId != fanex.customerSetting.FormType.EditIndirectMember) {
                var bettingLimitScope = angular.element('[ng-controller="fanex.customersetting.betlimitcontroller"]').scope();
                if (!bettingLimitScope.isValidData()) {
                    return;
                }
                bettingLimits = bettingLimitScope.saveData();
            }

            var userGroup = null;
            if (settingContext.FormId != fanex.customerSetting.FormType.EditIndirectMember) {
                userGroup = bettingLimitScope.settingContext.TargetRoleId == 1 ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup : ''
            }

            // position taking
            var positionTakingScope = angular.element('[ng-controller="customerSetting.positionTakingController"]').scope();
            if (!positionTakingScope.isValidData()) {
                return;
            }
            var positionTakingUpdated = positionTakingScope.saveData();

            CORE.blockUI();
            $http({
                method: 'POST',
                url: CORE.resolveUrl($('#custSetting').data('href')),
                headers: { 'Content-Type': 'application/json' },
                data: {
                    settingContext: settingContext,
                    updateDataSportsbookCustomer:
                        {
                            PositionTakingSetting: positionTakingUpdated,
                            CommissionSetting: {
                                CommissionItems: commissionSettingsModel,
                                UserGroup: userGroup
                            },
                            BettingLimits: bettingLimits
                        }
                }
            }).
            success(function (data, status, headers, config) {
                CORE.unblockUI();

                if (data.ErrCode != 0) {
                    $('.messages').show();
                    $scope.Messages = [{
                        ErrCode: -1,
                        ErrMsg: data.ErrMsg
                    }];
                    return;
                }

                var isSuccess = true;
                for (var i = 0; i < data.Messages.length; i++) {
                    var message = data.Messages[i];
                    if (message.ErrCode != 0) {
                        isSuccess = false;
                        break;
                    }
                }
                if (isSuccess) {
                    var orginalUrl = CORE.removeURLParameter(CORE.removeURLParameter(location.href, 'messages'), 'isSimpleMode');
                    var url = orginalUrl + '&messages=' + encodeMessages(data.Messages) + '&isSimpleMode=' + $scope.settingMode;
                    location.href = url;
                    return;
                }

                $('.messages').show();
                $scope.Messages = data.Messages;
                CORE.scrollTop();
            }).
            error(function (data, status, headers, config) {
                CORE.unblockUI();
                alert(data.ErrMsg);
            });
        }

        if (typeof settingContext == 'undefined') {
            $('#custSetting').html("It has a problem about data.");
        }

        $('#custSetting').show();

        var messagesQueryString = CORE.getParameterByName('messages');
        if (messagesQueryString !== '') {
            var messages = decodeMessages(messagesQueryString);
            $scope.Messages = messages;
            POPUP.close();
        }

        CORE.unblockUI();
    } ]);
}
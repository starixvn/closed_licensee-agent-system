/*global core, popup, settingContext, result, sportsBookSetting */

'use strict';
if (fanex.customerSetting) {
    fanex.customerSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        '$window',
        function ($scope, $http, $window) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];
            core.blockUI();
            if (typeof result !== 'undefined') {
                $('#custSetting').html(result.ErrMsg);
                $('#custSetting').show();
                core.unblockUI();
                return;
            }

            core.initInstruction();
            // reload setting mode from query string
            settingContext.IsSimpleMode = core.getParameterByName('isSimpleMode') !== 'false' && settingContext.IsSimpleMode;
            $('#modeSwitcher').bootstrapSwitch('setState', !settingContext.IsSimpleMode);

            $scope.settingMode = settingContext.IsSimpleMode;
            $scope.formId = settingContext.FormId;
            $scope.hasOfferedNumberGame = ($window['numberGameSetting'] !== undefined) ? true : false;
            $scope.result = fanex.customerSetting.Result;
            $scope.custInfo = sportsBookSetting.CustInfo;

            $('#modeSwitcher').on('switch-change', function (e, data) {
                var isSimpleMode = !data.value;
                fanex.hideHint();
                $scope.settingMode = isSimpleMode;
                var positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();
                positionTakingScope.settingContext.IsSimpleMode = isSimpleMode;
                $scope.$apply();
            });

            $scope.insertCustomer = function () {
                var commissionSettingScope,
                    bettingLimitScope,
                    positionTakingScope,
                    commissionItems,
                    bettingLimitItems,
                    positionTakingItems,
                    numberGameSetting = null;

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
                } else {
                    commissionSettingScope = angular.element('[data-ng-controller="CommissionController"]').scope();
                    bettingLimitScope = angular.element('[data-ng-controller="BetLimitController"]').scope();
                    positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();

                    if (!commissionSettingScope.isValidData() || !bettingLimitScope.isValidData() || !positionTakingScope.isValidData()) {
                        return;
                    }

                    commissionItems = commissionSettingScope.saveData();
                    bettingLimitItems = bettingLimitScope.saveData();
                    positionTakingItems = positionTakingScope.saveData();
                    if ($scope.hasOfferedNumberGame) {
                        numberGameSetting = fanex.customerSetting.numberGameSetting.getNumberGameSetting($scope.settingMode);
                        if (numberGameSetting == null) {
                            return;
                        }
                    }
                    $scope.Messages = null;
                    core.blockUI();
                    $http({
                        method: 'POST',
                        url: core.resolveUrl($('#custSetting').data('href')),
                        headers: { 'Content-Type': 'application/json' },
                        data: {
                            settingContext: settingContext,
                            sportsBookSetting:
                                {
                                    PositionTakingSetting: positionTakingItems,
                                    CustInfo: custInfoScope.custInfo,
                                    CommissionSetting: {
                                        CommissionItems: commissionItems,
                                        UserGroup: bettingLimitScope.settingContext.TargetRoleId === 1
                                            ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup
                                            : ''
                                    },
                                    BettingLimitSetting: bettingLimitItems
                                },
                            numberGameSetting: numberGameSetting
                        }
                    }).
                    success(function (data) {
                        core.unblockUI();

                        if (data.ErrCode !== 0) {
                            $scope.Messages = [
                                {
                                    ErrCode: -1,
                                    ErrMsg: data.ErrMsg
                                }
                            ];
                            return;
                        }

                        $scope.Messages = data.Messages;
                        core.scrollTop();
                        var isSuccess = true;
                        for (var i = 0; i < data.Messages.length; i++) {
                            var message = data.Messages[i];
                            if (message.ErrCode !== 0) {
                                isSuccess = false;
                                break;
                            }
                        }
                        if (isSuccess) {
                            if (window.parent.customerList) {
                                window.parent.customerList.editProducts();
                            }
                        }
                    }).
                    error(function (data) {
                        core.unblockUI();
                        alert(data.ErrMsg);
                    });
                }
            };

            $scope.updateCustomer = function () {
                $('.messages').hide();
                $scope.Messages = null;

                var commissionItems = null,
                    bettingLimitItems = null,
                    positionTakingItems = null,
                    userGroup = null,
                    commissionSettingScope,
                    bettingLimitScope,
                    positionTakingScope,
                    sportsBookSetting = null,
                    numberGameSetting = null;

                if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
                    commissionSettingScope = angular.element('[data-ng-controller="CommissionController"]').scope();
                    bettingLimitScope = angular.element('[data-ng-controller="BetLimitController"]').scope();

                    if (!commissionSettingScope.isValidData() || !bettingLimitScope.isValidData()) {
                        return;
                    }
                    commissionItems = commissionSettingScope.saveData();
                    bettingLimitItems = bettingLimitScope.saveData();

                    userGroup = bettingLimitScope.settingContext.TargetRoleId === 1
                        ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup
                        : '';
                }

                positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();
                if (!positionTakingScope.isValidData()) {
                    return;
                }
                positionTakingItems = positionTakingScope.saveData();

                sportsBookSetting = {
                    PositionTakingSetting: positionTakingItems,
                    CommissionSetting: {
                        CommissionItems: commissionItems,
                        UserGroup: userGroup
                    },
                    BettingLimitSetting: bettingLimitItems
                };

                if ($scope.hasOfferedNumberGame) {
                    numberGameSetting = fanex.customerSetting.numberGameSetting.getNumberGameSetting($scope.settingMode);
                    if (numberGameSetting == null) {
                        return;
                    }
                }

                if (!fanex.isChangedData(sportsBookSetting) && !fanex.isChangedData(numberGameSetting)) {
                    $('.messages').show();
                    $scope.Messages = [
                        {
                            ErrCode: $scope.result.NoChange,
                            ErrMsg: $scope.language.NoChangeToUpdate
                        }
                    ];
                    return;
                }

                core.blockUI();
                $http({
                    method: 'POST',
                    url: core.resolveUrl($('#custSetting').data('href')),
                    headers: { 'Content-Type': 'application/json' },
                    data: {
                        settingContext: settingContext,
                        sportsBookSetting: sportsBookSetting,
                        numberGameSetting: numberGameSetting
                    }
                }).
                success(function (data) {
                    core.unblockUI();

                    if (data.ErrCode !== 0) {
                        $('.messages').show();
                        $scope.Messages = [
                            {
                                ErrCode: $scope.result.Error,
                                ErrMsg: data.ErrMsg
                            }
                        ];
                        return;
                    }

                    var isSuccess = true;
                    for (var i = 0; i < data.Messages.length; i++) {
                        var message = data.Messages[i];
                        if (message.ErrCode !== $scope.result.Success) {
                            isSuccess = false;
                            break;
                        }
                    }
                    if (isSuccess) {
                        core.blockUI();
                        var timeout = 0;
                        if (settingContext.TargetRoleId === fanex.customerSetting.Role.Member) {
                            timeout = 3000;
                        }
                        setTimeout(function () {
                            var orginalUrl = core.removeURLParameter(core.removeURLParameter(location.href, 'messages'), 'isSimpleMode');
                            var url = orginalUrl + '&messages=' + core.encodeMessages(data.Messages) + '&isSimpleMode=' + $scope.settingMode;
                            location.href = url;
                            core.unblockUI();
                        }, timeout);
                        return;
                    }

                    $('.messages').show();
                    $scope.Messages = data.Messages;
                    core.scrollTop();
                }).
                error(function (data) {
                    core.unblockUI();
                    alert(data.ErrMsg);
                });
            };

            if (typeof settingContext === 'undefined') {
                $('#custSetting').html("It has a problem about data.");
            }

            $('#custSetting').show();
            setTimeout(function () {
                core.scrollTop();
                $('#userName').focus();
            }, 100);
            var messagesQueryString = core.getParameterByName('messages');
            if (messagesQueryString !== '') {
                var messages = core.decodeMessages(messagesQueryString);
                $scope.Messages = messages;
                popup.close();
            }
        }

    ]);
}
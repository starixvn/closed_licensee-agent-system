/*global core, settingContext, result, virtualSportsSetting:true */

if (fanex.customerSetting) {
    fanex.customerSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        'CustomerSettingService',
        'Fanex.CustomerSetting.LanguageService',
        function ($scope, $http, custSettingService, languageService) {
            'use strict';
            core.blockUI();
            if (typeof result !== 'undefined') {
                $('#custSetting').html(result.ErrMsg);
                $('#custSetting').show();
                core.unblockUI();
                return;
            }
            core.initInstruction();
            // reload setting mode from query string
            settingContext.IsSimpleMode = core.getQueryParamValue('isSimpleMode') !== 'false' && settingContext.IsSimpleMode;
            $('#modeSwitcher').bootstrapSwitch('setState', !settingContext.IsSimpleMode);

            $scope.settingMode = settingContext.IsSimpleMode;
            $scope.formId = settingContext.FormId;
            $scope.result = fanex.customerSetting.Result;
            $scope.custInfo = virtualSportsSetting.CustInfo;
            $scope.isDeposit = settingContext.IsDeposit;

            $('#modeSwitcher').on('switch-change', function (e, data) {
                var isSimpleMode = !data.value;
                fanex.hideHint();
                $scope.settingMode = isSimpleMode;
                var positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();
                positionTakingScope.settingContext.IsSimpleMode = isSimpleMode;
                $scope.$apply();
            });

            $scope.updateProductSetting = function (data) {
                virtualSportsSetting = data.productSetting;
            };

            $scope.updateCustomer = function () {
                $('.messages').hide();
                $scope.Messages = null;

                var bettingLimitItems = null,
                    positionTakingItems = null,
                    userGroup = null,
                    isChangeData = true,
                    bettingLimitScope,
                    virtualSportsSetting = null,
                    positionTakingScope;

                if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
                    bettingLimitScope = angular.element('[data-ng-controller="BetLimitController"]').scope();

                    if (!bettingLimitScope.isValidData()) {
                        return;
                    }

                    bettingLimitItems = bettingLimitScope.saveData();

                    userGroup = '';
                }

                positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();
                if (!positionTakingScope.isValidData()) {
                    return;
                }

                positionTakingItems = positionTakingScope.saveData();
                virtualSportsSetting = {
                    PositionTakingSetting: positionTakingItems,
                    BettingLimitSetting: bettingLimitItems
                };

                isChangeData = fanex.isChangedData(virtualSportsSetting);

                if (!isChangeData) {
                    $('.messages').show();
                    $scope.Messages = [
                        {
                            ErrCode: $scope.result.NoChange,
                            ErrMsg: languageService.getString('NoChangeToUpdate')
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
                        virtualSportsSetting: virtualSportsSetting
                    }
                }).
                    success(function (data) {
                        var timeout = 0;
                        core.unblockUI();

                        if (data.ErrCode !== 0) {
                            $scope.Messages = [
                                {
                                    ErrCode: -1,
                                    ErrMsg: data.ErrMsg
                                }
                            ];
                        }
                        else {
                            $scope.Messages = data.Messages;
                        }

                        if (settingContext.TargetRoleId === fanex.customerSetting.Role.Member) {
                            timeout = 3000;
                        }

                        core.blockUI();
                        setTimeout(function () {
                            custSettingService.updateSettingModel();
                        }, timeout);

                        $('.messages').show();
                        core.scrollTop();
                    }).
                    error(function (data) {
                        core.unblockUI();
                        alert(data.ErrMsg);
                    });
            };

            $scope.updatePartial = function (productSetting, isChangedData) {
                if (!isChangedData) {
                    $('.messages').show();
                    $scope.Messages = [{
                        ErrCode: $scope.result.NoChange,
                        ErrMsg: languageService.getString('NoChangeToUpdate')
                    }];
                    return;
                }

                productSetting.settingContext = settingContext;

                core.blockUI();
                $http({
                    method: 'POST',
                    url: core.resolveUrl($('#custSetting').data('href')),
                    headers: { 'Content-Type': 'application/json' },
                    data: productSetting
                }).
                success(function (data) {
                    var timeout = 0;

                    core.unblockUI();

                    if (data.ErrCode !== 0) {
                        $scope.Messages = [
                            {
                                ErrCode: $scope.result.Error,
                                ErrMsg: data.ErrMsg
                            }
                        ];
                    }
                    else {
                        $scope.Messages = data.Messages;
                    }

                    if (settingContext.TargetRoleId === fanex.customerSetting.Role.Member) {
                        timeout = 3000;
                    }

                    core.blockUI();
                    setTimeout(function () {
                        custSettingService.updateSettingModel();
                    }, timeout);

                    $('.messages').show();
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

            var messagesQueryString = core.getQueryParamValue('messages');
            if (messagesQueryString !== '') {
                var messages = core.decodeMessages(messagesQueryString);
                $scope.Messages = messages;
            }
        }
    ]);
}
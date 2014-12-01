/*global CORE, settingContext, result, sportsBookSetting:true, numberGameSetting:true */

if (fanex.customerSetting) {
    fanex.customerSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        '$window',
        'CustomerSettingService',
        'Fanex.CustomerSetting.LanguageService',
        function ($scope, $http, $window, custSettingService, languageService) {
            'use strict';
            CORE.blockUI();
            if (typeof result !== 'undefined') {
                $('#custSetting').html(result.ErrMsg);
                $('#custSetting').show();
                CORE.unblockUI();
                return;
            }

            CORE.initInstruction();
            // reload setting mode from query string
            settingContext.IsSimpleMode = CORE.getQueryParamValue('isSimpleMode') !== 'false' && settingContext.IsSimpleMode;
            $('#modeSwitcher').bootstrapSwitch('setState', !settingContext.IsSimpleMode);

            $scope.settingMode = settingContext.IsSimpleMode;
            $scope.formId = settingContext.FormId;
            $scope.hasOfferedNumberGame = ($window['numberGameSetting'] !== undefined) ? true : false;
            $scope.result = fanex.customerSetting.Result;
            $scope.custInfo = sportsBookSetting.CustInfo;
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
                sportsBookSetting = data.productSetting;
                if ($scope.hasOfferedNumberGame) {
                    numberGameSetting = data.productSetting3RD;
                }
            };

            $scope.insertCustomer = function () {
                var commissionSettingScope,
                    bettingLimitScope,
                    positionTakingScope,
                    commissionItems,
                    bettingLimitItems,
                    positionTakingItems,
                    numberGameSetting = null,
                    userGroup = null;

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
                    bettingLimitScope = angular.element('[data-ng-controller="BetLimitController"]').scope();
                    positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();
                    if (settingContext.IsCredit) {
                        commissionSettingScope = angular.element('[data-ng-controller="CommissionController"]').scope();
                        if (!commissionSettingScope.isValidData()) {
                            return;
                        }
                        commissionItems = commissionSettingScope.saveData();

                        userGroup = bettingLimitScope.settingContext.TargetRoleId === 1
                                            ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup
                                            : '';
                    }

                    if (!bettingLimitScope.isValidData() || !positionTakingScope.isValidData()) {
                        return;
                    }

                    bettingLimitItems = bettingLimitScope.saveData();
                    positionTakingItems = positionTakingScope.saveData();
                    if ($scope.hasOfferedNumberGame) {
                        numberGameSetting = fanex.customerSetting.numberGameSetting.getNumberGameSetting($scope.isDeposit);
                        if (numberGameSetting == null) {
                            return;
                        }
                    }

                    $scope.Messages = null;
                    CORE.blockUI();
                    $http({
                        method: 'POST',
                        url: CORE.resolveUrl($('#custSetting').data('href')),
                        headers: { 'Content-Type': 'application/json' },
                        data: {
                            settingContext: settingContext,
                            sportsBookSetting:
                                {
                                    PositionTakingSetting: positionTakingItems,
                                    CustInfo: custInfoScope.custInfo,
                                    CommissionSetting: {
                                        CommissionItems: commissionItems,
                                        UserGroup: userGroup
                                    },
                                    BettingLimitSetting: bettingLimitItems
                                },
                            numberGameSetting: numberGameSetting
                        }
                    }).
                    success(function (data) {
                        CORE.unblockUI();

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
                        CORE.scrollTop();
                        var isSuccess = true;
                        for (var i = 0; i < data.Messages.length; i++) {
                            var message = data.Messages[i];
                            if (message.ErrCode !== 0) {
                                isSuccess = false;
                                break;
                            }
                        }
                        if (isSuccess) {
                            var url;

                            if (data.HasOffered3RD) {
                                if (settingContext.TargetRoleId === 1) {
                                    url = "~/ProductSetting/Product/EditMember?CustId={0}"
                                    .format(data.CustId);
                                } else {
                                    url = "~/ProductSetting/Product/EditAgent?agentId={0}&agentLevel={1}"
                                    .format(data.CustId, data.CustomerLevel);
                                }

                                setTimeout(function () {
                                    window.location.href = CORE.resolveUrl(url);
                                }, 5000);
                            }
                            else if (window.parent.CUSTOMER_LIST) {
                                window.parent.CUSTOMER_LIST.reRenderAfterPaged();
                            }
                        }
                    }).
                    error(function (data) {
                        CORE.unblockUI();
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
                    numberGameSetting = null,
                    isChangeData = true;

                if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
                    bettingLimitScope = angular.element('[data-ng-controller="BetLimitController"]').scope();
                    if (settingContext.IsCredit) {
                        commissionSettingScope = angular.element('[data-ng-controller="CommissionController"]').scope();
                        if (!commissionSettingScope.isValidData()) {
                            return;
                        }
                        commissionItems = commissionSettingScope.saveData();

                        userGroup = bettingLimitScope.settingContext.TargetRoleId === 1
                        ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup
                        : '';
                    }
                    if (!bettingLimitScope.isValidData()) {
                        return;
                    }
                    bettingLimitItems = bettingLimitScope.saveData();
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
                    numberGameSetting = fanex.customerSetting.numberGameSetting.getNumberGameSetting($scope.isDeposit);
                    if (numberGameSetting == null) {
                        return;
                    }
                }

                isChangeData = fanex.isChangedData(sportsBookSetting);

                if ($scope.hasOfferedNumberGame) {
                    isChangeData = isChangeData ? true : fanex.isChangedData(numberGameSetting);
                }

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

                $scope.callUpdate({
                    settingContext: settingContext,
                    sportsBookSetting: sportsBookSetting,
                    numberGameSetting: numberGameSetting
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

                $scope.callUpdate(productSetting);
            };

            $scope.callUpdate = function (productSetting) {
                CORE.blockUI();
                $http({
                    method: 'POST',
                    url: CORE.resolveUrl($('#custSetting').data('href')),
                    headers: { 'Content-Type': 'application/json' },
                    data: productSetting
                }).
                success(function (data) {
                    var timeout = 0;

                    CORE.unblockUI();

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

                    CORE.blockUI();
                    setTimeout(function () {
                        custSettingService.updateSettingModel();
                    }, timeout);

                    $('.messages').show();
                    CORE.scrollTop();
                }).
                error(function (data) {
                    CORE.unblockUI();
                    alert(data.ErrMsg);
                });
            };

            if (typeof settingContext === 'undefined') {
                $('#custSetting').html("It has a problem about data.");
            }

            $('#custSetting').show();
            setTimeout(function () {
                CORE.scrollTop();
                $('#userName').focus();
            }, 100);
            var messagesQueryString = CORE.getQueryParamValue('messages');
            if (messagesQueryString !== '') {
                var messages = CORE.decodeMessages(messagesQueryString);
                $scope.Messages = messages;
            }
        }
    ]);
}
/*global CORE, settingContext, result, customerSetting:true */

if (fanex.customerSetting) {
    fanex.customerSetting.factory('ThirdProductFactory', ['Fanex.CustomerSetting.LanguageService', function (languageService) {
        'use strict';
        var settings;
        switch (settingContext.ProductId) {
            case 5:
                settings = {
                    firstSportType: languageService.getString('VirtualSoccerCopyPt'),
                    otherSportType: languageService.getString('OtherSporttypeOfVirtualSoccer')
                };
                break;
            case 8:
                settings = {
                    firstSportType: languageService.getString('AustraliaActtabCopyPt'),
                    otherSportType: languageService.getString('OtherSporttypeOfAustraliaActtab')
                };
                break;
        }

        return settings;
    } ]);

    fanex.customerSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        'CustomerSettingService',
        'Fanex.CustomerSetting.LanguageService',
        'ThirdProductFactory',
        function ($scope, $http, custSettingService, languageService, thirdProductFactory) {
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
            $scope.result = fanex.customerSetting.Result;
            $scope.custInfo = customerSetting.CustInfo;
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
                customerSetting = data.productSetting;
            };

            $scope.updateCustomer = function () {
                $('.messages').hide();
                $scope.Messages = null;

                var bettingLimitItems = null,
                    positionTakingItems = null,
                    userGroup = null,
                    isChangeData = true,
                    bettingLimitScope,
                    productSetting = null,
                    positionTakingScope,
                    updateData = {};

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
                productSetting = {
                    PositionTakingSetting: positionTakingItems,
                    BettingLimitSetting: bettingLimitItems
                };

                isChangeData = fanex.isChangedData(productSetting);

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

                CORE.blockUI();
                updateData["settingContext"] = settingContext;
                updateData[thirdProductFactory.productSettingName] = productSetting;

                $http({
                    method: 'POST',
                    url: CORE.resolveUrl($('#custSetting').data('href')),
                    headers: { 'Content-Type': 'application/json' },
                    data:
                    {
                        settingContext: settingContext,
                        customerSetting: productSetting
                    }
                }).
                    success(function (data) {
                        var timeout = 0;
                        CORE.unblockUI();

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

            var messagesQueryString = CORE.getQueryParamValue('messages');
            if (messagesQueryString !== '') {
                var messages = CORE.decodeMessages(messagesQueryString);
                $scope.Messages = messages;
            }
        }
    ]);
}
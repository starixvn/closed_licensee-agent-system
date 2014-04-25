/*global core, popup, settingContext, result */
/*exported custSettingController*/

'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$filter',
        '$http',
        '$window',
        function($scope, $filter, $http, $window) {
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
            $scope.haveNumberGame = ($window['numberGameSetting'] !== undefined) ? true : false;

            $('#modeSwitcher').on('switch-change', function(e, data) {
                var isSimpleMode = !data.value;
                fanex.hideHint();
                $scope.settingMode = isSimpleMode;
                var positionTakingScope = angular.element('[ng-controller="PositionTakingController"]').scope();
                positionTakingScope.settingContext.IsSimpleMode = isSimpleMode;
                $scope.$apply();
            });

            $scope.formatNumber = function(number) {
                return $filter('number')(number, 2);
            };

            $scope.insertCustomer = function() {
                var custInfoScope = angular.element('#custInfoController').scope();
                if (custInfoScope.formCustInfo.$invalid) {
                    var keepGoing = true;
                    angular.forEach(custInfoScope.formCustInfo.$error, function(field) {
                        if (angular.isArray(field)) {
                            if (keepGoing) {
                                var element = $('#' + field[0].$name);
                                element.focus();
                                keepGoing = false;
                            }
                        }
                    });
                } else {
                    // Commission Setting
                    var commissionSettingScope = angular.element('[ng-controller="CommissionController"]').scope();
                    if (!commissionSettingScope.isValidData()) {
                        return;
                    }
                    var commissionSettingsModel = commissionSettingScope.saveData();

                    // Betting Limit
                    var bettingLimitScope = angular.element('[ng-controller="BetLimitController"]').scope();
                    if (!bettingLimitScope.isValidData()) {
                        return;
                    }
                    var bettingLimits = bettingLimitScope.saveData();

                    // position taking
                    var positionTakingScope = angular.element('[ng-controller="PositionTakingController"]').scope();
                    if (!positionTakingScope.isValidData()) {
                        return;
                    }
                    var positionTakingUpdated = positionTakingScope.saveData();

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
                                    PositionTakingSetting: positionTakingUpdated,
                                    CustInfo: custInfoScope.custInfo,
                                    CommissionSetting: {
                                        CommissionItems: commissionSettingsModel,
                                        UserGroup: bettingLimitScope.settingContext.TargetRoleId === 1
                                            ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup
                                            : ''
                                    },
                                    BettingLimits: bettingLimits
                                }
                            }
                        }).
                        success(function(data) {
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
                                popup.close();
                                if (window.parent.customerList) {
                                    window.parent.customerList.reRenderAfterPaged();
                                }
                            }
                        }).
                        error(function(data) {
                            core.unblockUI();
                            alert(data.ErrMsg);
                        });
                }
            };

            $scope.updateCustomer = function() {
                $('.messages').hide();
                $scope.Messages = null;

                var commissionSettingsModel = null,
                    bettingLimits = null,
                    userGroup = null;

                if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
                    // Commission Setting
                    var commissionSettingScope = angular.element('[ng-controller="CommissionController"]').scope();
                    if (!commissionSettingScope.isValidData()) {
                        return;
                    }
                    commissionSettingsModel = commissionSettingScope.saveData();

                    // Betting Limit
                    var bettingLimitScope = angular.element('[ng-controller="BetLimitController"]').scope();
                    if (!bettingLimitScope.isValidData()) {
                        return;
                    }
                    bettingLimits = bettingLimitScope.saveData();

                    userGroup = bettingLimitScope.settingContext.TargetRoleId === 1
                        ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup
                        : '';
                }

                // position taking
                var positionTakingScope = angular.element('[ng-controller="PositionTakingController"]').scope();
                if (!positionTakingScope.isValidData()) {
                    return;
                }
                var positionTakingUpdated = positionTakingScope.saveData();

                core.blockUI();
                $http({
                        method: 'POST',
                        url: core.resolveUrl($('#custSetting').data('href')),
                        headers: { 'Content-Type': 'application/json' },
                        data: {
                            settingContext: settingContext,
                            sportsBookSetting:
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
                    success(function(data) {
                        core.unblockUI();

                        if (data.ErrCode !== 0) {
                            $('.messages').show();
                            $scope.Messages = [
                                {
                                    ErrCode: -1,
                                    ErrMsg: data.ErrMsg
                                }
                            ];
                            return;
                        }

                        var isSuccess = true;
                        for (var i = 0; i < data.Messages.length; i++) {
                            var message = data.Messages[i];
                            if (message.ErrCode !== 0) {
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
                            setTimeout(function() {
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
                    error(function(data) {
                        core.unblockUI();
                        alert(data.ErrMsg);
                    });
            };

            if (typeof settingContext === 'undefined') {
                $('#custSetting').html("It has a problem about data.");
            }

            $('#custSetting').show();

            var messagesQueryString = core.getParameterByName('messages');
            if (messagesQueryString !== '') {
                var messages = core.decodeMessages(messagesQueryString);
                $scope.Messages = messages;
                popup.close();
            }
        }
    ]);
}
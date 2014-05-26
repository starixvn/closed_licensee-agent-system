/*global core, popup, settingContext, result, virtualSportsSetting */

'use strict';
if (fanex.customerSetting) {
    fanex.customerSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        'Fanex.CustomerSetting.LanguageService',
        function ($scope, $http, languageService) {
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
            $scope.result = fanex.customerSetting.Result;
            $scope.custInfo = virtualSportsSetting.CustInfo;

            $('#modeSwitcher').on('switch-change', function (e, data) {
                var isSimpleMode = !data.value;
                fanex.hideHint();
                $scope.settingMode = isSimpleMode;
                var positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();
                positionTakingScope.settingContext.IsSimpleMode = isSimpleMode;
                $scope.$apply();
            });

            $scope.updateCustomer = function () {
                $('.messages').hide();
                $scope.Messages = null;

                var bettingLimitItems = null,
                    positionTakingItems = null,
                    userGroup = null,
                    bettingLimitScope,
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
                if (positionTakingItems.length === 0 && bettingLimitItems.length === 0) {
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
                        virtualSportsSetting:
                            {
                                PositionTakingSetting: positionTakingItems,
                                BettingLimitSetting: bettingLimitItems
                            }
                    }
                }).
                    success(function (data) {
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
                                timeout = 7000;
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

            var messagesQueryString = core.getParameterByName('messages');
            if (messagesQueryString !== '') {
                var messages = core.decodeMessages(messagesQueryString);
                $scope.Messages = messages;
                popup.close();
            }
        }

    ]);
}
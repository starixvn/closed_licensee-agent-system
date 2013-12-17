/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.sportsbookSetting) {
    // An agent edit another agent in the simple mode
    fanex.sportsbookSetting.directive('uniqueUserName', ['fanex.sportsbookSetting.custInfoService', function (custInfoService) {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, el, attrs, ctrl) {
                var currentValue = undefined;
                var timeout = undefined;
                var setValidity = function (isValid) {
                    ctrl.$setValidity('', isValid);
                }

                scope.$watch(function () {
                    return ctrl.$modelValue;
                }, function (value) {
                    if (value) {
                        // save current value to check whether user name is existed after 200 miliseconds.
                        currentValue = value;

                        if (timeout != undefined) {
                            clearTimeout(timeout);
                        }

                        timeout = setTimeout(function () {
                            if (currentValue == ctrl.$modelValue) {
                                var promise = custInfoService.checkUsername(value);
                                promise.then(function (data) {
                                    if (data.MsgId == 0) {
                                        ctrl.hintMessage = '';
                                        setValidity(true);
                                    }
                                    else {
                                        ctrl.hintMessage = data.MsgContent;
                                        setValidity(false);
                                    }
                                }, function (reason) {
                                    setValidity(false);
                                    alert(reason);
                                });
                            }
                        }, 200);
                    }
                });
            }
        }
    } ]);
}
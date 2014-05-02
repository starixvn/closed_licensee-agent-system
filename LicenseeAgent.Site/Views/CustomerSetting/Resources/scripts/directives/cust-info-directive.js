/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.customerSetting) {
    // An agent edit another agent in the simple mode
    fanex.customerSetting.directive('uniqueUserName', [
        '$timeout', 'CustomerInfoService', function ($timeout, custInfoService) {
            'use strict';
            return {
                require: 'ngModel',
                restrict: 'A',
                link: function (scope, el, attrs, ctrl) {
                    var timeout;
                    var setValidity = function (isValid) {
                        ctrl.$setValidity('', isValid);
                    };

                    scope.$watch(function () {
                        return ctrl.$modelValue;
                    }, function (value) {
                        if (value) {
                            // save current value to check whether user name is existed after 200 miliseconds.
                            var currentValue = value;
                            if (typeof timeout !== 'undefined') {
                                $timeout.cancel(timeout);
                            }

                            timeout = $timeout(function () {
                                if (currentValue === ctrl.$modelValue) {
                                    var promise = custInfoService.checkUsername(scope, currentValue);
                                    promise.userName = currentValue;
                                    promise.then(function (data) {
                                        if (promise.userName !== ctrl.$modelValue) {
                                            return;
                                        }
                                        if (data.ErrCode === 0) {
                                            ctrl.hintMessage = '';
                                            setValidity(true);
                                        } else {
                                            ctrl.hintMessage = data.ErrMsg;
                                            setValidity(false);
                                        }
                                    }, function (reason) {
                                        setValidity(false);
                                        alert(reason);
                                    }, function (update) {
                                        setValidity(false);
                                        alert(update);
                                    });
                                }
                            }, 200);
                        }
                    });
                }
            };
        }
    ]);
}
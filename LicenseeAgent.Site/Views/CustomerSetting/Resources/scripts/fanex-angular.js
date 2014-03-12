'use strict';

var fanex = fanex || {};

fanex.angularModule = angular.module('fanex.angular', []);

fanex.angularModule.config([
'$httpProvider',
function ($httpProvider) {
    var numLoadings = 0;
    $httpProvider.responseInterceptors.push(function () {
        return function (promise) {
            numLoadings++;
            CORE.blockUI();
            var hide = function (response) {
                if (!(--numLoadings)) {
                    CORE.unblockUI();
                }
                if (response.status != 200 && response.status != 404) {
                    CORE.handleError(response.data);
                }
                return response;
            };
            return promise.then(hide, hide);
        };
    });
} ]);

fanex.angularModule.directive('hintValidator', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attrs, ngModel) {
            var showHint = function () {
                var hintMessage = undefined;

                if (!hintMessage) {
                    var keepGoing = true;
                    angular.forEach(ngModel.$error, function (value, name) {
                        if (keepGoing) {
                            if (value) {
                                if (name == '') {
                                    hintMessage = ngModel.hintMessage;
                                }
                                else {
                                    hintMessage = element.attr(name + '-hint');
                                    ngModel.hintMessage = '';
                                }
                                if (hintMessage) {
                                    keepGoing = false;
                                }
                            }
                        }
                    });
                }

                if (!hintMessage) {
                    hintMessage = attrs.hintValidator;
                }

                if (hintMessage) {
                    fanex.showHint(element, hintMessage);
                }
                else {
                    fanex.hideHint();
                }
            }
            var hideHint = function () {
                fanex.hideHint();
            }

            element.bind('focus', function (event) {
                if (ngModel.$invalid) {
                    showHint();
                }
            }).bind('blur', function (event) {
                hideHint();
            });

            var validator = function () {
                if (ngModel.$invalid) {
                    showHint();
                }
                else {
                    hideHint();
                }
            }

            scope.$watch(function () { return ngModel.$modelValue; }, validator);
            scope.$watch(function () { return ngModel.$invalid; }, validator);
            scope.$watch(function () { return ngModel.hintMessage; }, validator);
        }
    }
});

fanex.angularModule.directive('passwordRule', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attrs, ctrl) {
            var isPassword = function (str) {
                if (str) {
                    var regex1 = /^(?=.*\d).{8,100}$/;
                    var regex2 = /^(?=.*[a-z]).{8,100}$/;
                    var regex3 = /^(?=.*[A-Z]).{8,100}$/;
                    var regex4 = /^(?=.*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{8,100}$/;

                    var whiteSpace = /^\S*$/;

                    var case1 = str.search(regex1) != -1 ? 1 : 0;
                    var case2 = str.search(regex2) != -1 ? 1 : 0;
                    var case3 = str.search(regex3) != -1 ? 1 : 0;
                    var case4 = str.search(regex4) != -1 ? 1 : 0;

                    var case5 = str.search(whiteSpace) != -1;

                    return (case1 + case2 + case3 + case4 >= 3) && case5;
                }
                return false;
            };

            var passwordValidator = function (value) {
                if (isPassword(value)) {
                    // it is valid
                    ctrl.$setValidity('passwordRule', true);
                    return value;
                } else {
                    // it is invalid, return undefined (no model update)
                    ctrl.$setValidity('passwordRule', false);
                    return undefined;
                }
            }

            ctrl.$parsers.unshift(passwordValidator);
            ctrl.$formatters.unshift(passwordValidator);
        }
    };
});

fanex.angularModule.directive('autoNumericFormat', [
'$parse',
 function ($parse) {
     return {
         restrict: 'A',
         require: 'ngModel',
         link: function (scope, element, attr, ctrl) {
             var params;
             try {
                 params = jQuery.parseJSON(attr.autoNumericFormat);
             }
             catch (err) {
                 //Handle errors here
             }

             var updateModel = function (newValue) {
                 var currentValue = ctrl.$modelValue;
                 if (currentValue != newValue) {
                     ctrl.$dirty = true;
                     ctrl.$viewValue = element.val();
                     ctrl.$modelValue = element.autoNumeric('get');
                     var model = $parse(attr.ngModel);
                     model.assign(scope, newValue);
                     scope.$apply();
                 }

                 if (params != undefined && params['command'] != undefined)
                     scope.$eval(params['command']);
             }

             var autoNumericOptions = {};

             if (params != undefined) {
                 if (params['wEmpty'] != undefined)
                     autoNumericOptions.wEmpty = params['wEmpty'];
                 else autoNumericOptions.wEmpty = 'zero';

                 if (params['vMin'] != undefined)
                     autoNumericOptions.vMin = params['vMin'];
                 else autoNumericOptions.vMin = 0;

                 if (params['vMax'] != undefined)
                     autoNumericOptions.vMax = params['vMax'];
                 else autoNumericOptions.vMax = 9999999999999;

                 if (params['lZero'] != undefined)
                     autoNumericOptions.lZero = params['lZero'];
                 else autoNumericOptions.lZero = 'deny';
             }

             element.autoNumeric('init', autoNumericOptions)
            .keyup(function () {
                var newValue = element.autoNumeric('get');
                updateModel(newValue);
            });
         }
     };
 } ]);
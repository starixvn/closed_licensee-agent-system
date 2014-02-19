/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.factory('fanex.customerSetting.custInfoService', [
    '$http',
    '$timeout',
    '$rootScope',
    '$q',
    function ($http, $timeout, $rootScope, $q) {
        if ($('#userName').length > 0) {
            var apiPath = CORE.resolveUrl($('#userName').data('href'));
            return {
                checkUsername: function (scope, userName) {
                    var data = {
                        userName: userName
                    };
                    var deferred = $q.defer();
                    $http({
                        method: 'POST',
                        url: apiPath,
                        data: data
                    }).success(function (data) {
                        //Passing data to deferred's resolve function on successful completion
                        deferred.resolve(data);
                    }).error(function (data) {
                        //Sending a friendly error message in case of failure
                        deferred.reject("An error occured while fetching items");
                    });

                    //Returning the promise object
                    return deferred.promise;
                }
            }
        }
    } ])
}
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.sportsbookSetting) {
    //// Helper

    fanex.sportsbookSetting.factory('fanex.sportsbookSetting.custInfoService', [
    '$http',
    '$q',
    function ($http, $q) {
        if ($('#userName').length > 0) {
            var apiPath = CORE.resolveUrl($('#userName').data('href'));
            return {
                checkUsername: function (userName) {
                    var data = {
                        userName: userName,
                        targetRoleId: settingContext.TargetRoleId
                    };
                    deferred = $q.defer();
                    //Calling Web API to fetch shopping cart items
                    $http.post(apiPath, data, { timeout: deferred.promise }
                ).success(function (data) {
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
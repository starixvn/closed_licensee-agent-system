/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global core */

'use strict';

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.factory('CustomerInfoService', [
        '$http',
        '$timeout',
        '$rootScope',
        '$q',
        function ($http, $timeout, $rootScope, $q) {
            if ($('#userName').length > 0) {
                var apiPath = core.resolveUrl($('#userName').data('href'));
                return {
                    checkUsername: function (scope, userName) {
                        var params = {
                            userName: userName
                        };
                        var deferred = $q.defer();
                        $http({
                            method: 'POST',
                            url: apiPath,
                            data: params
                        }).success(function (data) {
                            //Passing data to deferred's resolve function on successful completion
                            deferred.resolve(data);
                        }).error(function () {
                            //Sending a friendly error message in case of failure
                            deferred.reject("An error occured while fetching items");
                        });

                        //Returning the promise object
                        return deferred.promise;
                    }
                };
            }
        }
    ]);
}
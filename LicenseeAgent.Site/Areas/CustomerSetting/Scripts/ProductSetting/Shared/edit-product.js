/*global products*/

var editProductModule = angular.module('customerSetting.editProduct', []);
editProductModule.controller('EditProductController', ['$scope',
    function EditProductController($scope) {
        'use strict';
        $scope.products = products;
        $scope.activeUrl = function (productId) {
            var url = '';

            if ($(window.parent.document).find('#popup-dialog').length > 0) {
                $('.tab-content').height(640);
            }

            angular.forEach(products, function (value) {
                if (value.ProductId === productId) {
                    url = value.Url;
                    return;
                }
            });

            return url;
        };
        $scope.loadUrl = function (url) {
            var currentUrl = $('#tabFrame').attr('src');
            if (currentUrl !== url) {
                $('#tabFrame').attr('src', url);
            }
        };
    }
]);

editProductModule.controller('CustomerInfoController', ['$scope',
    function PhoneListCtrl($scope) {
        'use strict';
        $scope.custInfo = $scope.$parent.custInfo;
    }
]);
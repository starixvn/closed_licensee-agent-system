var TotalBetsSoccer = function ($) {    
    return {
        leagues: [],
        events: [],
             
        init: function (){
            
        }
    }
} (jQuery);
$(function () {
    TotalBetsSoccer.init();
});

var SoccerApp = angular.module('SoccerApp', []);

SoccerApp.controller('LeagueCotroller', function ($scope) {
    $scope.leagues = TotalBetsSoccer.leagues;
});


SoccerApp.controller('EventCotroller', function ($scope) {
    $scope.events = TotalBetsSoccer.events;
});
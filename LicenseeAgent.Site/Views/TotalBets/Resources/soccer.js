var TotalBetsSoccer = function ($) {
    return {
        
        events: [],

        init: function () {
            TotalBetsSoccer.initEvent();
        },
        initEvent: function () {
            $("#select-league-button").click(function () {
                LeagueFilter.showFilter();
            });
            $("#cancel-league-select").click(function () {
                LeagueFilter.hideFilter();                
            });
            $("#ok-league-select").click(function () {
                LeagueFilter.selectedLeagues();
            });
        }
    }
} (jQuery);
$(function () {
    TotalBetsSoccer.init();
});

var LeagueFilter = function ($) {
    return {
        leagues: [],
        urlGetLeague: $('#search-form').attr('action'),

        showFilter: function () {
            $(".select-league-area").slideDown();
            LeagueFilter.loadLeagues();
        },
        hideFilter: function () {
            $(".select-league-area").hide();
        },
        selectedLeagues: function () {
            $(".select-league-area").slideUp();
        },
        loadLeagues: function () {            
        }
    }
} (jQuery);

var SoccerApp = angular.module('SoccerApp', []);

SoccerApp.controller('LeagueCotroller', function ($scope, $http) {
    $scope.leagues = {};
    $scope.loadLeague = function () {
        $http.get(LeagueFilter.urlGetLeague).success(function (data) {
            $scope.leagues = data.List;
        });
    };

});

SoccerApp.controller('EventCotroller', function ($scope) {
    $scope.events = TotalBetsSoccer.events;
});
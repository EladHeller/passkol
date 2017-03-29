angular.module("passkolApp")
    .factory("srvHelper", ["$http", function ( $http) {
        return {
            day_month_yearToDateTime: function(day,month,year){
                return day + "/" + month + "/" + year;
            }
        }
    }]);
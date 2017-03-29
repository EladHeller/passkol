angular.module("pelecardApp",[])
    .controller("ctrlTransact", ["$scope", function ($scope) {
        var parentScope = parent.angular.element('.modal-content').scope().$parent;;
        setTimeout(function () {
            parentScope.handlePeleCardRes($scope.res);
        }, 10);

    }]);
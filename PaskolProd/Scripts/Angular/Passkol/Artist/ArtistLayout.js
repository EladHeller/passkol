    angular.module('passkolApp')
    .controller('ArtistLayout', ['$scope', 'srvArtist', 'srvAuction', function ($scope, srvArtist, srvAuction) {

        $scope.isFirstEntrance = window.location.search.indexOf('isFirstEntrance') > -1;
        $scope.href = {};
        $scope.href.path = 'MyProfile';
}]);
angular.module('passkolApp')
    .controller('ArtAgntPersonalMng', ['$scope', 'srvArtAgntRel', function ($scope, srvArtAgntRel) {

        // remove agency relationship
        $scope.removeArtist = function (artistId) {
            $scope.isLoading = true;
            var req = { ArtistId: artistId };
            srvArtAgntRel.AgntRemovedByArtAgnt(req, $scope.antiForgeryToken)
                .success(function (res) {
                    window.location.reload();
                }).error(function (res) {
                   // $scope.artistEmailNessage = res.data.Message;
                }).finally(function () { $scope.isLoading = false; });
        }

        // confirm agent relationship
        $scope.confirmArtist = function (artistId) {
            $scope.isLoading = true;
            var req = { ArtistId: artistId };
            srvArtAgntRel.AgntConfirmedByArtAgnt(req, $scope.antiForgeryToken)
                .success(function (res) {
                    window.location.reload();
                }).error(function (res) {
                    //$scope.artistEmailNessage = res.data.Message;
                }).finally(function () { $scope.isLoading = false; });
        }

        // add artist to artistmanager managment
        $scope.addArtist = function () {
            var artistEmail = $scope.artistEmail
            // if text box is shown == client wants to save his data
            if ($('#togglemanager.toggled')[0] == undefined && artistEmail != undefined) {
                var req = { ArtistEmail: artistEmail };
                srvArtAgntRel.AgntConfirmedViaEmailByArtAgnt(req, $scope.antiForgeryToken)
                    .success(function (res) {
                        if (!res.Success) {
                            $scope.artistEmailNessage = res.Message;
                        }
                        else {
                            window.location.reload();
                        }
                });
            }
        }
    }]);
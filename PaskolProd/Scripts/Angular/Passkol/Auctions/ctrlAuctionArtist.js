angular.module("passkolApp")
    .controller("ctrlAuctionArtist", ["$scope", "srvAuction", function ($scope, srvAuction) {
        // Get all auctions for artist

        $scope.href.path = 'OriginalMusic';

        $scope.getAuctionsForArtist = function () {
            $scope.isLoading = true;
            $scope.allAuctionsForArtist = [];
            var v = $scope.antiForgeryToken;
            srvAuction.getAuctionsForArtist(v)
                .success(function (res) {
                    if (res.Success) {
                        $scope.allAuctionsForArtist = res.Entities;
                    }
                })
                .error(function (res) {
                    var recs = res;
                })
                .finally(function (res) {
                    $scope.isLoading = false;
                })
        }
        setTimeout(function () {
            $scope.getAuctionsForArtist();
        }, 0);

        // Participate with original music toggele
        $scope.participateInAuctionToggele = function (p) {
            var v = $scope.antiForgeryToken;
            srvAuction.participateInAuctionToggele(v, {p:p});
        }

        $scope.getAuction = function (id) {
            var v = $scope.antiForgeryToken;
            srvAuction.getAuction(v, { id: id })
                .success(function (res) {
                    if (res.Success) {
                        $scope.sketchMode = true;
                        $scope.requirments = res.Entity;
                    }
                })
                .error(function (res) {
                    var recs = res;
                })
                .finally(function (res) {
                        $scope.isLoading = false;
            })
        }

        // send sketches
        $scope.uploadSketch = function (id) {
            var v = $scope.antiForgeryToken;
            srvAuction.uploadSketch(v, { id: id });
        }
    }]);
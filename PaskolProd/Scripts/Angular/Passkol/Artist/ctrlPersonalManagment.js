angular.module('passkolApp')
    .controller('ctrlPersonalManagment', ['$scope', 'srvArtAgntRel', function ($scope, srvArtAgntRel) {

        $scope.href.path = 'PersonalManagment';
        
        // remove agency relationship
        $scope.removeArtistAgent = function (artistAgentId) {
            $scope.isLoading = true;
            var req = { ArtistAgentId: artistAgentId };
            srvArtAgntRel.AgntRemovedByArt(req, $scope.antiForgeryToken)
                .success(function (res) {
                    window.location.reload();
                }).error(function (res) {
                    // $scope.artistEmailNessage = res.data.Message;
            }).finally(function () { $scope.isLoading = false; });
        }

        // confirm agent relationship
        $scope.confirmArtistAgent = function (artistAgntId) {
            $scope.isLoading = true;
            var req = { ArtistAgentId: artistAgntId };
            srvArtAgntRel.AgntConfirmedByArt(req, $scope.antiForgeryToken)
                .success(function (res) {
                    window.location.reload();
                }).error(function (res) {
                    // $scope.artistEmailNessage = res.data.Message;
            }).finally(function () { $scope.isLoading = false; });
        }

        // add artist to artistmanager managment
        $scope.addArtistAgent = function () {
           
            var artistAgentEmail = $scope.artistAgentEmail
            // if text box is shown == client wants to save his data
            if ($('#togglemanager.toggled')[0] == undefined && artistAgentEmail != undefined) {
                $scope.isLoading = true;
                var req = { ArtistAgentEmail: artistAgentEmail };
                srvArtAgntRel.AgntConfirmedViaEmailByArt(req, $scope.antiForgeryToken).success(function (res) {
                    if (!res.Success) {
                        $scope.artistAgentEmailNessage = res.Message;
                    }
                    else {
                        window.location.reload();
                    }
                }).error(function (res) {
                    // $scope.artistEmailNessage = res.data.Message;
                }).finally(function () { $scope.isLoading = false; });
            }
        }
    }]);
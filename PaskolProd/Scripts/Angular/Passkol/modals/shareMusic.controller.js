(function () {
    'use strict';

    angular
        .module('passkolApp')
        .controller('shareMusicCtrl', shareMusicCtrl);
    shareMusicCtrl.$inject = ['$scope', '$uibModalInstance', 'music','antiForgeryToken','appModals']
    function shareMusicCtrl($scope, $uibModalInstance, music,antiForgeryToken, appModals) {
        $scope.music = music;
        $scope.url = location.origin + '/Search?id=' + music.ID;
        $scope.antiForgeryToken = antiForgeryToken;

        // Methods
        $scope.close = function close() {
            $uibModalInstance.close();
        }

        $scope.shareInMail = function shareInMail() {
            appModals.shareMusicMailModal($scope.music, $scope.antiForgeryToken);
            $uibModalInstance.close();
        }
    }
})();

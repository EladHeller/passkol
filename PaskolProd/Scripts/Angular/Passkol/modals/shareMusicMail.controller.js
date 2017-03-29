(function () {
    'use strict';

    angular
        .module('passkolApp')
        .controller('shareMusicMailCtrl', shareMusicMailCtrl);
    shareMusicMailCtrl.$inject = ['$scope', '$uibModalInstance', 'music','antiForgeryToken','musicService']
    function shareMusicMailCtrl($scope, $uibModalInstance, music,antiForgeryToken, musicService) {
        $scope.music = music;
        $scope.antiForgeryToken = antiForgeryToken;
        
        // Methods
        $scope.close = function close() {
            $uibModalInstance.close();
        }
        
        $scope.shareInMail = function () {
            var data = {
                UserMail : $scope.UserMail,
                FriendMail : $scope.FriendMail,
                Comments : $scope.Comments,
                SendCopy : $scope.SendCopy,
                MusicId: $scope.music.ID,
            };
            musicService.shareInMail(data, $scope.antiForgeryToken).then(function success(evt) {
                if (evt.data.Success) {
                    $scope.close();
                } 
            }, function failed(evt) {
                
            });
        }
    }
})();

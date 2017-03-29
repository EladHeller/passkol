(function () {
    'use strict';

    angular
        .module('passkolApp')
        .controller('editMusicModalCtrl', editMusicModalCtrl);
    editMusicModalCtrl.$inject = ['$scope', '$uibModalInstance', 'music', 'antiForgeryToken', 'tags', 'search'];
    function editMusicModalCtrl($scope, $uibModalInstance, music, antiForgeryToken, tags, search) {
        $scope.music = music;
        $scope.antiForgeryToken = antiForgeryToken;
        $scope.tags = tags;
        $scope.isModal = true;
        $scope.search = search;

        // Methods
        $scope.close = function close() {
            $uibModalInstance.dismiss();
        }

        $scope.saveAndClose = function save() {
            var promise = $scope.save(false);
            if (promise) {
                promise.then(function success() {
                    $uibModalInstance.dismiss();
                });
            }
        }
    }
})();

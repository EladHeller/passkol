(function () {
    'use strict';

    angular
        .module('passkolApp')
        .controller('alertModalCtrl', alertModalCtrl);
    alertModalCtrl.$inject = ['$scope', '$uibModalInstance', 'text', 'subject']
    function alertModalCtrl($scope, $uibModalInstance, text, subject) {
        $scope.text = text;
        $scope.subject = subject;
        
        // Methods
        $scope.close = function close() {
            $uibModalInstance.close();
        }
    }
})();

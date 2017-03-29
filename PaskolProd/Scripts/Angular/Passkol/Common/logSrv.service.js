(function () {
    'use strict';
    angular.module("passkolApp").factory("logService", logService);
    logService.$inject = ['$http'];
    function logService($http) {
        var service = {
            logAction: logAction,
            logSearch:logSearch,
            logActions: logActions
        };

        return service;

        function logActions(logsData) {
            return $http.post('/Log/Logs', logsData);
        }

        function logSearch(logData) {
            return $http.post('/Log/SearchLog', logData);
        }

        function logAction(logData) {
            return $http.post('/Log/Log', logData);
        }
    }
})();